using ADOYamlMermaidParser.Model;
using YamlDotNet.Core.Tokens;

namespace ADOYamlMermaidParser.Parser;

public class JobParser : Parser
{
    public List<Parser> Steps { get; set; } = [];
    public JobParser(Dictionary<object,object> job, 
                        string prefix, 
                        int depth,
                        List<ADOVariable>? variables = null,
                        List<ADOParameter>? parameters = null)
    {
        Type = "job";
        Obj = job;
        Depth = depth;
        DisplayName = ParserHelper.GetDisplayName(Type,Obj, variables, parameters);
        MermaidId = ParserHelper.GetMermaidId(prefix, DisplayName);
        Variables = ParserHelper.GetVariables(Obj);
        Parameters = parameters ?? [];
        SetJobSteps();
    }

    private void SetJobSteps()
    {
        Steps = [];
        // No steps in job.
        if (Obj == null || !Obj.TryGetValue("steps", out var steps))
        {
            return;
        }
        // Steps is not a list.
        if(steps is not List<object> stepList)
        {
            return;
        }
        // Loop through all the steps.
        for(int i = 0; i < stepList.Count; i++)
        {
            // Ensure step is a map.
            if(stepList[i] is Dictionary<object,object> stepDict)
            {
                var newParser = ParserFactory.CreateParser("step", stepDict, $"{MermaidId}_{i}", Depth, Variables, Parameters);
                Steps.Add(newParser);
            }
            else
            {
                var erroredParser = new ErroredParser(new(), $"{MermaidId}_{i}", Depth,  $"Step is not a dict in {MermaidId}");
                Steps.Add(erroredParser);
            }
        }
    }
    public override string Parse()
    {
        string mermaidOutput = ParserHelper.GetStepsStr(Steps, Depth);
        
        // Add legend.
        var varDict = Variables.ToDictionary(v => v.Name ?? string.Empty, v => v.Value ?? string.Empty);
        mermaidOutput += ParserHelper.GetLegendMermaidStr(MermaidId, "Variables", varDict, Depth);
        var paramDict = Parameters.ToDictionary(p => p.DisplayName ?? p.Name ?? string.Empty, p => (string)(p.Default ?? string.Empty));
        mermaidOutput += ParserHelper.GetLegendMermaidStr(MermaidId, "Parameters", paramDict , Depth);
        return mermaidOutput;
    
    }
}