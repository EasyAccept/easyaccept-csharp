using System.Collections.Generic;
using EasyAccept.Core.Interpreter.Arguments;
using EasyAccept.Core.Interpreter.Exceptions;
using EasyAccept.Core.Utils;

namespace EasyAccept.Core.Interpreter.Commands
{
  public class ExpectCommand<F> : ICommand
  {
    private readonly UnknownCommand<F> UnknownCommand;
    private readonly NonNamedArgument ExpectedOutput;
    private readonly Dictionary<string, string> Variables;

    public ExpectCommand(UnknownCommand<F> unknownCommand, NonNamedArgument expectedOutput, Dictionary<string, string> variables)
    {
      UnknownCommand = unknownCommand;
      ExpectedOutput = expectedOutput;
      Variables = variables;
    }

    public void Execute()
    {
      UnknownCommand.Execute();
      string actualOutput = UnknownCommand.Result;
      string expectedOutputWithVariablesReplaced = String.ReplaceVariablesOnInput(ExpectedOutput.ToString(), Variables);
      if (actualOutput != expectedOutputWithVariablesReplaced)
      {
        throw new CommandException($"Expect command failed. Expected: \"{expectedOutputWithVariablesReplaced}\", Actual: \"{actualOutput}\"");
      }
    }
  }
}