namespace EasyAccept.Tests;

class FakeFacade
{
  public void unknownMethodOne()
  {
    // Can be called by test_4.easy
  }

  public string helloWorld()
  {
    // Can be called by test_5.easy
    return "Hello, World!";
  }

  public string wrongReturnType()
  {
    return "This method has a wrong return type for testing.";
  }
}
