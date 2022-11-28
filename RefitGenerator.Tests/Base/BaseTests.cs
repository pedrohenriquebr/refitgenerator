using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Readers;
using Microsoft.OpenApi.Readers.Interface;
using RefitGenerator.Converter;
using RefitGenerator.Converter.Factories;
using RefitGenerator.Converter.Mappers;
using RefitGenerator.Converter.ParameterObjects;
using RefitGenerator.Converter.Strategies;
using RefitGenerator.Core.Providers;
using RefitGenerator.Generators.CSharp;
using RefitGenerator.Generators.CSharp.AlgebraObjects;

namespace RefitGenerator.Tests.Base;

public abstract class BaseTests
{
    private static readonly string EOF = "\r\n";
    private static readonly string INDENT = "    ";

    public IServiceProvider BuildServiceProvider()
    {
        var collection = new ServiceCollection();
        collection.AddScoped<INormalizationStrategy, DefaultNormalizationStrategy>();
        collection.AddScoped<IModelsFactory, ModelsFactory>();
        collection.AddScoped<IModelsMapper, DefaultModelsMapper>();
        collection.AddScoped<ISourceFormatterProvider>((c) => new DefaultSourceFormatter(EOF, INDENT));
        collection.AddScoped<MethodAlgebraObject>();
        collection.AddScoped<IClassBuilder, DefaultClassBuilder>();
        collection.AddScoped<IMethodAttributeFactory, DefaultMethodAttributeFactory>();
        collection.AddScoped<IInterfaceMethodBuilder, DefaultInterfaceMethodBuilder>();
        collection.AddScoped<ISourceCodeBuilder, SourceCodeBuilder>();
        collection.AddScoped<SourceCodeBuilderParameters>(c => new SourceCodeBuilderParameters()
        {
            AlgebraObjFactory = c.GetRequiredService<MethodAlgebraObject>(),
            ClassBuilder = c.GetRequiredService<IClassBuilder>(),
            ExamplesJsonMapper = c.GetRequiredService<IModelsMapper>(),
            InterfaceMethodBuilder = c.GetRequiredService<IInterfaceMethodBuilder>()
        });
        collection.AddScoped<IOpenApiConverter,DefaultOpenApiConverter>();
        collection.AddScoped<IOpenApiReader<string, OpenApiDiagnostic>, OpenApiStringReader>();

        return collection.BuildServiceProvider();
    }

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
