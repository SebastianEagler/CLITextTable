using CLITextTable.CliTable;
using Xunit;

namespace CLITextTable.Tests
{
    [Collection("Console")]
    public class PrintOptionsTests
    {
        private static string Print(CliTable.CliTable table)
        {
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
            return writer.ToString();
        }

        [Fact]
        public void CustomColumnSeparator_IsUsedInRows()
        {
            using var table = new CliTable.CliTable(o =>
            {
                o.ColumnSeparator = '!';
                o.MinCellDataWidth = 3;
            }, ["A", "B"]);
            table.AddStringRow(new List<string> { "x", "y" });

            var lines = Print(table).Split(Environment.NewLine);

            Assert.Contains(lines, l => l == "!x  !y  !");
            Assert.DoesNotContain(lines, l => l.Contains('|'));
        }

        [Fact]
        public void MaxCellDataWidth_LimitsColumnWidth()
        {
            using var table = new CliTable.CliTable(o =>
            {
                o.MinCellDataWidth = 3;
                o.MaxCellDataWidth = 5;
                o.DrawRowLines = false;
            }, ["A"]);
            table.AddStringRow(new List<string> { "abcdefghij" });

            var lines = Print(table).Split(Environment.NewLine);

            Assert.Contains(lines, l => l == "|abcde|");
            Assert.Contains(lines, l => l == "+-----+");
        }
    }
}
