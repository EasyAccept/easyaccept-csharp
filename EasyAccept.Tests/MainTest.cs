using EasyAccept.Core;

namespace EasyAccept.Tests;

public class MainTest
{
    [Fact]
    public void Test1()
    {
        EasyAcceptFacade<object> facade = new(new object(), ["tests/test_1.easy"]);
        facade.ExecuteTests();
        Console.WriteLine(facade.GetCompleteResults());
    }

    [Fact]
    public void Test2()
    {
        EasyAcceptFacade<object> facade = new(new object(), ["tests/test_2.easy"]);
        facade.ExecuteTests();
        Console.WriteLine(facade.GetCompleteResults());
    }

    [Fact]
    public void Test3()
    {
        EasyAcceptFacade<object> facade = new(new object(), ["tests/test_3.easy"]);
        facade.ExecuteTests();
        Console.WriteLine(facade.GetCompleteResults());
    }

    [Fact]
    public void Test4()
    {
        EasyAcceptFacade<object> facade = new(new FakeFacade(), ["tests/test_4.easy"]);
        facade.ExecuteTests();
        Console.WriteLine(facade.GetCompleteResults());
    }
}