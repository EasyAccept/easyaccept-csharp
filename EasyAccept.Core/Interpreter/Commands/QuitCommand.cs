using EasyAccept.Core.Interpreter.Exceptions;

namespace EasyAccept.Core.Interpreter.Commands
{
  public class QuitCommand : ICommand
  {
    public void Execute()
    {
      throw new QuitException();
    }
  }
}