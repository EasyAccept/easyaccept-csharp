namespace EasyAccept.Core.Interpreter.Results
{
  /// <summary>
  /// Abstract base class for command execution results.
  /// </summary>
  public abstract class AResult : IResult
  {
    public abstract bool IsSuccess { get; }
    public abstract bool AreAssertion { get; }
    public abstract string Message { get; }
    public abstract bool NeedToBePrinted { get; }
    public bool IsFailure => !IsSuccess;

    public override string ToString()
    {
      return Message;
    }
  }
}