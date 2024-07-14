using System.Data;
using Terminal.Gui;
using WarehouseManager.UI.Utility;
using WarehouseManager.Core.Pages;

namespace WarehouseManager.UI.Pages
{
    public static class SupplierList
    {
        public static void Display()
        {

            Application.Top.RemoveAll();
            var mainWindow = UIComponent.LoggedInMainWindow("Suppliers");
            Application.Top.Add(mainWindow);

            var errorLabel = UIComponent.AnnounceLabel();

            var userPermissionLabel = UIComponent.UserPermissionLabel();

            var separatorLine = UIComponent.SeparatorLine();

            var refreshButton = UIComponent.RefreshButton();

            var searchLabel = UIComponent.SearchLabel();

            var searchInput = UIComponent.SearchInput();

            var deleteButton = UIComponent.DeleteButton();

            var addButton = UIComponent.AddButton("Add New Supplier");

            var tableContainer = new FrameView()
            {
                X = 3,
                Y = Pos.Bottom(searchLabel) + 2,
                Width = Dim.Fill(3),
                Height = Dim.Fill(6),
                Border = new Border() { BorderStyle = BorderStyle.None }
            };

            // Create a TableView and set its data source
            var tableView = UIComponent.Table(SupplierListLogic.GetData());

            // Khi người dùng bấm nút refresh sẽ tải lại trang
            refreshButton.Clicked += () =>
            {
                Display();
            };

            // Khi người dùng search một chuỗi gì đó
            searchInput.TextChanged += args =>
            {
                tableView.Table = SupplierListLogic.SortSupplierBySearchTerm(tableView.Table, $"{searchInput.Text}"); ;
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

                    tableView.Table = SupplierListLogic.SortSupplierByColumn(tableView.Table, columnClicked, sortColumnInDescendingOrder);
                }
            };


            tableView.CellActivated += args =>
            {
                int column = args.Col;
                int row = args.Row;


                var currentValue = tableView.Table.Rows[row][column].ToString();

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
                    tableView.Table.Rows[row][column] = newValue.Text.ToString();

                    int SupplierID = (int)tableView.Table.Rows[row][0];
                    string SupplierName = $"{tableView.Table.Rows[row][1]}";
                    string SupplierDescription = $"{tableView.Table.Rows[row][2]}";
                    string SupplierAddress = $"{tableView.Table.Rows[row][3]}";
                    string SupplierEmail = $"{tableView.Table.Rows[row][4]}";
                    string SupplierPhoneNumber = $"{tableView.Table.Rows[row][5]}";
                    string SupplierWebsite = $"{tableView.Table.Rows[row][6]}";

                    try
                    {
                        SupplierListLogic.UpdateSupplier(SupplierID, SupplierName, SupplierDescription, SupplierAddress, SupplierEmail, SupplierPhoneNumber, SupplierWebsite);
                        errorLabel.Text = $"Supplier {SupplierName} updated successfully";
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


                if (column != 0)
                {
                    Application.Run(editDialog);
                }
            };

            deleteButton.Clicked += () =>
            {

                DataRow selectedRow = tableView.Table.Rows[tableView.SelectedRow];
                int supplierID = (int)selectedRow[0];

                int result = MessageBox.Query("Delete", "Are you sure you want to delete this supplier?", "No", "Yes");
                if (result == 1)
                {
                    try
                    {
                        tableView.Table = SupplierListLogic.DeleteSupplier(tableView.Table, supplierID);
                        errorLabel.Text = $"Successfully deleted supplier#{supplierID}";
                        errorLabel.ColorScheme = UIComponent.AnnounceLabelSuccessColor();
                    }
                    catch (Exception ex)
                    {
                        errorLabel.Text = $"Error: {ex.Message}";
                        errorLabel.ColorScheme = UIComponent.AnnounceLabelErrorColor();
                    }
                }
            };

            addButton.Clicked += () =>
            {
                AddSupplier.Display();
            };

            tableContainer.Add(tableView);
            mainWindow.Add(searchLabel, searchInput, tableContainer, addButton, deleteButton, errorLabel, userPermissionLabel, separatorLine, refreshButton);
        }
    }
}