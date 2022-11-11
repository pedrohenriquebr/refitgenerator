using System.Text.RegularExpressions;

namespace RefitGenerator.Converter.Strategies;

public class DefaultNormalizationStrategy : INormalizationStrategy
{
    public string NormalizeName(string name)
    {
        const string pattern = @"(-|_)\w{1}|^\w";
        string modified = name[0].ToString().ToUpper() + name[1..];
        return Regex.Replace(modified, pattern, match => match.Value.Replace("-", string.Empty).Replace("_", string.Empty).ToUpper());
    }
}
