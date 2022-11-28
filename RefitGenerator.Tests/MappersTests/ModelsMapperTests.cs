using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using RefitGenerator.Converter.Factories;
using RefitGenerator.Converter.Mappers;
using RefitGenerator.Converter.Models;
using RefitGenerator.Converter.Strategies;
using RefitGenerator.Tests.Base;
using System.Text.Json.Nodes;

namespace RefitGenerator.Tests.MappersTests;

public class ModelsMapperTests : BaseTests
{
    private readonly IModelsMapper modelsMapper;
    public ModelsMapperTests()
    {
        modelsMapper = BuildServiceProvider()
            .GetRequiredService<IModelsMapper>();
    }

    [Fact]
    public void MapExamplesJsonList_EmptyExamplesJsonList_ReturnsEmptyListOfClassModels()
    {
        var emptyExamplesJsonList = new List<ExampleJsonModel>();

        var classModelsListResult = modelsMapper.Map(emptyExamplesJsonList);

        Assert.Empty(classModelsListResult);
    }

    [Fact]
    public void MapExamplesJsonList_ExamplesJsonListWithNullItem_ReturnsEmptyListOfClassModels()
    {
        var examplesJsonListWithNull = new List<ExampleJsonModel>();
        examplesJsonListWithNull.Add(null);

        var classModelsListResult = modelsMapper.Map(examplesJsonListWithNull);

        Assert.Empty(classModelsListResult);
    }

    [Fact]
    public void MapExamplesJsonList_ExamplesJsonListWithOneJsonPropNull_ReturnsEmptyListOfClassModels()
    {
        var examplesJsonList = new List<ExampleJsonModel>();
        var exampleJsonWithNull = new ExampleJsonModel(
            OperationId: "Example",
            Json: null
        );
        examplesJsonList.Add(exampleJsonWithNull);


        var classModelsListResult = modelsMapper.Map(examplesJsonList);

        Assert.Empty(classModelsListResult);
    }

    [Fact]
    public void MapJsonNode_NullJsonNode_ReturnsEmptyListOfClassModels()
    {
        JsonNode? nullNodeShouldBeEmptyList = null;
        var rootName = "teste";

        var classModelsListResult = modelsMapper.Map(nullNodeShouldBeEmptyList, rootName);

        Assert.Empty(classModelsListResult);
    }


    [Fact]
    public void MapJsonNode_JsonNodeWithOnePrimitiveProp_ReturnsListWithOneClassModel()
    {
        var expectedClassModelList = CreateClassModelWithOnePrimitiveProp();
        var rootName = expectedClassModelList.First().Name;
        var propName = expectedClassModelList.First().Props.First().Name;
        JsonNode? shouldBeOneClassModel = new JsonObject()
        {
            [propName] = JsonNode.Parse("\"nothing here\"")
        };

        var classModelsListResult = modelsMapper.Map(shouldBeOneClassModel, rootName);

        Assert.All(expectedClassModelList, c => classModelsListResult.Contains(c));
    }


    [Fact]
    public void MapJsonNode_JsonNodeWithMultiplePrimitiveProps_ReturnsListWithOneClassModel()
    {
        var expectedClassModelList = CreateClassModelWithMultiplePrimitiveProps();
        var rootName = expectedClassModelList.First().Name;
        var propName = expectedClassModelList.First().Props.First().Name;
        JsonNode? shouldBeOneClassModel = new JsonObject()
        {
            [propName] = JsonNode.Parse("\"nothing here\"")
        };

        var classModelsListResult = modelsMapper.Map(shouldBeOneClassModel, rootName);

        Assert.All(expectedClassModelList, c => classModelsListResult.Contains(c));
    }

    public List<ClassModel> CreateClassModelWithMultiplePrimitiveProps()
    {
        return new(){
            new ClassModel()
            {
                Name = "ClassModel",
                Props = new List<ClassPropModel>()
                {
                    new ClassPropModel()
                    {
                        Name = "PropName",
                        Type = "int"
                    },
                    new ClassPropModel()
                    {
                        Name = "PropName",
                        Type = "string"
                    },
                    new ClassPropModel()
                    {
                        Name = "PropName",
                        Type = "int"
                    }
                }
            }
        };
    }

    public List<ClassModel> CreateClassModelWithOnePrimitiveProp()
    {
        return new(){
            new ClassModel()
            {
                Name = "ClassModel",
                Props = new List<ClassPropModel>()
                {
                    new ClassPropModel()
                    {
                        Name = "PropName",
                        Type = "string"
                    }
                }
            }
        };
    }
}