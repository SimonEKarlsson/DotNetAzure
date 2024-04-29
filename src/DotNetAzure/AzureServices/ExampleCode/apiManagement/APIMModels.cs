namespace AzureServices.ExampleCode.apiManagement
{
    public class APIMModels
    {
        public List<APIMValue> value { get; set; } = new List<APIMValue>();
        public int count { get; set; } = 0;
        public string nextLink { get; set; } = string.Empty;
    }

    public class APIMValue
    {
        public string id { get; set; } = string.Empty;
        public string type { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public APIMProperties properties { get; set; } = new APIMProperties();
    }
    public class APIMProperties
    {
        public string displayName { get; set; } = string.Empty;
        public string apiRevision { get; set; } = string.Empty;
        public string serviceUrl { get; set; } =string.Empty;
        public string path { get; set; } = string.Empty;
        public List<string> protocols { get; set; } = new List<string>();
        public bool isCurrent { get; set; } 
        public string apiVersionSetId { get; set; } = string.Empty;
        public string apiVersion { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
    }


}
