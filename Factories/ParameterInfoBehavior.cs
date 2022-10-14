using RefitGenerator.Core;

namespace RefitGenerator.Factories;

public class ParameterInfoBehavior : IParameterInfoBehavior
{
    private readonly Func<string> _func;

    public ParameterInfoBehavior(Func<string> func)
    {
        _func = func;
    }
    public string Generate() => _func.Invoke();

}
