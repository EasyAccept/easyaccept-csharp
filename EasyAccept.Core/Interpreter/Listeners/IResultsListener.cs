using EasyAccept.Core.Interpreter.Results;

namespace EasyAccept.Core.Interpreter.Listeners
{
  /// <summary>
  /// Interface for listening to results during test execution.
  /// </summary>
  public interface IResultsListener
  {
    /// <summary>
    /// Method called when a new result is available.
    /// </summary>
    /// <param name="result">The result of the executed command.</param>
    void OnResult(IResult result);
  }
}