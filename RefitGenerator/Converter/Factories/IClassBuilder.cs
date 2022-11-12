using RefitGenerator.Generators.CSharp.Behaviors;
using RefitGenerator.Converter.Models;

namespace RefitGenerator.Converter.Factories;

public interface IClassBuilder
{
    public IClassBuilder WithName(string name);
    public IClassBuilder WithProps(List<ClassPropModel> props);
    public IStatementBehavior Build();
}
