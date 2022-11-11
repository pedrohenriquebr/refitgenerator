using Microsoft.OpenApi.Models;
using RefitGenerator.Generators.CSharp.Behaviors;
using RefitGenerator.Generators.CSharp.AlgebraObjects;
using RefitGenerator.Converter.Strategies;
using RefitGenerator.Converter.Models;

namespace RefitGenerator.Converter.Factories;

public class DefaultInterfaceMethodBuilder : IInterfaceMethodBuilder
{
    private readonly MethodAlgebraObject factory;
    private IMethodNameStrategy _methodNameStrategy = MethodNameStrategies.TitleCaseStrategy;
    private IMethodResponseNameStrategy _methodResponseNameStrategy = MethodResponseNameStrategies.ResponseSuffixStrategy;
    private readonly IMethodAttributeFactory _methodAttributeFactory;

    public DefaultInterfaceMethodBuilder(IMethodAttributeFactory methodAttributeFactory, MethodAlgebraObject factory)
    {
        _methodAttributeFactory = methodAttributeFactory;
        this.factory = factory;
    }

    public IInterfaceMethodBuilder WithMethodNameStrategy(IMethodNameStrategy strategy)
    {
        _methodNameStrategy = strategy;
        return this;
    }

    public IInterfaceMethodBuilder WithMethodResponseNameStrategy(IMethodResponseNameStrategy strategy)
    {
        _methodResponseNameStrategy = strategy;
        return this;
    }

    public IStatementBehavior Build(string path, KeyValuePair<OperationType, OpenApiOperation> operation)
    {
        var methodName = _methodNameStrategy.Create(operation.Value.OperationId);
        var methodResponseTypeName = _methodResponseNameStrategy.Create(methodName);
        return factory.InterfaceMethod(
                    name: methodName,
                    returnType: factory.Type($"Task<{methodResponseTypeName}>"),
                    modifiers: new[] { factory.Public() },
                    attributes: new[] { _methodAttributeFactory.Create(path, operation.Key) }
        );
    }

    public IEnumerable<IStatementBehavior> Build(List<OperationModel> operationModelsList)
    {
        return operationModelsList
            .Select(obj => Build(obj.Path, obj.Operation));
    }
}
