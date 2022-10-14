using RefitGenerator.Core;

namespace RefitGenerator.Factories;

public class DefaultSourceFormatter : ISourceFormatterProvider
{
    public static string WINDOWS_EOF => "\r\n";
    public static string LINUX_EOF => "\n";
    public static string TAB_INDENT => "\t";
    public static string SPACE_INDENT => " ";

    private readonly string _indentation;
    private readonly string _eof;

    public DefaultSourceFormatter()
        : this(WINDOWS_EOF, SPACE_INDENT)
    {

    }

    public DefaultSourceFormatter(string eof, string indentation)
    {
        _eof = eof;
        _indentation = indentation;
    }

    public string GetEof()
    {
        return _eof;
    }

    public string GetEof(int n)
    {
        return string.Concat(Enumerable.Repeat(_eof, n));
    }

    public string GetIndent(int n)
    {
        return string.Concat(Enumerable.Repeat(_indentation, n));
    }

    public string GetIndent()
    {
        return _indentation;
    }
}
