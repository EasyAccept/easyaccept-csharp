using EasyAccept.Core.Exceptions;

namespace EasyAccept.Core.Interpreter.Exceptions
{
  public class QuitException : EasyAcceptException
  {
    public QuitException() : base("Quit command executed.") { }
  }
}