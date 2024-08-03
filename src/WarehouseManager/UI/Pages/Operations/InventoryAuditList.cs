using System.Data;
using Terminal.Gui;
using WarehouseManager.UI.Utility;
using WarehouseManager.Core.Pages;

namespace WarehouseManager.UI.Pages
{
    public static class InventoryAuditList
    {
        public static void Display()
        {

            Application.Top.RemoveAll();
            var mainWindow = UIComponent.LoggedInMainWindow("Inventory Audits");
            Application.Top.Add(mainWindow);

            var errorLabel = UIComponent.AnnounceLabel();

            var userPermissionLabel = UIComponent.UserPermissionLabel();

            var separatorLine = UIComponent.SeparatorLine();

            var refreshButton = UIComponent.RefreshButton();

            var searchLabel = UIComponent.SearchLabel();

            var searchInput = UIComponent.SearchInput();

            var deleteButton = UIComponent.DeleteButton();
            deleteButton.Visible = UIComponent.CanExecuteMenu(2);

            var addButton = UIComponent.AddButton("Add New Inventory Audit");
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
            var tableView = UIComponent.Table(InventoryAuditListLogic.GetData());

            // Refresh button for renew display
            refreshButton.Clicked += () =>
            {
                Display();
            };

            // Khi người dùng search một chuỗi gì đó
            searchInput.TextChanged += args =>
            {
                tableView.Table = InventoryAuditListLogic.SortInventoryAuditBySearchTerm(tableView.Table, $"{searchInput.Text}"); ;
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

                    tableView.Table = InventoryAuditListLogic.SortInventoryAuditListByColumn(tableView.Table, columnClicked, sortColumnInDescendingOrder);
                }
            };

            // khi bấm vào 1 ô trong bảng
            tableView.CellActivated += args =>
            {
                int inventoryAuditID = (int)tableView.Table.Rows[args.Row][0];
                EditInventoryAudit.Display(inventoryAuditID);
            };

            // khi nút Delete được bấm
            deleteButton.Clicked += () =>
            {
                DataRow selectedRow = tableView.Table.Rows[tableView.SelectedRow];
                int inventoryAuditID = (int)selectedRow[0];

                int result = MessageBox.Query("Delete", "Are you sure you want to delete this?", "No", "Yes");
                if (result == 1) // "Yes" button was pressed
                {
                    try
                    {
                        tableView.Table = InventoryAuditListLogic.DeleteInventoryAudit(tableView.Table, inventoryAuditID);
                        errorLabel.Text = "";
                    }
                    catch (Exception ex)
                    {
                        errorLabel.Text = $"Error: {ex.Message}";
                    }
                }
            };

            // Add Inventory audit Button
            addButton.Clicked += () =>
            {
                AddInventoryAudit.Display();
            };

            tableContainer.Add(tableView);
            mainWindow.Add(refreshButton, searchLabel, searchInput, tableContainer, addButton, deleteButton, errorLabel, userPermissionLabel, separatorLine);
        }

    }
}