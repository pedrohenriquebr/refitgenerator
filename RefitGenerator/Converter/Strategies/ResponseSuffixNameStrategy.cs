namespace RefitGenerator.Converter.Strategies
{
    public class ResponseSuffixNameStrategy : IMethodResponseNameStrategy
    {
        public string Create(string operationName) => operationName + "Response";
    }
}