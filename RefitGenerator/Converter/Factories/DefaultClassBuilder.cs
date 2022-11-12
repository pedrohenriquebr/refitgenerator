using RefitGenerator.Generators.CSharp.Behaviors;
using RefitGenerator.Generators.CSharp.AlgebraObjects;
using RefitGenerator.Converter.Models;

namespace RefitGenerator.Converter.Factories;

public class DefaultClassBuilder : IClassBuilder
{

    private readonly MethodAlgebraObject _algebraObjFactory;
    private string _name = null;
    private List<ClassPropModel> _props;

    public DefaultClassBuilder(MethodAlgebraObject algebraObjFactory)
    {
        _algebraObjFactory = algebraObjFactory;
    }

    public IStatementBehavior Build()
    {
        if (_name is null)
            throw new Exception("Name has to be specified");

        if (_props is null)
            throw new Exception("Props has to be specified");

        return _algebraObjFactory
                .Class(
                    name: _name,
                    body: _algebraObjFactory
                        .Block(BuildProps(_props))
                );
    }

    private IEnumerable<IStatementBehavior> BuildProps(List<ClassPropModel> props)
    {
        return props
                    .Select(prop =>
                        _algebraObjFactory
                        .Property(
                            name: prop.Name,
                            type: _algebraObjFactory.Type(prop.Type),
                            modifiers: new[] { _algebraObjFactory.Public() }
                            )
                        );
    }

    public IClassBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public IClassBuilder WithProps(List<ClassPropModel> props)
    {
        _props = props;
        return this;
    }
}
