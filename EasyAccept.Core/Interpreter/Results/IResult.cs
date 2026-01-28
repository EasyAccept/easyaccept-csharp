namespace EasyAccept.Core.Interpreter.Results
{
  /// <summary>
  /// Interface representing the result of a command execution.
  /// </summary>
  public interface IResult
  {
    /// <summary>
    /// Indicates whether the command execution was successful.
    /// </summary>
    bool IsSuccess { get; }

    /// <summary>
    /// Indicates whether the command execution failed.
    /// </summary>
    bool IsFailure { get; }

    /// <summary>
    /// Indicates if the command are a assertion 
    /// </summary>
    bool AreAssertion { get; }

    /// <summary>
    /// Indicates whether the command need to be printed
    /// </summary>
    bool NeedToBePrinted { get; }

    /// <summary>
    /// Provides a message associated with the command execution result.
    /// </summary>
    string Message { get; }
  }
}