using EasyAccept.Core.Interpreter.Arguments;
using EasyAccept.Core.Interpreter.Exceptions;
using EasyAccept.Core.Interpreter.Results;

namespace EasyAccept.Core.Interpreter.Commands
{
  public class ExpectErrorCommand<F> : ICommand
  {
    private readonly UnknownCommand<F> UnknownCommand;
    private readonly NonNamedArgument ExpectedError;

    public ExpectErrorCommand(UnknownCommand<F> unknownCommand, NonNamedArgument expectedError)
    {
      UnknownCommand = unknownCommand;
      ExpectedError = expectedError;
    }

    public IResult Execute()
    {
      try
      {
        UnknownCommand.Execute();
      }
      catch (CommandException ex)
      {
        if (ex.Message != ExpectedError.ToString())
        {
          throw new CommandException($"ExpectError command failed. Expected: \"{ExpectedError}\", Actual: \"{ex.Message}\"");
        }

        return new SuccessfulResult("", true); // Expected error occurred
      }

      throw new CommandException($"ExpectError command failed. Expected: \"{ExpectedError}\", but no error was thrown.");
    }
  }
}