namespace RefitGenerator.Generators.CSharp.Behaviors;

public class ModifierBehavior : IModifierBehavior
{
    private readonly Func<string> _generate;

    public ModifierBehavior(Func<string> generate)
    {
        _generate = generate;
    }
    public string Generate() => _generate();
}
