using ADOYamlMermaidParser.Parser;
using YamlDotNet.Serialization;

string yamlContent = File.ReadAllText("../pipelines/azure-pipelines.yml");

var deserializer = new DeserializerBuilder()
    .Build();

try
{
    var yaml = deserializer.Deserialize<Dictionary<object, object>>(yamlContent);
    if(yaml is null)
    {
        throw new Exception("Null Yaml");
    }
    
    var pipelineParser = ParserFactory.CreateParser("pipeline",yaml,"",1);
    var mermaidOutput = "flowchart LR\n";
    mermaidOutput += pipelineParser.Parse();
    Console.WriteLine(mermaidOutput);
    File.WriteAllText("../output.mmd", mermaidOutput);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}