using RefitGenerator.Converter.Factories;
using RefitGenerator.Converter.Models;
using RefitGenerator.Converter.Strategies;
using System.Text.Json.Nodes;

namespace RefitGenerator.Converter.Mappers;

public interface IModelsMapper
{
    IEnumerable<ClassModel> Map(JsonNode? json, string rootName);
    IEnumerable<ClassModel> Map(List<ExampleJsonModel> examplesJsonList);
}
