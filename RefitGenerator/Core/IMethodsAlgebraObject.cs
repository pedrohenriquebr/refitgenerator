namespace RefitGenerator.Core;

public interface IMethodsAlgebraObject<TStatement, TFile, TModifier, TType, TPropAccessor, TParam, TValue, TAttribute>
    : ITypesAndMembersAlgebraObject<TStatement, TFile, TModifier, TType, TPropAccessor>
{

    TStatement InterfaceMethod(string name, TType returnType, TModifier[] modifiers, TParam[] @params);
    TStatement InterfaceMethod(string name, TType returnType, TModifier[] modifiers);
    TStatement InterfaceMethod(string name, TType returnType);
    TStatement InterfaceMethod(string name, TType returnType, TModifier[] modifiers, TParam[] @params, TAttribute[] attributes);
    TStatement InterfaceMethod(string name, TType returnType, TModifier[] modifiers, TAttribute[] attributes);
    TParam ParamInfo(string name, TType type);
    TParam ParamInfo(string name, TType type, TAttribute[] attributes);
    TAttribute Attribute(string name, params TValue[] arguments);
    TValue StringConst(string value);
}