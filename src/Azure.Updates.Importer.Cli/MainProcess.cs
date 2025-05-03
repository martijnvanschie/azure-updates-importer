using Azure.Updates.Importer.Cli.Model;
using CodeHollow.FeedReader;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using System.Text;

namespace Azure.Updates.Importer.Cli
{
    internal class MainProcess
    {
        private readonly ILogger<MainProcess> _logger;
        private readonly IConfiguration _configuration;
        private readonly DateTime _dateImport = DateTime.Now;

        private DirectoryInfo _outputPath;
        private string _updatesUrl;

        private const string FILENAME_LASTIMPORT = ".lastimport";

        public MainProcess(ILogger<MainProcess> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

            ParseOutputPath();
            ReadUpdatesUrl();
        }

        private void ParseOutputPath()
        {
            var path = _configuration["Settings:OutputPath"];
            _outputPath = new DirectoryInfo(path);
        }

        private void ReadUpdatesUrl()
        {
            _updatesUrl = _configuration["Settings:UpdatesUrl"];
        }

        internal async Task<int> RunAsync(string[] args)
        {
            _logger.LogInformation("Main process started {argsInfo}",
                args.Length == 0 ? "without arguments" : $"with arguments: {string.Join(", ", args)}");

            var feeds = await FeedReader.ReadAsync(_updatesUrl);

            var lastImportDate = await GetLastimportTimeAsync();
            _logger.LogInformation("Last import date was [{lastImportDate}]", lastImportDate);

            var selectedFeeds = feeds.Items
                .Where(feed => feed.PublishingDate > lastImportDate)
                .Select(feed => new RssFeed(feed))
                .ToList();

            ParquetHandler ph = new ParquetHandler();
            ph.WriteParquetFile($"c:\\TEMP\\{_dateImport.ToString("yyyyMMdd-HHmmss")}-azureupdates.parquet", selectedFeeds);

            //var test = ph.ReadParquetFile($"c:\\TEMP\\{_dateImport.ToString("yyyyMMdd-HHmmss")}-azureupdates.parquet");
            //var test2 = ph.ReadParquetFile($"c:\\TEMP\\{_dateImport.ToString("yyyyMMdd-HHmmss")}-azureupdates.parquet");

            //var test3 = test.Concat(test2).ToList();
            //var test4 = test.Concat(test2).Distinct().ToList();

            foreach (var feed in selectedFeeds)
            {
                _logger.LogInformation("Feed : [{id}] - {title}", feed.Id, feed.Title);
            }

            return Environment.ExitCode;
        }

        internal async Task<DateTime> GetLastimportTimeAsync()
        {
            FileInfo fi = new FileInfo(FILENAME_LASTIMPORT);
            if (fi.Exists)
            {
                var dateString = await File.ReadAllTextAsync(fi.FullName, Encoding.UTF8);

                // Parse the date string using the RFC 1123 format
                return DateTime.ParseExact(dateString, "R", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeUniversal);
            }

            return DateTime.Now;
        }

        internal async Task WriteLastImportTimeAsync(DateTime date)
        {
            // Convert the date to UTC and format it in RFC 1123 format
            string formattedDate = date.ToUniversalTime().ToString("R", System.Globalization.CultureInfo.InvariantCulture);

            // Write the formatted date to the file
            await File.WriteAllTextAsync(FILENAME_LASTIMPORT, formattedDate);
        }
    }
}