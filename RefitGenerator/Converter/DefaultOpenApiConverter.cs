using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using Microsoft.OpenApi.Readers.Interface;
using RefitGenerator.Util;
using Microsoft.OpenApi.Any;
//using NJsonSchema;
using RefitGenerator.Generators.CSharp.Behaviors;
using RefitGenerator.Generators.CSharp.AlgebraObjects;
using System.Text.Json;
using System.Text.Json.Nodes;
using RefitGenerator.Converter.Mappers;

namespace RefitGenerator.Converter;

public class DefaultOpenApiConverter : IOpenApiConverter
{
    private readonly IOpenApiReader<string, OpenApiDiagnostic> openApiReader;
    private readonly MethodAlgebraObject factory;
    public DefaultOpenApiConverter(
        IOpenApiReader<string, OpenApiDiagnostic> openApiReader,
        MethodAlgebraObject methodAlgebraGenerator)
    {
        this.openApiReader = openApiReader;
        factory = methodAlgebraGenerator;
    }

    public string CreateMethodName(string operationId)
        => operationId[0].ToString().ToUpper() + operationId[1..].ToString();
    private string Convert(OpenApiDocument openApiDoc)
    {
        var methods = openApiDoc.Paths.MapToInterfaceMethod();

        //extract from all 200 examples
        var examples = methods
                .SelectMany(method =>
                {
                    return method.Item2.Value.Responses
                    .Where(x => x.Key == "200")
                    .SelectMany(x => x.Value.Content)
                    .Select(x => (method.Item2.Value.OperationId, ((OpenApiString)x.Value?.Example)?.Value ?? null));
                });


        var jsonSchemas = examples.SelectMany(x =>
        {
            if(x.Item2 is not null)
                return JsonSerializer.Deserialize<JsonNode>(x.Item2).Map(x.OperationId);
            return null;
        })
            .ToHashSet()
            .ToList();


        return factory
            .Block(factory.Compose(
                            factory.UsingNamespace("System"),
                            factory.UsingNamespace("Refit")),
                        factory.Interface(
                            name: "ISimpleOpenApiOverviewService",
                            body: factory.Compose(
                                    methods
                                   .Select(obj => CreateInterfaceMethod(obj.Item1, obj.Item2))
                                   )),
                        factory.Block(jsonSchemas.Select(x => factory.Class(x.Name,
                            factory.Block(
                                x.Props.Select(prop =>
                                    factory.Property(prop.Name, factory.Type(prop.Type), new[] { factory.Public() }))
                                )))
                        ))
            .Generate();
    }

    public IAttributeBehavior CreateMethodAttribute(string path, OperationType key)
        => key switch
        {
            >= OperationType.Get and <= OperationType.Delete =>
                path switch
                {
                    null or "" or "/" => factory.Attribute(key.ToString()),
                    _ => factory.Attribute(key.ToString(), factory.StringConst(path))
                },
            _ => throw new ArgumentException("OperationType not supported!")
        };

    public string Convert(string openApiJson)
    {
        var openApiDoc = openApiReader.Read(openApiJson, out var diagnostic);




        if (factory.Fomatter is null)
            throw new Exception("SourceFormatter is null");

        return factory.Fomatter.FormatCode(Convert(openApiDoc));
    }

    public string CreateMethodResponseName(string methodName)
        => methodName + "Response";

    public IStatementBehavior CreateInterfaceMethod(string path, KeyValuePair<OperationType, OpenApiOperation> operation)
    {
        var methodName = CreateMethodName(operation.Value.OperationId);
        var methodResponseTypeName = CreateMethodResponseName(methodName);
        IAttributeBehavior attribute = CreateMethodAttribute(path, operation.Key);
        return factory.InterfaceMethod(
                    name: methodName,
                    returnType: factory.Type($"Task<{methodResponseTypeName}>"),
                    modifiers: new[] { factory.Public() },
                    attributes: new[] { attribute }
        );
    }
}
