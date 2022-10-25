using Microsoft.OpenApi.Models;

namespace RefitGenerator.Converter;

public static class OpenApiMappers
{
    public static IEnumerable<(string, KeyValuePair<OperationType, OpenApiOperation>)> MapToInterfaceMethod(
        this OpenApiPaths paths
        ) => paths.SelectMany((path, _) => path.Value.Operations.Select(operation => (path.Key, operation)));
}