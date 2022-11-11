using RefitGenerator.Converter.Models;

namespace RefitGenerator.Converter;

public interface IOpenApiConverter
{
    public string Convert(string openApiJson, string serviceName);
}