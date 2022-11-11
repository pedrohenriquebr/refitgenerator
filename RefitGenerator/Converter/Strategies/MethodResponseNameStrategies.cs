//using NJsonSchema;

namespace RefitGenerator.Converter.Strategies;

public static class MethodResponseNameStrategies
{
    /// <summary>
    /// Add "Response" suffix to name of Method <br/>
    /// Eg. CreateUser -> CreateUserResponse
    /// </summary>
    public static IMethodResponseNameStrategy ResponseSuffixStrategy = new ResponseSuffixNameStrategy();
}
