using EasyAccept.Core;

namespace EasyAccept.Tests;

public class MainTest
{
    [Fact]
    public void Test1()
    {
        EasyAcceptFacade<object> facade = new(new object(), ["tests/test_1.easy"]);
        facade.ExecuteTests();
        Assert.Empty(facade.GetCompleteResults());
    }

    [Fact]
    public void Test2()
    {
        EasyAcceptFacade<object> facade = new(new object(), ["tests/test_2.easy"]);
        facade.ExecuteTests();
        Assert.Equal("Echo instruction is present.\n", facade.GetCompleteResults());
    }

    [Fact]
    public void Test3()
    {
        EasyAcceptFacade<object> facade = new(new object(), ["tests/test_3.easy"]);
        facade.ExecuteTests();
        Assert.Equal("Quit command executed.\n", facade.GetCompleteResults());
    }

    [Fact]
    public void Test4()
    {
        EasyAcceptFacade<object> facade = new(new FakeFacade(), ["tests/test_4.easy"]);
        facade.ExecuteTests();
        Console.WriteLine(facade.GetCompleteResults());
        Assert.Empty(facade.GetCompleteResults());
    }

    [Fact]
    public void Test5()
    {
        EasyAcceptFacade<object> facade = new(new FakeFacade(), ["tests/test_5.easy"]);
        facade.ExecuteTests();
        Assert.Equal("Expect command failed. Expected: \"42\", Actual: \"This method has a wrong return type for testing.\"\n", facade.GetCompleteResults());
    }

    [Fact]
    public void Test6()
    {
        EasyAcceptFacade<object> facade = new(new FakeFacade(), ["tests/test_6.easy"]);
        facade.ExecuteTests();
        Assert.Equal(
            "ExpectError command failed. Expected: \"Different exception message\", Actual: \"A exception was occurred\"\n" +
            "ExpectError command failed. Expected: \"No exception\", but no error was thrown.\n",
            facade.GetCompleteResults());
    }

    [Fact]
    public void Test7()
    {
        EasyAcceptFacade<object> facade = new(new FakeFacade(), ["tests/test_7.easy"]);
        facade.ExecuteTests();
        Assert.Equal("The value of a is 42\n", facade.GetCompleteResults());
    }
}