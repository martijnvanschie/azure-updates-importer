using Azure.Updates.Importer.Cli.Core;
using Azure.Updates.Importer.Cli.Model;
using CodeHollow.FeedReader;
using Microsoft.Extensions.Logging;
using Spectre.Console;
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

        public StatusContext StatusContext { get; set; }

        private void ReadUpdatesUrl()
        {
            _updatesUrl = new Uri(_settings.UpdatesUrl);
        }

        public async Task<int> RunAsync()
        {
            _logger.LogInformation("Main process started.");

            StatusContext?.Status("Calling RSS endpoint");
            var feeds = await FeedReader.ReadAsync(_updatesUrl.AbsoluteUri);
            _logger.LogInformation("Feed count: {count}", feeds.Items.Count);

            var lastImportDate = await GetLastimportTimeAsync();
            _logger.LogInformation("Last import date was [{lastImportDate}]", lastImportDate);

            var selectedFeeds = feeds.Items
                .Where(feed => feed.PublishingDate > lastImportDate)
                .Select(feed => new RssFeed(feed))
                .ToList();

            _logger.LogInformation("Selected feeds count after fitlering on publishing date: {count}", selectedFeeds.Count);

            StatusContext?.Status("Writing parguet file");
            var path = Path.Combine(ImporterContext.LandingPath.FullName, $"{ImporterContext.DateImport:yyyyMMdd-HHmmss}-azureupdates.parquet");
            ParquetHandler ph = new ParquetHandler();
            ph.WriteParquetFile(path, selectedFeeds);
            AnsiConsoleLogger.LogSuccess($"Parquet file written to {path}");

            foreach (var feed in selectedFeeds)
            {
                _logger.LogTrace("Feed : [{id}] - {title}", feed.Id, feed.Title);
            }

            return 0;
        }

        internal async Task<DateTime> GetLastimportTimeAsync()
        {
            FileInfo fi = new FileInfo(FILENAME_LASTIMPORT);
            if (fi.Exists)
            {
                _logger.LogDebug("Last import file found [{file}]", fi.FullName);
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
