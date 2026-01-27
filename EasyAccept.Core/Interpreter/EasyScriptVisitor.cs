using System.Collections.Generic;
using Antlr4.Runtime.Misc;
using EasyAccept.Core.Grammar;
using EasyAccept.Core.Interpreter.Arguments;
using EasyAccept.Core.Interpreter.Commands;
using EasyAccept.Core.Interpreter.Exceptions;
using EasyAccept.Core.Output;
using ICommand = EasyAccept.Core.Interpreter.Commands.ICommand;

namespace EasyAccept.Core.Interpreter
{
  public class EasyScriptVisitor<F> : EasyScriptBaseVisitor<object>
  {
    /// <summary>
    /// Facade instance used to execute commands on.
    /// </summary>
    private readonly F Facade;

    /// <summary>
    /// Driver used to output messages during script execution.
    /// </summary>
    private readonly IOutputDriver OutputDriver;

    /// <summary>
    /// Holds the variables defined during the script execution.
    /// </summary>
    private readonly Dictionary<string, string> Variables = new Dictionary<string, string>();

    public EasyScriptVisitor(F facade, IOutputDriver outputDriver)
    {
      Facade = facade;
      OutputDriver = outputDriver;
    }

    public override object VisitEcho_([NotNull] EasyScriptParser.Echo_Context context)
    {
      NonNamedArgument arg = new NonNamedArgument(Visit(context.data()).ToString());
      ICommand command = new EchoCommand(arg, OutputDriver);
      command.Execute();
      return null;
    }

    public override object VisitQuit_([NotNull] EasyScriptParser.Quit_Context context)
    {
      ICommand command = new QuitCommand();
      command.Execute();
      return null;
    }

    public override object VisitExpect_([NotNull] EasyScriptParser.Expect_Context context)
    {
      // Retrieve the expected output argument
      NonNamedArgument expectedOutput = new NonNamedArgument(Visit(context.data()).ToString());

      // Retrieve the unknown command information
      EasyScriptParser.UnknownCommandContext unknownCommandContext = context.unknownCommand();
      string commandName = unknownCommandContext.WORD().GetText();
      List<IEasyArgument> args = ArgumentListContextToArguments(unknownCommandContext.argumentList());
      UnknownCommand<F> unknownCommand = new UnknownCommand<F>(Facade, commandName, args);
      
      // Run the expect command
      ICommand command = new ExpectCommand<F>(unknownCommand, expectedOutput);
      try
      {
        command.Execute();
      }
      catch (CommandException ex)
      {
        OutputDriver.WriteLine(ex.Message);        
      }

      return null;
    }

    public override object VisitExpect_error_([NotNull] EasyScriptParser.Expect_error_Context context)
    {
      // Retrieve the expected error argument
      NonNamedArgument expectedError = new NonNamedArgument(Visit(context.data()).ToString());

      // Retrieve the unknown command information
      EasyScriptParser.UnknownCommandContext unknownCommandContext = context.unknownCommand();
      string commandName = unknownCommandContext.WORD().GetText();
      List<IEasyArgument> args = ArgumentListContextToArguments(unknownCommandContext.argumentList());
      UnknownCommand<F> unknownCommand = new UnknownCommand<F>(Facade, commandName, args);

      // Run the expect error command
      ICommand command = new ExpectErrorCommand<F>(unknownCommand, expectedError);
      try
      {
        command.Execute();
      }
      catch (CommandException ex)
      {
        OutputDriver.WriteLine(ex.Message);
      }

      return null;
    }

    public override object VisitUnknownCommand([NotNull] EasyScriptParser.UnknownCommandContext context)
    {
      // Retrieve the unknown command information
      string commandName = context.WORD().GetText();
      List<IEasyArgument> args = ArgumentListContextToArguments(context.argumentList());

      // Execute the unknown command
      ICommand command = new UnknownCommand<F>(Facade, commandName, args);
      try
      {
        command.Execute();
      }
      catch (CommandException ex)
      {
        OutputDriver.WriteLine(ex.Message);
      }

      return null;
    }

    public override object VisitAssignment([NotNull] EasyScriptParser.AssignmentContext context)
    {
      // Create the variable
      string variableName = context.WORD().GetText();
      Variables[variableName] = string.Empty; // Initialize with empty string, will be updated later

      // Retrieve the unknown command information
      EasyScriptParser.UnknownCommandContext unknownCommandContext = context.unknownCommand();
      string commandName = unknownCommandContext.WORD().GetText();
      List<IEasyArgument> args = ArgumentListContextToArguments(unknownCommandContext.argumentList());

      // Execute the unknown command
      UnknownCommand<F> command = new UnknownCommand<F>(Facade, commandName, args);
      try
      {
        command.Execute();
      }
      catch (CommandException ex)
      {
        OutputDriver.WriteLine(ex.Message);
      }

      // Store the result in the variable
      Variables[variableName] = command.Result ?? string.Empty;

      return null;
    }

    public override object VisitData([NotNull] EasyScriptParser.DataContext context)
    {
      if (context.WORD() != null)
      {
        return context.WORD().GetText();
      }

      if (context.VARIABLE() != null)
      {
        string variableName = context.VARIABLE().GetText().TrimStart('$').TrimStart('{').TrimEnd('}');
        string variableData = Variables[variableName];
        return variableData;
      }

      if (context.STRING() != null)
      {
        string stringData = context.STRING().GetText().Trim('"').Trim('\'');
        stringData = ReplaceVariablesOnInput(stringData);
        return stringData;
      }

      return null;
    }

    private List<IEasyArgument> ArgumentListContextToArguments(EasyScriptParser.ArgumentListContext arglcv)
    {
      List<IEasyArgument> args = ParseArguments(arglcv);
      args.Reverse();
      return args;
    }
    
    private List<IEasyArgument> ParseArguments(EasyScriptParser.ArgumentListContext arglcv) => ParseArguments(arglcv, new List<IEasyArgument>());

    private List<IEasyArgument> ParseArguments(EasyScriptParser.ArgumentListContext arglcv, List<IEasyArgument> args)
    {
      // If argument list is null, return the current list of arguments
      if (arglcv == null)
      {
        return args;
      }

      // Call the next argument list recursively
      EasyScriptParser.ArgumentListContext nextArglcv = arglcv.argumentList();
      if (nextArglcv != null)
      {
        ParseArguments(nextArglcv, args);
      }

      // Get the current argument
      EasyScriptParser.ArgumentContext argcv = arglcv.argument();

      // Get the argument text
      string argv = argcv.GetText();

      // Create the appropriate argument type
      string[] argument = argv.Split('=');
      IEasyArgument easyArgument;
      if (argument.Length == 2)
      {
        easyArgument = new NamedArgument(argument[0], argument[1].Trim('"').Trim('\''));
      }
      else
      {
        easyArgument = new NonNamedArgument(argument[0].Trim('"').Trim('\''));
      }

      // Push the argument to the list
      args.Add(easyArgument);

      return args;
    }

    private string ReplaceVariablesOnInput(string input)
    {
      foreach (var variable in Variables)
      {
        string placeholder = "${" + variable.Key + "}";
        input = input.Replace(placeholder, variable.Value);
      }

      return input;
    }
  }
}