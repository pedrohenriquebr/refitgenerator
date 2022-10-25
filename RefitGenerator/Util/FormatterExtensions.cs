using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using RefitGenerator.Core.Providers;

namespace RefitGenerator.Util;

public static class FormatterExtensions
{
    public static string? FormatCode(this ISourceFormatterProvider formatterProvider, string source)
    {
        var tree = CSharpSyntaxTree.ParseText(source);
        var root = tree?.GetRoot().NormalizeWhitespace(indentation: formatterProvider.GetIndent(),
                                                       eol: formatterProvider.GetEof());
        var ret = root.ToFullString();
        return ret;
    }
}
