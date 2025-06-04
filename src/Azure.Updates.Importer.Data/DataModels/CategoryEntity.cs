namespace Azure.Updates.Importer.Data.DataModels
{
    public class CategoryEntity
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset DateModified { get; set; }
    }
}
