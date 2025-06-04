using Azure.TrendsAndInsights.Data;
using Azure.Updates.Importer.Cli.Core;
using Azure.Updates.Importer.Cli.Model;
using Azure.Updates.Importer.Data;
using Azure.Updates.Importer.Data.DataModels;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace Azure.Updates.Importer.Cli.Tasks
{
    public class PrepareTask : ITask
    {
        private static readonly ILogger<PrepareTask> _logger = LoggerManager.GetLogger<PrepareTask>();
        private static readonly Settings _settings = ConfigurationManager.GetConfiguration().Settings;

        public PrepareTask()
        {
            
        }
        public StatusContext StatusContext { get; set; }

        public async Task<int> RunAsync()
        {
            var azureServices = new List<AzureService>();
            var parquetFiles = GetParquetFilesFromBronzeZone();

            if (parquetFiles.Count == 0)
            {
                AnsiConsoleLogger.LogInfo("No parquet files found in the bronze zone.", _logger);
                return 0;
            }

            AnsiConsoleLogger.LogInfo($"Found [{parquetFiles.Count}] parquet files in the bronze zone.", _logger);

            AzureUpdatesClient azureUpdatesClient = new AzureUpdatesClient();
            CategoriesClient categoriesClient = new CategoriesClient();

            foreach (var item in parquetFiles)
            {
                AnsiConsoleLogger.LogInfo($"Processing file [{item.Name}], imported at [{item.CreationTime}].", _logger);

                var ph = new ParquetHandler();
                var feeds = ph.ReadRssFeedsFromParquetFile(item.FullName);

                //foreach (var feed in feeds)
                //{
                //    var categories = feed.Categories.Split(",").Select(c => c.Trim()).ToList();

                //    var t = categories
                //        .Select(c => new AzureService() 
                //        { 
                //            Id = c.ToLowerInvariant(),
                //            Name = c 
                //        });

                //    azureServices.AddRange(t);
                //}
                
                //var filteredAzureServices = azureServices.Distinct().ToList();

                foreach (var feed in feeds)
                {
                    AzureUpdateEntity service = new AzureUpdateEntity
                    {
                        Id = feed.Id,
                        Title = feed.Title,
                        Description = feed.Description,
                        Url = feed.Link,
                        DatePublished = DateTime.Parse(feed.PublishingDateUtc)
                    };

                    try
                    {
                        await azureUpdatesClient.InsertAzureUpdateSP(service);
                    }
                    catch (Exception ex)
                    {
                        AnsiConsoleLogger.LogWarning($"Error inserting update [{service.Id}]: {ex.Message}");
                    }

                    var categories = feed.Categories.Split(",").Select(c => c.Trim()).ToList();
                    foreach (var category in categories)
                    {
                        var categoryEntity = new CategoryEntity
                        {
                            Title = category
                        };

                        categoryEntity.Id = await categoriesClient.InsertAzureUpdateSP(categoryEntity);

                        await azureUpdatesClient.InsertAzureUpdateCategory(service, categoryEntity);
                    }
                }

                //ph.WriteAzureServicesToParquetFile(Path.Combine(ImporterContext.SilverPath.FullName, "services.parguet"), filteredAzureServices);

                //var newFileName = $"{item.FullName}.backup";
                //item.MoveTo(newFileName);
                //AnsiConsoleLogger.LogInfo($"Moved file to [{newFileName}]", _logger);
            }

            return 0;
        }

        private List<FileInfo> GetParquetFilesFromBronzeZone()
        {
            var directory = new DirectoryInfo(ImporterContext.BronsePath.FullName);
            _logger.LogInformation("Reading files from source directory [{directory}]", directory);

            var files = directory.GetFiles("*.parquet", SearchOption.TopDirectoryOnly).ToList();
            _logger.LogInformation($"Found [{files.Count}] parquet files in the landing folder.");

            return files;
        }
    }
}
