using EasyAccept.Core.Exceptions;

namespace EasyAccept.Core.Interpreter.Exceptions
{
  public class SyntaxException : EasyAcceptException
  {
    public SyntaxException(string message) : base(message) { }
  }
}