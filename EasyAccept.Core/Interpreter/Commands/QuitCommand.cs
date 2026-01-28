using EasyAccept.Core.Interpreter.Exceptions;
using EasyAccept.Core.Interpreter.Results;

namespace EasyAccept.Core.Interpreter.Commands
{
  public class QuitCommand : ICommand
  {
    public IResult Execute()
    {
      throw new QuitException();
    }
  }
}