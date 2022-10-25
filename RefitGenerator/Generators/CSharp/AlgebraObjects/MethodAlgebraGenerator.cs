using RefitGenerator.Core;
using RefitGenerator.Generators.CSharp.Behaviors;

namespace RefitGenerator.Generators.CSharp.AlgebraObjects;

public class MethodAlgebraGenerator : TypesAndMemberGenerator,
    IMethodsAlgebra<IStatementBehavior, IFileBehavior, IModifierBehavior, ITypeBehavior, IPropAccessorBehavior, IParameterInfoBehavior, IValueBehavior, IAttributeBehavior>
{
    public MethodAlgebraGenerator(ISourceFormatterProvider provider) : base(provider)
    {
    }

    protected string JoinParamsInfo(IParameterInfoBehavior[] stmts)
        => string.Join(", ", stmts.Select((x) => x.Generate()));

    protected string JoinAttributes(IAttributeBehavior[] attributes, string @char)
        => attributes.Aggregate("", (acc, x) => acc + x.Generate() + @char);

    public IStatementBehavior InterfaceMethod(string name, ITypeBehavior returnType, IModifierBehavior[] modifiers, IParameterInfoBehavior[] @params)
        => InterfaceMethod(name, returnType, modifiers, @params, new AttributeBehavior[] { });

    public IStatementBehavior InterfaceMethod(string name, ITypeBehavior returnType, IModifierBehavior[] modifiers, IAttributeBehavior[] attributes)
        => InterfaceMethod(name, returnType, modifiers, new ParameterInfoBehavior[] { }, attributes);

    public IStatementBehavior InterfaceMethod(string name,
                                              ITypeBehavior returnType,
                                              IModifierBehavior[] modifiers,
                                              IParameterInfoBehavior[] @params,
                                              IAttributeBehavior[] attributes)
     => Make(() => JoinAttributes(attributes, e.GetEof()) +
                   JoinModifiers(modifiers, " ") +
                   returnType.Generate() +
                   " " +
                   name +
                   $"({JoinParamsInfo(@params)})");

    public IStatementBehavior InterfaceMethod(string name, ITypeBehavior returnType, IModifierBehavior[] modifiers)
        => InterfaceMethod(name, returnType, modifiers, new ParameterInfoBehavior[] { });

    public IStatementBehavior InterfaceMethod(string name, ITypeBehavior returnType)
        => InterfaceMethod(name, returnType, new ModifierBehavior[] { });

    public IParameterInfoBehavior MakeParam(Func<string> f)
        => new ParameterInfoBehavior(f);

    public IAttributeBehavior MakeAttribute(Func<string> f)
        => new AttributeBehavior(f);
    public IParameterInfoBehavior ParamInfo(string name, ITypeBehavior type)
        => ParamInfo(name, type, new AttributeBehavior[] { });
    public IParameterInfoBehavior ParamInfo(string name, ITypeBehavior type, IAttributeBehavior[] attributes)
        => MakeParam(() => $"{JoinAttributes(attributes, " ")}{type.Generate()} {name}");
    public IAttributeBehavior Attribute(string name, params IValueBehavior[] arguments)
    {
        if (arguments.Length == 0)
            return MakeAttribute(() => $"[{name}]");

        var _args = JoinArguments(arguments, ",");
        return MakeAttribute(() => $"[{name}({_args})]");
    }

    public IValueBehavior MakeValue(Func<string> f)
        => new ValueBehavior(f);
    public IValueBehavior StringConst(string value)
     => MakeValue(() => $"\"{value}\"");


}