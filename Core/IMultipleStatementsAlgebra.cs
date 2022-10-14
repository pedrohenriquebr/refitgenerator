namespace RefitGenerator.Core;

public interface IMultipleStatementsAlgebra<TStatement, TFile, TModifier> : IStatementsAlgebra<TStatement, TFile, TModifier>
{
    TStatement Block(params TStatement[] statements);
    TStatement Interface(string name, TStatement body);
    TStatement Interface(string name);
    TStatement Blank();
}
