namespace Azure.Updates.Importer.Data.DataModels
{
    public class TagEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset DateModified { get; set; }
    }
}