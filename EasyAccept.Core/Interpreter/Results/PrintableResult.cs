namespace EasyAccept.Core.Interpreter.Results
{
  /// <summary>
  /// A result that is always successful and needs to be printed.
  /// </summary>
  public class PrintableResult : AResult
  {
    public override bool IsSuccess { get; } = true;
    public override bool AreAssertion { get; } = false;
    public override string Message { get; }
    public override bool NeedToBePrinted { get; } = true;

    /// <summary>
    /// Initializes a new instance of the <see cref="PrintableResult"/> class with a message.
    /// </summary>
    /// <param name="message"></param>
    public PrintableResult(string message)
    {
      Message = message;
    }
  }
}