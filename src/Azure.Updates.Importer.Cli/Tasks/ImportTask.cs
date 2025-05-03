using Azure.Updates.Importer.Cli.Core;
using Azure.Updates.Importer.Cli.Model;
using CodeHollow.FeedReader;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Azure.Updates.Importer.Cli.Tasks
{
    public class ImportTask : ITask
    {
        private static readonly ILogger<ImportTask> _logger = LoggerManager.GetLogger<ImportTask>();
        private static readonly Settings _settings = ConfigurationManager.GetConfiguration().Settings;
        private const string FILENAME_LASTIMPORT = ".lastimport";

        private Uri _updatesUrl;

        public ImportTask()
        {
            ReadUpdatesUrl();
        }

        private void ReadUpdatesUrl()
        {
            _updatesUrl = new Uri(_settings.UpdatesUrl);
        }

        public async Task<int> RunAsync()
        {
            _logger.LogInformation("Main process started.");

            var feeds = await FeedReader.ReadAsync(_updatesUrl.AbsoluteUri);

            var lastImportDate = await GetLastimportTimeAsync();
            _logger.LogInformation("Last import date was [{lastImportDate}]", lastImportDate);

            var selectedFeeds = feeds.Items
                .Where(feed => feed.PublishingDate > lastImportDate)
                .Select(feed => new RssFeed(feed))
                .ToList();

            var path = Path.Combine(ImporterContext.LandingPath.FullName, $"{ImporterContext.DateImport:yyyyMMdd-HHmmss}-azureupdates.parquet");
            ParquetHandler ph = new ParquetHandler();
            ph.WriteParquetFile(path, selectedFeeds);

            foreach (var feed in selectedFeeds)
            {
                _logger.LogInformation("Feed : [{id}] - {title}", feed.Id, feed.Title);
            }

            return 0;
        }

        internal async Task<DateTime> GetLastimportTimeAsync()
        {
            FileInfo fi = new FileInfo(FILENAME_LASTIMPORT);
            if (fi.Exists)
            {
                var dateString = await File.ReadAllTextAsync(fi.FullName, Encoding.UTF8);
                return DateTime.ParseExact(dateString, "R", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeUniversal);
            }

            return DateTime.Now;
        }

        internal async Task WriteLastImportTimeAsync(DateTime date)
        {
            string formattedDate = date.ToUniversalTime().ToString("R", System.Globalization.CultureInfo.InvariantCulture);
            await File.WriteAllTextAsync(FILENAME_LASTIMPORT, formattedDate);
        }
    }
}
