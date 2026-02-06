namespace EasyAccept.Tests
{
  class FakeFacade
  {
    private int _counter = 0;
    public void unknownMethodOne() { }
    public void unknownMethodTwo(string param1, string param2) { }
    public string helloWorld() => "Hello, World!";
    public string wrongReturnValue() => "This method has a wrong return value for testing.";
    public void throwsAnException() => throw new System.Exception("A exception was occurred");
    public void throwsAPersonalizedException(string message) => throw new System.Exception(message);
    public string theAnswer() => "42";
    public string echoCommandWithParam(string value) => value;
    public string nextNumber() => (++_counter).ToString();
  }
}

