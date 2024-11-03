namespace ADOYamlMermaidParser.Model;
public abstract class YamlBaseEntity
{
    public string? MermaidId { get; set; }
    public string? MermaidDisplayName { get; set; }
    public int Depth { get; set; } = 0;
    public List<VariableDef>? MermaidVariables { get; set;} = null;

    // public abstract void InitMermaid(string prefix, int depth);
    // public void ImportVariables(List<VariableDef> variables)
    // {
    //     if(MermaidVariables is null)
    //     {
    //         MermaidVariables = new List<VariableDef>(variables);
    //     }
    //     else
    //     {
    //         MermaidVariables.AddRange(variables);
    //     }
    // }
    // public abstract string Parse();
}