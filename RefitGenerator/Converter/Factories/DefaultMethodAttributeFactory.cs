using Microsoft.OpenApi.Models;
//using NJsonSchema;
using RefitGenerator.Generators.CSharp.Behaviors;
using RefitGenerator.Generators.CSharp.AlgebraObjects;

namespace RefitGenerator.Converter.Factories;

public class DefaultMethodAttributeFactory : IMethodAttributeFactory
{

    private readonly MethodAlgebraObject factory;

    public DefaultMethodAttributeFactory(MethodAlgebraObject factory)
    {
        this.factory = factory;
    }

    public IAttributeBehavior Create(string path, OperationType key)
        => key switch
        {
            >= OperationType.Get and <= OperationType.Delete =>
                path switch
                {
                    null or "" or "/" => factory.Attribute(key.ToString()),
                    _ => factory.Attribute(key.ToString(), factory.StringConst(path))
                },
            _ => throw new ArgumentException("OperationType not supported!")
        };
}