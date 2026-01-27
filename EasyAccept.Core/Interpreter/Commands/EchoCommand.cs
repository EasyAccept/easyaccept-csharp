using System.Collections.Generic;
using EasyAccept.Core.Interpreter.Arguments;
using EasyAccept.Core.Output;
using EasyAccept.Core.Utils;

namespace EasyAccept.Core.Interpreter.Commands
{
  public class EchoCommand : ICommand
  {
    private readonly NonNamedArgument Argument;
    private readonly IOutputDriver OutputDriver;
    private readonly Dictionary<string, string> Variables;

    public EchoCommand(NonNamedArgument argument, IOutputDriver outputDriver, Dictionary<string, string> variables)
    {
      Argument = argument;
      OutputDriver = outputDriver;
      Variables = variables;
    }

    public void Execute()
    {
      OutputDriver.WriteLine(String.ReplaceVariablesOnInput(Argument.ToString(), Variables));
    }
  }
}