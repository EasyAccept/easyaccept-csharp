using EasyAccept.Core.Interpreter.Arguments;
using EasyAccept.Core.Interpreter.Results;

namespace EasyAccept.Core.Interpreter.Commands
{
  public class EchoCommand : ICommand
  {
    private readonly NonNamedArgument Argument;

    public EchoCommand(NonNamedArgument argument) => Argument = argument;

    public IResult Execute() => new PrintableResult(Argument.ToString());
  }
}