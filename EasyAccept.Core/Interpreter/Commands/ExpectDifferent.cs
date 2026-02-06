using EasyAccept.Core.Interpreter.Arguments;
using EasyAccept.Core.Interpreter.Exceptions;
using EasyAccept.Core.Interpreter.Results;

namespace EasyAccept.Core.Interpreter.Commands
{
  public class ExpectDifferentCommand : ICommand
  {
    private readonly ICommand ToBeExecutedCommand;
    private readonly NonNamedArgument UnexpectedOutput;

    public ExpectDifferentCommand(ICommand toBeExecutedCommand, NonNamedArgument unexpectedOutput)
    {
      ToBeExecutedCommand = toBeExecutedCommand;
      UnexpectedOutput = unexpectedOutput;
    }

    public IResult Execute()
    {
      IResult toBeExecutedCommandResult = ToBeExecutedCommand.Execute();
      string actualOutput = toBeExecutedCommandResult.ToString();
      if (actualOutput == UnexpectedOutput.ToString())
      {
        throw new CommandException($"ExpectDifferent command failed. Unexpected: \"{UnexpectedOutput}\"");
      }
      return new SuccessfulResult("", true);
    }
  }
}