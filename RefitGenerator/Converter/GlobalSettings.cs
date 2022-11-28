using RefitGenerator.Converter.Strategies;

namespace RefitGenerator.Converter;

public static class GlobalSettings
{
    public static IMethodResponseNameStrategy MethodResponseNameStrategy  = MethodResponseNameStrategies.ResponseSuffixStrategy;
    public static IMethodNameStrategy MethodNameStrategy = MethodNameStrategies.TitleCaseStrategy;
    public static IClassPropNameStrategy ClassPropNameStrategy = ClassPropNameStrategies.TitleCaseStrategy;
}
