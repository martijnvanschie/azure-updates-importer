using Azure.Updates.Importer.Cli.Core;
using Azure.Updates.Importer.Cli.Model;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace Azure.Updates.Importer.Cli.Tasks
{
    public class VerifyTask : ITask
    {
        private static readonly ILogger<VerifyTask> _logger = LoggerManager.GetLogger<VerifyTask>();
        private static readonly Settings _settings = ConfigurationManager.GetConfiguration().Settings;

        public StatusContext StatusContext { get; set; }

        public async Task<int> RunAsync()
        {
            List<RssFeed> _mergedList = new List<RssFeed>();
            List<RssFeed> _mergedList2 = new List<RssFeed>();
            ParquetHandler ph = new ParquetHandler();

            var parquetFiles = GetParquetFilesFromLandingZone();
            if (parquetFiles.Count == 0)
            {
                AnsiConsoleLogger.LogInfo("No parquet files found in the landing folder. Exiting process.");
                return 0;
            }

            foreach (var item in parquetFiles)
            {
                AnsiConsoleLogger.LogDebug($"Processing file [{item.Name}], imported at [{item.CreationTime}].", _logger);
                var feeds = ph.ReadRssFeedsFromParquetFile(item.FullName);
                var newFeeds = feeds.Where(feed => !_mergedList.Contains(feed));
                _mergedList.AddRange(newFeeds);
            }

            AnsiConsoleLogger.LogInfo($"Found total of [{_mergedList.Count}] unique feeds in Landing Zone files", _logger);

            var parquetFiles2 = GetParquetFilesFromBronzeZone();
            if (parquetFiles2.Count == 0)
            {
                AnsiConsoleLogger.LogInfo("No parquet files found in the landing folder. Exiting process.");
                return 0;
            }

            foreach (var item in parquetFiles2)
            {
                AnsiConsoleLogger.LogDebug($"Processing file [{item.Name}], imported at [{item.CreationTime}].", _logger);
                var feeds = ph.ReadRssFeedsFromParquetFile(item.FullName);
                var newFeeds = feeds.Where(feed => !_mergedList2.Contains(feed));
                _mergedList2.AddRange(newFeeds);
            }

            AnsiConsoleLogger.LogInfo($"Found total of [{_mergedList2.Count}] unique feeds in Brons files", _logger);

            return 0;
        }

        private List<FileInfo> GetParquetFilesFromLandingZone()
        {
            var directory = new DirectoryInfo(ImporterContext.LandingPath.FullName);
            _logger.LogDebug("Reading files from source directory [{directory}]", directory);

            var files = directory.GetFiles("*.parquet*", SearchOption.TopDirectoryOnly).ToList();
            _logger.LogInformation($"Found [{files.Count}] parquet files in the landing folder.");

            return files;
        }

        private List<FileInfo> GetParquetFilesFromBronzeZone()
        {
            var directory = new DirectoryInfo(ImporterContext.BronsePath.FullName);
            _logger.LogDebug("Reading files from source directory [{directory}]", directory);

            var files = directory.GetFiles("*.parquet*", SearchOption.TopDirectoryOnly).ToList();
            _logger.LogInformation($"Found [{files.Count}] parquet files in the landing folder.");

            return files;
        }
    }
}
