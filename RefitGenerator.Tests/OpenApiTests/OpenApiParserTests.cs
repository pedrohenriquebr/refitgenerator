using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using Microsoft.OpenApi.Readers.Interface;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using RefitGenerator.Core;
using RefitGenerator.Factories;
using RefitGenerator.Util;
using System;
using System.Runtime.Serialization;

namespace RefitGenerator.Tests.OpenApiTests;

public class OpenApiParserTests
{
    private static readonly string EOF = "\r\n";
    private static readonly string INDENT = "    ";

    [Fact]
    public void Should_Generate_For_OpenApi()
    {
        var formatter = CreateSourceFormatter;
        var factory = CreateFactory(formatter);
        var openApiReader = CreateOpenApiReader();
        var (input, expected) = LoadTestAssets("SimpleApi");
        string output = new DefaultOpenApiConverter(openApiReader, factory)
            .Convert(input);

        Assert.Equal(expected, output);
    }

    public static ISourceFormatterProvider CreateSourceFormatter()
        => new DefaultSourceFormatter(EOF, INDENT);
    public static MethodAlgebraGenerator CreateFactory(Func<ISourceFormatterProvider> func)
        => new MethodAlgebraGenerator(func());

    public static OpenApiStringReader CreateOpenApiReader()
        => new OpenApiStringReader();

    public static Dictionary<string, (string, string)> matrix = new();

    public static (string, string) LoadTestAssets(string testName)
    {
        if (!matrix.ContainsKey(testName))
        {
            var basePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var baseDir = Path.GetDirectoryName(basePath);
            var assetsDir = Path.Combine(baseDir, @"OpenApiTests\Assets");
            var currentTestPath = Path.Combine(assetsDir, testName);
            var inputFilePath = Path.Combine(currentTestPath, "input.json");
            var expectedFilePath = Path.Combine(currentTestPath, "expected.cs");

            matrix[testName] = (File.ReadAllText(inputFilePath), File.ReadAllText(expectedFilePath));
        }

        return matrix[testName];
    }
}


public interface IOpenApiConverter
{
    public string Convert(string openApiJson);
    public string CreateMethodName(string operationId);
    public string CreateMethodResponseName(string methodName);
    public IAttributeBehavior CreateMethodAttribute(OperationType key);
    public IStatementBehavior CreateInterfaceMethod(KeyValuePair<OperationType, OpenApiOperation> operation);
}


public class DefaultOpenApiConverter : IOpenApiConverter
{
    private readonly IOpenApiReader<string, OpenApiDiagnostic> openApiReader;
    private readonly MethodAlgebraGenerator factory;
    public DefaultOpenApiConverter(
        IOpenApiReader<string, OpenApiDiagnostic> openApiReader,
        MethodAlgebraGenerator methodAlgebraGenerator)
    {
        this.openApiReader = openApiReader;
        this.factory = methodAlgebraGenerator;
    }

    public string CreateMethodName(string operationId)
        => operationId[0].ToString().ToUpper() + operationId[1..].ToString();
    private string Convert(OpenApiDocument openApiDoc)
    {
        return factory
            .Block(factory.Compose(
                            factory.UsingNamespace("System"),
                            factory.UsingNamespace("Refit")),
                        
                        factory.Interface(
                            name: "ISimpleOpenApiOverviewService",
                            body: factory.Compose(
                                openApiDoc.Paths.Keys.SelectMany(pathKey =>
                                    openApiDoc.Paths[pathKey].Operations
                                        .Select(operation => CreateInterfaceMethod(operation))
                                    ).ToArray()
                                )
                        )
                        )
            .Generate();
    }

    public IAttributeBehavior CreateMethodAttribute(OperationType key)
        => key switch
        {
            >= OperationType.Get and <= OperationType.Delete => factory.Attribute(key.ToString()),
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

    public IStatementBehavior CreateInterfaceMethod(KeyValuePair<OperationType, OpenApiOperation> operation)
    {
        var methodName = CreateMethodName(operation.Value.OperationId);
        var methodResponseTypeName = CreateMethodResponseName(methodName);
        IAttributeBehavior attribute = CreateMethodAttribute(operation.Key);
        return factory.InterfaceMethod(
                    name: methodName,
                    returnType: factory.Type($"Task<{methodResponseTypeName}>"),
                    modifiers: new[] { factory.Public() },
                    attributes: new[] { attribute }
        );
    }
}

