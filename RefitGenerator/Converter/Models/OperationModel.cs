using Microsoft.OpenApi.Models;

namespace RefitGenerator.Converter.Models;

public sealed record OperationModel(
    string Path,
    KeyValuePair<OperationType, OpenApiOperation> Operation
);
