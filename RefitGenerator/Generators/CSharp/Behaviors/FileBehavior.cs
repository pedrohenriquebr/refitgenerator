namespace RefitGenerator.Generators.CSharp.Behaviors;

public class FileBehavior : IFileBehavior
{
    private readonly Func<string> _generate;

    public FileBehavior(Func<string> generate)
    {
        _generate = generate;
    }
    public string Generate() => _generate();
}
