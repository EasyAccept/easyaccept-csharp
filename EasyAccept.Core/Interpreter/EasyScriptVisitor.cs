using System.Collections.Generic;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
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
    private readonly F Facade;
    private readonly IOutputDriver OutputDriver;

    public EasyScriptVisitor(F facade, IOutputDriver outputDriver)
    {
      Facade = facade;
      OutputDriver = outputDriver;
    }

    public override object VisitEcho_([NotNull] EasyScriptParser.Echo_Context context)
    {
      NonNamedArgument arg = new NonNamedArgument(context.STRING().GetText().Trim('"').Trim('\''));
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
      ITerminalNode expectedOutputNode = context.WORD() ?? context.STRING();
      NonNamedArgument expectedOutput = new NonNamedArgument(expectedOutputNode.GetText().Trim('"').Trim('\''));

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

    public override object VisitUnknownCommand([NotNull] EasyScriptParser.UnknownCommandContext context)
    {
      string commandName = context.WORD().GetText();
      List<IEasyArgument> args = ArgumentListContextToArguments(context.argumentList());
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
      EasyScriptParser.ArgumentContext argcv = arglcv.argument() ?? throw new SyntaxException("Unexpected end of input.");

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
  }
}