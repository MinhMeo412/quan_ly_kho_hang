using System.Data;
using Terminal.Gui;
using WarehouseManager.UI.Utility;
using WarehouseManager.Core;

namespace WarehouseManager.UI.Menu
{
    public static class ShipmentList
    {
        public static void Display()
        {
            Application.Top.RemoveAll();
            var mainWindow = UIComponent.LoggedInMainWindow("Shipments");
            Application.Top.Add(mainWindow);

            var errorLabel = UIComponent.ErrorMessageLabel();

            var userPermissionLabel = UIComponent.UserPermissionLabel();

            var separatorLine = UIComponent.SeparatorLine();

            var refreshButton = UIComponent.RefreshButton();

            var searchLabel = UIComponent.SearchLabel();

            var searchInput = UIComponent.SearchInput();

            var deleteButton = UIComponent.DeleteButton();

            var addButtonRight = UIComponent.AddButton("Add New Inbound Shipment");

            var addButtonLeft = UIComponent.AddButton("Add New Outbound Shipment");
            addButtonLeft.X = Pos.Left(addButtonRight) - addButtonLeft.Text.Length - 5;

            var addButtonLeft2 = UIComponent.AddButton("Add New Transfer Shipment");
            addButtonLeft2.X = Pos.Left(addButtonLeft) - addButtonLeft2.Text.Length - 5;

            var tableContainer = new FrameView()
            {
                X = 1,
                Y = Pos.Bottom(searchLabel) + 1,
                Width = Dim.Fill(1),
                Height = Dim.Fill(4),
                Border = new Border() { BorderStyle = BorderStyle.None }
            };

            // Create a TableView and set its data source
            var tableView = UIComponent.Table(ShipmentListLogic.GetData());

            // Refresh button
            refreshButton.Clicked += () =>
            {
                Display();
            };


            // Khi người dùng search một chuỗi gì đó
            searchInput.TextChanged += args =>
            {
                tableView.Table = ShipmentListLogic.SortShipmentListBySearchTerm(tableView.Table, $"{searchInput.Text}"); ;
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

                    tableView.Table = ShipmentListLogic.SortShipmentListByColumn(tableView.Table, columnClicked, sortColumnInDescendingOrder);
                }
            };

            //Row clicked
            tableView.CellActivated += args =>
            {
                int shipmentID = (int)tableView.Table.Rows[args.Row][1];
                string shipmentType = (string)tableView.Table.Rows[args.Row][0];
                switch (shipmentType)
                {
                    case "Inbound":
                        EditInboundShipment.Display(shipmentID);
                        break;
                    case "Outbound":
                        EditOutboundShipment.Display(shipmentID);
                        break;
                    case "Transfer":
                        EditStockTransfer.Display(shipmentID);
                        break;
                }
            };

            //Delete button
            deleteButton.Clicked += () =>
            {
                DataRow selectedRow = tableView.Table.Rows[tableView.SelectedRow];
                int shipmentID = (int)selectedRow[1];
                string shipmentType = (string)selectedRow[0];

                int result = MessageBox.Query("Delete", "Are you sure you want to delete this?", "No", "Yes");
                if (result == 1) // "Yes" button was pressed
                {
                    try
                    {
                        tableView.Table = ShipmentListLogic.DeleteShipment(tableView.Table, shipmentType, shipmentID);
                        errorLabel.Text = "";
                    }
                    catch (Exception ex)
                    {
                        errorLabel.Text = $"Error: {ex.Message}";
                    }
                }
            };

            addButtonRight.Clicked += () =>
            {
                AddInboundShipment.Display();
            };

            addButtonLeft.Clicked += () =>
            {
                AddOutboundShipment.Display();
            };

            addButtonLeft2.Clicked += () =>
            {
                AddStockTransfer.Display();
            };

            tableContainer.Add(tableView);
            mainWindow.Add(refreshButton, searchLabel, searchInput, tableContainer, addButtonRight, addButtonLeft, addButtonLeft2, deleteButton, errorLabel, userPermissionLabel, separatorLine);
        }

    }
}