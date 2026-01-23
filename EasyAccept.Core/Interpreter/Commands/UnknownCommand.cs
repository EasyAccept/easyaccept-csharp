using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EasyAccept.Core.Interpreter.Arguments;
using EasyAccept.Core.Interpreter.Exceptions;

namespace EasyAccept.Core.Interpreter.Commands
{
  public class UnknownCommand<F> : ICommand
  {
    private readonly F Facade;
    private readonly string CommandName;
    private readonly List<IEasyArgument> Arguments;

    public UnknownCommand(F facade, string commandName, List<IEasyArgument> args)
    {
      Facade = facade;
      CommandName = commandName;
      Arguments = args;
    }

    public void Execute()
    {
      Type facadeType = Facade.GetType() ?? throw new CommandException("Facade type is null.");

      // Find method with matching name and argument count
      MethodInfo method = null;
      foreach (MethodInfo mi in facadeType.GetMethods())
      {
        if (mi.Name == CommandName)
        {
          ParameterInfo[] parameters = mi.GetParameters();
          if (parameters.Length == Arguments.Count)
          {
            method = mi;
            break;
          }
        }
      }

      if (method == null)
      {
        throw new CommandException("Method " + CommandName + "(" + string.Join(", ", Arguments.Select(arg => $"\"{arg.ToString()}\"")) + ") not found in facade.");
      }

      // Prepare argument values
      object[] argumentValues = new object[Arguments.Count];
      foreach (ParameterInfo parameter in method.GetParameters())
      {
        // IMPORTANT: Currently only string arguments are supported
        if (parameter.GetType() != typeof(string))
        {
          throw new CommandException("Parameter " + parameter.Name + " in method " + CommandName + " is not of type string.");
        }
        string argumentName = parameter.Name ?? throw new CommandException("Parameter name is null.");
        IEasyArgument matchingArg = Arguments.FirstOrDefault(arg => arg.Name == argumentName) ?? throw new CommandException("Argument " + argumentName + " not found for method " + CommandName + ".");
        argumentValues[parameter.Position] = matchingArg.Value;
      }

      // Invoke the method
      try
      {
        method.Invoke(Facade, argumentValues);
      }
      catch (Exception ex)
      {
        throw CommandException.CreateBy(ex);
      }
    }
  }
}