﻿namespace RefitGenerator.Core;

public interface IMethodsAlgebra<TStatement, TFile, TModifier, TType, TPropAccessor, TParam, TValue, TAttribute>
    : ITypesAndMembersAlgebra<TStatement, TFile, TModifier, TType, TPropAccessor>
{

    TStatement InterfaceMethod(string name, TType returnType, TModifier[] modifiers, TParam[] @params);
    TStatement InterfaceMethod(string name, TType returnType, TModifier[] modifiers);
    TStatement InterfaceMethod(string name, TType returnType);
    TStatement InterfaceMethod(string name, TType returnType, TModifier[] modifiers, TParam[] @params, TAttribute[] attributes);
    TParam ParamInfo(string name, TType type);
    TAttribute Attribute(string name, params TValue[] arguments);
    TValue StringConst(string value);
}