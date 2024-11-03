using System.Text.RegularExpressions;
using ADOYamlMermaidParser.Model;

namespace ADOYamlMermaidParser;

public static class Helpers
{
    public static string GetMermaidId(string prefix, string name)
    {
        var mermaidId = $"{prefix}_{name}";
        mermaidId = Regex.Replace(mermaidId, "[^a-zA-Z0-9 _]", "");
        mermaidId = Regex.Replace(mermaidId, " ", "_");
        return mermaidId;
    }
    public static Dictionary<string, string> GetVariableDict(List<VariableDef> variableList)
    {
        var dict = new Dictionary<string, string>();
        foreach(var variable in variableList)
        {
            if(variable.Name is not null && variable.Value is not null)
            {
                dict.Add(variable.Name, variable.Value);
            }
        }
        return dict;
    }
    public static List<VariableDef> GetMermaidVariableDefs(object? variables)
    {
        if(variables is not null)
        {
            Console.WriteLine(variables.GetType().FullName);
            if(variables is IDictionary<object,object> var_dict)
            {
                return var_dict
                    .Select(pair => new VariableDef {
                    Group = null,
                    Name = (string)pair.Key,
                    Value = (string)pair.Value,
                    ReadOnly = false
                }).ToList();
            }
            else if(variables is List<object> variableList)
            {
                return variableList
                    .Where(v => v is Dictionary<object, object>)
                    .Cast<Dictionary<object, object>>()
                    .Select(dict => new VariableDef
                    {
                        Group = dict.ContainsKey("group") ? (string)dict["group"] : null,
                        Name = dict.ContainsKey("name") ? (string)dict["name"] : null,
                        Value = dict.ContainsKey("value") ? (string)dict["value"] : null,
                        ReadOnly = dict.ContainsKey("readOnly") && (bool)dict["readOnly"]
                    })
                    .ToList();
            }
            else
            {

            }
        }
        return new();
    }

    public static Dictionary<string, object> FormatYamlDictionary(object yaml)
    {
        if(yaml is IDictionary<object, object> objDictionary)
        {
            return objDictionary
                .ToDictionary(
                    kv => (string)kv.Key,
                    kv => kv.Value
                );
        }
        return new();
    }

    public static string GetMermaidLegendStr(Dictionary<string,string> map, string parent, string title, int depth)
    {
        if(map.Count() == 0)
        {
            return string.Empty;
        }
        var tabs1 = new string('\t',depth);
        var tabs2 = new string('\t',depth+1);
        
        var legendId = GetMermaidId(parent,"legend");
        var output = $"{tabs1}subgraph {legendId} [{title}]\n";
        output += $"{tabs2}\tdirection LR\n";
        foreach(var pair in map)
        {
            var componentId = GetMermaidId(legendId,pair.Key);
            output += $"{tabs2}\t{componentId}[{pair.Key} : {pair.Value}]\n";
        } 
        output += $"{tabs1}end";
        return output;
    }
}