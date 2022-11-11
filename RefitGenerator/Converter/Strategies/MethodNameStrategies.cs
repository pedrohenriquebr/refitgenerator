//using NJsonSchema;

namespace RefitGenerator.Converter.Strategies;

public static class MethodNameStrategies
{
    public static IMethodNameStrategy TitleCaseStrategy = new TitleCaseNameStrategy();
}
