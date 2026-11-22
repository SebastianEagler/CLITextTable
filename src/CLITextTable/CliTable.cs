using CLITextTable.Configuration;
using CLITextTable.Data;
using CLITextTable.Lib;
using CLITextTable.UI;

namespace CLITextTable.CliTable
{

    public sealed class CliCellCollection : List<List<CliCell>> { }

    public sealed class CliTable : IDisposable
    {
        public void Dispose()
        {
            // Nothing to release; kept so the table can be used in a using statement.
        }

        private readonly List<string> headers;
        private readonly CliCellCollection cell = new CliCellCollection();
        private List<RowOptions> column = new List<RowOptions>();
        private readonly CliConfigureOptions options;

        public CliCellCollection Cell { get => cell; }
        public List<RowOptions> Column { get => column; }
        public CliTable(Action<CliConfigureOptions> options, string[] headers)
        {
            this.options = new CliConfigureOptions();
            options?.Invoke(this.options);

            this.headers = headers.ToList();
            foreach (var header in headers)
            {
                column.Add(new RowOptions() { Color = this.options.DefaultRowColor, Width = this.options.MinCellDataWidth });
                column.Last().Width = Math.Max(column.Last().Width, header.Length );
            }
        }
        public void AddStringRow(List<string> row)
        {
            AddRow(row.Select(x => new CliCell(x, options.DefaultRowColor) { BackgroundColor = options.DefaultBackgroundColor }).ToList());
        }
        public void AddStringRow(IEnumerable<string> row)
        {
            AddStringRow(row.ToList());
        }

        public void AddRow(List<CliCell> row)
        {
            if (row.Count() != headers.Count)
            {
                throw new ArgumentException("Row length must match header length.");
            }
            cell.Add(row.ToList());
            for (int i = 0; i < column.Count; i++)
            {
                column[i].Width = Math.Max(column[i].Width, Math.Min(row.ElementAt(i).Value.Length, options.MaxCellDataWidth));
            }
            //columnLength = columnLength.Zip(row, (col, rowItem) => Math.Max(col, rowItem.Value.Length)).ToList();
        }

        public void SetColor(int columnIndex, int rowIndex, ConsoleColor color, ConsoleColor? BackgroundColor = null)
        {
            if (columnIndex < 0 || columnIndex >= headers.Count)
                throw new ArgumentOutOfRangeException(nameof(columnIndex), "Column index is out of range.");
            if (rowIndex < 0 || rowIndex >= cell.Count)
                throw new ArgumentOutOfRangeException(nameof(rowIndex), "Row index is out of range.");

            cell[rowIndex][columnIndex].Color = color;
            cell[rowIndex][columnIndex].BackgroundColor = BackgroundColor ?? options.DefaultBackgroundColor;
        }

        public void ColorCells(List<string> notes, CellColorOptions options)
        {

            for (int i = 0; i < cell.Count; i++)

                for (int j = 0; j < cell[i].Count; j++)

                    foreach (string note in notes)
                        if (note == cell[i][j].Value)
                        {
                            cell[i][j].Color = options.Color;
                            cell[i][j].BackgroundColor = options.BackgroundColor;
                        }
        }
        /// <summary>Prints the table to the console.</summary>
        public void Print() => Print(new ConsoleCliOutput());

        /// <summary>Prints the table to any output (console, SVG, HTML, ...).</summary>
        public void Print(ICliOutput output)
        {

            var separators = new CliLineSeparators(options.RowSeparatorMarginLeft, options.RowSeparatorMarginRight, options.ColumnSeparator);
            var formater = new CliLineFormater(this.options, column, separators);
            var bg = options.DefaultBackgroundColor;

            //  Prepare the line 
            var theLine = formater.PrepareLine();

            //  Print Header
            if (options.DrawHeaderLine)
                output.WriteLine(theLine, options.DefaultRowLineColor, bg);
            // Print Header names
            output.WriteLine(formater.PrepareHeader(headers), options.DefaultHeaderColor, bg);

            //  Print line below header  - always visible            
            output.WriteLine(theLine, options.DefaultRowLineColor, bg);

            //  Print Rows
            foreach (var row in cell)
            {
                // Calculate cells width
                formater.CalculateCellWidth(row);

                //  Prepare the first separator  | ----
                output.Write(separators.LeftSeparator, options.DefaultRowLineColor, bg);

                //  Prepare the row data  
                for (var x = 0; x < column.Count; x++)
                {
                    output.Write(row[x].Value, row[x].Color, row[x].BackgroundColor);

                    //  Primt the separator behind cell data  "data |"
                    if (x < column.Count - 1)
                        output.Write(separators.Separator, options.DefaultRowLineColor, bg);
                }

                //  Prepare the last separator  --- |
                output.WriteLine(separators.RightSeparator, options.DefaultRowLineColor, bg);

                //  Print the row below data
                if (options.DrawRowLines)
                    output.WriteLine(theLine, options.DefaultRowLineColor, bg);
            }

            //  Print the last line
            if (!options.DrawRowLines || cell.Count == 0)
                output.Write(theLine, options.DefaultRowLineColor, bg);

            output.WriteLine("", options.DefaultRowLineColor, bg);
        }
    }
}