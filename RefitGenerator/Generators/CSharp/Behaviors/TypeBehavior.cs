namespace RefitGenerator.Generators.CSharp.Behaviors;

public class TypeBehavior : ITypeBehavior
{

    private readonly Func<string> _func;
    public TypeBehavior(Func<string> func)
    {
        _func = func;
    }

    public string Generate() => _func();
}
