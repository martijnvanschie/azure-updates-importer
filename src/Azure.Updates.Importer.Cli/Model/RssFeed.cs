using Azure.Updates.Importer.Data.DataModels;
using CodeHollow.FeedReader;

namespace Azure.Updates.Importer.Cli.Model
{
    // public class ReleaseCommunicationItem
    // {
    //     public ReleaseCommunicationItem()
    //     {
            
    //     }
    // }

    public class AzureReleaseCommunicationItem : ReleaseCommunicationItem
    {
        public AzureReleaseCommunicationItem()
        {
            
        }
    }

    public class ReleaseCommunicationItem
    {
        public ReleaseCommunicationItem()
        {
            
        }

        public ReleaseCommunicationItem(ReleaseCommunicationDto dto)
        {
            Id = dto.Id;
            Title = dto.Title;
            Description = dto.Description;
            PublishingDateUtc = dto.Created.ToString();
            Link = string.Format("https://azure.microsoft.com/en-us/updates?id={0}", dto.Id);
            
            // Merge productCategories, tags, and products with comma separator
            var allCategories = new List<string>();
            if (dto.ProductCategories != null) allCategories.AddRange(dto.ProductCategories);
            if (dto.Tags != null) allCategories.AddRange(dto.Tags);
            if (dto.Products != null) allCategories.AddRange(dto.Products);
            
            Categories = string.Join(", ", allCategories);
        }

        public ReleaseCommunicationItem(FeedItem feed)
        {
            Id = feed.Id;
            Title = feed.Title;
            Description = feed.Description;
            PublishingDateUtc = feed.PublishingDateString;
            Link = feed.Link;
            Categories = string.Join(", ", feed.Categories);
        }

        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PublishingDateUtc { get; set; }
        public string Link { get; set; }
        public string Categories { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is null)
                return base.Equals(obj);

            if (obj is ReleaseCommunicationItem other)
                return Id == other.Id;

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
