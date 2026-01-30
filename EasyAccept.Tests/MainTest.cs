using EasyAccept.Core;

namespace EasyAccept.Tests;

public class MainTest
{
    [Fact]
    public void Test1()
    {
        EasyAcceptFacade<object> facade = new(new object(), ["tests/test_1.easy"]);
        facade.ExecuteTests();
        Assert.Equal("Test file: tests/test_1.easy | Passed Tests: 0 | Not Passed Tests: 0\n\n==============================\n", facade.GetCompleteResults());
        Assert.Equal("Test file: tests/test_1.easy | Passed Tests: 0 | Not Passed Tests: 0\n", facade.GetScriptCompleteResults("tests/test_1.easy"));
        Assert.Equal("Test file: tests/test_1.easy | Passed Tests: 0 | Not Passed Tests: 0\n", facade.GetSummarizedResults());
        Assert.Equal("Test file: tests/test_1.easy | Passed Tests: 0 | Not Passed Tests: 0\n", facade.GetScriptSummarizedResults("tests/test_1.easy"));
        Assert.Equal(0, facade.GetTotalNumberOfPassedTests());
        Assert.Equal(0, facade.GetTotalNumberOfNotPassedTests());
        Assert.Equal(0, facade.GetScriptNumberOfPassedTests("tests/test_1.easy"));
        Assert.Equal(0, facade.GetScriptNumberOfNotPassedTests("tests/test_1.easy"));
    }

    [Fact]
    public void Test2()
    {
        EasyAcceptFacade<object> facade = new(new object(), ["tests/test_2.easy"]);
        facade.ExecuteTests();
        Assert.Equal("Test file: tests/test_2.easy | Passed Tests: 0 | Not Passed Tests: 0\n\nEcho instruction is present.\n\n==============================\n", facade.GetCompleteResults());
        Assert.Equal("Test file: tests/test_2.easy | Passed Tests: 0 | Not Passed Tests: 0\n\nEcho instruction is present.\n", facade.GetScriptCompleteResults("tests/test_2.easy"));
        Assert.Equal("Test file: tests/test_2.easy | Passed Tests: 0 | Not Passed Tests: 0\n", facade.GetSummarizedResults());
        Assert.Equal("Test file: tests/test_2.easy | Passed Tests: 0 | Not Passed Tests: 0\n", facade.GetScriptSummarizedResults("tests/test_2.easy"));
        Assert.Equal(0, facade.GetTotalNumberOfPassedTests());
        Assert.Equal(0, facade.GetTotalNumberOfNotPassedTests());
        Assert.Equal(0, facade.GetScriptNumberOfPassedTests("tests/test_2.easy"));
        Assert.Equal(0, facade.GetScriptNumberOfNotPassedTests("tests/test_2.easy"));
    }

    [Fact]
    public void Test3()
    {
        EasyAcceptFacade<object> facade = new(new object(), ["tests/test_3.easy"]);
        facade.ExecuteTests();
        Assert.Equal("Test file: tests/test_3.easy | Passed Tests: 0 | Not Passed Tests: 0\n\nQuit command executed.\n\n==============================\n", facade.GetCompleteResults());
        Assert.Equal("Test file: tests/test_3.easy | Passed Tests: 0 | Not Passed Tests: 0\n\nQuit command executed.\n", facade.GetScriptCompleteResults("tests/test_3.easy"));
        Assert.Equal("Test file: tests/test_3.easy | Passed Tests: 0 | Not Passed Tests: 0\n", facade.GetSummarizedResults());
        Assert.Equal("Test file: tests/test_3.easy | Passed Tests: 0 | Not Passed Tests: 0\n", facade.GetScriptSummarizedResults("tests/test_3.easy"));
        Assert.Equal(0, facade.GetTotalNumberOfPassedTests());
        Assert.Equal(0, facade.GetTotalNumberOfNotPassedTests());
        Assert.Equal(0, facade.GetScriptNumberOfPassedTests("tests/test_3.easy"));
        Assert.Equal(0, facade.GetScriptNumberOfNotPassedTests("tests/test_3.easy"));
    }

    [Fact]
    public void Test4()
    {
        EasyAcceptFacade<object> facade = new(new FakeFacade(), ["tests/test_4.easy"]);
        facade.ExecuteTests();
        Assert.Equal("Test file: tests/test_4.easy | Passed Tests: 0 | Not Passed Tests: 0\n\n==============================\n", facade.GetCompleteResults());
        Assert.Equal("Test file: tests/test_4.easy | Passed Tests: 0 | Not Passed Tests: 0\n", facade.GetScriptCompleteResults("tests/test_4.easy"));
        Assert.Equal("Test file: tests/test_4.easy | Passed Tests: 0 | Not Passed Tests: 0\n", facade.GetSummarizedResults());
        Assert.Equal("Test file: tests/test_4.easy | Passed Tests: 0 | Not Passed Tests: 0\n", facade.GetScriptSummarizedResults("tests/test_4.easy"));
        Assert.Equal(0, facade.GetTotalNumberOfPassedTests());
        Assert.Equal(0, facade.GetTotalNumberOfNotPassedTests());
        Assert.Equal(0, facade.GetScriptNumberOfPassedTests("tests/test_4.easy"));
        Assert.Equal(0, facade.GetScriptNumberOfNotPassedTests("tests/test_4.easy"));
    }

    [Fact]
    public void Test5()
    {
        EasyAcceptFacade<object> facade = new(new FakeFacade(), ["tests/test_5.easy"]);
        facade.ExecuteTests();
        Assert.Equal("Test file: tests/test_5.easy | Passed Tests: 1 | Not Passed Tests: 1\n\nExpect command failed. Expected: \"42\", Actual: \"This method has a wrong return value for testing.\"\n\n==============================\n", facade.GetCompleteResults());
        Assert.Equal("Test file: tests/test_5.easy | Passed Tests: 1 | Not Passed Tests: 1\n\nExpect command failed. Expected: \"42\", Actual: \"This method has a wrong return value for testing.\"\n", facade.GetScriptCompleteResults("tests/test_5.easy"));
        Assert.Equal("Test file: tests/test_5.easy | Passed Tests: 1 | Not Passed Tests: 1\n", facade.GetSummarizedResults());
        Assert.Equal("Test file: tests/test_5.easy | Passed Tests: 1 | Not Passed Tests: 1\n", facade.GetScriptSummarizedResults("tests/test_5.easy"));
        Assert.Equal(1, facade.GetTotalNumberOfPassedTests());
        Assert.Equal(1, facade.GetTotalNumberOfNotPassedTests());
        Assert.Equal(1, facade.GetScriptNumberOfPassedTests("tests/test_5.easy"));
        Assert.Equal(1, facade.GetScriptNumberOfNotPassedTests("tests/test_5.easy"));
    }

    [Fact]
    public void Test6()
    {
        EasyAcceptFacade<object> facade = new(new FakeFacade(), ["tests/test_6.easy"]);
        facade.ExecuteTests();
        Assert.Equal(
            "Test file: tests/test_6.easy | Passed Tests: 2 | Not Passed Tests: 2\n\n" +
            "ExpectError command failed. Expected: \"Different exception message\", Actual: \"A exception was occurred\"\n" +
            "ExpectError command failed. Expected: \"No exception\", but no error was thrown.\n" +
            "\n==============================\n",
            facade.GetCompleteResults());
        Assert.Equal(
            "Test file: tests/test_6.easy | Passed Tests: 2 | Not Passed Tests: 2\n\n" +
            "ExpectError command failed. Expected: \"Different exception message\", Actual: \"A exception was occurred\"\n" +
            "ExpectError command failed. Expected: \"No exception\", but no error was thrown.\n",
            facade.GetScriptCompleteResults("tests/test_6.easy"));
        Assert.Equal("Test file: tests/test_6.easy | Passed Tests: 2 | Not Passed Tests: 2\n", facade.GetSummarizedResults());
        Assert.Equal("Test file: tests/test_6.easy | Passed Tests: 2 | Not Passed Tests: 2\n", facade.GetScriptSummarizedResults("tests/test_6.easy"));
        Assert.Equal(2, facade.GetTotalNumberOfPassedTests());
        Assert.Equal(2, facade.GetTotalNumberOfNotPassedTests());
        Assert.Equal(2, facade.GetScriptNumberOfPassedTests("tests/test_6.easy"));
        Assert.Equal(2, facade.GetScriptNumberOfNotPassedTests("tests/test_6.easy"));
    }

    [Fact]
    public void Test7()
    {
        EasyAcceptFacade<object> facade = new(new FakeFacade(), ["tests/test_7.easy"]);
        facade.ExecuteTests();
        Assert.Equal("Test file: tests/test_7.easy | Passed Tests: 1 | Not Passed Tests: 0\n\nThe value of a is 42\n\n==============================\n", facade.GetCompleteResults());
        Assert.Equal("Test file: tests/test_7.easy | Passed Tests: 1 | Not Passed Tests: 0\n\nThe value of a is 42\n", facade.GetScriptCompleteResults("tests/test_7.easy"));
        Assert.Equal("Test file: tests/test_7.easy | Passed Tests: 1 | Not Passed Tests: 0\n", facade.GetSummarizedResults());
        Assert.Equal("Test file: tests/test_7.easy | Passed Tests: 1 | Not Passed Tests: 0\n", facade.GetScriptSummarizedResults("tests/test_7.easy"));
        Assert.Equal(1, facade.GetTotalNumberOfPassedTests());
        Assert.Equal(0, facade.GetTotalNumberOfNotPassedTests());
        Assert.Equal(1, facade.GetScriptNumberOfPassedTests("tests/test_7.easy"));
        Assert.Equal(0, facade.GetScriptNumberOfNotPassedTests("tests/test_7.easy"));
    }

    [Fact]
    public void Test8()
    {
        EasyAcceptFacade<object> facade = new(new FakeFacade(), ["tests/test_8.easy"]);
        facade.ExecuteTests();
        Assert.Equal("Test file: tests/test_8.easy | Passed Tests: 1 | Not Passed Tests: 0\n\nThe value of a is 42\n\n==============================\n", facade.GetCompleteResults());
        Assert.Equal("Test file: tests/test_8.easy | Passed Tests: 1 | Not Passed Tests: 0\n\nThe value of a is 42\n", facade.GetScriptCompleteResults("tests/test_8.easy"));
        Assert.Equal("Test file: tests/test_8.easy | Passed Tests: 1 | Not Passed Tests: 0\n", facade.GetSummarizedResults());
        Assert.Equal("Test file: tests/test_8.easy | Passed Tests: 1 | Not Passed Tests: 0\n", facade.GetScriptSummarizedResults("tests/test_8.easy"));
        Assert.Equal(1, facade.GetTotalNumberOfPassedTests());
        Assert.Equal(0, facade.GetTotalNumberOfNotPassedTests());
        Assert.Equal(1, facade.GetScriptNumberOfPassedTests("tests/test_8.easy"));
        Assert.Equal(0, facade.GetScriptNumberOfNotPassedTests("tests/test_8.easy"));
    }
}