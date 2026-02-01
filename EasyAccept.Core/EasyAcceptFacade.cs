using System.Collections.Generic;
using Antlr4.Runtime;
using EasyAccept.Core.Exceptions;
using EasyAccept.Core.Grammar;
using EasyAccept.Core.Interpreter;
using EasyAccept.Core.Interpreter.Exceptions;
using EasyAccept.Core.Interpreter.Listeners;
using EasyAccept.Core.Interpreter.Results;

namespace EasyAccept.Core
{
  public class EasyAcceptFacade<F>
  {
    /// <summary>
    /// The facade instance used for executing commands.
    /// </summary>
    private readonly F Facade;

    /// <summary>
    /// The list of test script files to be executed.
    /// </summary>
    private readonly List<string> Files;

    /// <summary>
    /// The list of results from test executions. The results are stored as IResult instances and grouped by test file.
    /// </summary>
    public readonly Dictionary<string, List<IResult>> Results = new Dictionary<string, List<IResult>>();

    /// <summary>
    /// The list of listeners for results that will be notified during test execution.
    /// </summary>
    public readonly List<IResultsListener> ResultsListeners = new List<IResultsListener>();

    /// <summary>
    /// Initializes a new instance of the <see cref="EasyAcceptFacade{F}"/> class.
    /// </summary>
    /// <param name="facade">The facade instance to be used for executing commands.</param>
    /// <param name="files">The list of test script files to be executed.</param>

    public EasyAcceptFacade(F facade, List<string> files)
    {
      Facade = facade;
      Files = files;
    }
    
    public void AddResultsListener(IResultsListener listener) => ResultsListeners.Add(listener);

    public void ExecuteTests()
    {
      foreach (string file in Files)
      {
        // Construct the Grammar
        ICharStream charStream = CharStreams.fromPath(file);
        EasyScriptLexer lexer = new EasyScriptLexer(charStream);
        CommonTokenStream tokens = new CommonTokenStream(lexer);
        EasyScriptParser parser = new EasyScriptParser(tokens);
        parser.BuildParseTree = true;

        // Add semantic listener
        EasyScriptSemanticListener semanticListener = new EasyScriptSemanticListener();
        parser.AddParseListener(semanticListener);

        // Check for syntax or semantic errors
        EasyScriptParser.EasyContext tree = parser.easy();
        if (parser.NumberOfSyntaxErrors > 0)
        {
          throw new EasyAcceptException("File '" + file + "' has " + parser.NumberOfSyntaxErrors + " syntax errors.");
        }

        // Construct the results list
        Results[file] = new List<IResult>();

        // Construct the visitor
        EasyScriptVisitor<F> visitor = new EasyScriptVisitor<F>(Facade);

        // Add results listeners to the visitor
        foreach (IResultsListener listener in ResultsListeners)
        {
          visitor.AddResultsListener(listener);
        }

        // Execute tests using the Visitor
        try
        {
          visitor.Visit(tree);
        }
        catch (QuitException ex)
        {
          // Stop execution on Quit command
          Results[file].AddRange(visitor.Results);

          // Create a result indicating that execution was stopped
          IResult quitResult = new PrintableResult(ex.Message);

          // Add a result indicating that execution was stopped
          Results[file].Add(quitResult);

          // Notify listeners about the quit result
          NotifyListeners(quitResult);

          break;
        }

        Results[file].AddRange(visitor.Results);
      }
    }

    public string GetCompleteResults()
    {
      string completeResults = "";

      foreach (KeyValuePair<string, List<IResult>> resultsByFile in Results)
      {
        string result = GetScriptCompleteResults(resultsByFile.Key);
        completeResults += result;
        if (!result.EndsWith("\n\n"))
        {
          completeResults += "\n"; // Add the secondary newline if not present
        }
        completeResults += "==============================\n";
      }

      return completeResults;
    }

    public string GetScriptCompleteResults(string scriptFile)
    {
      if (!Results.ContainsKey(scriptFile))
      {
        throw new EasyAcceptException($"No results found for the script file \"{scriptFile}\".");
      }

      string completeResults = "";

      int passedTests = 0;
      int failedTests = 0;
      string fileResults = "";

      foreach (IResult result in Results[scriptFile])
      {
        if (result.AreAssertion && result.IsSuccess)
        {
          passedTests++;
        }
        else if (result.AreAssertion && !result.IsSuccess)
        {
          failedTests++;
        }

        if (result.NeedToBePrinted)
        {
          fileResults += result.ToString() + "\n";
        }
      }

      completeResults += $"Test file: {scriptFile} | Passed Tests: {passedTests} | Not Passed Tests: {failedTests}\n";
      if (fileResults != "")
      {
        completeResults += $"\n{fileResults}";
      }

      return completeResults;
    }

    public string GetSummarizedResults()
    {
      string completeResults = "";

      foreach (KeyValuePair<string, List<IResult>> resultsByFile in Results)
      {
        completeResults += GetScriptSummarizedResults(resultsByFile.Key);
      }

      return completeResults;
    }
    
    public string GetScriptSummarizedResults(string scriptFile)
    {
      if (!Results.ContainsKey(scriptFile))
      {
        throw new EasyAcceptException($"No results found for the script file \"{scriptFile}\".");
      }

      string completeResults = "";

      int passedTests = 0;
      int failedTests = 0;

      foreach (IResult result in Results[scriptFile])
      {
        if (result.AreAssertion && result.IsSuccess)
        {
          passedTests++;
        }
        else if (result.AreAssertion && !result.IsSuccess)
        {
          failedTests++;
        }
      }

      completeResults += $"Test file: {scriptFile} | Passed Tests: {passedTests} | Not Passed Tests: {failedTests}\n";

      return completeResults;
    }

    public List<IResult> GetScriptResults(string scriptFile)
    {
      if (!Results.ContainsKey(scriptFile))
      {
        throw new EasyAcceptException($"No results found for the script file \"{scriptFile}\".");
      }

      return Results[scriptFile];
    }

    public int GetTotalNumberOfPassedTests()
    {
      int totalPassedTests = 0;

      foreach (KeyValuePair<string, List<IResult>> resultsByFile in Results)
      {
        totalPassedTests += GetScriptNumberOfPassedTests(resultsByFile.Key);
      }

      return totalPassedTests;
    }

    public int GetTotalNumberOfNotPassedTests()
    {
      int totalNotPassedTests = 0;

      foreach (KeyValuePair<string, List<IResult>> resultsByFile in Results)
      {
        totalNotPassedTests += GetScriptNumberOfNotPassedTests(resultsByFile.Key);
      }

      return totalNotPassedTests;
    }

    public int GetScriptNumberOfPassedTests(string scriptFile)
    {
      if (!Results.ContainsKey(scriptFile))
      {
        throw new EasyAcceptException($"No results found for the script file \"{scriptFile}\".");
      }

      int passedTests = 0;

      foreach (IResult result in Results[scriptFile])
      {
        if (result.AreAssertion && result.IsSuccess)
        {
          passedTests++;
        }
      }

      return passedTests;
    }
    
    public int GetScriptNumberOfNotPassedTests(string scriptFile)
    {
      if (!Results.ContainsKey(scriptFile))
      {
        throw new EasyAcceptException($"No results found for the script file \"{scriptFile}\".");
      }

      int notPassedTests = 0;

      foreach (IResult result in Results[scriptFile])
      {
        if (result.AreAssertion && !result.IsSuccess)
        {
          notPassedTests++;
        }
      }

      return notPassedTests;
    }

    public int GetTotalNumberOfTests()
    {
      return GetTotalNumberOfPassedTests() + GetTotalNumberOfNotPassedTests();
    }

    public int GetScriptTotalNumberOfTests(string scriptFile)
    {
      return GetScriptNumberOfPassedTests(scriptFile) + GetScriptNumberOfNotPassedTests(scriptFile);
    }

    public List<IResult> GetScriptFailures(string scriptFile)
    {
      if (!Results.ContainsKey(scriptFile))
      {
        throw new EasyAcceptException($"No results found for the script file \"{scriptFile}\".");
      }

      List<IResult> failures = new List<IResult>();

      foreach (IResult result in Results[scriptFile])
      {
        if (result.AreAssertion && !result.IsSuccess)
        {
          failures.Add(result);
        }
      }

      return failures;
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

