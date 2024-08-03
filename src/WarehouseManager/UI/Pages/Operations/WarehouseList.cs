using System.Data;
using Terminal.Gui;
using WarehouseManager.UI.Utility;
using WarehouseManager.Core.Pages;

namespace WarehouseManager.UI.Pages
{
    public static class WarehouseList
    {
        public static void Display()
        {
            Application.Top.RemoveAll();
            var mainWindow = UIComponent.LoggedInMainWindow("Warehouses");
            Application.Top.Add(mainWindow);

            var errorLabel = UIComponent.AnnounceLabel();

            var userPermissionLabel = UIComponent.UserPermissionLabel();

            var separatorLine = UIComponent.SeparatorLine();

            var refreshButton = UIComponent.RefreshButton();

            var searchLabel = UIComponent.SearchLabel();

            var searchInput = UIComponent.SearchInput();

            var deleteButton = UIComponent.DeleteButton();
            deleteButton.Visible = UIComponent.CanExecuteMenu(2);


            var addButton = UIComponent.AddButton("Add New Warehouse");
            addButton.Visible = UIComponent.CanExecuteMenu(3);

            var tableContainer = new FrameView()
            {
                X = 3,
                Y = Pos.Bottom(searchLabel) + 2,
                Width = Dim.Fill(3),
                Height = Dim.Fill(6),
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
                    string warehouseAddressAddress = $"{tableView.Table.Rows[row][2]}";
                    string warehouseAddressDistrict = $"{tableView.Table.Rows[row][3]}";
                    string warehouseAddressPostalCode = $"{tableView.Table.Rows[row][4]}";
                    string warehouseAddressCity = $"{tableView.Table.Rows[row][5]}";
                    string warehouseAddressCountry = $"{tableView.Table.Rows[row][6]}";


                    // try catch để xử lý trường hợp nhập dữ liệu quá dài
                    try
                    {
                        WarehouseListLogic.Update(warehouseID, warehouseName, warehouseAddressAddress, warehouseAddressDistrict, warehouseAddressPostalCode, warehouseAddressCity, warehouseAddressCountry);
                        errorLabel.Text = $"Warehouse {warehouseName} updated successfully";
                        errorLabel.ColorScheme = UIComponent.AnnounceLabelSuccessColor();
                    }
                    catch (Exception ex)
                    {
                        tableView.Table.Rows[row][column] = currentValue;
                        errorLabel.Text = $"Error: {ex.Message}";
                        errorLabel.ColorScheme = UIComponent.AnnounceLabelErrorColor();
                    }


                    Application.RequestStop();
                };

                editDialog.Add(newValue);
                editDialog.AddButton(cancelButton);
                editDialog.AddButton(okButton);

                // Không cho sửa khi click vào id
                if (column != 0 && UIComponent.CanExecuteMenu(2))
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
                        errorLabel.Text = $"Successfully deleted warehouse#{warehouseID}";
                        errorLabel.ColorScheme = UIComponent.AnnounceLabelSuccessColor();
                    }
                    catch (Exception ex)
                    {
                        errorLabel.Text = $"Error: {ex.Message}";
                        errorLabel.ColorScheme = UIComponent.AnnounceLabelErrorColor();
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