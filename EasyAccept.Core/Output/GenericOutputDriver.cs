namespace EasyAccept.Core.Output
{
  public class GenericOutputDriver : IOutputDriver
  {
    private string Buffer = "";
    public void Clear() => Buffer = "";
    public void Flush() { }
    public void Write(string message) => Buffer += message;
    public void WriteLine(string message) => Buffer += message + "\n";
    public string GetContent() => Buffer;
  }
}