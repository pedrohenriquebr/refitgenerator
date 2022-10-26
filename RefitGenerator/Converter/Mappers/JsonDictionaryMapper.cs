using RefitGenerator.Converter.Models;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace RefitGenerator.Converter.Mappers;

public interface IJsonSchemaFactory
{
    public ClassPropModel CreateProForNestedObject(string propName);
    public ClassModel CreateClassForNestedObject(string propName);
    public ClassPropModel CreatePropForEmptyArray(string propName);
    public ClassPropModel CreatePropForArray(string propName, string generic);
    public ClassPropModel CreatePropForArrayItem(string propName, string generic);
    public ClassModel CreateClass(string name);
    public ClassPropModel CreateProp(string name, string type);
}



public static class JsonDictionaryMapper
{
    public static List<ClassModel> Map(this JsonNode? json, string rootName)
    {
        var set = new HashSet<ClassModel>();
        var stack = new Stack<ClassModel>();

        var initialObj   = CreateClass(rootName);
        set.Add(initialObj);
        stack.Push(initialObj);
        Traverse(json, set, stack);

        return set.ToList();
    }


    public static void Traverse(JsonNode? obj, HashSet<ClassModel> set, Stack<ClassModel> stack)
    {
        var lastClass = stack.First();

        foreach (var prop in obj.AsObject())
        {
            if (prop.Value is JsonArray)
            {

                if (prop.Value.AsArray().Count() == 0)
                {
                    lastClass.Props.Add(CreatePropForEmptyArray(prop.Key));
                    continue;
                }

                var array = prop.Value.AsArray();
                var firstElement = array[0];

                if (firstElement is JsonObject)
                {
                    var arrayItem = CreatePropForArrayOfObjects(prop.Key);
                    lastClass.Props.Add(arrayItem);

                    var newObj = CreateClassForArrayItem(prop.Key);
                    stack.Push(newObj);
                    set.Add(newObj);
                    Traverse(firstElement, set, stack);
                    stack.Pop();
                }
                else
                {
                    var typeName = firstElement.AsValue().GetType();
                    lastClass.Props.Add(CreatePropForArray(prop.Key, typeName.ToString()));
                }
            }
            else if (prop.Value is JsonObject)
            {
                lastClass.Props.Add(CreateProForNestedObject(prop.Key));
                var newObj = CreateClassForNestedObject(prop.Key);

                set.Add(newObj);
                stack.Push(newObj);
                Traverse(prop.Value, set, stack);
                stack.Pop();
            }
            else
            {
                lastClass.Props.Add(CreateProp(prop.Key, prop.Value.AsValue().GetValue<JsonElement>().ValueKind.ToString().ToLower()));
            }
            lastClass = stack.First();
        }
    }

    private static ClassModel CreateClassForArrayItem(string key)
    {
        return CreateClass($"{NormalizeName(key)}Item");
    }

    public static ClassPropModel CreateProForNestedObject(string propName)
    {
        return CreateProp(propName, NormalizeName(propName) + "Response");
    }

    public static ClassModel CreateClassForNestedObject(string propName)
    {
        return CreateClass(propName + "Response");
    }

    public static ClassPropModel CreatePropForEmptyArray(string propName)
    {
        return CreateProp(propName, "List<object>");
    }

    public static ClassPropModel CreatePropForArray(string propName, string generic)
    {
        return CreateProp(propName, $"List<{NormalizeName(generic)}>");
    }


    public static ClassPropModel CreatePropForArrayOfObjects(string propName)
    {
        return CreatePropForArray(propName, $"{NormalizeName(propName)}Item");
    }


    public static ClassModel CreateClass(string name)
    {
        return new ClassModel
        {
            Name = NormalizeName(name)
        };
    }

    public static ClassPropModel CreateProp(string name, string type)
    {
        return new ClassPropModel
        {
            Name = NormalizeName(name),
            Type = type
        };
    }

    public static string NormalizeName(string name)
    {
        const string pattern = @"(-|_)\w{1}|^\w";
        string modified = name[0].ToString().ToUpper() + name[1..];
        return Regex.Replace(modified, pattern, match => match.Value.Replace("-", string.Empty).Replace("_", string.Empty).ToUpper());
    }
}