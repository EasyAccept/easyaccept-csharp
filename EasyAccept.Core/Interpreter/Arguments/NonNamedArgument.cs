namespace EasyAccept.Core.Interpreter.Arguments
{
  public class NonNamedArgument : AArgument
  {
    public override bool IsNamed => false;
    public override string Name => null;
    public override string Value { get; }
    
    public NonNamedArgument(string value)
    {
      Value = value;
    }
  }
}