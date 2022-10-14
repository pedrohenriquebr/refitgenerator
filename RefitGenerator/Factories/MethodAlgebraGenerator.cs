using RefitGenerator.Core;

namespace RefitGenerator.Factories;

public class MethodAlgebraGenerator : TypesAndMemberGenerator,
    IMethodsAlgebra<IStatementBehavior, IFileBehavior, IModifierBehavior, ITypeBehavior, IPropAccessorBehavior, IParameterInfoBehavior, IValueBehavior, IAttributeBehavior>
{
    public MethodAlgebraGenerator(ISourceFormatterProvider provider) : base(provider)
    {
    }

    protected string JoinParamsInfo(IParameterInfoBehavior[] stmts)
        => string.Join(", ", stmts.Select((x) => x.Generate()));

    protected string JoinAttributes(IAttributeBehavior[] attributes)
        => attributes.Aggregate("", (acc,x) => acc + x.Generate() + $"{e.GetEof()}");

    public IStatementBehavior InterfaceMethod(string name, ITypeBehavior returnType, IModifierBehavior[] modifiers, IParameterInfoBehavior[] @params)
        => InterfaceMethod(name, returnType, modifiers, @params, new AttributeBehavior[] { });

    public IStatementBehavior InterfaceMethod(string name, ITypeBehavior returnType, IModifierBehavior[] modifiers, IAttributeBehavior[] attributes)
        => InterfaceMethod(name, returnType, modifiers, new ParameterInfoBehavior[]{ }, attributes);

    public IStatementBehavior InterfaceMethod(string name, 
                                              ITypeBehavior returnType,
                                              IModifierBehavior[] modifiers, 
                                              IParameterInfoBehavior[] @params,
                                              IAttributeBehavior[] attributes)
     => Make(() => JoinAttributes(attributes) +
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
        => MakeParam(() => $"{type.Generate()} {name}");

    public IAttributeBehavior Attribute(string name, params IValueBehavior[] arguments)
        => MakeAttribute(() => $"[{name}({JoinArguments(arguments, ",")})]");

    public IValueBehavior MakeValue(Func<string> f)
        => new ValueBehavior(f);
    public IValueBehavior StringConst(string value)
     => MakeValue(() => $"\"{value}\"");


}