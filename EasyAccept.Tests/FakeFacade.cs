namespace EasyAccept.Tests;

class FakeFacade
{
  public void unknownMethodOne() { }
  public string helloWorld() => "Hello, World!";
  public string wrongReturnType() => "This method has a wrong return type for testing.";
  public void throwsAnException() => throw new Exception("A exception was occurred");
}
