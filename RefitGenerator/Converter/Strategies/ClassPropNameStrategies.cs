using RefitGenerator.Converter.Mappers;

namespace RefitGenerator.Converter.Strategies;

public static class ClassPropNameStrategies
{
    public static IClassPropNameStrategy TitleCaseStrategy = new TitleCasePropNameStrategy();
}
