using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Azure.Updates.Importer.Data.DataModels
{
    public class ReleaseCommunicationsResponse
    {
        [JsonPropertyName("@odata.context")]
        public string ODataContext { get; set; } = string.Empty;

        [JsonPropertyName("@odata.nextLink")]
        public string ODataNextLink { get; set; } = string.Empty;

        [JsonPropertyName("value")]
        public List<ReleaseCommunicationDto> Value { get; set; } = new List<ReleaseCommunicationDto>();
    }

    public class ReleaseCommunicationDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;

        [JsonPropertyName("created")]
        public DateTime Created { get; set; }

        // [JsonPropertyName("modified")]
        // public DateTime Modified { get; set; }

        // [JsonPropertyName("published")]
        // public DateTimeOffset Published { get; set; }

        [JsonPropertyName("categories")]
        public List<string> Categories { get; set; } = new List<string>();

        [JsonPropertyName("products")]
        public List<string> Products { get; set; } = new List<string>();

        [JsonPropertyName("productCategories")]
        public List<string> ProductCategories { get; set; } = new List<string>();

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("featured")]
        public bool Featured { get; set; }

        [JsonPropertyName("impactArea")]
        public string ImpactArea { get; set; } = string.Empty;

        [JsonPropertyName("availabilities")]
        public List<AvailabilityDto>? Availabilities { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        public string? GeneralAvailabilityDate { get; set; }
        public string? PreviewAvailabilityDate { get; set; }
        public string? PrivatePreviewAvailabilityDate { get; set; }
    }

    public class AvailabilityDto
    {
        [JsonPropertyName("ring")]
        public string Ring { get; set; }

        [JsonPropertyName("year")]
        public int Year { get; set; }

        [JsonPropertyName("month")]
        public string Month { get; set; }
    }
}