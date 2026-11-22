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

## Przykładowe tabele
Pełny kod przykładów jest w [examples/CLITextTable.Examples/Program.cs](examples/CLITextTable.Examples/Program.cs). Uruchomienie:
```
dotnet run --project examples/CLITextTable.Examples
```
Poniżej wynik w postaci tekstu (w konsoli tabele są dodatkowo kolorowane: nagłówki, linie, wybrane komórki i tło).

**1. Podstawowa tabela (linie pod każdym wierszem)**
```
+--------------+--------+--------+
|Name          |Language|Stars   |
+--------------+--------+--------+
|CLITextTable  |C#      |12      |
+--------------+--------+--------+
|GuitarChordCLI|C#      |5       |
+--------------+--------+--------+
|Notes         |Markdown|1       |
+--------------+--------+--------+
```

**2. Kompaktowa tabela (bez linii wierszy, z odstępami wokół separatorów)**
```
+-------------+------------+----------------+
| Quality [m] | Root       | Chord type [t] |
+-------------+------------+----------------+
| Major       | C          | Triad          |
+-------------+------------+----------------+
```

**3. Własne znaki ramki i ograniczenie szerokości komórek (`MaxCellDataWidth`)**
```
+======+============#
!Id    !Description !
+======+============#
!1     !Short       !
!2     !A very long !
+======+============#
```

**4. Kolorowanie komórek (`ColorCells`, `SetColor`)** - fragment gryfu gitary; nuty skali na zielono, akord C (C, E, G) na czerwono, jedna komórka czarna na żółtym
```
  0     1     2     3     4     5
+-----+-----+-----+-----+-----+-----+
| E   | F   | F#  | G   | G#  | A   |
| B   | C   | C#  | D   | D#  | E   |
| G   | G#  | A   | A#  | B   | C   |
+-----+-----+-----+-----+-----+-----+
```

**5. Jasne tło (`DefaultBackgroundColor`)**
```
+----------+------------+----------+
| Theme    | Background | Text     |
+----------+------------+----------+
| Light    | White      | Black    |
| Dark     | Black      | Gray     |
+----------+------------+----------+
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
