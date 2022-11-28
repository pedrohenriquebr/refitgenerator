using RefitGenerator.Converter.Models;
using RefitGenerator.Converter.Strategies;

namespace RefitGenerator.Converter.Factories;

public class ModelsFactory : IModelsFactory
{
    private readonly INormalizationStrategy _normalizationStrategy;

    public ModelsFactory(INormalizationStrategy normalizationStrategy)
    {
        this._normalizationStrategy = normalizationStrategy;
    }

    public ClassModel CreateClassForArrayItem(string key)
    {
        return CreateClass($"{key}Item");
    }

    public ClassPropModel CreatePropForNestedObject(string propName)
    {
        var model = CreateProp(propName, GlobalSettings.MethodResponseNameStrategy.Create(_normalizationStrategy.NormalizeName(propName)));
        return model with
        {
            Name = GlobalSettings.ClassPropNameStrategy.Create(model.Name)
        };
    }

    public ClassModel CreateClassForNestedObject(string propName)
    {
        var model = CreateClass(propName);
        return model with
        {
            Name = GlobalSettings.MethodResponseNameStrategy.Create(model.Name)
        };
    }

    public ClassPropModel CreatePropForEmptyArray(string propName)
    {
        return CreateProp(propName, "List<object>");
    }

    public ClassPropModel CreatePropForArray(string propName, string generic)
    {
        return CreateProp(propName, $"List<{_normalizationStrategy.NormalizeName(generic)}>");
    }

    public ClassPropModel CreatePropForArrayOfObjects(string propName)
    {
        return CreatePropForArray(propName, $"{_normalizationStrategy.NormalizeName(propName)}Item");
    }

    public ClassModel CreateClass(string name)
    {
        return new ClassModel
        {
            Name = _normalizationStrategy.NormalizeName(name)
        };
    }

    public ClassPropModel CreateProp(string name, string type)
    {
        return new ClassPropModel
        {
            Name = _normalizationStrategy.NormalizeName(name),
            Type = type
        };
    }
}
