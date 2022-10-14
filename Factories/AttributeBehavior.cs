using RefitGenerator.Core;

namespace RefitGenerator.Factories;

public class AttributeBehavior : IAttributeBehavior
{
    private readonly Func<string> f;

    public AttributeBehavior(Func<string> f)
    {
        this.f = f;
    }

    public string Generate()
     => f();
}
