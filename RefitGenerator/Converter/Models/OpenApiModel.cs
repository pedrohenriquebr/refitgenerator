namespace RefitGenerator.Converter.Models;
public sealed record OpenApiModel(
   List<OperationModel> Methods,
   List<ExampleJsonModel> Examples
);