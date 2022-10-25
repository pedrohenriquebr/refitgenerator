namespace RefitGenerator.Generators.CSharp.Behaviors;

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
