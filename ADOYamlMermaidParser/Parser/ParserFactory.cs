using ADOYamlMermaidParser.Model;

namespace ADOYamlMermaidParser.Parser;

public class ParserFactory
{
    public static List<string> ValidSteps { get; } = [ "bash", "checkout", "download",
                                                        "downloadBuild", "getPackage",
                                                        "powershell", "publish", "pwsh",
                                                        "reviewApp", "script", "task", 
                                                        "template"  ];
    public static Parser CreateParser(string parseType,
                                        Dictionary<object,object> yaml, 
                                        string prefix, 
                                        int depth,
                                        List<ADOVariable>? variables = null,
                                        List<ADOParameter>? parameters = null)
    {
        return parseType.ToLower() switch
        {
            "pipeline" => CreatePipelineParser(yaml, prefix, depth, variables, parameters),
            "stage" => new StageParser(yaml, prefix, depth, variables, parameters),
            "job" => new JobParser(yaml, prefix, depth, variables, parameters),
            "step" => CreateStepParser(yaml, prefix, depth, variables, parameters),
            _ => new ErroredParser(yaml, prefix, depth, "Unknown parseType")
        };
    }
    public static Parser CreatePipelineParser(Dictionary<object,object> yaml, 
                                                string prefix, 
                                                int depth, 
                                                List<ADOVariable>? variables = null,
                                                List<ADOParameter>? parameters = null)
    {
        if(yaml.ContainsKey("stages"))
        {
            // Pipeline parser.
            return new PipelineParser(yaml, prefix, depth, parameters);
        }
        else if(yaml.ContainsKey("jobs"))
        {
            // Stage parser.
            return new StageParser(yaml, prefix, depth, variables, parameters);
        }
        else if(yaml.ContainsKey("steps"))
        {
            // Job parser.
            return new JobParser(yaml, prefix, depth, variables, parameters);
        }
        return new ErroredParser(yaml, prefix, depth, "Unspoorted pipeline type");
    }
    public static Parser CreateStepParser(Dictionary<object,object> yaml, 
                                            string prefix, 
                                            int depth, 
                                            List<ADOVariable>? variables = null,
                                            List<ADOParameter>? parameters = null)
    {
        // Check if valid step and get valid type.
        var stepType = (string?)yaml.Keys.FirstOrDefault(k => ValidSteps.Contains(k));

        if(stepType == null)
        {
            return new ErroredParser(yaml, prefix, depth, $"step type not found in {prefix}");
        }
        else if(stepType == "template")
        {
            return new TemplateParser(yaml,prefix,depth,variables, parameters);
        }
        else
        {
            return new StepParser(yaml,prefix,depth,stepType.ToLower(),variables, parameters);
        }
    }
}