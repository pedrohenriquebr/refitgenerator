using Microsoft.OpenApi.Readers;
using RefitGenerator.Converter;
using RefitGenerator.Converter.Factories;
using RefitGenerator.Converter.Mappers;
using RefitGenerator.Converter.Strategies;
using RefitGenerator.Core.Providers;
using RefitGenerator.Generators.CSharp;
using RefitGenerator.Generators.CSharp.AlgebraObjects;

namespace RefitGenerator.Tests.Base;

public abstract class BaseTests
{
    private static readonly string EOF = "\r\n";
    private static readonly string INDENT = "    ";
    public ISourceCodeBuilder CreateSourceCodeBuilder()
    {
        var factory = CreateAlgebraObjectFactory();
        var classBuilder = new DefaultClassBuilder(factory);
        var normalizationStrategy = new DefaultNormalizationStrategy();
        var modelsFactory = new ModelsFactory(normalizationStrategy);
        var modelsMapper = new DefaultModelsMapper(modelsFactory);
        var methodAttributeFactory = new DefaultMethodAttributeFactory(factory);
        var interfaceMethodBuilder = new DefaultInterfaceMethodBuilder(methodAttributeFactory, factory);
        var sourceCodeBuilder = new SourceCodeBuilder(factory,
                                                      modelsMapper,
                                                      interfaceMethodBuilder,
                                                      classBuilder);
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
