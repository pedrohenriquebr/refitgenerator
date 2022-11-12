using RefitGenerator.Util;
using RefitGenerator.Generators.CSharp.Behaviors;
using RefitGenerator.Generators.CSharp.AlgebraObjects;
using RefitGenerator.Converter.Mappers;
using RefitGenerator.Converter.Models;
using RefitGenerator.Converter.Factories;

namespace RefitGenerator.Converter;

public class SourceCodeBuilder : ISourceCodeBuilder
{
    private readonly MethodAlgebraObject algebraObjFactory;
    private OpenApiModel openApiSpec = null;
    private string interfaceName = "";

    private readonly IModelsMapper examplesJsonMapper;
    private readonly IInterfaceMethodBuilder interfaceMethodBuilder;
    private readonly IClassBuilder _classBuilder;

    public IInterfaceMethodBuilder InterfaceMethodBuilder => interfaceMethodBuilder;

    public SourceCodeBuilder(MethodAlgebraObject algebraObjFactory,
                            IModelsMapper examplesJsonMapper,
                            IInterfaceMethodBuilder interfaceMethodBuilder,
                            IClassBuilder classBuilder)
    {
        this.algebraObjFactory = algebraObjFactory;
        this.examplesJsonMapper = examplesJsonMapper;
        this.interfaceMethodBuilder = interfaceMethodBuilder;
        _classBuilder = classBuilder;
    }

    public IStatementBehavior BuildStatementTree()
    {
        var mappedSchemasFromExamplesJson = examplesJsonMapper.Map(openApiSpec.Examples);

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
                                        this._classBuilder
                                            .WithName(x.Name)
                                            .WithProps(x.Props)
                                            .Build())
                                    )
                        );
    }
    public string Build()
    {
        if (algebraObjFactory.Fomatter is null)
            throw new Exception("SourceFormatter is null");
        var tree = BuildStatementTree();

        return algebraObjFactory.Fomatter.FormatCode(tree.Generate());
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
