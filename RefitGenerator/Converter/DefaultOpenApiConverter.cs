using Microsoft.OpenApi.Readers;
using Microsoft.OpenApi.Readers.Interface;
using RefitGenerator.Converter.Mappers;
using RefitGenerator.Converter.Strategies;
//using NJsonSchema;

namespace RefitGenerator.Converter;
public class DefaultOpenApiConverter : IOpenApiConverter
{
    private readonly IOpenApiReader<string, OpenApiDiagnostic> openApiReader;
    private readonly ISourceCodeBuilder sourceCodeBuilder;
    public DefaultOpenApiConverter(
        IOpenApiReader<string, OpenApiDiagnostic> openApiReader,
        ISourceCodeBuilder sourceCodeBuilder)
    {
        this.openApiReader = openApiReader;
        this.sourceCodeBuilder = sourceCodeBuilder;
    }

    public string Convert(string openApiJson, string serviceName)
    {
        var openApiDoc = openApiReader.Read(openApiJson, out var diagnostic);

        this.sourceCodeBuilder
            .WithInterfaceName(serviceName);

        return this.sourceCodeBuilder 
                .New(openApiDoc.Map())
                .Build();
    }


}
