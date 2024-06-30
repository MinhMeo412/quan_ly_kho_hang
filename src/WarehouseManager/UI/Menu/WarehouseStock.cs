using System.Data;
using Terminal.Gui;
using WarehouseManager.UI.Utility;

namespace WarehouseManager.UI.Menu
{
    public static class WarehouseStock
    {
        /*
            Todo.
            Xem tồn kho.
        */
        public static void Display()
        {
            Application.Top.RemoveAll();
            var mainWindow = UIComponent.LoggedInMainWindow("Warehouses");
            Application.Top.Add(mainWindow);

            var errorLabel = UIComponent.ErrorMessageLabel("Error Message Here");

            var userPermissionLabel = UIComponent.UserPermissionLabel();

            var separatorLine = UIComponent.SeparatorLine();

            var warehouseCheckList = new Button("Select Warehouse")
            {
                X = 1,
                Y = 1
            };

            var searchContainer = new FrameView()
            {
                X = Pos.Right(warehouseCheckList),
                Width = Dim.Fill(1),
                Height = 3,
                Border = new Border() { BorderStyle = BorderStyle.None }
            };

            var searchLabel = UIComponent.SearchLabel();

            var searchInput = UIComponent.SearchInput();

            var excelButton = UIComponent.AddButton("Export to Excel");

            var tableContainer = new FrameView()
            {
                X = 1,
                Y = Pos.Bottom(searchInput) + 1,
                Width = Dim.Fill(1),
                Height = Dim.Fill(4),
                Border = new Border() { BorderStyle = BorderStyle.None }
            };

            var dataTable = new DataTable();
            dataTable.Columns.Add("ID", typeof(int));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Age", typeof(int));
            dataTable.Rows.Add(1, "Alice", 30);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(3, "Charlie", 35);
            dataTable.Rows.Add(1, "Alice", 30);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(3, "Charlie", 35);
            dataTable.Rows.Add(1, "Alice", 30);
            dataTable.Rows.Add(1, "Alice", 30);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(3, "Charlie", 35);
            dataTable.Rows.Add(1, "Alice", 30);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(3, "Charlie", 35);
            dataTable.Rows.Add(1, "Alice", 30);
            dataTable.Rows.Add(1, "Alice", 30);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(3, "Charlie", 35);
            dataTable.Rows.Add(1, "Alice", 30);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(3, "Charlie", 35);
            dataTable.Rows.Add(1, "Alice", 30);
            dataTable.Rows.Add(1, "Alice", 30);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(3, "Charlie", 35);
            dataTable.Rows.Add(1, "Alice", 30);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(3, "Charlie", 35);
            dataTable.Rows.Add(1, "Alice", 30);
            dataTable.Rows.Add(1, "Alice", 30);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(3, "Charlie", 35);
            dataTable.Rows.Add(1, "Alice", 30);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(3, "Charlie", 35);
            dataTable.Rows.Add(1, "Alice", 30);


            // Create a TableView and set its data source
            var tableView = UIComponent.Table(dataTable);

            // Khi click vào select warehouse
            warehouseCheckList.Clicked += () =>
            {
                var dialog = new Dialog("Warehouses")
                {
                    X = Pos.Center(),
                    Y = Pos.Center(),
                    Width = Dim.Percent(50),
                    Height = Dim.Percent(50)
                };
                var checkList = new List<CheckBox>
                {
                    new CheckBox(1, 1, "Warehouse 1"),
                    new CheckBox(1, 2, "Warehouse 2"),
                    new CheckBox(1, 3, "Warehouse 3")
                };
                foreach (var checkBox in checkList)
                {
                    dialog.Add(checkBox);
                }
                var ok = new Button("Ok", is_default: true);
                ok.Clicked += () =>
                {
                    List<string> selectedWarehouses = new List<string>();
                    foreach (var checkBox in checkList)
                    {
                        if (checkBox.Checked)
                        {
                            selectedWarehouses.Add($"{checkBox.Text}");
                        }
                    }
                    // Print the selected warehouses or do something with the list
                    MessageBox.Query("Selected Warehouses", string.Join("\n", selectedWarehouses), "Ok");
                    Application.RequestStop();
                };
                dialog.AddButton(ok);
                Application.Run(dialog);
            };

            // khi bấm vào cột
            tableView.MouseClick += e =>
            {
                tableView.ScreenToCell(e.MouseEvent.X, e.MouseEvent.Y, out DataColumn clickedCol);
                if (clickedCol != null)
                {
                    MessageBox.Query("Column Clicked", $"Column: {clickedCol}", "OK");
                }
            };

            // khi bấm vào 1 ô trong bảng
            tableView.CellActivated += args =>
            {
                int column = args.Col;
                int row = args.Row;
                MessageBox.Query("Cell Clicked", $"Column: {column}, Row: {row}", "OK");
            };

            excelButton.Clicked += () =>
            {
                // khi nút category đc click
                WarehouseStock.Display();
            };

            tableContainer.Add(tableView);
            searchContainer.Add(searchLabel, searchInput);
            mainWindow.Add(warehouseCheckList, searchLabel,searchInput, tableContainer, excelButton, separatorLine, errorLabel, userPermissionLabel);
        }
    }
}