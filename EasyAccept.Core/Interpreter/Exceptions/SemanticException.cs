using EasyAccept.Core.Exceptions;

namespace EasyAccept.Core.Interpreter.Exceptions
{
  public class SemanticException : EasyAcceptException
  {
    public SemanticException(string message) : base(message) { }
  }
}