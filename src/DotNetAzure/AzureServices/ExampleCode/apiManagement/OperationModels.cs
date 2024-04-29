namespace AzureServices.ExampleCode.apiManagement
{
    public class OperationModels
    {
        public List<OperationValue> Value { get; set; } = new();
        public int Count { get; set; } = 0;
    }

    public class OperationValue
    {
        public string Id { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public OperationProperties Properties { get; set; } = new();
        public string Headers { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
    }

    public class OperationTag
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    public class OperationProperties
    {
        public string DisplayName { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public string UrlTemplate { get; set; } = string.Empty;
        public List<OperationTemplateParameter> TemplateParameters { get; set; } = new();
        public string Description { get; set; } = string.Empty;
        public List<OperationResponse> Responses { get; set; } = new();
        //public object Policies { get; set; } = new();
        public OperationRequest Request { get; set; } = new();
    }

    public class OperationTemplateParameter
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool Required { get; set; }
        //public List<object> Values { get; set; } = new();
        public string SchemaId { get; set; } = string.Empty;
        public string TypeName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
    public class OperationRequest
    {
        public List<OperationQueryParameter> QueryParameters { get; set; } = new();
        //public List<object> Headers { get; set; } = new();
        public List<OperationRepresentation> Representations { get; set; } = new();
        public string Description { get; set; } = string.Empty;
    }

    public class OperationResponse
    {
        public int StatusCode { get; set; } = 0;
        public string Description { get; set; } = string.Empty;
        public List<OperationRepresentation> Representations { get; set; } = new();
        //public List<object> Headers { get; set; } = new();
    }

    public class OperationQueryParameter
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        //public List<object> Values { get; set; } = new();
        public string SchemaId { get; set; } = string.Empty;
        public string TypeName { get; set; } = string.Empty;
    }

    public class OperationRepresentation
    {
        public string ContentType { get; set; } = string.Empty;
        //public Examples Examples { get; set; } = new();
        public string SchemaId { get; set; } = string.Empty;
        public string TypeName { get; set; } = string.Empty;
        public string GeneratedSample { get; set; } = string.Empty;
        public string Sample { get; set; } = string.Empty;
        public List<OperationFormParameter> FormParameters { get; set; } = new();
    }
    public class OperationDefault
    {
        public OperationValue Value { get; set; } = new();
    }

    public class Examples
    {
        public OperationDefault @default { get; set; } = new();
    }

    public class OperationFormParameter
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        //public List<object> Values { get; set; } = new();
    }

    public class ApisOpenAI
    {
        public string Id { get; set; } = string.Empty;
        public string Format { get; set; } = string.Empty;
        public ApisOpenAIValue Value { get; set; } = new();
    }

    public class ApisOpenAIValue
    {
        public string Link { get; set; } = string.Empty;
    }
}
