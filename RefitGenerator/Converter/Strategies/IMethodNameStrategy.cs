//using NJsonSchema;

namespace RefitGenerator.Converter.Strategies;

public interface IMethodNameStrategy
{
    string Create(string operationId);
}
