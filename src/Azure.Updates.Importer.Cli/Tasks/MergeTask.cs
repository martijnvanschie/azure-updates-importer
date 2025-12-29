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
            List<ReleaseCommunicationItem> _mergedList = new List<ReleaseCommunicationItem>();
            ParquetHandler ph = new ParquetHandler();

            var weekNumber = DateTimeUtils.GetWeekNumber(ImporterContext.DateImport);
            var outputFile = Path.Combine(ImporterContext.BronsePath.FullName, $"{ImporterContext.DateImport:yyyyMM}-{weekNumber}-azureupdates.parquet");
            var fi = new FileInfo(outputFile);

            if (fi.Exists)
            {
                AnsiConsoleLogger.LogDebug($"File [{outputFile}] already exists. Reading file for merger.", _logger);
                _mergedList = ph.ReadRssFeedsFromParquetFile(fi.FullName);
            }

            var parquetFiles = GetParquetFilesFromLandingZone();
            if (parquetFiles.Count == 0)
            {
                AnsiConsoleLogger.LogInfo("No parquet files found in the landing folder. Exiting process.");
                return 0;
            }

            AnsiConsoleLogger.LogInfo($"Found [{parquetFiles.Count}] parquet files in the landing folder.");
            foreach (var item in parquetFiles)
            {
                AnsiConsoleLogger.LogDebug($"Processing file [{item.Name}], imported at [{item.CreationTime}].", _logger);
                var feeds = ph.ReadRssFeedsFromParquetFile(item.FullName);
                var newFeeds = feeds.Where(feed => !_mergedList.Contains(feed));
                _mergedList.AddRange(newFeeds);
            }

            AnsiConsoleLogger.LogDebug($"Writing merged file [{outputFile}]", _logger);
            ph.WriteRawRssFeedsToParquetFile(outputFile, _mergedList);
            AnsiConsoleLogger.LogInfo($"Succesfully written [{_mergedList.Count}] merged feeds to file [{outputFile}].", _logger);

            foreach (var item in parquetFiles)
            {
                AnsiConsoleLogger.LogDebug($"Creating backup of file [{item.FullName}]", _logger);
                item.MoveTo($"{item.FullName}.backup");
                AnsiConsoleLogger.LogInfo($"Backup of file [{item.FullName}] created successfully", _logger);
            }

            return 0;
        }

        private List<FileInfo> GetParquetFilesFromLandingZone()
        {
            var directory = new DirectoryInfo(ImporterContext.LandingPath.FullName);
            _logger.LogDebug("Reading files from source directory [{directory}]", directory);

            var files = directory.GetFiles("*.parquet", SearchOption.TopDirectoryOnly).ToList();
            _logger.LogInformation($"Found [{files.Count}] parquet files in the landing folder.");

            return files;
        }
    }
}
