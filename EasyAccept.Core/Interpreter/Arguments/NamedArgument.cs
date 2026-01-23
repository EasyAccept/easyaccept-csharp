namespace EasyAccept.Core.Interpreter.Arguments
{
  public class NamedArgument : AArgument
  {
    public override bool IsNamed => true;
    public override string Name { get; }
    public override string Value { get; }
    public NamedArgument(string name, string value)
    {
      Name = name;
      Value = value;
    }
  }
}