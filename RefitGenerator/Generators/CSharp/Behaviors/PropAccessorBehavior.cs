namespace RefitGenerator.Generators.CSharp.Behaviors;

public class PropAccessorBehavior : IPropAccessorBehavior
{
    private readonly Func<string> f;

    public PropAccessorBehavior(Func<string> f)
    {
        this.f = f;
    }

    public string Generate()
     => f();
}
