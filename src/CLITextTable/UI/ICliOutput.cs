namespace CLITextTable.UI
{
    /// <summary>
    /// Destination for a printed table. Implement it to render tables somewhere
    /// other than the console (for example to SVG or HTML).
    /// </summary>
    public interface ICliOutput
    {
        void Write(string text, ConsoleColor color, ConsoleColor backgroundColor);

        void WriteLine(string text, ConsoleColor color, ConsoleColor backgroundColor);
    }

    /// <summary>Writes colored text to <see cref="Console"/>.</summary>
    public sealed class ConsoleCliOutput : ICliOutput
    {
        public void Write(string text, ConsoleColor color, ConsoleColor backgroundColor)
        {
            Console.ForegroundColor = color;
            Console.BackgroundColor = backgroundColor;
            Console.Write(text);
        }

        public void WriteLine(string text, ConsoleColor color, ConsoleColor backgroundColor)
        {
            Console.ForegroundColor = color;
            Console.BackgroundColor = backgroundColor;
            Console.WriteLine(text);
        }
    }
}
