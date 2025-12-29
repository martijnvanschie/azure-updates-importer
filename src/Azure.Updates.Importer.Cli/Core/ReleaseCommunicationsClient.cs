using System.Text.Json;
using Azure.Updates.Importer.Data.DataModels;
using Microsoft.Extensions.Logging;

namespace Azure.Updates.Importer.Cli.Core
{
    public class ReleaseCommunicationsClient
    {
        private Uri _updatesUrl;
        private static readonly ILogger<ReleaseCommunicationsClient> _logger = LoggerManager.GetLogger<ReleaseCommunicationsClient>();
        private static readonly Settings _settings = ConfigurationManager.GetConfiguration().Settings;

        public ReleaseCommunicationsClient()
        {
            ReadUpdatesUrl();
        }

        private void ReadUpdatesUrl()
        {
            _updatesUrl = new Uri(_settings.ReleaseCommunicationsUrl);
        }

        public async Task<List<ReleaseCommunicationDto>> GetAllAzureUpdatesAsync(int? maxResults = 200, DateTime? modifiedSince = null)
        {
            int pageCount = 0;
            var allUpdates = new List<ReleaseCommunicationDto>();
            string nextUrl = _updatesUrl.AbsoluteUri;

            if (modifiedSince.HasValue)
            {
                string formattedDate = modifiedSince.Value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
                string filter = $"$filter=modified ge {formattedDate}";
                nextUrl = $"{nextUrl}?{filter}";
            }

            while (!string.IsNullOrEmpty(nextUrl) && allUpdates.Count < maxResults)
            {
                var response = await GetAzureUpdatesAsync(nextUrl);

                if (response.Value != null)
                {
                    _logger.LogInformation("Fetched {count} updates from current page", response.Value.Count);
                    allUpdates.AddRange(response.Value);
                }

                nextUrl = response.ODataNextLink;
                pageCount++;

                if (!string.IsNullOrEmpty(nextUrl))
                {
                    _logger.LogInformation("Fetching next page: {url} (Page {pageCount}, Total items: {totalItems})", 
                        nextUrl, pageCount + 1, allUpdates.Count);
                }
            }

            return allUpdates;
        }

        public async Task<ReleaseCommunicationsResponse> GetAzureUpdatesAsync(string? url = null, string? id = null)
        {
            try
            {
                using var httpClient = new HttpClient();

                // Build URL based on parameters
                string requestUrl;
                if (!string.IsNullOrEmpty(url))
                {
                    requestUrl = url;
                }
                else if (!string.IsNullOrEmpty(id))
                {
                    // Filter by specific ID using OData query
                    requestUrl = $"{_updatesUrl.AbsoluteUri}?$filter=id eq '{id}'";
                }
                else
                {
                    requestUrl = _updatesUrl.AbsoluteUri;
                }

                _logger.LogInformation("Calling Azure Updates API: {url}", requestUrl);

                var response = await httpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();

                var jsonContent = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var azureUpdates = JsonSerializer.Deserialize<ReleaseCommunicationsResponse>(jsonContent, options);

                if (!string.IsNullOrEmpty(id))
                {
                    _logger.LogInformation("Retrieved Azure update with ID: {id}", id);
                }
                else
                {
                    _logger.LogInformation("Successfully retrieved {count} Azure updates", azureUpdates?.Value?.Count ?? 0);
                }

                return azureUpdates ?? new ReleaseCommunicationsResponse();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to retrieve Azure updates from API");
                throw;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to deserialize Azure updates response");
                throw;
            }
        }

        // Optional: Convenience method to get a single update by ID
        public async Task<ReleaseCommunicationDto?> GetAzureUpdateByIdAsync(string id)
        {
            var response = await GetAzureUpdatesAsync(id: id);
            return response.Value?.FirstOrDefault();
        }
    }

}