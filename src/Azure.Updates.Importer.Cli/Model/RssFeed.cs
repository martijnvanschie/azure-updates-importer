using CodeHollow.FeedReader;

namespace Azure.Updates.Importer.Cli.Model
{
    public class RssFeed
    {
        public RssFeed()
        {
            
        }
        public RssFeed(FeedItem feed)
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

            if (obj is RssFeed other)
                return Id == other.Id;

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
