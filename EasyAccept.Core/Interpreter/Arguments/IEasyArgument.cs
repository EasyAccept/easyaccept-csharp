namespace EasyAccept.Core.Interpreter.Arguments
{
  public interface IEasyArgument
  {
    bool IsNamed { get; }
    string Name { get; }
    string Value { get; }
  }
}