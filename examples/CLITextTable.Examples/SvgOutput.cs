using System.Globalization;
using System.Text;
using CLITextTable.UI;

/// <summary>
/// <see cref="ICliOutput"/> that renders a table to an SVG image (keeps all colors),
/// used to generate the pictures in the README.
/// </summary>
internal sealed class SvgOutput : ICliOutput
{
    private const double CharWidth = 8.4;
    private const double LineHeight = 19;
    private const double Padding = 14;

    private readonly ConsoleColor _background;
    private readonly List<List<(string Text, ConsoleColor Fg, ConsoleColor Bg)>> _lines = [[]];

    public SvgOutput(ConsoleColor background) => _background = background;

    public void Write(string text, ConsoleColor color, ConsoleColor backgroundColor)
        => _lines[^1].Add((text, color, backgroundColor));

    public void WriteLine(string text, ConsoleColor color, ConsoleColor backgroundColor)
    {
        Write(text, color, backgroundColor);
        _lines.Add([]);
    }

    public void Save(string path)
    {
        // Ignore a trailing empty line.
        var lines = _lines.Where(l => l.Count > 0 && l.Any(s => s.Text.Length > 0)).ToList();
        int maxChars = lines.Max(l => l.Sum(s => s.Text.Length));
        double width = maxChars * CharWidth + Padding * 2;
        double height = lines.Count * LineHeight + Padding * 2;

        var sb = new StringBuilder();
        sb.AppendLine($"<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"{F(width)}\" height=\"{F(height)}\" viewBox=\"0 0 {F(width)} {F(height)}\">");
        sb.AppendLine($"  <rect width=\"100%\" height=\"100%\" rx=\"6\" fill=\"{Hex(_background)}\"/>");
        sb.AppendLine("  <g font-family=\"Consolas, 'DejaVu Sans Mono', Menlo, monospace\" font-size=\"14\">");

        for (int row = 0; row < lines.Count; row++)
        {
            double baseline = Padding + row * LineHeight + 14;
            double top = Padding + row * LineHeight;
            int col = 0;
            foreach (var (text, fg, bg) in lines[row])
            {
                double x0 = Padding + col * CharWidth;
                if (bg != _background && text.Length > 0)
                    sb.AppendLine($"    <rect x=\"{F(x0)}\" y=\"{F(top)}\" width=\"{F(text.Length * CharWidth)}\" height=\"{F(LineHeight)}\" fill=\"{Hex(bg)}\"/>");

                // One explicit x per character keeps the grid aligned in any font.
                var chars = new StringBuilder();
                var xs = new List<string>();
                for (int i = 0; i < text.Length; i++)
                {
                    if (text[i] == ' ') continue;
                    xs.Add(F(x0 + i * CharWidth));
                    chars.Append(Escape(text[i]));
                }
                if (chars.Length > 0)
                    sb.AppendLine($"    <text x=\"{string.Join(' ', xs)}\" y=\"{F(baseline)}\" fill=\"{Hex(fg)}\">{chars}</text>");
                col += text.Length;
            }
        }

        sb.AppendLine("  </g>");
        sb.AppendLine("</svg>");

        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, sb.ToString());
    }

    private static string F(double value) => value.ToString("0.##", CultureInfo.InvariantCulture);

    private static string Escape(char c) => c switch
    {
        '&' => "&amp;",
        '<' => "&lt;",
        '>' => "&gt;",
        '"' => "&quot;",
        _ => c.ToString()
    };

    // Windows Terminal "Campbell" palette.
    private static string Hex(ConsoleColor color) => color switch
    {
        ConsoleColor.Black => "#0C0C0C",
        ConsoleColor.DarkBlue => "#0037DA",
        ConsoleColor.DarkGreen => "#13A10E",
        ConsoleColor.DarkCyan => "#3A96DD",
        ConsoleColor.DarkRed => "#C50F1F",
        ConsoleColor.DarkMagenta => "#881798",
        ConsoleColor.DarkYellow => "#C19C00",
        ConsoleColor.Gray => "#CCCCCC",
        ConsoleColor.DarkGray => "#767676",
        ConsoleColor.Blue => "#3B78FF",
        ConsoleColor.Green => "#16C60C",
        ConsoleColor.Cyan => "#61D6D6",
        ConsoleColor.Red => "#E74856",
        ConsoleColor.Magenta => "#B4009E",
        ConsoleColor.Yellow => "#F9F1A5",
        _ => "#F2F2F2" // White
    };
}
