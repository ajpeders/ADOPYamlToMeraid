namespace ADOYamlMermaidParser.Model.Pipeline;

public abstract class PipelineDef : YamlBaseEntity
{
    public string? Name { get; set; } // Pipeline run number.
}