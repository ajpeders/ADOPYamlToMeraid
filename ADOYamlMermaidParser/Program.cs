using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using ADOYamlMermaidParser.Model.Pipeline;
using ADOYamlMermaidParser.Model.Step;
using ADOYamlMermaidParser.Parser;

string yamlContent = File.ReadAllText("../pipelines/azure-pipelines.yml");

var deserializer = new DeserializerBuilder()
    .Build();

try
{
    var yaml = deserializer.Deserialize(yamlContent);
    var pipelineParser = new PipelineParser(yaml, "");
    Console.WriteLine(pipelineParser.Parse());
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

/*
var deserializer = new DeserializerBuilder()
    .WithNamingConvention(CamelCaseNamingConvention.Instance)
    .WithTypeDiscriminatingNodeDeserializer(options => 
        {
            var keyMap = new Dictionary<string, Type>
            {
                {"task", typeof(StepTaskDef)},
                {"script", typeof(StepScriptDef)}
            };
            options.AddUniqueKeyTypeDiscriminator<StepDef>(keyMap);
        }
    )
    .Build();

// Try to parse as PipelineDef
try
{
    var pipelineDef = deserializer.Deserialize<PipelineExDef>(yamlContent);
    Console.WriteLine("Parsed as PipelineDef.");
    // Use pipelineDef here...
    return;
}
catch (Exception ex)
{
    Console.WriteLine("X    Not PipelineDef: " + ex.Message);
}

// Try to parse as PipelineImpStage
try
{
    var pipelineImpStage = deserializer.Deserialize<PipelineImpStageDef>(yamlContent);
    Console.WriteLine("Parsed as PipelineImpStageDef.");
    // Use pipelineImpStage here...
    return;
}
catch (Exception ex)
{
    Console.WriteLine("X    Not PipelineImpStageDef: " + ex.Message);
}

// Try to parse as ImpJob
// try
// {
    var impJob = deserializer.Deserialize<PipelineImpJobDef>(yamlContent);
    Console.WriteLine("Parsed as PipelineImpJobDef.\n");
    // Use impJob here...
    string mermaidStr = "flowchart TD\n";
    impJob.InitMermaid(string.Empty, 0);
    mermaidStr += impJob.Parse();
    Console.WriteLine(mermaidStr);
    File.WriteAllText("../output.mmd", mermaidStr);
    return;
// }
// catch (Exception ex)
// {
//     Console.WriteLine("X    Not PipelineImpJobDef: " + ex.Message);
// }

Console.WriteLine("X    The YAML could not be parsed as any known pipeline format.");
*/