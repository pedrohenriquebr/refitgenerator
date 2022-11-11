namespace RefitGenerator.Converter.Strategies
{
    public class TitleCasePropNameStrategy : IClassPropNameStrategy
    {
        public string Create(string propName)
        {
            return propName;
        }
    }
}