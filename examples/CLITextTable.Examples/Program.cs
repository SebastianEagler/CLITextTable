using CLITextTable.CliTable;
using CLITextTable.Configuration;

// Sample tables generated with CLITextTable. Run with:
//   dotnet run --project examples/CLITextTable.Examples

Title("1. Basic table (lines under every row)");
using (var table = new CliTable(o =>
{
    o.MinCellDataWidth = 8;
    o.DrawHeaderLine = true;
    o.DrawRowLines = true;
    o.DefaultHeaderColor = ConsoleColor.Yellow;
}, ["Name", "Language", "Stars"]))
{
    table.AddStringRow(new List<string> { "CLITextTable", "C#", "12" });
    table.AddStringRow(new List<string> { "GuitarChordCLI", "C#", "5" });
    table.AddStringRow(new List<string> { "Notes", "Markdown", "1" });
    table.Print();
}

Title("2. Compact table (no row lines, margins around separators)");
using (var table = new CliTable(o =>
{
    o.MinCellDataWidth = 10;
    o.RowSeparatorMarginLeft = 1;
    o.RowSeparatorMarginRight = 1;
    o.DrawHeaderLine = true;
    o.DrawRowLines = false;
    o.DefaultHeaderColor = ConsoleColor.Cyan;
    o.DefaultRowColor = ConsoleColor.Gray;
    o.DefaultRowLineColor = ConsoleColor.DarkGray;
}, ["Quality [m]", "Root", "Chord type [t]"]))
{
    table.AddStringRow(new List<string> { "Major", "C", "Triad" });
    table.Print();
}

Title("3. Custom frame characters and fixed column width limit");
using (var table = new CliTable(o =>
{
    o.ColumnSeparator = '!';
    o.RowSeparator = '=';
    o.FrameConnector = '#';
    o.MinCellDataWidth = 6;
    o.MaxCellDataWidth = 12;
    o.DrawRowLines = false;
    o.DefaultHeaderColor = ConsoleColor.Magenta;
}, ["Id", "Description"]))
{
    table.AddStringRow(new List<string> { "1", "Short" });
    table.AddStringRow(new List<string> { "2", "A very long description is cut" });
    table.Print();
}

Title("4. Colored cells (ColorCells and SetColor)");
using (var table = new CliTable(o =>
{
    o.MinCellDataWidth = 3;
    o.DrawHeaderLine = false;
    o.DrawRowLines = false;
    o.RowSeparatorMarginLeft = 1;
    o.RowSeparatorMarginRight = 1;
    o.DefaultRowColor = ConsoleColor.DarkGray;
    o.DefaultHeaderColor = ConsoleColor.DarkCyan;
    o.DefaultRowLineColor = ConsoleColor.DarkGray;
}, ["0", "1", "2", "3", "4", "5"]))
{
    // A small piece of a guitar fretboard (strings E, B, G).
    table.AddStringRow(new List<string> { "E", "F", "F#", "G", "G#", "A" });
    table.AddStringRow(new List<string> { "B", "C", "C#", "D", "D#", "E" });
    table.AddStringRow(new List<string> { "G", "G#", "A", "A#", "B", "C" });

    // Scale notes in green, then chord notes in red (they override the scale).
    table.ColorCells(new List<string> { "C", "D", "E", "F", "G", "A", "B" },
        new CellColorOptions { Color = ConsoleColor.Green });
    table.ColorCells(new List<string> { "C", "E", "G" },
        new CellColorOptions { Color = ConsoleColor.Red });

    // Color one cell: column 5, row 2, black on yellow.
    table.SetColor(5, 2, ConsoleColor.Black, ConsoleColor.Yellow);
    table.Print();
}

Title("5. Light theme (DefaultBackgroundColor)");
using (var table = new CliTable(o =>
{
    o.MinCellDataWidth = 8;
    o.DrawRowLines = false;
    o.RowSeparatorMarginLeft = 1;
    o.RowSeparatorMarginRight = 1;
    o.DefaultBackgroundColor = ConsoleColor.White;
    o.DefaultHeaderColor = ConsoleColor.DarkBlue;
    o.DefaultRowColor = ConsoleColor.Black;
    o.DefaultRowLineColor = ConsoleColor.Gray;
}, ["Theme", "Background", "Text"]))
{
    table.AddStringRow(new List<string> { "Light", "White", "Black" });
    table.AddStringRow(new List<string> { "Dark", "Black", "Gray" });
    table.Print();
}

Console.ResetColor();

static void Title(string text)
{
    Console.ResetColor();
    Console.WriteLine();
    Console.WriteLine(text);
}
