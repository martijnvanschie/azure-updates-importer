using Azure.Updates.Importer.Cli.Core;
using Azure.Updates.Importer.Cli.Model;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace Azure.Updates.Importer.Cli.Tasks
{
    public class ImportTaskV2 : ITask
    {
        private static readonly ILogger<ImportTask> _logger = LoggerManager.GetLogger<ImportTask>();
        private static readonly Settings _settings = ConfigurationManager.GetConfiguration().Settings;

        public StatusContext StatusContext { get; set; }

        public async Task<int> RunAsync()
        {
            try
            {
                var lastImportDate = await Utils.GetLastimportTimeAsync();
                _logger.LogInformation("Last import date was [{lastImportDate}]", lastImportDate);

                StatusContext?.Status("Initializing Release Communications Client");
                ReleaseCommunicationsClient releaseCommunicationsClient = new ReleaseCommunicationsClient();

                // Get all updates modified since a specific date
                StatusContext?.Status("Getting all Azure updates modified since last import date.");
                var recentUpdates = await releaseCommunicationsClient.GetAllAzureUpdatesAsync(modifiedSince: lastImportDate);
                StatusContext?.Status($"Returned {recentUpdates.Count} release items since {lastImportDate}");
                AnsiConsoleLogger.LogInfo($"Returned {recentUpdates.Count} release items since {lastImportDate}");

                // Convert DTOs to ReleaseCommunicationItems
                var releaseCommunicationItems = recentUpdates
                    .Select(dto => new ReleaseCommunicationItem(dto))
                    .ToList();

                if (releaseCommunicationItems.Count == 0)
                {
                    AnsiConsoleLogger.LogInfo("No new or updated release communications found since last import date.");
                    return 0;
                }

                StatusContext?.Status("Writing information to parquet.");
                await WriteToParquetAsync(releaseCommunicationItems);

                return 1;
            }
            catch (System.Exception ex)
            {
                AnsiConsoleLogger.LogError(ex.Message);
                _logger.LogError(ex, "Error occurred during ImportTaskV2 execution.");
                throw;
            }
        }

        private async Task WriteToParquetAsync(List<ReleaseCommunicationItem> recentUpdates)
        {
            StatusContext?.Status("Writing parguet file");
            var path = Path.Combine(ImporterContext.LandingPath.FullName, $"{ImporterContext.DateImport:yyyyMMdd-HHmmss}-azureupdates.parquet");
            ParquetHandler ph = new ParquetHandler();
            ph.WriteRawRssFeedsToParquetFile(path, recentUpdates);
            AnsiConsoleLogger.LogSuccess($"Parquet file written to {path}");
            await Task.CompletedTask;
        }
    }
}
