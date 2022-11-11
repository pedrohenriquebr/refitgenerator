using RefitGenerator.Converter.Models;

namespace RefitGenerator.Converter.Factories;

public interface IModelsFactory
{
    ClassModel CreateClassForArrayItem(string key);
    ClassPropModel CreateProForNestedObject(string propName);
    ClassModel CreateClassForNestedObject(string propName);
    ClassPropModel CreatePropForEmptyArray(string propName);
    ClassPropModel CreatePropForArray(string propName, string generic);
    ClassPropModel CreatePropForArrayOfObjects(string propName);
    ClassModel CreateClass(string name);
    ClassPropModel CreateProp(string name, string type);
}
