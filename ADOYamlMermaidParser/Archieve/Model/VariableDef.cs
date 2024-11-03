namespace ADOYamlMermaidParser.Model;

public class VariableDef
{
    public string? Group { get; set;} // Required as first property. Variable group name.   
     public string? Name { get; set; } // Variable name (required as the first property).
    public string? Value { get; set; } // Variable value.
    public bool? ReadOnly { get; set; } // Whether a YAML variable is read-only; default is false.
}