using Microsoft.CodeAnalysis;
using RefitGenerator.Converter;
using RefitGenerator.Converter.Mappers;
using RefitGenerator.Converter.Strategies;
using RefitGenerator.Tests.Base;
using System;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace RefitGenerator.Tests.OpenApiTests;
public class OpenApiParserTests : BaseTests
{
    

    //[Fact]
    //public void Should_Generate_For_OpenApi()
    //{
    //    var factory = CreateFactory();
    //    var openApiReader = CreateOpenApiReader();
    //    var (input, expected) = LoadTestAssets("SimpleApi");
    //    string output = new DefaultOpenApiConverter(openApiReader, factory)
    //        .Convert(input);

    //    Assert.Equal(expected, output);
    //}

    [Fact]
    public void Should_Generate_For_OpenApiWithSchema()
    {

        var sourceCodeBuilder = CreateSourceCodeBuilder();
        var openApiReader = CreateOpenApiReader();
        var (input, expected) = LoadTestFiles("Schema");
        string output = new DefaultOpenApiConverter(openApiReader, sourceCodeBuilder)
            .Convert(input, "ISimpleOpenApiOverviewService");

        Assert.Equal(expected, output);
    }

    [Fact]
    public void Should_Traverse_ObjJson()
    {
        var input = @"{
    ""versions"": [
        {
            ""status"": ""CURRENT"",
            ""updated"": ""2011-01-21T11:33:21Z"",
            ""id"": ""v2.0"",
            ""links"": [
                {
                    ""href"": ""http://127.0.0.1:8774/v2/"",
                    ""rel"": ""self""
                }
            ]
        },
        {
            ""status"": ""EXPERIMENTAL"",
            ""updated"": ""2013-07-23T11:33:21Z"",
            ""id"": ""v3.0"",
            ""links"": [
                {
                    ""href"": ""http://127.0.0.1:8774/v3/"",
                    ""rel"": ""self""
                }
            ]
        }
    ]
}
";
        var json = JsonSerializer.Deserialize<JsonNode>(input);

        var output = json.Map("Test");

        Assert.NotNull(output);
    }

    [Theory]
    [InlineData("MediaType", "media-type")]
    [InlineData("MediaType", "mediaType")]
    [InlineData("Name", "name")]
    [InlineData("Nameprop", "nameprop")]
    public void Should_Normalize_PropName(string expected, string name)
    {
        var normalizationStrategy = new DefaultNormalizationStrategy();
        var result = normalizationStrategy.NormalizeName(name);
        Assert.Equal(expected, result);
    }
}
