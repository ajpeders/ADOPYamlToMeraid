using ADOYamlMermaidParser.Model.Step;

namespace ADOYamlMermaidParser.Parser;

public class PipelineParser : Parser
{
    public List<Parser> Instructions { get; set;}
    public PipelineParser(Dictionary<string, object> pipeline, string prefix)
    {
        base.Yaml = pipeline;
        string name = Yaml.TryGetValue("`name", out object? value) ? (string)value : "";
        base.DisplayName = "PL"+name;
        base.MermaidId = Helpers.GetMermaidId(prefix, DisplayName);
        Instructions = GetInstructions();
    }

    public List<Dictionary<string,object>>? GetSteps()
    {
        var steps = Yaml.ContainsKey("steps") ? Yaml["steps"] : null;
        if(steps is null)
        {
            return null;
        }
        if(steps is List<object> objSteps)
        {
            return objSteps
                .Where(s => s is Dictionary<object,object>)
                .Select(Helpers.FormatYamlDictionary)
                .ToList();
        }
        return null;
    }

    public List<Parser> GetInstructions()
    {
        var steps = GetSteps();
        Instructions = new();
        for(int i = 0; i < steps.Count(); i++)
        {
            Instructions.Add(ParserFactory.CreateParser("step", steps[i], MermaidId+"_i"));
        }
        return Instructions;   
    }

    public override string Parse()
    {
        string mermaid_output = string.Empty;
        Parser? prev_ins = null; 
        for(int i = 0; i < Instructions.Count(); i++)
        {
            var ins = Instructions[i];
            mermaid_output += ins.Parse()+'\n';
            if(prev_ins != null)
            {
                mermaid_output += $"{prev_ins.MermaidId} --> {ins.MermaidId}\n";
            }
            prev_ins = ins;
        } 
        return mermaid_output;
    }

    /*
    if(pipeline is PipelineImpJobDef impJobPl)
    {

    }
    else
    {
        DisplayName = "PL";
        MermaidId = Helpers.GetMermaidId(prefix, DisplayName);
    }
}*/
}