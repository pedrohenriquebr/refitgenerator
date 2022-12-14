using RefitGenerator.Core;

namespace RefitGenerator.Factories;

public class MultipleStatementsAlgebra : RefitGeneratorAlgebra, IMultipleStatementsAlgebra<IStatementBehavior, IFileBehavior, IModifierBehavior>
{
    public MultipleStatementsAlgebra(ISourceFormatterProvider provider) : base(provider)
    {
    }

    public ModifierBehavior PUBLIC_MOD = MakeMod(() => "public");
    public ModifierBehavior PRIVATE_MOD = MakeMod(() => "private");
    public ModifierBehavior STATIC_MOD = MakeMod(() => "static");
    public ModifierBehavior INTERNAL_MOD = MakeMod(() => "internal");
    public ModifierBehavior PROTECTED_MOD = MakeMod(() => "protected");

    public IStatementBehavior Block(params IStatementBehavior[] statements)
        => Make(() => JoinStatements(statements));


    public IStatementBehavior Interface(string name, IStatementBehavior body)
    {
        var defaultInterfaceModifiers = new ModifierBehavior[]
        {
            PUBLIC_MOD
        };

        return Interface(name, defaultInterfaceModifiers, body);
    }

    public IStatementBehavior Interface(string name)
        => Interface(name, Blank());

}
