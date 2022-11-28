using Microsoft.OpenApi.Models;
using RefitGenerator.Generators.CSharp.Behaviors;
using RefitGenerator.Converter.Strategies;
using RefitGenerator.Converter.Models;

namespace RefitGenerator.Converter.Factories;

public interface IInterfaceMethodBuilder
{
    public IStatementBehavior Build(string path, KeyValuePair<OperationType, OpenApiOperation> operation);
    public IEnumerable<IStatementBehavior> Build(List<OperationModel> operationModelsList);
}
