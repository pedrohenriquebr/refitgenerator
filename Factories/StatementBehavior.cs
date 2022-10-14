using RefitGenerator.Core;

namespace RefitGenerator.Factories;

public class StatementBehavior : IStatementBehavior
{
    private readonly Func<string> _generate;

    public StatementBehavior(Func<string> generate)
    {
        _generate = generate;
    }
    public string Generate() => _generate();
}
