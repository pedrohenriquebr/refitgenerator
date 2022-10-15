using RefitGenerator.Core;
using RefitGenerator.Factories;
using RefitGenerator.Util;
using Xunit;
namespace RefitGenerator.Tests;


public class RefitTests
{
    private readonly string EOF = "\r\n";
    private readonly string INDENT = " ";
    private readonly MethodAlgebraGenerator factory;
    private readonly DefaultSourceFormatter formatterProvider;
    public RefitTests()
    {
        formatterProvider = new DefaultSourceFormatter(EOF, INDENT);
        factory = new MethodAlgebraGenerator(formatterProvider);
    }


    [Fact]
    public void UsingNamespace_Should_Generate()
    {
        string expected = $"using System;{EOF}" +
                          $"using Microsoft.CodeAnalysis.CSharp.Syntax;{EOF}" +
                          $"using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;{EOF}" +
                          $"using System.Dynamic;{EOF}" +
                          $"using System.Net.Http.Headers;{EOF}" +
                          $"using Xunit;{EOF}";
        string actual = GenerateUsingNameSpace(factory)
            .Generate();

        Assert.Equal(expected, actual);
    }


    [Fact]
    public void Namespace_Should_Generate()
    {
        string expected = $"namespace Test.Api;{EOF}";
        string actual = factory.Compose(factory.Namespace("Test.Api"))
            .Generate();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ClassDeclaration_EmptyBody_Should_Generate()
    {
        string expected = $"public class UserInputModel{EOF}{{{EOF}}}";
        string actual = factory.Class("UserInputModel")
            .Generate();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void InterfaceDeclaration_EmptyBody_Should_Generate()
    {
        string expected = $"public interface IWebApiService" +
                            $"{EOF}{{{EOF}" +
                            $"}}";
        string actual = factory.Interface("IWebApiService")
            .Generate();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ClassDeclarationAndNamespaces_WithNamespaceDeclaration_And_Usings_And_ClassWith_EmptyBody_Should_Generate()
    {
        string expected = $"using System;{EOF}" +
            $"namespace TestWebApi.Application.Models;{EOF}" +
            $"public class UserInputModel{EOF}" +
            $"{{{EOF}" +
            $"}}";
        string actual = GenerateNamespacesAndEmptyClass(factory)
            .Generate();

        Assert.Equal(expected, actual);
    }


    [Fact]
    public void ClassDeclaration_WithOneMember_Should_Generate()
    {
        string expected = $"public class UserInputModel{EOF}{{{EOF}" +
            $"public int RandomInteger;{EOF}" +
            $"}}";
        string actual = factory
            .Class("UserInputModel",
                factory.Compose(
                    factory.Member("RandomInteger", factory.Integer(), new[] { factory.Public() })
                    )
                )
            .Generate();

        Assert.Equal(expected, actual);
    }


    [Fact]
    public void ClassDeclaration_WithMultipleMembers_Should_Generate()
    {
        string expected = $"public class UserInputModel{EOF}" +
            $"{{{EOF}" +
            $"public int RandomInteger;{EOF}" +
            $"private double X;{EOF}" +
            $"public static float K;{EOF}" +
            $"}}";
        string actual = factory
            .Class("UserInputModel",
                factory.Compose(
                    factory.Member("RandomInteger",
                        factory.Integer(),
                        new[] { factory.Public() }),
                    factory.Member("X",
                        factory.Double(),
                        new[] { factory.Private() }),
                    factory.Member("K",
                        factory.Float(),
                        new[] { factory.Public(), factory.Static() })
                    )
                  )
            .Generate();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ClassDeclaration_WithMultipleMembersAndProperties_Should_Generate()
    {
        string expected = $"public class UserInputModel{EOF}" +
            $"{{{EOF}" +
            $"public int RandomInteger;{EOF}" +
            $"private double X;{EOF}" +
            $"public static float K;{EOF}" +
            $"protected double C {{ get; set; }}{EOF}" +
            $"public static float D {{ get; set; }}{EOF}" +
            $"}}";
        string actual = factory
            .Class("UserInputModel",
                factory.Block(
                    factory.Compose(
                        factory.Member("RandomInteger",
                            factory.Integer(),
                            new[] { factory.Public() }),
                        factory.Member("X",
                            factory.Double(),
                            new[] { factory.Private() }),
                        factory.Member("K",
                            factory.Float(),
                            new[] { factory.Public(), factory.Static() })
                        ),
                    factory.Property("C",
                        factory.Double(),
                        new[] { factory.Protected() }),
                    factory.Property("D",
                        factory.Float(),
                        new[] { factory.Public(), factory.Static() })
                )
                  )
            .Generate();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ClassDeclaration_WithOneProperty_Should_Generate()
    {
        string expected =
            $"public class UserInputModel{EOF}" +
            $"{{{EOF}" +
            $"public int RandomInteger {{ get; set; }}{EOF}" +
            $"}}";
        string actual = factory
            .Class("UserInputModel",
                factory.Block(
                    factory.Property("RandomInteger", factory.Integer(), new[] { factory.Public() })
             ))
            .Generate();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ClassDeclaration_WithMultipleProperties_Should_Generate()
    {
        string expected =
             $"public class UserInputModel{EOF}" +
            $"{{{EOF}" +
            $"public int A {{ get; set; }}{EOF}" +
            $"private string B {{ get; set; }}{EOF}" +
            $"protected double C {{ get; set; }}{EOF}" +
            $"public static float D {{ get; set; }}{EOF}" +
            $"private static int E {{ get; set; }}{EOF}" +
            $"int JJ {{ get; set; }}{EOF}" +
            $"}}";
        string actual = factory
            .Class("UserInputModel",
                factory.Block(
                    factory.Property("A", factory.Integer(), new[] { factory.Public() }),
                    factory.Property("B", factory.String(), new[] { factory.Private() }),
                    factory.Property("C", factory.Double(), new[] { factory.Protected() }),
                    factory.Property("D", factory.Float(), new[] { factory.Public(), factory.Static() }),
                    factory.Property("E", factory.Integer(), new[] { factory.Private(), factory.Static() }),
                    factory.Property("JJ", factory.Integer())
                    ))
            .Generate();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ClassDeclaration_WithMultipleAccessorsProperty_Should_Generate()
    {
        string expected =
            $"public class UserInputModel{EOF}" +
            $"{{{EOF}" +
            $"public int F {{ public get; private set; }}{EOF}" +
            $"public int G {{ private get; public set; }}{EOF}" +
            $"public int H {{ public set; }}{EOF}" +
            $"public int I {{ private set; }}{EOF}" +
            $"public int J {{ private get; }}{EOF}" +
            $"public int K {{ get; }}{EOF}" +
            $"public int L {{ set; }}{EOF}" +
            $"}}";
        string actual = factory
            .Class("UserInputModel",
                factory.Block(

                    factory.Property("F",
                        factory.Integer(),
                        new[] { factory.Public() },
                        new[] { factory.Getter(factory.Public()), factory.Setter(factory.Private()) }),

                    factory.Property("G",
                        factory.Integer(),
                        new[] { factory.Public() },
                        new[] { factory.Getter(factory.Private()), factory.Setter(factory.Public()) }),

                    factory.Property("H",
                        factory.Integer(),
                        new[] { factory.Public() },
                        new[] { factory.Setter(factory.Public()) }),

                     factory.Property("I",
                        factory.Integer(),
                        new[] { factory.Public() },
                        new[] { factory.Setter(factory.Private()) }),

                      factory.Property("J",
                        factory.Integer(),
                        new[] { factory.Public() },
                        new[] { factory.Getter(factory.Private()) }),

                      factory.Property("K",
                        factory.Integer(),
                        new[] { factory.Public() },
                        new[] { factory.Getter() }),

                      factory.Property("L",
                        factory.Integer(),
                        new[] { factory.Public() },
                        new[] { factory.Setter() })

             ))
            .Generate();

        Assert.Equal(expected, actual);
    }


    [Fact]
    public void Type_Should_Genereate()
    {
        string expected = "int";
        string actual = factory
                .Integer()
                .Generate();

        Assert.Equal(expected, actual);
    }


    [Fact]
    public void CustomType_Should_Genereate()
    {
        string expected = "UserType";
        string actual = factory
                .Type("UserType")
                .Generate();

        Assert.Equal(expected, actual);
    }


    [Fact]
    public void CustomType2_Should_Genereate()
    {
        string actual = factory
                .Type<DateTime>()
                .Generate();

        string expected = "DateTime";
        Assert.Equal(expected, actual);
    }


    [Fact]
    public void InterfaceDeclaration_OneMethod_Should_Generate()
    {
        string expected = $"public interface IWebApiService{EOF}" +
                            $"{{{EOF}" +
                            $"public Task<CreateResponse> Create(CreateRequest request);{EOF}" +
                            $"public Task<DeleteResponse> Delete(int i);{EOF}" +
                            $"public Task<SearchResponse> Search(SearchRequest searchRequest);{EOF}" +
                            $"}}";
        string actual = factory.Interface("IWebApiService",
                                    modifiers: new[] { factory.Public() },
                                    body: factory.Block(
                                        factory.Compose(
                                        factory.InterfaceMethod("Create",
                                                returnType: factory.Type(name: "Task<CreateResponse>"),
                                                modifiers: new[] { factory.Public() },
                                                @params: new[] { factory.ParamInfo("request", factory.Type("CreateRequest")) }
                                                ),
                                        factory.InterfaceMethod("Delete",
                                                returnType: factory.Type("Task<DeleteResponse>"),
                                                modifiers: new[] { factory.Public() },
                                                @params: new[] { factory.ParamInfo("i", factory.Integer()) }
                                                ),
                                        factory.InterfaceMethod("Search",
                                                returnType: factory.Type("Task<SearchResponse>"),
                                                modifiers: new[] { factory.Public() },
                                                @params: new[] { factory.ParamInfo("searchRequest", factory.Type("SearchRequest")) }
                                                )
                                             )
                                        )
                                    ).Generate();

        Assert.Equal(expected, actual);
    }


    [Fact]
    public void InterfaceMethod_WithAttribute_Should_Generate()
    {
        var expected = $"[Get(\"/api/test\")]{EOF}" +
                       $"public Task<int> DoIt(int a, string b);{EOF}";

        var result = factory.Compose(
                        factory.InterfaceMethod(
                                        name: "DoIt",
                                        returnType: factory.Type("Task<int>"),
                                        modifiers: new[] { factory.Public() },
                                        @params: new[] {factory.ParamInfo("a", factory.Integer()), 
                                                        factory.ParamInfo("b", factory.String())},
                                        attributes: new [] {factory.Attribute("Get", factory.StringConst("/api/test"))}))
                    .Generate();

        Assert.Equal(expected, result);
    }


    [Fact]
    public void InterfaceMethod_MultipleMethods_Should_Generate()
    {
        var expected = $"public interface IWebApiService" +
                            $"{EOF}{{" +
                            $"{EOF}" +
                            $"[Post(\"/api/user\")]{EOF}" +
                            $"public Task<UserCreated> CreateUser([Body] CreateUserAction action);{EOF}" +
                            $"[Get(\"/api/user\")]{EOF}" +
                            $"public Task<User> GetUser(GetUserQuery query);{EOF}" +
                            $"}}";

        var result = factory.Interface("IWebApiService",
                            body: factory.Compose(
                                    factory.InterfaceMethod(
                                                    name: "CreateUser",
                                                    returnType: factory.Type("Task<UserCreated>"),
                                                    modifiers: new[] { factory.Public() },
                                                    @params: new[] { factory.ParamInfo("action", 
                                                                        type: factory.Type("CreateUserAction"), 
                                                                        attributes: new [] { factory.Attribute("Body") }) },
                                                    attributes: new[] { factory.Attribute("Post", factory.StringConst("/api/user")) }),
                                    factory.InterfaceMethod(
                                                    name: "GetUser",
                                                    returnType: factory.Type("Task<User>"),
                                                    modifiers: new[] { factory.Public() },
                                                    @params: new[] { factory.ParamInfo("query", factory.Type("GetUserQuery")) },
                                                    attributes: new[] { factory.Attribute("Get", factory.StringConst("/api/user")) })
                                 )
                            )
                    .Generate();

        Assert.Equal(expected, result);
    }


    [Fact]
    public void SourceFormatter_Format_Extension()
    {
        var expected = $"public interface IWebApiService" +
                            $"{EOF}{{" +
                            $"{EOF}" +
                            $"{INDENT}[Post(\"/api/user\")]{EOF}" +
                            $"{INDENT}public Task<UserCreated> CreateUser([Body] CreateUserAction action);{EOF}" +
                            $"{INDENT}[Get(\"/api/user\")]{EOF}" +
                            $"{INDENT}public Task<User> GetUser(GetUserQuery query);{EOF}" +
                            $"}}";

        var result = factory.Interface("IWebApiService",
                            body: factory.Compose(
                                    factory.InterfaceMethod(
                                                    name: "CreateUser",
                                                    returnType: factory.Type("Task<UserCreated>"),
                                                    modifiers: new[] { factory.Public() },
                                                    @params: new[] { factory.ParamInfo("action", 
                                                                        type: factory.Type("CreateUserAction"), 
                                                                        attributes: new [] { factory.Attribute("Body") }) },
                                                    attributes: new[] { factory.Attribute("Post", factory.StringConst("/api/user")) }),
                                    factory.InterfaceMethod(
                                                    name: "GetUser",
                                                    returnType: factory.Type("Task<User>"),
                                                    modifiers: new[] { factory.Public() },
                                                    @params: new[] { factory.ParamInfo("query", factory.Type("GetUserQuery")) },
                                                    attributes: new[] { factory.Attribute("Get", factory.StringConst("/api/user")) })
                                 )
                            )
                    .Generate();

        var formatted = formatterProvider.FormatCode(result);
        Assert.Equal(expected, formatted);
    }


    [Fact]
    public void Regions_Should_Generate()
    {
        string expected = $"#region Service {EOF}" +
                          $"{EOF}" +
                          $"public interface IWebApiService" +
                          $"{EOF}{{" +
                          $"{EOF}" +
                          $"{INDENT}[Post(\"/api/user\")]{EOF}" +
                          $"{INDENT}public Task<UserCreated> CreateUser([Body] CreateUserAction action);{EOF}" +
                          $"{INDENT}[Get(\"/api/user\")]{EOF}" +
                          $"{INDENT}public Task<User> GetUser(GetUserQuery query);{EOF}" +
                          $"}}{EOF}{EOF}"+
                          $"#endregion{EOF}";

        string result = factory.Region("Service",
            factory.Interface("IWebApiService",
                            body: factory.Compose(
                                    factory.InterfaceMethod(
                                                    name: "CreateUser",
                                                    returnType: factory.Type("Task<UserCreated>"),
                                                    modifiers: new[] { factory.Public() },
                                                    @params: new[] { factory.ParamInfo("action",
                                                                        type: factory.Type("CreateUserAction"),
                                                                        attributes: new [] { factory.Attribute("Body") }) },
                                                    attributes: new[] { factory.Attribute("Post", factory.StringConst("/api/user")) }),
                                    factory.InterfaceMethod(
                                                    name: "GetUser",
                                                    returnType: factory.Type("Task<User>"),
                                                    modifiers: new[] { factory.Public() },
                                                    @params: new[] { factory.ParamInfo("query", factory.Type("GetUserQuery")) },
                                                    attributes: new[] { factory.Attribute("Get", factory.StringConst("/api/user")) })
                                 )
                            ))
                    .Generate();
        Assert.Equal(expected,result);
    }

    public static T GenerateNamespacesAndEmptyClass<T>(IMultipleStatementsAlgebra<T, IFileBehavior, IModifierBehavior> factory)
    => factory.Root(
        factory.Compose(
             factory.UsingNamespace("System"),
             factory.Namespace("TestWebApi.Application.Models")
         ),
         factory.Class("UserInputModel")
       );

    public static T GenerateUsingNameSpace<T>(IStatementsAlgebra<T, IFileBehavior, IModifierBehavior> factory)
        => factory.Compose(
                factory.UsingNamespace("System"),
                factory.UsingNamespace("Microsoft.CodeAnalysis.CSharp.Syntax"),
                factory.UsingNamespace("Microsoft.EntityFrameworkCore.ChangeTracking.Internal"),
                factory.UsingNamespace("System.Dynamic"),
                factory.UsingNamespace("System.Net.Http.Headers"),
                factory.UsingNamespace("Xunit")
            );

}
