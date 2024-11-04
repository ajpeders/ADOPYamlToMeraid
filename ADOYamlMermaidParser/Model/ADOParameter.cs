namespace ADOYamlMermaidParser.Model
{
    public class ADOParameter
    {
        public string? Name { get; set; } //name of the parameter; required
        public string? DisplayName { get; set; } // Human-readable name for the parameter.
        public string? Type { get; set; }
        public object? Default { get; set; } // Can be string, parameters, or a list of parameters.
        public List<object>? Values { get; set; } // List of possible values for the parameter.
        
        public string SimpleParse()
        {
            return $"{Name ?? "Unknown"} : {Default?.ToString() ?? "No Default"}";
        }
    }
}