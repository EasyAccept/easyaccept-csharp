namespace EasyAccept.Core.Interpreter.Arguments
{
  public abstract class AArgument : IEasyArgument
  {
    public abstract bool IsNamed { get; }
    public abstract string Name { get; }
    public abstract string Value { get; }

    public override string ToString()
    {
      if (IsNamed)
      {
        return Name + "=" + Value;
      }
      else
      {
        return Value;
      }
    }
  }
}