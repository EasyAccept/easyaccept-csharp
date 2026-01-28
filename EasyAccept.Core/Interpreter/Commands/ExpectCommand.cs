using EasyAccept.Core.Interpreter.Arguments;
using EasyAccept.Core.Interpreter.Exceptions;
using EasyAccept.Core.Interpreter.Results;

namespace EasyAccept.Core.Interpreter.Commands
{
  public class ExpectCommand<F> : ICommand
  {
    private readonly UnknownCommand<F> UnknownCommand;
    private readonly NonNamedArgument ExpectedOutput;

    public ExpectCommand(UnknownCommand<F> unknownCommand, NonNamedArgument expectedOutput)
    {
      UnknownCommand = unknownCommand;
      ExpectedOutput = expectedOutput;
    }

    public IResult Execute()
    {
      IResult unknownCommandResult = UnknownCommand.Execute();
      string actualOutput = unknownCommandResult.ToString();
      if (actualOutput != ExpectedOutput.ToString())
      {
        throw new CommandException($"Expect command failed. Expected: \"{ExpectedOutput}\", Actual: \"{actualOutput}\"");
      }
      return new SuccessfulResult("", true);
    }
  }
}