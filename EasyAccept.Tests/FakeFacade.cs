namespace EasyAccept.Tests;

class FakeFacade
{
  public void unknownMethodOne() { }
  public void unknownMethodTwo(string param1, string param2) { }
  public string helloWorld() => "Hello, World!";
  public string wrongReturnValue() => "This method has a wrong return value for testing.";
  public void throwsAnException() => throw new Exception("A exception was occurred");
  public void throwsAPersonalizedException(string message) => throw new Exception(message);
  public string theAnswer() => "42";
  public string echoCommandWithParam(string value) => value;
}
