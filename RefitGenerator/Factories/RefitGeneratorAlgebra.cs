using RefitGenerator.Core;

namespace RefitGenerator.Factories;
public class RefitGeneratorAlgebra : IStatementsAlgebra<IStatementBehavior, IFileBehavior, IModifierBehavior>
{
    protected readonly ISourceFormatterProvider e;
    public ISourceFormatterProvider Fomatter => e;

    public RefitGeneratorAlgebra() : this(new DefaultSourceFormatter())
    {

    }
    public RefitGeneratorAlgebra(ISourceFormatterProvider e)
    {
        this.e = e;
    }
    public static StatementBehavior Make(Func<string> str)
        => new StatementBehavior(str);

    public static ModifierBehavior MakeMod(Func<string> str)
        => new ModifierBehavior(str);

    public string MakeBlock(string? body = "")
        => $"{e.GetEof()}" +
            $"{{" +
            $"{e.GetEof()}" +
            body +
            $"{e.GetEof()}" +
            $"}}";

    protected string JoinArguments(IValueBehavior[] stmts, string @char = "")
        => stmts.Aggregate("", (acc, x) => acc + x.Generate() + @char);

    protected string JoinStatements(IStatementBehavior[] stmts, string @char = "")
        => stmts.Aggregate("", (acc, x) => acc + x.Generate() + @char);

    protected string JoinModifiers(IModifierBehavior[] stmts, string @char = "")
        => stmts.Aggregate("", (acc, x) => acc + x.Generate() + @char);

    public IStatementBehavior Class(string name, IStatementBehavior body)
        => Make(() => $"public class {name}{MakeBlock(body.Generate())}");


    public IStatementBehavior Blank()
        => Make(() => "");

    public IStatementBehavior Class(string name)
    {
        return Class(name, Blank());
    }

    public IStatementBehavior Compose(params IStatementBehavior[] stmts) => Make(() => JoinStatements(stmts, $";{e.GetEof()}"));

    public IStatementBehavior Root(params IStatementBehavior[] stmts) => Make(() => JoinStatements(stmts, $""));

    public IFileBehavior File(string s, IStatementBehavior[] statements)
    {
        throw new NotImplementedException();
    }

    public IStatementBehavior Interface(string name, IModifierBehavior[] modifiers, IStatementBehavior body)
    {
        var mods = JoinModifiers(modifiers, " ");
        return Make(() => $"{mods}interface {name}{MakeBlock(body.Generate())}");
    }

    public IStatementBehavior Namespace(string s)
    {
        return Make(() => $"namespace {s}");
    }

    public IStatementBehavior UsingNamespace(string s)
    {
        return Make(() => $"using {s}");

    }
}
