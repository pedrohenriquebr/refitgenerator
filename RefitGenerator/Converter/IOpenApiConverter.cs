﻿using Microsoft.OpenApi.Models;
using RefitGenerator.Generators.CSharp.Behaviors;

namespace RefitGenerator.Converter;

public interface IOpenApiConverter
{
    public string Convert(string openApiJson);
    public string CreateMethodName(string operationId);
    public string CreateMethodResponseName(string methodName);
    public IAttributeBehavior CreateMethodAttribute(string path, OperationType key);
    public IStatementBehavior CreateInterfaceMethod(string path, KeyValuePair<OperationType, OpenApiOperation> operation);
}