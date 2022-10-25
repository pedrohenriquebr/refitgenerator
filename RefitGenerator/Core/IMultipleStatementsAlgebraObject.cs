namespace RefitGenerator.Core;

public interface IMultipleStatementsAlgebraObject<TStatement, TFile, TModifier> : IStatementsAlgebraObject<TStatement, TFile, TModifier>
{
    TStatement Block(params TStatement[] statements);
    TStatement Interface(string name, TStatement body);
    TStatement Interface(string name);
    TStatement Blank();
}
