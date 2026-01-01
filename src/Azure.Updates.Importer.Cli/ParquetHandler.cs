using Azure.Updates.Importer.Cli.Core;
using Azure.Updates.Importer.Cli.Model;
using Azure.Updates.Importer.Cli.Tasks;
using Microsoft.Extensions.Logging;
using ParquetSharp;

namespace Azure.Updates.Importer.Cli
{
    public class ParquetHandler
    {
        private static readonly ILogger<ParquetHandler> _logger = LoggerManager.GetLogger<ParquetHandler>();

        public List<ReleaseCommunicationItem> ReadRssFeedsFromParquetFile(string filePath)
        {
            var data = new List<ReleaseCommunicationItem>();

            using (var reader = new ParquetFileReader(filePath))
            {
                for (int rowGroupIndex = 0; rowGroupIndex < reader.FileMetaData.NumRowGroups; rowGroupIndex++)
                {
                    using (var rowGroupReader = reader.RowGroup(rowGroupIndex))
                    {
                        var groupNumRows = checked((int)rowGroupReader.MetaData.NumRows);

                        var ids = rowGroupReader.Column(0).LogicalReader<string>().ReadAll(groupNumRows);
                        var titles = rowGroupReader.Column(1).LogicalReader<string>().ReadAll(groupNumRows);
                        var desciprions = rowGroupReader.Column(2).LogicalReader<string>().ReadAll(groupNumRows);
                        var dates = rowGroupReader.Column(3).LogicalReader<string>().ReadAll(groupNumRows);
                        var links = rowGroupReader.Column(4).LogicalReader<string>().ReadAll(groupNumRows);
                        var categories = rowGroupReader.Column(5).LogicalReader<string>().ReadAll(groupNumRows);

                        for (int i = 0; i < ids.Length; i++)
                        {
                            data.Add(new ReleaseCommunicationItem
                            { 
                                Id = ids[i],
                                Title = titles[i],
                                Description = desciprions[i],
                                PublishingDateUtc = dates[i],
                                Link = links[i],
                                Categories = categories[i],
                            });
                        }
                    }
                }
            }

            return data;
        }

        public void WriteRawRssFeedsToParquetFile(string filePath, List<ReleaseCommunicationItem> feeds)
        {
            if (feeds.Count == 0)
            {
                AnsiConsoleLogger.LogDebug("No items in collection. No feeds to write to parquet file");
                return;
            }

            var schema = new Column[]
            {
                new Column<string>("Id"),
                new Column<string>("Title"),
                new Column<string>("Description"),
                new Column<string>("PublishingDateUtc"),
                new Column<string>("Link"),
                new Column<string>("Categories")
            };

            using (var writer = new ParquetFileWriter(filePath, schema))
            {
                using (var rowGroupWriter = writer.AppendRowGroup())
                {
                    // Write each column
                    using var ids = rowGroupWriter.NextColumn().LogicalWriter<string>();
                    ids.WriteBatch(feeds.ConvertAll(p => p.Id).ToArray());

                    using var titles = rowGroupWriter.NextColumn().LogicalWriter<string>();
                    titles.WriteBatch(feeds.ConvertAll(p => p.Title).ToArray());

                    using var descriptions = rowGroupWriter.NextColumn().LogicalWriter<string>();
                    descriptions.WriteBatch(feeds.ConvertAll(p => p.Description).ToArray());

                    using var dates = rowGroupWriter.NextColumn().LogicalWriter<string>();
                    dates.WriteBatch(feeds.ConvertAll(p => p.PublishingDateUtc).ToArray());

                    using var links = rowGroupWriter.NextColumn().LogicalWriter<string>();
                    links.WriteBatch(feeds.ConvertAll(p => p.Link).ToArray());

                    using var categories = rowGroupWriter.NextColumn().LogicalWriter<string>();
                    categories.WriteBatch(feeds.ConvertAll(p => p.Categories).ToArray());
                }
                writer.Close();
                _logger.LogInformation("Finished writing feeds to parquet file {filePath}", filePath);
            }
        }

        public List<AzureService> ReadAzureServicesFromParquetFile(string filePath)
        {
            var data = new List<AzureService>();

            using (var reader = new ParquetFileReader(filePath))
            {
                for (int rowGroupIndex = 0; rowGroupIndex < reader.FileMetaData.NumRowGroups; rowGroupIndex++)
                {
                    using (var rowGroupReader = reader.RowGroup(rowGroupIndex))
                    {
                        var groupNumRows = checked((int)rowGroupReader.MetaData.NumRows);

                        var ids = rowGroupReader.Column(0).LogicalReader<string>().ReadAll(groupNumRows);
                        var titles = rowGroupReader.Column(1).LogicalReader<string>().ReadAll(groupNumRows);

                        for (int i = 0; i < ids.Length; i++)
                        {
                            data.Add(new AzureService
                            {
                                Id = ids[i],
                                Name = titles[i]
                            });
                        }
                    }
                }
            }

            return data;
        }

        public void WriteAzureServicesToParquetFile(string filePath, List<AzureService> services)
        {
            if (services.Count == 0)
            {
                AnsiConsoleLogger.LogWarning("No services to write to parquet file");
                return;
            }

            var schema = new Column[]
            {
                new Column<string>("Id"),
                new Column<string>("Name")
            };

            using (var writer = new ParquetFileWriter(filePath, schema))
            {
                using (var rowGroupWriter = writer.AppendRowGroup())
                {
                    // Write each column
                    using var ids = rowGroupWriter.NextColumn().LogicalWriter<string>();
                    ids.WriteBatch(services.ConvertAll(p => p.Id).ToArray());

                    using var titles = rowGroupWriter.NextColumn().LogicalWriter<string>();
                    titles.WriteBatch(services.ConvertAll(p => p.Name).ToArray());
                }

                writer.Close();
            }
        }

    }
}
