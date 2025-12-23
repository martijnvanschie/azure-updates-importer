using System;

namespace Azure.Updates.Importer.Data.DataModels
{
    public class AzureUpdateEntity
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Url { get; set; }
        public DateTimeOffset DatePublished { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset DateModified { get; set; }
    }
}