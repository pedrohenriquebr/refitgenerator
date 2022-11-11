//using NJsonSchema;

namespace RefitGenerator.Converter.Strategies;

public class TitleCaseNameStrategy : IMethodNameStrategy
{
    public string Create(string operationId)
     => operationId[0].ToString().ToUpper() + operationId[1..].ToString();
}