namespace EasyAccept.Core.Interpreter.Results
{
  /// <summary>
  /// Represents a successful result of a command execution.
  /// </summary>
  public class SuccessfulResult : AResult
  {
    public override bool IsSuccess { get; } = true;
    public override bool AreAssertion { get; }
    public override string Message { get; }
    public override bool NeedToBePrinted { get; } = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="SuccessfulResult"/> class with an optional message and assertion flag.
    /// </summary>
    /// <param name="message">The message associated with the successful result.</param>
    /// <param name="areAssertion">Indicates if the command are a assertion.</param>
    public SuccessfulResult(string message = "", bool areAssertion = false)
    {
      AreAssertion = areAssertion;
      Message = message;
    }
  }
}