namespace Azure.Updates.Importer.Cli.Model
{
    public class AzureService
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is null)
                return base.Equals(obj);

            if (obj is AzureService other)
                return Id == other.Id;

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
