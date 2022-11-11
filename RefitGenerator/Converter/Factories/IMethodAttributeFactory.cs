using Microsoft.OpenApi.Models;
//using NJsonSchema;
using RefitGenerator.Generators.CSharp.Behaviors;

namespace RefitGenerator.Converter.Factories;

public interface IMethodAttributeFactory
{
    IAttributeBehavior Create(string path, OperationType key);
}
