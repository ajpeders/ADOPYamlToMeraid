namespace ADOYamlMermaidParser.Parser;

public class ParserFactory
{
    public static Parser CreateParser(string parseType, Dictionary<string,object> yaml, string prefix)
    {
        return parseType.ToLower() switch
        {
            "pipeline" => new PipelineParser(yaml, prefix),

            "_" => throw new Exception("Unknown parseType")
        };
    }
    private static Parser CreateStepParser(Dictionary<string,object> yaml, string prefix)
    {
        if(yaml.ContainsKey("task"))
        {
            return new TaskParser(yaml, prefix);
        }
    }
}