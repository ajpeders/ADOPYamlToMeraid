namespace ADOYamlMermaidParser.Parser;

public class ErroredParser : Parser
{
    public object? Yaml { get; set; }
    public ErroredParser(object? yaml, string prefix, int depth, string msg = "")
    {
        Type = "error";
        Yaml = yaml;
        DisplayName = $"Error init parser in {prefix} [{msg}]";
        MermaidId = ParserHelper.GetMermaidId(prefix,"error");
        Depth = depth;
    }
}