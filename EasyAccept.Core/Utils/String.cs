using System.Collections.Generic;

namespace EasyAccept.Core.Utils
{
  public static class String
  {
    public static string ReplaceVariablesOnInput(string input, Dictionary<string, string> variables)
    {
      foreach (var variable in variables)
      {
        string placeholder = "${" + variable.Key + "}";
        input = input.Replace(placeholder, variable.Value);
      }

      return input;
    }
  }
}