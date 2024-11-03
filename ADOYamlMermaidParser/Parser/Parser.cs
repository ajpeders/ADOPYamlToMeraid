using ADOYamlMermaidParser.Model;

namespace ADOYamlMermaidParser.Parser;

public abstract class Parser
{
    public Dictionary<string, object> Yaml { get; set; }
    public string? DisplayName { get; set; }
    public string? MermaidId { get; set; }
    public abstract string Parse();
}