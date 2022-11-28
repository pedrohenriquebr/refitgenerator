using RefitGenerator.Generators.CSharp.AlgebraObjects;
using RefitGenerator.Converter.Mappers;
using RefitGenerator.Converter.Factories;

namespace RefitGenerator.Converter.ParameterObjects;

public sealed record SourceCodeBuilderParameters
{
    public MethodAlgebraObject AlgebraObjFactory { get; init; }
    public IModelsMapper ExamplesJsonMapper { get; init; }
    public IInterfaceMethodBuilder InterfaceMethodBuilder { get; init; }
    public IClassBuilder ClassBuilder { get; init; }
}
