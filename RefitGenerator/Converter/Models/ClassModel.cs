namespace RefitGenerator.Converter.Models;

public sealed record ClassModel
{
    public string Name { get; init; }
    public List<ClassPropModel> Props { get; init; } = new List<ClassPropModel>();
}
