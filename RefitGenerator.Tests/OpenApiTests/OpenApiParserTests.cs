using Microsoft.CodeAnalysis;
using Microsoft.OpenApi.Readers;
using RefitGenerator.Converter;
using RefitGenerator.Converter.Factories;
using RefitGenerator.Converter.Mappers;
using RefitGenerator.Converter.Strategies;
using RefitGenerator.Core.Providers;
using RefitGenerator.Generators.CSharp;
using RefitGenerator.Generators.CSharp.AlgebraObjects;
using System;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace RefitGenerator.Tests.OpenApiTests;


public abstract class BaseTests
{
    private static readonly string EOF = "\r\n";
    private static readonly string INDENT = "    ";
    public ISourceCodeBuilder CreateSourceCodeBuilder()
    {
        var factory = CreateAlgebraObjectFactory();
        var normalizationStrategy = new DefaultNormalizationStrategy();
        var modelsFactory = new ModelsFactory(normalizationStrategy);
        var modelsMapper = new DefaultModelsMapper(modelsFactory);
        var methodAttributeFactory = new DefaultMethodAttributeFactory(factory);
        var interfaceMethodBuilder = new DefaultInterfaceMethodBuilder(methodAttributeFactory, factory);
        var sourceCodeBuilder = new SourceCodeBuilder(factory,
                                                      modelsMapper,
                                                      interfaceMethodBuilder);
        return sourceCodeBuilder;
    }

    public static ISourceFormatterProvider CreateSourceFormatter()
        => new DefaultSourceFormatter(EOF, INDENT);
    public static MethodAlgebraObject CreateAlgebraObjectFactory()
        => new MethodAlgebraObject(CreateSourceFormatter());

    public static OpenApiStringReader CreateOpenApiReader()
        => new OpenApiStringReader();

    public static (string, string) LoadTestFiles(string testName)
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
    
    private static Dictionary<string, (string, string)> matrix = new();
}
public class OpenApiParserTests : BaseTests
{
    

    //[Fact]
    //public void Should_Generate_For_OpenApi()
    //{
    //    var factory = CreateFactory();
    //    var openApiReader = CreateOpenApiReader();
    //    var (input, expected) = LoadTestAssets("SimpleApi");
    //    string output = new DefaultOpenApiConverter(openApiReader, factory)
    //        .Convert(input);

    //    Assert.Equal(expected, output);
    //}

    [Fact]
    public void Should_Generate_For_OpenApiWithSchema()
    {

        var sourceCodeBuilder = CreateSourceCodeBuilder();
        var openApiReader = CreateOpenApiReader();
        var (input, expected) = LoadTestFiles("Schema");
        string output = new DefaultOpenApiConverter(openApiReader, sourceCodeBuilder)
            .Convert(input, "ISimpleOpenApiOverviewService");

        Assert.Equal(expected, output);
    }

    [Fact]
    public void Should_Traverse_ObjJson()
    {
        var input = @"{
    ""versions"": [
        {
            ""status"": ""CURRENT"",
            ""updated"": ""2011-01-21T11:33:21Z"",
            ""id"": ""v2.0"",
            ""links"": [
                {
                    ""href"": ""http://127.0.0.1:8774/v2/"",
                    ""rel"": ""self""
                }
            ]
        },
        {
            ""status"": ""EXPERIMENTAL"",
            ""updated"": ""2013-07-23T11:33:21Z"",
            ""id"": ""v3.0"",
            ""links"": [
                {
                    ""href"": ""http://127.0.0.1:8774/v3/"",
                    ""rel"": ""self""
                }
            ]
        }
    ]
}
";
        var json = JsonSerializer.Deserialize<JsonNode>(input);

        var output = json.Map("Test");

        Assert.NotNull(output);
    }

    [Theory]
    [InlineData("MediaType", "media-type")]
    [InlineData("MediaType", "mediaType")]
    [InlineData("Name", "name")]
    [InlineData("Nameprop", "nameprop")]
    public void Should_Normalize_PropName(string expected, string name)
    {
        var normalizationStrategy = new DefaultNormalizationStrategy();
        var result = normalizationStrategy.NormalizeName(name);
        Assert.Equal(expected, result);
    }
}
