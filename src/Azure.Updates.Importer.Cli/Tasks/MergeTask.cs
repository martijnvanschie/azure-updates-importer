using Azure.Updates.Importer.Cli.Core;
using Azure.Updates.Importer.Cli.Model;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace Azure.Updates.Importer.Cli.Tasks
{
    public class MergeTask : ITask
    {
        private static readonly ILogger<MergeTask> _logger = LoggerManager.GetLogger<MergeTask>();
        private static readonly Settings _settings = ConfigurationManager.GetConfiguration().Settings;

        public MergeTask()
        {
        }

        public StatusContext StatusContext { get; set; }

        public async Task<int> RunAsync()
        {
            List<RssFeed> _mergedList = new List<RssFeed>();
            ParquetHandler ph = new ParquetHandler();

            var weekNumber = DateTimeUtils.GetWeekNumber(ImporterContext.DateImport);
            var outputFile = Path.Combine(ImporterContext.BronsePath.FullName, $"{ImporterContext.DateImport:yyyyMM}-{weekNumber}-azureupdates.parquet");
            var fi = new FileInfo(outputFile);

            if (fi.Exists)
            {
                AnsiConsoleLogger.LogInfo($"File [{outputFile}] already exists. Reading file for merger.", _logger);
                _mergedList = ph.ReadParquetFile(fi.FullName);
            }

            var parquetFiles = GetParquetFilesFromLandingZone();
            if (parquetFiles.Count == 0)
            {
                AnsiConsoleLogger.LogInfo("No parquet files found in the landing folder.");
                return 0;
            }
            AnsiConsoleLogger.LogInfo($"Found [{parquetFiles.Count}] parquet files in the landing folder.");


            foreach (var item in parquetFiles)
            {
                AnsiConsoleLogger.LogInfo($"Processing file [{item.Name}], imported at [{item.CreationTime}].", _logger);
                var feeds = ph.ReadParquetFile(item.FullName);
                var newFeeds = feeds.Where(feed => !_mergedList.Contains(feed));
                _mergedList.AddRange(newFeeds);
                item.MoveTo($"{item.FullName}.backup");
            }

            AnsiConsoleLogger.LogInfo($"Writing merged file [{outputFile}]", _logger);
            ph.WriteParquetFile(outputFile, _mergedList);

            return 0;
        }

        private List<FileInfo> GetParquetFilesFromLandingZone()
        {
            var directory = new DirectoryInfo(ImporterContext.LandingPath.FullName);
            _logger.LogInformation("Reading files from source directory [{directory}]", directory);

            var files = directory.GetFiles("*.parquet", SearchOption.TopDirectoryOnly).ToList();
            _logger.LogInformation($"Found [{files.Count}] parquet files in the landing folder.");
            
            return files;
        }
    }
}
