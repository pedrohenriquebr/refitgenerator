using Microsoft.CodeAnalysis;
using RefitGenerator.Converter.Factories;
using RefitGenerator.Converter.Models;
using RefitGenerator.Converter.Strategies;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace RefitGenerator.Converter.Mappers;

public class DefaultModelsMapper : IModelsMapper
{
    private IModelsFactory modelsFactory;
    public DefaultModelsMapper(IModelsFactory factory)
    {
        this.modelsFactory = factory;
    }
    public IEnumerable<ClassModel> Map(JsonNode? json, string rootName)
    {
        var set = new HashSet<ClassModel>();
        var stack = new Stack<ClassModel>();

        var initialObj = modelsFactory.CreateClass(rootName);
        set.Add(initialObj);
        stack.Push(initialObj);
        Traverse(json, set, stack);

        return set;
    }
    public void Traverse(JsonNode? obj, HashSet<ClassModel> set, Stack<ClassModel> stack)
    {
        var lastClass = stack.First();

        foreach (var prop in obj.AsObject())
        {
            if (prop.Value is JsonArray)
            {

                if (prop.Value.AsArray().Count() == 0)
                {
                    lastClass.Props.Add(modelsFactory.CreatePropForEmptyArray(prop.Key));
                    continue;
                }

                var array = prop.Value.AsArray();
                var firstElement = array[0];

                if (firstElement is JsonObject)
                {
                    var arrayItem = modelsFactory.CreatePropForArrayOfObjects(prop.Key);
                    lastClass.Props.Add(arrayItem);

                    var newObj = modelsFactory.CreateClassForArrayItem(prop.Key);
                    stack.Push(newObj);
                    set.Add(newObj);
                    Traverse(firstElement, set, stack);
                    stack.Pop();
                }
                else
                {
                    var typeName = firstElement.AsValue().GetType();
                    lastClass.Props.Add(modelsFactory.CreatePropForArray(prop.Key, typeName.ToString()));
                }
            }
            else if (prop.Value is JsonObject)
            {
                lastClass.Props.Add(modelsFactory.CreatePropForNestedObject(prop.Key));
                var newObj = modelsFactory.CreateClassForNestedObject(prop.Key);

                set.Add(newObj);
                stack.Push(newObj);
                Traverse(prop.Value, set, stack);
                stack.Pop();
            }
            else
            {
                lastClass.Props.Add(modelsFactory.CreateProp(prop.Key, prop.Value.AsValue().GetValue<JsonElement>().ValueKind.ToString().ToLower()));
            }
            lastClass = stack.First();
        }
    }
    public IEnumerable<ClassModel> Map(List<ExampleJsonModel> examplesJsonList)
    {
        return examplesJsonList.SelectMany(x =>
        {
            if (x.Json is null)
                return null;
             return Map(JsonSerializer.Deserialize<JsonNode>(x.Json), x.OperationId);
        });
    }
}
