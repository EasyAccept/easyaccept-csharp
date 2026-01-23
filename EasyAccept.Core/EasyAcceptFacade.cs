using System.Collections.Generic;
using Antlr4.Runtime;
using EasyAccept.Core.Exceptions;
using EasyAccept.Core.Grammar;
using EasyAccept.Core.Interpreter;
using EasyAccept.Core.Interpreter.Exceptions;
using EasyAccept.Core.Output;

namespace EasyAccept.Core
{
  public class EasyAcceptFacade<F>
  {
    private readonly F Facade;
    private readonly List<string> Files;
    private string CompleteResults = "";
    private readonly IOutputDriver OutputDriver;

    public EasyAcceptFacade(F facade, List<string> files, IOutputDriver outputDriver)
    {
      Facade = facade;
      Files = files;
      OutputDriver = outputDriver;
    }

    public EasyAcceptFacade(F facade, List<string> files)
    {
      Facade = facade;
      Files = files;
      OutputDriver = new GenericOutputDriver();
    }

    public void ExecuteTests()
    {
      foreach (string file in Files)
      {
        // Construct the Grammar
        ICharStream charStream = CharStreams.fromPath(file);
        EasyScriptLexer lexer = new EasyScriptLexer(charStream);
        CommonTokenStream tokens = new CommonTokenStream(lexer);
        EasyScriptParser parser = new EasyScriptParser(tokens);
        // parser.AddErrorListener(new ConsoleErrorListener<object>());
        parser.BuildParseTree = true;

        // Check for syntax errors
        EasyScriptParser.EasyContext tree = parser.easy();
        if (parser.NumberOfSyntaxErrors > 0)
        {
          throw new EasyAcceptException("File '" + file + "' has " + parser.NumberOfSyntaxErrors + " syntax errors.");
        }

        // Execute tests using the Visitor
        EasyScriptVisitor<F> visitor = new EasyScriptVisitor<F>(Facade, OutputDriver);
        try
        {
          visitor.Visit(tree);
        }
        catch (QuitException ex)
        {
          // Stop execution on Quit command
          OutputDriver.WriteLine(ex.Message);
          CompleteResults += OutputDriver.GetContent();
          return;
        }

        CompleteResults += OutputDriver.GetContent();
      }
    }

    public string GetCompleteResults()
    {
      return CompleteResults;
    }
  }
}

