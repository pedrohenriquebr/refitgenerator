namespace RefitGenerator.Core;

public interface ITypesAndMembersAlgebraObject<TStatement, TFile, TModifier, TType, TPropAccessor> : IMultipleStatementsAlgebraObject<TStatement, TFile, TModifier>
{
    TType Integer();
    TType String();
    TType Double();
    TType Float();
    TType Decimal();
    TType Boolean();
    TType Void();
    TType Type(string name);
    TType Type<T>();

    TModifier Public();
    TModifier Private();
    TModifier Static();
    TModifier Internal();
    TModifier Protected();
    TStatement Member(string name, TType type, TModifier[] modifiers);
    TStatement Member(string name, TType type);
    TStatement Property(string name, TType type, TModifier[] modifiers, TPropAccessor[] accessors);
    TStatement Property(string name, TType type, TModifier[] modifiers);
    TStatement Property(string name, TType type);
    TPropAccessor Getter(TModifier modifier);
    TPropAccessor Setter(TModifier modifier);
    TPropAccessor Getter();
    TPropAccessor Setter();
}
