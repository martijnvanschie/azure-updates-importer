using System;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace Azure.Updates.Importer.Cli.Core;

public class Utils
{
    private static readonly ILogger<Utils> _logger = LoggerManager.GetLogger<Utils>();
    private const string FILENAME_LASTIMPORT = ".lastimport";
    
        static internal async Task<DateTime> GetLastimportTimeAsync()
        {
            FileInfo fi = new FileInfo(FILENAME_LASTIMPORT);
            if (fi.Exists)
            {
                _logger.LogDebug("Last import file found [{file}]", fi.FullName);
                var dateString = await File.ReadAllTextAsync(fi.FullName, Encoding.UTF8);
                return DateTime.ParseExact(dateString, "R", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeUniversal);
            }

            var dt = DateTime.Now.AddMonths(-6);
            _logger.LogDebug($"Last import file not found. Date set to [{dt}]");
            return dt;
        }

        static internal async Task WriteLastImportTimeAsync(DateTime date)
        {
            string formattedDate = date.ToUniversalTime().ToString("R", System.Globalization.CultureInfo.InvariantCulture);
            await File.WriteAllTextAsync(FILENAME_LASTIMPORT, formattedDate);
        }

        static internal string StripHtml(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return string.Empty;

            // Remove HTML tags
            string text = Regex.Replace(html, "<.*?>", string.Empty);
            
            // Decode HTML entities (like &nbsp;, &amp;, etc.)
            text = System.Net.WebUtility.HtmlDecode(text);
            
            // Clean up extra whitespace
            text = Regex.Replace(text, @"\s+", " ").Trim();
            
            return text;
        }
}
