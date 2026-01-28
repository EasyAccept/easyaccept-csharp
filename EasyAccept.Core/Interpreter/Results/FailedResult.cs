namespace EasyAccept.Core.Interpreter.Results
{
  /// <summary>
  /// Represents a failed result of a command execution.
  /// </summary>
  public class FailedResult : AResult
  {
    public override bool IsSuccess { get; } = false;
    public override bool AreAssertion { get; }
    public override string Message { get; }
    public override bool NeedToBePrinted { get; } = true;

    /// <summary>
    /// Initializes a new instance of the <see cref="FailedResult"/> class with a message and optional assertion flag.
    /// </summary>
    /// <param name="message">The message associated with the failed result.</param>
    /// <param name="areAssertion">Indicates if the command are a assertion.</param>
    public FailedResult(string message, bool areAssertion = false)
    {
      AreAssertion = areAssertion;
      Message = message;
    }
  }
}