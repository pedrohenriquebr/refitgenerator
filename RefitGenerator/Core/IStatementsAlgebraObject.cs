namespace RefitGenerator.Core;

public interface IStatementsAlgebraObject<TStatement, TFile, TModifier> : IRefitExpAlgebraObject<TStatement, TFile>
{
    TStatement UsingNamespace(string s);
    TStatement Namespace(string s);
    TStatement Interface(string name, TModifier[] modifiers, TStatement body);
    TStatement Class(string name, TStatement body);
    TStatement Class(string name);
    TStatement Region(string name, TStatement body);
}
