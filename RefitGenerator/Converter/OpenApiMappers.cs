using Microsoft.OpenApi.Models;

namespace RefitGenerator.Tests.OpenApiTests;

public static class OpenApiMappers
{
    public static IEnumerable<(string,KeyValuePair<OperationType, OpenApiOperation>)> MapToInterfaceMethod(
        this OpenApiPaths paths
        ) => paths.SelectMany((path, _) => path.Value.Operations.Select(operation => (path.Key, operation)));
} 