namespace ADOYamlMermaidParser.Model.Step;

public class StepScriptDef : StepDef
{
    // Required properties
    public string? Script { get; set; } // An inline script (required as the first property).
    // Optional properties
    public string? FailOnStderr { get; set; } // Fail the task if output is sent to Stderr?
    public string? WorkingDirectory { get; set; } // Start the script with this working directory.
    public string? Condition { get; set; } // Evaluate this condition expression to determine whether to run this task.
    public bool? ContinueOnError { get; set; } // Continue running even on failure?
    public string? DisplayName { get; set; } // Human-readable name for the task.
    public object? Target { get; set; } // Environment in which to run this task (can be string or target).
    public bool? Enabled { get; set; } // Run this task when the job runs?
    public object? Env { get; set; } // Variables to map into the process's environment (name/value pairs).
    public string? Name { get; set; } // ID of the step.
    public string? TimeoutInMinutes { get; set; } // Time to wait for this task to complete before the server kills it.
    public string? RetryCountOnTaskFailure { get; set; } // Number of retries if the task fails.

/*
    public override void InitMermaid(string prefix, int depth)
    {
        MermaidDisplayName = GetDisplayName();
        MermaidId = Helpers.GetMermaidId(prefix, MermaidDisplayName);
        Depth = depth;
    }


    public override string Parse()
    {
        var tabs = new string('\t', Depth);
        return $"{tabs}{MermaidId}[\"script: {MermaidDisplayName}\"]\n";
    }
    public string GetDisplayName()
    {
        var displayName = "Unnamed";

        if(DisplayName is not null)
        {
            displayName = DisplayName;
        }
        else if(Script is not null)
        {
            displayName = Script;
        }
        return displayName;
    }
    */
} 
