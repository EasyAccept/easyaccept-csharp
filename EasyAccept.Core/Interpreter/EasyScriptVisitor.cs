using System;
using System.Collections.Generic;
using Antlr4.Runtime.Misc;
using EasyAccept.Core.Grammar;
using EasyAccept.Core.Interpreter.Arguments;
using EasyAccept.Core.Interpreter.Commands;
using EasyAccept.Core.Interpreter.Exceptions;
using EasyAccept.Core.Interpreter.Listeners;
using EasyAccept.Core.Interpreter.Results;
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
    /// List of results from command executions.
    /// </summary>
    public readonly List<IResult> Results = new List<IResult>();

    /// <summary>
    /// List of listeners for results during test execution.
    /// </summary>
    public readonly List<IResultsListener> ResultsListeners = new List<IResultsListener>();

    /// <summary>
    /// Holds the variables defined during the script execution.
    /// </summary>
    private readonly Dictionary<string, string> Variables = new Dictionary<string, string>();

    public EasyScriptVisitor(F facade) => Facade = facade;

    public void AddResultsListener(IResultsListener listener) => ResultsListeners.Add(listener);

    public override object VisitEcho_([NotNull] EasyScriptParser.Echo_Context context)
    {
      NonNamedArgument arg = new NonNamedArgument(Visit(context.data()).ToString());
      ICommand command = new EchoCommand(arg);
      IResult result = command.Execute();
      AddResult(result);
      return null;
    }

    public override object VisitQuit_([NotNull] EasyScriptParser.Quit_Context context)
    {
      ICommand command = new QuitCommand();
      IResult result = command.Execute();
      AddResult(result);
      return null;
    }

    public override object VisitExpect_([NotNull] EasyScriptParser.Expect_Context context)
    {
      // Retrieve the expected output argument
      NonNamedArgument expectedOutput = new NonNamedArgument(Visit(context.data()).ToString());

      // Retrieve the command information to be executed and expected against. In both cases, we
      // construct the ICommand instance to be executed, not visiting the context directly.
      ICommand toBeExecutedCommand = null;
      if (context.unknownCommand() != null)
      {
        // Retrieve the unknown command information
        EasyScriptParser.UnknownCommandContext unknownCommandContext = context.unknownCommand();
        string commandName = unknownCommandContext.WORD().GetText();
        List<IEasyArgument> args = ArgumentListContextToArguments(unknownCommandContext.argumentList());
        toBeExecutedCommand = new UnknownCommand<F>(Facade, commandName, args);
      }
      else if (context.echo_() != null)
      {
        // Retrieve the echo command information
        NonNamedArgument echoArg = new NonNamedArgument(Visit(context.echo_().data()).ToString());
        toBeExecutedCommand = new EchoCommand(echoArg);
      }

      // Run the expect command
      ICommand command = new ExpectCommand(toBeExecutedCommand, expectedOutput);
      IResult result;
      try
      {
        result = command.Execute();
      }
      catch (CommandException ex)
      {
        result = new FailedResult(ex.Message, true);
      }

      AddResult(result);
      return null;
    }

    public override object VisitExpect_different_([NotNull] EasyScriptParser.Expect_different_Context context)
    {
      // Retrieve the unexpected output argument
      NonNamedArgument unexpectedOutput = new NonNamedArgument(Visit(context.data()).ToString());

      // Retrieve the command information to be executed. In both cases, we
      // construct the ICommand instance to be executed, not visiting the context directly.
      ICommand toBeExecutedCommand = null;
      if (context.unknownCommand() != null)
      {
        // Retrieve the unknown command information
        EasyScriptParser.UnknownCommandContext unknownCommandContext = context.unknownCommand();
        string commandName = unknownCommandContext.WORD().GetText();
        List<IEasyArgument> args = ArgumentListContextToArguments(unknownCommandContext.argumentList());
        toBeExecutedCommand = new UnknownCommand<F>(Facade, commandName, args);
      }
      else if (context.echo_() != null)
      {
        // Retrieve the echo command information
        NonNamedArgument echoArg = new NonNamedArgument(Visit(context.echo_().data()).ToString());
        toBeExecutedCommand = new EchoCommand(echoArg);
      }

      // Run the expect different command
      ICommand command = new ExpectDifferentCommand(toBeExecutedCommand, unexpectedOutput);
      IResult result;
      try
      {
        result = command.Execute();
      }
      catch (CommandException ex)
      {
        result = new FailedResult(ex.Message, true);
      }

      AddResult(result);
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
      IResult result;
      try
      {
        result = command.Execute();
      }
      catch (CommandException ex)
      {
        result = new FailedResult(ex.Message, true);
      }

      AddResult(result);
      return null;
    }

    public override object VisitUnknownCommand([NotNull] EasyScriptParser.UnknownCommandContext context)
    {
      // Retrieve the unknown command information
      string commandName = context.WORD().GetText();
      List<IEasyArgument> args = ArgumentListContextToArguments(context.argumentList());

      // Execute the unknown command
      ICommand command = new UnknownCommand<F>(Facade, commandName, args);
      IResult result;
      try
      {
        result = command.Execute();
      }
      catch (CommandException ex)
      {
        result = new FailedResult(ex.Message);
      }

      AddResult(result);
      return null;
    }

    public override object VisitAssignment([NotNull] EasyScriptParser.AssignmentContext context)
    {
      // Create the variable
      string variableName = context.WORD().GetText();
      Variables[variableName] = ""; // Initialize with empty string, will be updated later

      // Retrieve the unknown command information
      EasyScriptParser.UnknownCommandContext unknownCommandContext = context.unknownCommand();
      string commandName = unknownCommandContext.WORD().GetText();
      List<IEasyArgument> args = ArgumentListContextToArguments(unknownCommandContext.argumentList());

      // Execute the unknown command
      UnknownCommand<F> command = new UnknownCommand<F>(Facade, commandName, args);
      IResult result;
      try
      {
        result = command.Execute();
      }
      catch (CommandException ex)
      {
        result = new FailedResult(ex.Message);
      }

      // Store the result in the variable if successful
      if (result.IsSuccess)
      {
        Variables[variableName] = result.ToString() ?? "";
      }

      AddResult(result);
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

      // With the argument, get the WORD and the data
      string argName = argcv.WORD().GetText();
      string argValue = Visit(argcv.data()).ToString() ?? "";

      // Create the appropriate argument type
      IEasyArgument easyArgument = new NamedArgument(argName, argValue);

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

    private void AddResult(IResult result)
    {
      Results.Add(result);
      NotifyListeners(result);
    }

    private void NotifyListeners(IResult result)
    {
      foreach (IResultsListener listener in ResultsListeners)
      {
        listener.OnResult(result);
      }
    }
  }
}