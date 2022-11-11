using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using RefitGenerator.Converter.Models;

namespace RefitGenerator.Converter.Mappers;

public static class OpenApiMappers
{
    public static IEnumerable<OperationModel> Map(
        this OpenApiPaths paths
        ) => paths.SelectMany((path, _) =>
            path.Value.Operations
                    .Select(operation =>
                        new OperationModel(path.Key, operation)));


    public static OpenApiModel Map(this OpenApiDocument openApi)
    {
        var methods = openApi.Paths.Map();
        var examples = methods
                .SelectMany(method =>
                {
                    return method.Operation.Value.Responses
                    .Where(x => x.Key == "200")
                    .SelectMany(x => x.Value.Content)
                    .Select(x => new ExampleJsonModel(
                            OperationId: method.Operation.Value.OperationId,
                            Json: ((OpenApiString)x.Value?.Example)?.Value ?? null));
                });

        return new OpenApiModel(
                Methods: methods.ToList(),
                Examples: examples.ToList()
            );
    }
}