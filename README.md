# CLITextTable

Mała biblioteka .NET do rysowania tabel tekstowych w konsoli, z kolorami wierszy i pojedynczych komórek.

Powstała jako część [GuitarChordCLI](https://github.com/SebastianEagler/GuitarChordCLI) (rysowanie gryfu gitary i menu).

## Wymagania
- .NET 9

## Przykład
```csharp
using CLITextTable.CliTable;
using CLITextTable.Configuration;

using var table = new CliTable(options =>
{
    options.MinCellDataWidth = 6;
    options.DrawHeaderLine = true;
    options.DrawRowLines = false;
    options.DefaultHeaderColor = ConsoleColor.Yellow;
    options.DefaultRowColor = ConsoleColor.Gray;
    options.DefaultRowLineColor = ConsoleColor.DarkGray;
}, ["Name", "Value"]);

table.AddStringRow(new List<string> { "Root", "C" });
table.AddStringRow(new List<string> { "Scale", "Major" });

// Color every cell whose text equals one of the given values.
table.ColorCells(new List<string> { "C" }, new CellColorOptions { Color = ConsoleColor.Red });

// Color a single cell: column 1, row 1.
table.SetColor(1, 1, ConsoleColor.Green);

table.Print();
```

## Opcje (`CliConfigureOptions`)
| Opcja | Znaczenie | Domyślnie |
|---|---|---|
| `ColumnSeparator`, `RowSeparator`, `FrameConnector` | znaki ramki | `\|`, `-`, `+` |
| `DrawHeaderLine` | linia nad nagłówkiem | `true` |
| `DrawRowLines` | linia pod każdym wierszem | `true` |
| `MinCellDataWidth`, `MaxCellDataWidth` | minimalna i maksymalna szerokość komórki | 3, 64 |
| `RowSeparatorMarginLeft`, `RowSeparatorMarginRight` | odstępy wokół separatora | 0, 0 |
| `DefaultHeaderColor`, `DefaultRowColor`, `DefaultRowLineColor` | kolory tekstu | White |
| `DefaultBackgroundColor` | tło całej tabeli (np. jasny motyw) | Black |

## Budowanie i testy
```
dotnet build
dotnet test
```

## Użycie w innym projekcie
Do czasu publikacji pakietu NuGet dodaj referencję do projektu:
```xml
<ProjectReference Include="..\CLITextTable\src\CLITextTable\CLITextTable.csproj" />
```

## Licencja
MIT, zobacz [LICENSE](LICENSE).
