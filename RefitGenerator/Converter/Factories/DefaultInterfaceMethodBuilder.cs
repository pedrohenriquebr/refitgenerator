using Microsoft.OpenApi.Models;
using RefitGenerator.Generators.CSharp.Behaviors;
using RefitGenerator.Generators.CSharp.AlgebraObjects;
using RefitGenerator.Converter.Strategies;
using RefitGenerator.Converter.Models;

namespace RefitGenerator.Converter.Factories;

public class DefaultInterfaceMethodBuilder : IInterfaceMethodBuilder
{
    private readonly MethodAlgebraObject factory;
    private readonly IMethodAttributeFactory _methodAttributeFactory;

    public DefaultInterfaceMethodBuilder(IMethodAttributeFactory methodAttributeFactory, MethodAlgebraObject factory)
    {
        _methodAttributeFactory = methodAttributeFactory;
        this.factory = factory;
    }


    public IStatementBehavior Build(string path, KeyValuePair<OperationType, OpenApiOperation> operation)
    {
        var methodName = GlobalSettings.MethodNameStrategy.Create(operation.Value.OperationId);
        var methodResponseTypeName = GlobalSettings.MethodResponseNameStrategy.Create(methodName);
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
