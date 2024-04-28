namespace DotNetAzure.UI.Models
{
    public class CosmosModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }


        public CosmosModel(string? name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }
    }
}
