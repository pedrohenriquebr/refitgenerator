using RefitGenerator.Util;
using RefitGenerator.Generators.CSharp.Behaviors;
using RefitGenerator.Generators.CSharp.AlgebraObjects;
using RefitGenerator.Converter.Models;
using RefitGenerator.Converter.Factories;
using RefitGenerator.Converter.ParameterObjects;

namespace RefitGenerator.Converter;

public class SourceCodeBuilder : ISourceCodeBuilder
{
    private OpenApiModel openApiSpec = null;
    private string interfaceName = "";

    public IInterfaceMethodBuilder InterfaceMethodBuilder => _parameters.InterfaceMethodBuilder;

    private readonly SourceCodeBuilderParameters _parameters;

    public SourceCodeBuilder(SourceCodeBuilderParameters parameters)
    {
        _parameters = parameters;
    }

    public IStatementBehavior BuildStatementTree()
    {
        var mappedSchemasFromExamplesJson = _parameters.ExamplesJsonMapper.Map(openApiSpec.Examples);
        var algebraObjFactory = _parameters.AlgebraObjFactory;
        var interfaceMethodBuilder = _parameters.InterfaceMethodBuilder;
        var classBuilder = _parameters.ClassBuilder;

        var generatedSchemas = Enumerable.Empty<ClassModel>()
            .ToHashSet()
            .ToList();

        generatedSchemas.AddRange(mappedSchemasFromExamplesJson);


        return algebraObjFactory
            .Block(algebraObjFactory.Compose(
                            algebraObjFactory.UsingNamespace("System"),
                            algebraObjFactory.UsingNamespace("Refit")),
                        algebraObjFactory.Interface(
                            name: interfaceName,
                            body: algebraObjFactory.Compose(
                                    interfaceMethodBuilder.Build(openApiSpec.Methods)
                                   )
                            ),
                        algebraObjFactory.Block(
                            generatedSchemas
                                    .Select(x =>
                                        classBuilder
                                            .WithName(x.Name)
                                            .WithProps(x.Props)
                                            .Build())
                                    )
                        );
    }
    public string Build()
    {
        if (_parameters.AlgebraObjFactory.Fomatter is null)
            throw new Exception("SourceFormatter is null");
        var tree = BuildStatementTree();

        return _parameters.AlgebraObjFactory.Fomatter.FormatCode(tree.Generate());
    }
    public ISourceCodeBuilder New(OpenApiModel openApiDocument)
    {
        this.openApiSpec = openApiDocument;
        return this;
    }

    public ISourceCodeBuilder WithInterfaceName(string interfaceName)
    {
        this.interfaceName = interfaceName;
        return this;
    }
}
