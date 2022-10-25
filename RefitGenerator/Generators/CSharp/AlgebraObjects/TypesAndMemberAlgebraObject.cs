using RefitGenerator.Core;
using RefitGenerator.Core.Providers;
using RefitGenerator.Generators.CSharp.Behaviors;

namespace RefitGenerator.Generators.CSharp.AlgebraObjects;

public class TypesAndMemberAlgebraObject : MultipleStatementsAlgebraObject, ITypesAndMembersAlgebraObject<IStatementBehavior, IFileBehavior, IModifierBehavior, ITypeBehavior, IPropAccessorBehavior>
{
    public TypesAndMemberAlgebraObject(ISourceFormatterProvider provider) : base(provider)
    {
    }

    public ITypeBehavior MakeType(Func<string> f)
        => new TypeBehavior(f);

    public ITypeBehavior Boolean()
        => MakeType(() => "bool");

    public ITypeBehavior Type(string name)
        => MakeType(() => name);

    public ITypeBehavior Decimal()
        => MakeType(() => "decimal");

    public ITypeBehavior Double()
        => MakeType(() => "double");

    public ITypeBehavior Float()
        => MakeType(() => "float");

    public ITypeBehavior Integer()
        => MakeType(() => "int");

    public ITypeBehavior String()
        => MakeType(() => "string");

    public ITypeBehavior Void()
        => MakeType(() => "void");

    public IModifierBehavior Public()
        => MakeMod(() => "public");

    public IModifierBehavior Private()
        => MakeMod(() => "private");

    public IModifierBehavior Static()
        => MakeMod(() => "static");

    public IModifierBehavior Internal()
        => MakeMod(() => "internal");

    public IModifierBehavior Protected()
        => MakeMod(() => "protected");

    public IStatementBehavior Member(string name, ITypeBehavior type, IModifierBehavior[] modifiers)
        => Make(() => JoinModifiers(modifiers, " ") + Member(name, type).Generate());

    public IStatementBehavior Member(string name, ITypeBehavior type)
        => Make(() => type.Generate() + " " + name);

    public IStatementBehavior Property(string name, ITypeBehavior type, IModifierBehavior[] modifiers, IPropAccessorBehavior[] accessors)
        => Make(() => Member(name, type, modifiers).Generate()
            + $" {{{accessors.Aggregate(" ", (acc, x) => acc + x.Generate() + "; ")}}}{e.GetEof()}");

    public IStatementBehavior Property(string name, ITypeBehavior type, IModifierBehavior[] modifiers)
        => Make(() => Member(name, type, modifiers).Generate() + $" {{ get; set; }}{e.GetEof()}");

    public IStatementBehavior Property(string name, ITypeBehavior type)
        => Make(() => Property(name, type, new ModifierBehavior[] { }).Generate());

    public IPropAccessorBehavior MakePropAccessor(Func<string> f)
         => new PropAccessorBehavior(f);
    public IPropAccessorBehavior Getter(IModifierBehavior modifier)
        => MakePropAccessor(() => modifier.Generate() + " " + Getter().Generate());
    public IPropAccessorBehavior Getter()
        => MakePropAccessor(() => "get");

    public IPropAccessorBehavior Setter(IModifierBehavior modifier)
        => MakePropAccessor(() => modifier.Generate() + " " + Setter().Generate());

    public IPropAccessorBehavior Setter()
        => MakePropAccessor(() => "set");

    public ITypeBehavior Type<T>()
        => Type(typeof(T).Name);
}
