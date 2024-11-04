using System.Text.RegularExpressions;
using ADOYamlMermaidParser.Model;
using ADOYamlMermaidParser.Parser;
namespace ADOYamlMermaidParser.Parser;
public static class ParserHelper
{
    public static string GetMermaidId(string prefix, string name)
    {

        var mermaidId = (prefix == "") ? name : $"{prefix}_{name}";
        mermaidId = Regex.Replace(mermaidId, "[^a-zA-Z0-9 _]", "");
        mermaidId = Regex.Replace(mermaidId, " ", "_");
        return mermaidId;
    }

    public static string GetDisplayName(string type, 
                                        Dictionary<object,object> yaml,
                                        List<ADOVariable>? variables = null,
                                        List<ADOParameter>? parameters = null)
    {
        string ret = "untitled";
        // If pipeline.
        if(type == "pipeline")
        {
            yaml.TryGetValue("name", out object? value);
            ret = $"PL{value}";
        }
        // Anything else that'll have 'displayName'.
        if(yaml.TryGetValue("displayName", out object? displayName))
        {
            ret = (string)displayName;
        }
        else if(yaml.TryGetValue(type, out object? typeVal))
        {
            ret = (string)typeVal;
        }
        else if(type == "stage")
        {
            ret = "ST";
        }
        else if(type == "job")
        {
            ret = "JB";
        }
        return ReplaceParameters(ReplaceVariables(ret, variables ?? []), parameters ?? []);
    }
    public static string ReplaceVariables(string input, List<ADOVariable> variables)
    {
        foreach(var variable in variables)
        {
            input = input.Replace($"$({variable.Name})", variable.Value);
        }
        return input;
    }
    public static string ReplaceParameters(string input, List<ADOParameter> parameters)
    {
        foreach(var parameter in parameters)
        {
            input = input.Replace($"${{{{ parameters.{parameter.Name} }}}}", parameter.Default?.ToString() ?? string.Empty);
        }
        return input;
    }
    public static string GetLegendMermaidStr(string id, string title, Dictionary<string,string> legend, int depth)
    {
        if(legend.Count == 0)
        {
            return string.Empty;
        }
        var tabs1 = new string('\t', depth);
        var tabs2 = new string('\t', depth + 1);

        var legendId = GetMermaidId(id, $"{title}_legend");

        var output = $"{tabs1}subgraph {legendId} [\"{title}\"]\n";
        output += $"{tabs2}direction LR\n";
        for(int i = 0; i < legend.Count; i++)
        {
            var comp = legend.ElementAt(i);
            var componentId = GetMermaidId($"{legendId}_{i}", comp.Key);
            output += $"{tabs2}{componentId}[\"{comp.Key} : {comp.Value}\"]\n";
        } 
        output += $"{tabs1}end\n";
        return output;

    }

    public static List<ADOVariable> GetVariables(Dictionary<object,object> yaml)
    {
        if(yaml.TryGetValue("variables", out var vars))
        {
            // String dictionary.
            if(vars is Dictionary<object,object> varsDict)
            {
                return varsDict
                    .Select(pair => new ADOVariable
                    {
                        Name = (string)pair.Key,
                        Value = (string)pair.Value
                    })
                    .ToList();
            }
            // Variable list.
            else if(vars is List<object> varsList)
            {
                return varsList
                    .Where(varObj => varObj is Dictionary<object,object>)
                    .Select(varObj => (Dictionary<object,object>)varObj)
                    .Select(varDict => new ADOVariable
                        {
                            Name = varDict.TryGetValue("name", out var name) ? (string)name : "unknown",
                            Value = varDict.TryGetValue("value", out var value) ? (string)value : "unknown",
                            ReadOnly = varDict.TryGetValue("readOnly", out var readOnly) && (bool)readOnly,
                            Group = varDict.TryGetValue("group", out var group) ? (string)group : null

                        })
                    .ToList();
            }   
        }
        return [];
    }

    public static List<ADOParameter> GetParameters(Dictionary<object,object> yaml)
    {
        if(yaml.TryGetValue("parameters", out var parameters))
        {
            // String dictionary.
            if(parameters is Dictionary<object,object> paramsDict)
            {
                return paramsDict
                    .Select(pair => new ADOParameter
                    {
                        Name = (string)pair.Key,
                        Default = pair.Value,
                        Type = "string"

                    })
                    .ToList();
            }
            // Param list.
            else if(parameters is List<object> paramsList)
            {
                return paramsList
                    .Where(p => p is Dictionary<object,object>)
                    .Select(p => (Dictionary<object,object>)p)
                    .Select(p => new ADOParameter
                        {
                            Name = p.TryGetValue("name", out var name) ? (string)name : "unknown",
                            DisplayName = p.TryGetValue("displayName", out var displayName) ? (string)displayName : "unknown",
                            Type = p.TryGetValue("type", out var type) ? (string)type : "string",
                            Default = p.TryGetValue("default", out var defaultValue) ? defaultValue : null,
                            Values = p.TryGetValue("values", out var values) ? ((List<object>)values).Select(v => v).ToList() : null,
                        })
                    .ToList();
            }   
        }
        return [];
    }

    public static string GetStepsStr(List<Parser> steps, int depth)
    {
        string mermaidOutput = string.Empty;
        for(int i = 0; i < steps.Count; i++)
        {
            var ins = steps[i];   
            mermaidOutput += ins.Parse()+'\n';
        }
        Parser? prev_ins = null;
        string tabs = new string('\t', depth);
        foreach(var ins in steps)
        {
            if(prev_ins != null)
            {
                if(ins is TemplateParser templateParser)
                {
                    string paramList = string.Join(",\n", templateParser.PassedParameters?.Select(p => $"\"{p.SimpleParse()}\"") ?? []);
                    mermaidOutput += $"{tabs}{prev_ins.MermaidId} --> {(paramList.Length > 0 ? $"|\"{{{paramList}}}\"|" : "")}{templateParser.MermaidId}\n";
                }
                else
                {
                    mermaidOutput += $"{tabs}{prev_ins.MermaidId} --> {ins.MermaidId}\n";
                }
            }
            prev_ins = ins;
        }
        return mermaidOutput;
    }

    public static List<ADOVariable> UpdateVariables(List<ADOVariable> variables, List<ADOVariable> passedVariables)
    {
        foreach(var passedVariable in passedVariables)
        {
            var variable = passedVariables.FirstOrDefault(v => v.Name == passedVariable.Name);
            if(variable != null)
            {
                variable.Value = passedVariable.Value;
            }
            else 
            {
                variables.Add(passedVariable);
            }
        }
        return variables;
    }

    public static List<ADOParameter> UpdateParameters(List<ADOParameter> parameters, List<ADOParameter> passedParameters)
    {
        foreach(var parameter in parameters)
        {
            var passedParameter = passedParameters.FirstOrDefault(p => p.Name == parameter.Name);
            if(passedParameter != null)
            {
                parameter.Default = passedParameter.Default;
            }
        }
        return parameters;
    }

    public static List<ADOVariable> GetAndUpdateVariables(Dictionary<object,object> yaml, List<ADOVariable> variables)
    {
        var newVariables = GetVariables(yaml);
        return UpdateVariables(variables, newVariables);
    }

    public static List<ADOParameter> GetAndUpdateParameters(Dictionary<object,object> yaml, List<ADOParameter> parameters)
    {
        var newParameters = GetParameters(yaml);
        return UpdateParameters(parameters, newParameters);
    }
}