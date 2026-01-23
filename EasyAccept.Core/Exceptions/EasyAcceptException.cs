using System;

namespace EasyAccept.Core.Exceptions
{
  public class EasyAcceptException : Exception
  {
    public EasyAcceptException(string message) : base(message)
    {
    }
  }
}