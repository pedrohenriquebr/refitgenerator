namespace RefitGenerator.Core;

public interface IRefitExpAlgebraObject<TStatement, TFile>
{

    TStatement Compose(params TStatement[] stmts);
    TStatement Root(params TStatement[] stmts);
    TFile File(string s, TStatement[] statements);
}
