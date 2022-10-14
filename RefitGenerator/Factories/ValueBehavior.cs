using RefitGenerator.Core;

namespace RefitGenerator.Factories;

public class ValueBehavior : IValueBehavior
{
    private readonly Func<string> func;

    public ValueBehavior(Func<string> func)
    {
        this.func = func;
    }

    public string Generate()
     => func();
}
