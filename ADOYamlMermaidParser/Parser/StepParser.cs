using ADOYamlMermaidParser.Model;

namespace ADOYamlMermaidParser.Parser;

public class StepParser : Parser
{
    public StepParser(Dictionary<object,object> step, 
                        string prefix, 
                        int depth,
                        string stepType,
                        List<ADOVariable>? variables = null,
                        List<ADOParameter>? parameters = null)
    {
        Type = stepType;
        Obj = step;
        DisplayName = ParserHelper.GetDisplayName(Type,Obj,variables, parameters);
        MermaidId = ParserHelper.GetMermaidId(prefix, DisplayName);
        Depth = depth;
        Variables = variables ?? [];
    }
}