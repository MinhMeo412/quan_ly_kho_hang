using System.Data;
using Terminal.Gui;
using WarehouseManager.UI.Utility;
using WarehouseManager.Core;

namespace WarehouseManager.UI.Menu
{
    public static class WarehouseList
    {
        public static void Display()
        {
            Application.Top.RemoveAll();
            var mainWindow = UIComponent.LoggedInMainWindow("Warehouses");
            Application.Top.Add(mainWindow);

            var errorLabel = UIComponent.ErrorMessageLabel();

            var userPermissionLabel = UIComponent.UserPermissionLabel();

            var separatorLine = UIComponent.SeparatorLine();

            var refreshButton = UIComponent.RefreshButton();

            var searchLabel = UIComponent.SearchLabel();

            var searchInput = UIComponent.SearchInput();

            var deleteButton = UIComponent.DeleteButton();

            var addButton = UIComponent.AddButton("Add New Warehouse");

            var tableContainer = new FrameView()
            {
                X = 1,
                Y = Pos.Bottom(searchLabel) + 1,
                Width = Dim.Fill(1),
                Height = Dim.Fill(4),
                Border = new Border() { BorderStyle = BorderStyle.None }
            };

            // Create a TableView and set its data source
            var tableView = UIComponent.Table(WarehouseListLogic.GetData());

            // Khi người dùng bấm nút refresh sẽ tải lại trang
            refreshButton.Clicked += () =>
            {
                Display();
            };

            // Khi người dùng search một chuỗi gì đó
            searchInput.TextChanged += args =>
            {
                tableView.Table = WarehouseListLogic.SortWarehouseBySearchTerm(tableView.Table, $"{searchInput.Text}"); ;
            };

            int columnCurrentlySortBy = -1;
            bool sortColumnInDescendingOrder = false;

            // khi sort cột
            tableView.MouseClick += e =>
            {
                tableView.ScreenToCell(e.MouseEvent.X, e.MouseEvent.Y, out DataColumn clickedCol);
                if (clickedCol != null)
                {
                    int columnClicked = tableView.Table.Columns.IndexOf(clickedCol);
                    if (columnClicked == columnCurrentlySortBy)
                    {
                        sortColumnInDescendingOrder = !sortColumnInDescendingOrder;
                    }

                    columnCurrentlySortBy = columnClicked;
                    searchInput.Text = "";

                    tableView.Table = WarehouseListLogic.SortWarehouseListByColumn(tableView.Table, columnClicked, sortColumnInDescendingOrder);
                }
            };

            // khi bấm vào 1 ô trong bảng
            tableView.CellActivated += args =>
            {
                int column = args.Col;
                int row = args.Row;

                // Retrieve the current value of the cell
                var currentValue = tableView.Table.Rows[row][column].ToString();

                // Create a dialog box with an input field for editing the cell value
                var editDialog = new Dialog("Edit Cell")
                {
                    X = Pos.Center(),
                    Y = Pos.Center(),
                    Width = Dim.Percent(50),
                    Height = Dim.Percent(50)
                };

                var newValue = new TextView()
                {
                    X = Pos.Center(),
                    Y = Pos.Center(),
                    Width = Dim.Fill(),
                    Height = Dim.Fill(),
                    Text = currentValue
                };

                var cancelButton = new Button("Cancel");
                cancelButton.Clicked += () =>
                {
                    Application.RequestStop();
                };

                var okButton = new Button("OK", is_default: true);
                okButton.Clicked += () =>
                {
                    // Update the table with the new value
                    tableView.Table.Rows[row][column] = newValue.Text.ToString();

                    int warehouseID = (int)tableView.Table.Rows[row][0];
                    string warehouseName = $"{tableView.Table.Rows[row][1]}";
                    int? warehouseAddressID = (int?)tableView.Table.Rows[row][2];


                    // try catch để xử lý trường hợp nhập dữ liệu quá dài
                    try
                    {
                        WarehouseListLogic.UpdateWarehouse(warehouseID, warehouseName, warehouseAddressID);
                    }
                    catch (Exception ex)
                    {
                        tableView.Table.Rows[row][column] = currentValue;
                        errorLabel.Text = $"Error: {ex.Message}";
                    }


                    Application.RequestStop();
                };

                editDialog.Add(newValue);
                editDialog.AddButton(cancelButton);
                editDialog.AddButton(okButton);

                // Không cho sửa khi click vào id
                if (column != 0)
                {
                    Application.Run(editDialog);
                }
            };

            deleteButton.Clicked += () =>
            {
                // khi nút Delete được bấm
                DataRow selectedRow = tableView.Table.Rows[tableView.SelectedRow];
                int warehouseID = (int)selectedRow[0];

                int result = MessageBox.Query("Delete", "Are you sure you want to delete this?", "No", "Yes");
                if (result == 1) // "Yes" button was pressed
                {
                    try
                    {
                        tableView.Table = WarehouseListLogic.DeleteWarehouse(tableView.Table, warehouseID);
                        errorLabel.Text = "";
                    }
                    catch (Exception ex)
                    {

                        errorLabel.Text = $"Error: {ex.Message}";
                    }

                }
            };

            // Add Warehouse Button
            addButton.Clicked += () =>
            {
                AddWarehouse.Display();
            };

            tableContainer.Add(tableView);
            mainWindow.Add(refreshButton, searchLabel, searchInput, tableContainer, addButton, deleteButton, errorLabel, userPermissionLabel, separatorLine);
        }
    }
}