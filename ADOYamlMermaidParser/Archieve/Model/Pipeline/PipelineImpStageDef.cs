namespace ADOYamlMermaidParser.Model.Pipeline;

public class PipelineImpStageDef : PipelineDef
{
    // Required properties
    public object? Jobs { get; set; } // Jobs represent units of work which can be assigned to a single agent or server (can be a job, deployment, or template).
    // Optional properties
    public object? Pool { get; set; } // Pool where jobs in this pipeline will run unless otherwise specified (can be string or pool).
    public bool? AppendCommitMessageToRunName { get; set; } // Append the commit message to the build number. Default is true.
    public object? Trigger { get; set; } // Continuous integration triggers (can be none, trigger, or list of strings).
    public object? Parameters { get; set; } // Pipeline template parameters.
    public object? Pr { get; set; } // Pull request triggers (can be none, pr, or list of strings).
    public object? Schedules { get; set; } // Scheduled triggers (cron expressions).
    public object? Resources { get; set; } // Containers and repositories used in the build.
    public object? Variables { get; set; } // Variables for this pipeline (can be a dictionary or list of variable objects).
    public string? LockBehavior { get; set; } // Behavior lock requests from this stage should exhibit (sequential or runLatest).
}