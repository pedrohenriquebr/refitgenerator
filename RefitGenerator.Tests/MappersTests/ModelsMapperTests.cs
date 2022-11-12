using Moq;
using RefitGenerator.Converter.Factories;
using RefitGenerator.Converter.Mappers;
using RefitGenerator.Converter.Models;

namespace RefitGenerator.Tests.MappersTests;

public class ModelsMapperTests
{
    private readonly IModelsMapper modelsMapper;
    private readonly Mock<IModelsFactory> stubModelsFactory;
    public ModelsMapperTests()
    {
        stubModelsFactory = new Mock<IModelsFactory>();
        modelsMapper = new DefaultModelsMapper(stubModelsFactory.Object);
    }

    [Fact]
    public void MapExamplesJsonList_EmptyExamplesJsonList_ReturnsEmptyListOfClassModels()
    {
        var examplesJsonList = new List<ExampleJsonModel>();
        var classModelsListResult = modelsMapper.Map(examplesJsonList);

        Assert.Empty(classModelsListResult);
    }

    [Fact]
    public void MapExamplesJsonList_ExamplesJsonListWithNullItem_ReturnsEmptyListOfClassModels()
    {
        var examplesJsonList = new List<ExampleJsonModel>();
        examplesJsonList.Add(null);
        var classModelsListResult = modelsMapper.Map(examplesJsonList);

        Assert.Empty(classModelsListResult);
    }

    [Fact]
    public void MapExamplesJsonList_ExamplesJsonListWithOneJsonPropNull_ReturnsEmptyListOfClassModels()
    {
        var examplesJsonList = new List<ExampleJsonModel>();
        examplesJsonList.Add(new ExampleJsonModel(
            OperationId: "Example",
            Json: null
        ));


        var classModelsListResult = modelsMapper.Map(examplesJsonList);

        Assert.Empty(classModelsListResult);
    }

}
