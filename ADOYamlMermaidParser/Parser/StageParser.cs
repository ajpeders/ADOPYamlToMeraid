using ADOYamlMermaidParser.Model;

namespace  ADOYamlMermaidParser.Parser;
public class StageParser : Parser
{
    public List<Parser> Jobs { get; set; } = new List<Parser>();
    public StageParser(Dictionary<object,object> stage, 
                        string prefix, 
                        int depth,
                        List<ADOVariable>? variables = null,
                        List<ADOParameter>? parameters = null)
    {
        Type = "stage";
        Obj = stage;
        Depth = depth;
        Variables = ParserHelper.GetAndUpdateVariables(stage, variables ?? []);
        Parameters = ParserHelper.GetAndUpdateParameters(stage, parameters ?? []);
        DisplayName = ParserHelper.GetDisplayName(Type, Obj, Variables, Parameters);
        MermaidId = ParserHelper.GetMermaidId(prefix, DisplayName);
        SetJobs();
    }

    private void SetJobs()
    {
        Jobs = new List<Parser>();
        if(Obj == null || !Obj.TryGetValue("jobs", out var jobs))
        {
            return;
        }
        if(jobs is not List<object> jobList)
        {
            return;
        }
        for(int i = 0; i < jobList.Count; i++)
        {
            if(jobList[i] is Dictionary<object,object> jobDict)
            {
                var newParser = ParserFactory.CreateParser("job", jobDict, $"{MermaidId}_{i}", Depth+1, Variables, Parameters);
                Jobs.Add(newParser);
            }
            else
            {
                var erroredParser = new ErroredParser(new(), $"{MermaidId}_{i}", Depth+1,  $"Job is not a dict in {MermaidId}");
                Jobs.Add(erroredParser);
            }
        }
    }

    public override string Parse()
    {
        string mermaidOutput = string.Empty;
        string tabs = new string('\t', Depth);
        for(int i = 0; i < Jobs.Count; i++)
        {
            var job = Jobs[Jobs.Count - 1 - i];
            mermaidOutput += $"{tabs}subgraph {job.MermaidId}[\"job: {job.DisplayName}\"]\n";
            mermaidOutput += $"{tabs}direction LR\n";
            mermaidOutput += job.Parse();
            mermaidOutput += $"{tabs}end\n";
        }

          // Add legend.
        var varDict = Variables.ToDictionary(v => v.Name ?? string.Empty, v => v.Value ?? string.Empty);
        mermaidOutput += ParserHelper.GetLegendMermaidStr(MermaidId, "Variables", varDict, Depth);
        var paramDict = Parameters.ToDictionary(p => p.DisplayName ?? p.Name ?? string.Empty, p => (string)(p.Default ?? string.Empty));
        mermaidOutput += ParserHelper.GetLegendMermaidStr(MermaidId, "Parameters", paramDict , Depth);
        return mermaidOutput;
    }
}