using RefitGenerator.Converter.Factories;
using RefitGenerator.Converter.Models;
using RefitGenerator.Converter.Strategies;
using System.Globalization;
using System.Text.Json.Nodes;
using static System.Net.Mime.MediaTypeNames;

namespace RefitGenerator.Converter.Mappers;

public static class JsonDictionaryMapper
{
    public static List<ClassModel> Map(this JsonNode? json, string rootName)
    {
        return new DefaultModelsMapper(new ModelsFactory(new DefaultNormalizationStrategy())).Map(json, rootName).ToList();
    }
}