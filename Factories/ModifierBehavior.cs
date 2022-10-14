using RefitGenerator.Core;

namespace RefitGenerator.Factories;

public class ModifierBehavior : IModifierBehavior
{
    private readonly Func<string> _generate;

    public ModifierBehavior(Func<string> generate)
    {
        _generate = generate;
    }
    public string Generate() => _generate();
}
