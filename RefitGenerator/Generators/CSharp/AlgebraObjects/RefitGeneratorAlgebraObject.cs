using RefitGenerator.Core;
using RefitGenerator.Core.Providers;
using RefitGenerator.Generators.CSharp.Behaviors;
using RefitGenerator.Util;

namespace RefitGenerator.Generators.CSharp.AlgebraObjects;
public class RefitGeneratorAlgebraObject : IStatementsAlgebraObject<IStatementBehavior, IFileBehavior, IModifierBehavior>
{
    protected readonly ISourceFormatterProvider e;
    public ISourceFormatterProvider Fomatter => e;

    public RefitGeneratorAlgebraObject() : this(new DefaultSourceFormatter())
    {

    }
    public RefitGeneratorAlgebraObject(ISourceFormatterProvider e)
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
            $"}}";

    protected string JoinArguments(IValueBehavior[] stmts, string @char = "")
        => string.Join(@char, stmts.Select(x => x.Generate()));

    protected string JoinStatements(IStatementBehavior[] stmts, string @char = "")
        => stmts.Aggregate("", (acc, x) => acc + x.Generate() + @char);

    protected string JoinStatements(IEnumerable<IStatementBehavior> stmts, string @char = "")
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
    public IStatementBehavior Compose(IEnumerable<IStatementBehavior> stmts) => Make(() => JoinStatements(stmts, $";{e.GetEof()}"));

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


    public IStatementBehavior Region(string name, IStatementBehavior body)
        => Make(() => $"#region {name} {e.GetEof(2)}" +
                      $"{e.FormatCode(body.Generate())}" +
                      $"{e.GetEof(2)}" +
                      $"#endregion{e.GetEof()}");
}
