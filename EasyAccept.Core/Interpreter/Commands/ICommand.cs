using EasyAccept.Core.Interpreter.Results;

namespace EasyAccept.Core.Interpreter.Commands
{
  public interface ICommand
  {
    IResult Execute();
  }
}