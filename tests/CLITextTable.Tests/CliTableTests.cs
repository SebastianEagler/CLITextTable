using CLITextTable.CliTable;
using CLITextTable.Configuration;
using CLITextTable.Lib;
using CLITextTable.UI;
using Xunit;

namespace CLITextTable.Tests
{
    public class CliLineFormaterTests
    {
        [Theory]
        [InlineData("ab", 5, 10, "ab   ")]
        [InlineData("  ab  ", 4, 10, "ab  ")]
        [InlineData("abcdefghijkl", 3, 5, "abcde")]
        public void SetRowDataLength_PadsAndTruncates(string value, int min, int max, string expected)
        {
            Assert.Equal(expected, CliLineFormater.SetRowDataLength(value, min, max));
        }

        [Fact]
        public void PrepareLine_HasConnectorsAtColumnBoundaries()
        {
            var options = new CliConfigureOptions();
            var columns = new List<RowOptions> { new() { Width = 6 }, new() { Width = 6 } };
            var formater = new CliLineFormater(options, columns, new CliLineSeparators(0, 0));

            Assert.Equal("+------+------+", formater.PrepareLine());
        }
    }

    [Collection("Console")]
    public class CliTableTests
    {
        private static CliTable.CliTable CreateTable(Action<CliConfigureOptions>? configure = null)
            => new(configure ?? (o => o.MinCellDataWidth = 6), ["Name", "Value"]);

        [Fact]
        public void AddRow_WithWrongColumnCount_Throws()
        {
            using var table = CreateTable();
            Assert.Throws<ArgumentException>(() => table.AddStringRow(new List<string> { "only one" }));
        }

        [Fact]
        public void ColorCells_ColorsOnlyMatchingCells()
        {
            using var table = CreateTable();
            table.AddStringRow(new List<string> { "C", "D" });

            table.ColorCells(new List<string> { "C" },
                new CellColorOptions { Color = ConsoleColor.Red, BackgroundColor = ConsoleColor.Yellow });

            Assert.Equal(ConsoleColor.Red, table.Cell[0][0].Color);
            Assert.Equal(ConsoleColor.Yellow, table.Cell[0][0].BackgroundColor);
            Assert.NotEqual(ConsoleColor.Red, table.Cell[0][1].Color);
        }

        [Fact]
        public void SetColor_WithoutBackground_UsesTableDefaultBackground()
        {
            using var table = CreateTable(o => o.DefaultBackgroundColor = ConsoleColor.White);
            table.AddStringRow(new List<string> { "a", "b" });

            table.SetColor(1, 0, ConsoleColor.Green);

            Assert.Equal(ConsoleColor.Green, table.Cell[0][1].Color);
            Assert.Equal(ConsoleColor.White, table.Cell[0][1].BackgroundColor);
        }

        [Fact]
        public void SetColor_OutOfRange_Throws()
        {
            using var table = CreateTable();
            table.AddStringRow(new List<string> { "a", "b" });

            Assert.Throws<ArgumentOutOfRangeException>(() => table.SetColor(5, 0, ConsoleColor.Red));
            Assert.Throws<ArgumentOutOfRangeException>(() => table.SetColor(0, 5, ConsoleColor.Red));
        }

        [Fact]
        public void Print_WritesHeaderAndRows()
        {
            using var table = CreateTable();
            table.AddStringRow(new List<string> { "Root", "C" });

            var original = Console.Out;
            var writer = new StringWriter();
            try
            {
                Console.SetOut(writer);
                table.Print();
            }
            finally
            {
                Console.SetOut(original);
            }

            var output = writer.ToString();
            Assert.Contains("Name", output);
            Assert.Contains("Value", output);
            Assert.Contains("Root", output);
            Assert.Contains("+------+------+", output);
        }
    }
}
