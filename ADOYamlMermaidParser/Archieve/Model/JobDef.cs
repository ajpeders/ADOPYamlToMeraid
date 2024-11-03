namespace ADOYamlMermaidParser.Model;

public class JobDef
{
    // Required properties
    public string? Job { get; set; }  // Required as first property. ID of the job.
    public string? DisplayName { get; set; } // Human-readable name for the job.
    // Optional properties
    public object? DependsOn { get; set; } // Jobs that must complete before this one (can be string or list of strings).
    public string? Condition { get; set; } // Condition expression to determine whether to run this job.
    public bool? ContinueOnError { get; set; } // Continue running even on failure.
    public int? TimeoutInMinutes { get; set; } // Time to wait for the job to complete before termination.
    public int? CancelTimeoutInMinutes { get; set; } // Time to wait for job cancellation before forcing termination.
    public object? Variables { get; set; } // Job-specific variables.
    public string? Strategy { get; set; } // Execution strategy for this job.
    public string? Pool { get; set; } // Pool where this job will run.
    public string? Container { get; set; } // Container resource name.
    public object? Services { get; set; } // Container services as name/value pairs.
    public object? Workspace { get; set; } // Workspace options on the agent.
    public object? Uses { get; set; } // Any resources required by this job.
    public object? Steps { get; set; } // List of steps to run.
    public string? TemplateContext { get; set; } // Job related information passed from a pipeline when extending a template.
    /*
    public override void InitMermaid(string prefix, int depth)
    {
        throw new NotImplementedException();
    }
    public override string Parse()
    {
        throw new NotImplementedException();
    }*/
}

// Supporting classes to represent more complex fields
public class WorkspaceOptions
{
    public string? Clean { get; set; } // Which parts of the workspace should be cleaned.
}

public class ResourceUses
{
    public List<string>? Repositories { get; set; } // Repository references.
    public List<string>? Pools { get; set; } // Pool references.
}
