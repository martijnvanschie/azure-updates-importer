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
        private static readonly DateTime _dateImport = DateTime.Now;

        private const string FILENAME_LASTIMPORT = ".lastimport";

        private DirectoryInfo _outputPath;
        private DirectoryInfo _landngPath;
        private DirectoryInfo _bronsePath;
        private DirectoryInfo _silverPath;
        private DirectoryInfo _goldPath;

        private Uri _updatesUrl;

        public ImportTask()
        {
            ParseOutputPath();
            ReadUpdatesUrl();
        }

        private void ParseOutputPath()
        {
            _outputPath = new DirectoryInfo(_settings.OutputPath);
            _landngPath = new DirectoryInfo(Path.Combine(_outputPath.FullName, "landing"));
            _bronsePath = new DirectoryInfo(Path.Combine(_outputPath.FullName, "bronze"));
            _silverPath = new DirectoryInfo(Path.Combine(_outputPath.FullName, "silver"));
            _goldPath = new DirectoryInfo(Path.Combine(_outputPath.FullName, "gold"));

            if (_landngPath.Exists == false)
            {
                _landngPath.Create();
            }

            if (_bronsePath.Exists == false)
            {
                _bronsePath.Create();
            }

            if (_silverPath.Exists == false)
            {
                _silverPath.Create();
            }

            if (_goldPath.Exists == false)
            {
                _goldPath.Create();
            }
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

            var path = Path.Combine(_landngPath.FullName, $"{_dateImport:yyyyMMdd-HHmmss}-azureupdates.parquet");
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
