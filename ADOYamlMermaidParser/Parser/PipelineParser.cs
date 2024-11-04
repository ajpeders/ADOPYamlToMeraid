using ADOYamlMermaidParser.Model;

namespace ADOYamlMermaidParser.Parser;

public class PipelineParser : Parser
{
    List<Parser> Stages { get; set; } = [];
    public PipelineParser(Dictionary<object,object> pipeline, 
                            string prefix, 
                            int depth,
                            List<ADOParameter>? parameters = null)
    {
        Type = "pipeline";
        Obj = pipeline;
        Depth = depth;
        Variables = ParserHelper.GetVariables(pipeline);
        Parameters = ParserHelper.UpdateParameters(ParserHelper.GetParameters(pipeline), parameters ?? []);
        DisplayName = ParserHelper.GetDisplayName(Type, Obj, Variables, Parameters);
        MermaidId = ParserHelper.GetMermaidId(prefix, DisplayName);
        SetStages();
    }

    public void SetStages()
    {
        Stages = [];
        if(Obj == null || !Obj.TryGetValue("stages", out var stages))
        {
            return;
        }
        if(stages is not List<object> stageList)
        {
            return;
        }
        for(int i = 0; i < stageList.Count; i++)
        {
            if(stageList[i] is Dictionary<object,object> stageDict)
            {
                var newParser = ParserFactory.CreateParser("stage", stageDict, $"{MermaidId}_{i}", Depth+1, Variables, Parameters);
                Stages.Add(newParser);
            }
            else
            {
                var erroredParser = new ErroredParser(new(), $"{MermaidId}_{i}", Depth+1,  $"Stage is not a dict in {MermaidId}");
                Stages.Add(erroredParser);
            }
        }
    }
    public override string Parse()
    {
        string mermaidOutput = string.Empty;
        string tabs = new string('\t', Depth);
        for(int i = 0; i < Stages.Count; i++)
        {
            var stage = Stages[Stages.Count - 1 - i];
            mermaidOutput += $"{tabs}subgraph {stage.MermaidId}[\"stage: {stage.DisplayName}\"]\n";
            mermaidOutput += $"{tabs}direction LR\n";
            mermaidOutput += stage.Parse();
            mermaidOutput += $"{tabs}end\n";
        }

        Parser? prev_stage = null;
        for(int i = 0; i < Stages.Count; i++)
        {
            var stage = Stages[i];
            if(prev_stage != null)
            {
                mermaidOutput += $"{prev_stage.MermaidId} --> {stage.MermaidId}\n";
            }
            prev_stage = stage;
        }

          // Add legend.
        var varDict = Variables.ToDictionary(v => v.Name ?? string.Empty, v => v.Value ?? string.Empty);
        mermaidOutput += ParserHelper.GetLegendMermaidStr(MermaidId, "Variables", varDict, Depth);
        var paramDict = Parameters.ToDictionary(p => p.DisplayName ?? p.Name ?? string.Empty, p => (string)(p.Default ?? string.Empty));
        mermaidOutput += ParserHelper.GetLegendMermaidStr(MermaidId, "Parameters", paramDict , Depth);
        return mermaidOutput;    
    }
}