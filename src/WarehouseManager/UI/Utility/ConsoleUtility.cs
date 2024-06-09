using Spectre.Console;

namespace WarehouseManager.UI.Utility
{
    public static class ConsoleUtility
    {
        public static string Input(string prompt = "")
        {
            return AnsiConsole.Prompt(new TextPrompt<string>(prompt).AllowEmpty());
        }

        public static bool Confirmation(string prompt)
        {
            if (AnsiConsole.Confirm(prompt))
            {
                return true;
            }
            return false;
        }

        public static string SingleSelection(List<string> options)
        {
            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select an option. Use [green]up/down[/] arrow keys to navigate. Press [green]space/enter[/] to confirm.")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                    .AddChoices(options));

            return selection;
        }

        public static List<int> MultipleSelection(string title, List<string> choices, bool required = true)
        {
            List<int> selected = new List<int>();

            if (required)
            {
                var selection = AnsiConsole.Prompt(
                    new MultiSelectionPrompt<string>()
                        .Title(title)
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                        .InstructionsText("[grey](Press [blue]<space>[/] to toggle an option, " + "[green]<enter>[/] to accept)[/]")
                        .AddChoiceGroup("All", choices)
                );
                foreach (string choice in selection)
                {
                    selected.Add(choices.IndexOf(choice));
                }
            }
            else
            {
                var selection = AnsiConsole.Prompt(
                    new MultiSelectionPrompt<string>()
                        .Title(title)
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                        .InstructionsText("[grey](Press [blue]<space>[/] to toggle an option, " + "[green]<enter>[/] to accept)[/]")
                        .AddChoiceGroup("All", choices)
                        .NotRequired()
                );
                foreach (string choice in selection)
                {
                    selected.Add(choices.IndexOf(choice));
                }
            }

            return selected;
        }
        
        public static void PrintFiglet(string figlet)
        {
            try
            {
                var font = FigletFont.Load("ANSI_Shadow.flf");
                AnsiConsole.Write(new FigletText(font, figlet).LeftJustified().Color(Color.SteelBlue));
            }
            catch (System.Exception)
            {
                AnsiConsole.Write(new FigletText(figlet).LeftJustified().Color(Color.SteelBlue));
            }
        }
        
        public static void PrintTable(List<string> columns, List<List<string>> rows)
        {
            var table = new Table();
            table.Border(TableBorder.Rounded);
            table.BorderColor(Color.SteelBlue);

            for (int i = 0; i < columns.Count; i++)
            {
                table.AddColumn(new TableColumn($"[orange3]{columns[i]}[/]"));
                table.Columns[i].LeftAligned();
            }

            for (int i = 0; i < rows.Count; i++)
            {
                string[] row = rows[i].ToArray();
                table.AddRow(row);
            }

            AnsiConsole.Write(table);
        }
        
        public static void WrappingTable(List<string> columns, List<List<string>> rows, int pageSize = 10)
        {
            int pageCount = (int)Math.Ceiling((double)rows.Count / (double)pageSize);
            int currentPage = 1;
            int currentStartingRow = 0;

            ConsoleKeyInfo pressedKey;
            while (true)
            {
                currentStartingRow = (currentPage - 1) * pageSize;
                try
                {
                    PrintTable(columns, rows.GetRange(currentStartingRow, pageSize));
                }
                catch (Exception)
                {
                    PrintTable(columns, rows.GetRange(currentStartingRow, rows.Count - currentStartingRow));
                    if (currentPage != 1)
                    {
                        for (int i = 0; i < pageSize - (rows.Count - currentStartingRow); i++)
                        {
                            Console.WriteLine();
                        }
                    }
                }
                AnsiConsole.Markup($"Current Page: <[cyan]{currentPage}[/]/{pageCount}>\n");
                AnsiConsole.Markup($"Use [green]left/right[/] arrow keys to navigate between pages. Press [green]space/enter[/] to exit viewing.\n");

                while (true)
                {
                    pressedKey = Console.ReadKey();
                    if (pressedKey.Key == ConsoleKey.LeftArrow && currentPage > 1)
                    {
                        currentPage -= 1;
                        break;
                    }
                    if (pressedKey.Key == ConsoleKey.RightArrow && currentPage < pageCount)
                    {
                        currentPage += 1;
                        break;
                    }
                    if (pressedKey.Key == ConsoleKey.Spacebar || pressedKey.Key == ConsoleKey.Enter)
                    {
                        Console.WriteLine();
                        return;
                    }
                }

                // calculate the amount of space needed to clear the previous page
                int longestRowSize = 0;
                List<int> columnLengths = new List<int>();
                for (int i = 0; i < columns.Count; i++)
                {
                    int maxLength = columns[i].Length;
                    for (int ii = 0; ii < rows.Count; ii++)
                    {
                        if (maxLength < rows[ii][i].Length)
                        {
                            maxLength = rows[ii][i].Length;
                        }
                    }
                    columnLengths.Add(maxLength);
                }
                foreach (int columnLength in columnLengths)
                {
                    longestRowSize += columnLength;
                }
                longestRowSize += (2 * columns.Count) + (columns.Count + 1);
                string hint = "Use left/right arrow keys to navigate between pages. Press space/enter to exit viewing.";
                if (longestRowSize < hint.Length)
                {
                    longestRowSize = hint.Length;
                }

                // clear the previous page
                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - (6 + pageSize));
                string eraser = "";
                for (int i = 0; i < longestRowSize; i++)
                {
                    eraser = $"{eraser} ";
                }
                for (int i = 0; i < 6 + pageSize; i++)
                {
                    Console.WriteLine(eraser);
                }
                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - (6 + pageSize));
            }
        }
    }
}