using EasyAccept.Core.Interpreter.Arguments;
using EasyAccept.Core.Output;

namespace EasyAccept.Core.Interpreter.Commands
{
  public class EchoCommand : ICommand
  {
    private readonly NonNamedArgument Argument;
    private readonly IOutputDriver OutputDriver;

    public EchoCommand(NonNamedArgument argument, IOutputDriver outputDriver)
    {
      Argument = argument;
      OutputDriver = outputDriver;
    }

    public void Execute()
    {
      OutputDriver.WriteLine(Argument.ToString());
    }
  }
}