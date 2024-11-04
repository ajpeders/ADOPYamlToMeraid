using ADOYamlMermaidParser.Model;

namespace ADOYamlMermaidParser.Parser;

public abstract class Parser
{
    public Dictionary<object,object>? Obj { get; set; } = null;
    public string DisplayName { get; set; } = string.Empty;
    public string MermaidId { get; set; } = string.Empty;
    public int Depth { get; set; }
    public string Type { get; set; } = string.Empty;
    public List<ADOVariable> Variables = [];
    public List<ADOParameter> Parameters = [];
    public virtual string Parse()
    {
        return $"{new string('\t',Depth)}{MermaidId}[\"{Type}: {DisplayName}\"]";
    }
}