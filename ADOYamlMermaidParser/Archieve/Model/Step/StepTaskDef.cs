using System.Net.NetworkInformation;

namespace ADOYamlMermaidParser.Model.Step;

public class StepTaskDef : StepDef
{
    // Required properties
    public string? Task { get; set; } // Name of the task to run (required as the first property).
    // Optional properties
    public Dictionary<string, string>? Inputs { get; set; } // Inputs for the task (name/value pairs).
    public string? Condition { get; set; } // Evaluate this condition expression to determine whether to run this task.
    public bool? ContinueOnError { get; set; } // Continue running even on failure?
    public string? DisplayName { get; set; } // Human-readable name for the task.
    public object? Target { get; set; } // Environment in which to run this task (can be string or target).
    public bool? Enabled { get; set; } // Run this task when the job runs?
    public Dictionary<string, string>? Env { get; set; } // Variables to map into the process's environment (name/value pairs).
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
        return $"{tabs}{MermaidId}[\"task: {MermaidDisplayName}\"]\n";
    }

    public string GetDisplayName()
    {
        var displayName = "Unnamed";

        if(DisplayName is not null)
        {
            displayName = DisplayName;
        }
        else if(Task is not null)
        {
            displayName = Task;
        }
        return displayName;
    }
    */
}