using System.Collections.Generic;
using Antlr4.Runtime.Misc;
using EasyAccept.Core.Grammar;
using EasyAccept.Core.Interpreter.Exceptions;

namespace EasyAccept.Core.Interpreter
{
  public class EasyScriptSemanticListener : EasyScriptBaseListener
  {
    private readonly List<string> Variables = new List<string>();

    public override void ExitAssignment([NotNull] EasyScriptParser.AssignmentContext context)
    {
      Variables.Add(context.WORD().GetText());
      base.ExitAssignment(context);
    }

    public override void ExitData([NotNull] EasyScriptParser.DataContext context)
    {
      if (context.VARIABLE() != null)
      {
        string variableName = context.VARIABLE().GetText().TrimStart('$').TrimStart('{').TrimEnd('}');
        if (!Variables.Contains(variableName))
        {
          throw new SemanticException("Variable '" + variableName + "' is not defined.");
        }
      }

      if (context.STRING() != null)
      {
        // Split the string by variables and check each one
        string str = context.STRING().GetText();
        int startIndex = 0;
        while (startIndex < str.Length)
        {
          int varStart = str.IndexOf("${", startIndex);
          if (varStart == -1) break;
          int varEnd = str.IndexOf("}", varStart);
          if (varEnd == -1) break;

          string variableName = str.Substring(varStart + 2, varEnd - varStart - 2);
          if (!Variables.Contains(variableName))
          {
            throw new SemanticException("Variable '" + variableName + "' is not defined.");
          }
          startIndex = varEnd + 1;
        }
      }

      base.ExitData(context);
    }
  }
}