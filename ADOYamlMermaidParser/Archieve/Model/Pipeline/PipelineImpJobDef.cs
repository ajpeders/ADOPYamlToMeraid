using System.Linq;
using ADOYamlMermaidParser.Model.Step;

namespace ADOYamlMermaidParser.Model.Pipeline;

public class PipelineImpJobDef : PipelineDef
{
    // Required properties
    public List<StepDef> Steps { get; set; } // A list of steps to run in this job (can be task, script, powershell, etc.).
    // Optional properties
    public string? Strategy { get; set; } // Execution strategy for this job.
    public string? ContinueOnError { get; set; } // Continue running even on failure?
    public object? Pool { get; set; } // Pool where jobs in this pipeline will run unless otherwise specified (can be string or pool).
    public object? Container { get; set; } // Container resource name (can be string or container).
    public object? Services { get; set; } // Container resources to run as a service container (name/value pairs).
    public object? Workspace { get; set; } // Workspace options on the agent.
    public bool? AppendCommitMessageToRunName { get; set; } // Append the commit message to the build number. Default is true.
    public object? Trigger { get; set; } // Continuous integration triggers (can be none, trigger, or list of strings).
    public object? Parameters { get; set; } // Pipeline template parameters.
    public object? Pr { get; set; } // Pull request triggers (can be none, pr, or list of strings).
    public object? Schedules { get; set; } // Scheduled triggers (cron expressions).
    public object? Resources { get; set; } // Containers and repositories used in the build.
    public object? Variables { get; set; } // Variables for this pipeline (can be a dictionary or list of variable objects).
    public string? LockBehavior { get; set; } // Behavior lock requests from this stage should exhibit (sequential or runLatest).

    /*
    public override void InitMermaid(string prefix, int depth)
    {
        MermaidId = Helpers.GetMermaidId(prefix, Name ?? "PL");
        MermaidDisplayName = Name ?? "PL";
        if(prefix == "")
        {
            MermaidId = MermaidId.Substring(1);
        }
        Depth = depth;
        
        // Set all local variables
        MermaidVariables = Helpers.GetMermaidVariableDefs(Variables);
    }

    public override string Parse()
    {
        if(MermaidId is null)
        {
            return "Failed init Mermaid for PipelineImpJob";
        }
        string mermaid_output = string.Empty;
        if(MermaidVariables is not null)
        {
            var var_dict = Helpers.GetVariableDict(MermaidVariables);
            mermaid_output += Helpers.GetMermaidLegendStr(var_dict, MermaidId, "Variables", Depth);
        }
        StepDef? prev_step = null;
        for(int i = 0; i < Steps.Count(); i++)
        {
            StepDef step = Steps[i];
            step.InitMermaid(MermaidId, Depth+1);
            if(MermaidVariables is not null)
            {
                step.ImportVariables(MermaidVariables);
            }
            mermaid_output += step.Parse();
            if(prev_step != null)
            {
                var tabs = new string('\t', Depth+1);
                mermaid_output += $"{tabs}{prev_step.MermaidId} --> {step.MermaidId}\n";    
            }
            prev_step = step;
            
        }
        return mermaid_output;
    }
    */
}