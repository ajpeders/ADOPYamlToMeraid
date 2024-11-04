using ADOYamlMermaidParser.Model;
using YamlDotNet.Serialization;

namespace ADOYamlMermaidParser.Parser;
public class TemplateParser : Parser
{
    public string FilePath { get; set; } = string.Empty;
    public Parser? Template { get; set; }
    public List<ADOParameter>? PassedParameters { get; set; }

    public TemplateParser(Dictionary<object, object> template, 
                        string prefix, 
                        int depth, 
                        List<ADOVariable>? variables = null, 
                        List<ADOParameter>? parameters = null)
    {
        Type = "template";
        Obj = template;
        Depth = depth;
        Variables = variables ?? [];
        Parameters = parameters ?? [];;
        DisplayName = ParserHelper.GetDisplayName(Type, Obj, Variables, Parameters);
        MermaidId = ParserHelper.GetMermaidId(prefix, DisplayName);
        FilePath = Obj.TryGetValue("template", out object? templateFileObj) ? (string)templateFileObj ?? string.Empty : string.Empty;
        PassedParameters = ParserHelper.GetParameters(Obj);
        GetSetTemplate();
    }

    private void GetSetTemplate()
    {
        try
        {
            string fileContent = File.ReadAllText($"../pipelines/{FilePath}");
            var deserializer = new DeserializerBuilder().Build();
            var yaml = deserializer.Deserialize<Dictionary<object, object>>(fileContent);
            if(yaml == null)
            {
                DisplayName += "(Error: Template file is empty)";
                return;
            }
            Template = ParserFactory.CreateParser("pipeline", yaml, MermaidId, Depth+1, null, PassedParameters);
        }
        catch (Exception ex)
        {
            DisplayName += $"(Error: {ex.Message})";
        }
    }

    public override string Parse()
    {
        var tabs = new string('\t', Depth);
        string mermaidOutput = $"{tabs}subgraph {MermaidId}[\"Template: {DisplayName}\"]\n";
        if (Template != null)
        {
            mermaidOutput += Template.Parse();
        }
        else
        {
            mermaidOutput += $"{tabs}%% Error: Template is null\n";
        }
        mermaidOutput += $"{tabs}end\n";
        return mermaidOutput;
    
    }
}
