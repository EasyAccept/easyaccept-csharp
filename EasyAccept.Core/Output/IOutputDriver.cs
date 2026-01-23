namespace EasyAccept.Core.Output
{
  public interface IOutputDriver
  {
    // <summary>
    // Writes a message to the output.
    // </summary>
    void Write(string message);

    // <summary>
    // Writes a message followed by a newline to the output.
    // </summary>
    void WriteLine(string message);

    // <summary>
    // Clears the output.
    // </summary>
    void Clear();

    // <summary>
    // Flushes any buffered output.
    // </summary>
    void Flush();

    // <summary>
    // Retrieves the current content of the output.
    // </summary>
    string GetContent();
  }
}