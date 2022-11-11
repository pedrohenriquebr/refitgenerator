using Microsoft.OpenApi.Models;
using RefitGenerator.Converter.Factories;
using RefitGenerator.Converter.Models;
using RefitGenerator.Converter.Strategies;
using RefitGenerator.Generators.CSharp.Behaviors;

namespace RefitGenerator.Converter;

public interface ISourceCodeBuilder
{
    public string Build();
    public ISourceCodeBuilder New(OpenApiModel openApiDocument);
    public ISourceCodeBuilder WithInterfaceName(string interfaceName);
    public IInterfaceMethodBuilder InterfaceMethodBuilder { get; }
}
