using System;
using EasyAccept.Core.Exceptions;

namespace EasyAccept.Core.Interpreter.Exceptions
{
  public class CommandException : EasyAcceptException
  {
    public CommandException(string message) : base(message) { }
    public static CommandException CreateBy(Exception innerException)
    {
      return new CommandException(innerException.Message);
    }
  }
}