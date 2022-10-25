using Microsoft.OpenApi.Readers;
using RefitGenerator.Core;
using RefitGenerator.Factories;

namespace RefitGenerator.Tests.OpenApiTests;

public class OpenApiParserTests
{
    private static readonly string EOF = "\r\n";
    private static readonly string INDENT = "    ";

    [Fact]
    public void Should_Generate_For_OpenApi()
    {
        var factory = CreateFactory();
        var openApiReader = CreateOpenApiReader();
        var (input, expected) = LoadTestAssets("SimpleApi");
        string output = new DefaultOpenApiConverter(openApiReader, factory)
            .Convert(input);

        Assert.Equal(expected, output);
    }

    public static ISourceFormatterProvider CreateSourceFormatter()
        => new DefaultSourceFormatter(EOF, INDENT);
    public static MethodAlgebraGenerator CreateFactory()
        => new MethodAlgebraGenerator(CreateSourceFormatter());

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
