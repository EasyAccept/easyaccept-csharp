using EasyAccept.Core.Interpreter.Arguments;
using EasyAccept.Core.Interpreter.Exceptions;

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

    public void Execute()
    {
      UnknownCommand.Execute();
      string actualOutput = UnknownCommand.Result;
      if (actualOutput != ExpectedOutput.ToString())
      {
        throw new CommandException($"Expect command failed. Expected: \"{ExpectedOutput}\", Actual: \"{actualOutput}\"");
      }
    }
  }
}