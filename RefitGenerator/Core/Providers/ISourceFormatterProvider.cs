namespace RefitGenerator.Core.Providers;

public interface ISourceFormatterProvider
{
    public string GetEof();
    public string GetEof(int n);
    public string GetIndent(int n);
    public string GetIndent();
}
