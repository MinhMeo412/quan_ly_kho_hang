using System.Data;
using Terminal.Gui;
using WarehouseManager.UI.Utility;

namespace WarehouseManager.UI.Menu
{
    public static class AddOutboundShipment
    {
        /*
            Todo.
            Thêm phiếu xuất.
        */
        public static void Display()
        {
            Application.Top.RemoveAll();
            var mainWindow = UIComponent.LoggedInMainWindow("Add Outbound Shipment");
            Application.Top.Add(mainWindow);

            var errorLabel = UIComponent.ErrorMessageLabel("Error Message Here");

            var userPermissionLabel = UIComponent.UserPermissionLabel("Username", "Permission");

            var separatorLine = UIComponent.SeparatorLine();

            var container = new FrameView()
            {
                X = Pos.Center(),
                Y = 1,
                Width = Dim.Percent(85),
                Height = Dim.Percent(30)
            };

            var fromWarehouseLabel = new Label("From Warehouse:")
            {
                X = 3,
                Y = 1
            };

            var fromWarehouseInput = new TextField("")
            {
                X = 20,
                Y = Pos.Top(fromWarehouseLabel),
                Width = Dim.Percent(25),
            };

            var toAddressLabel = new Label("To Address:")
            {
                X = 3,
                Y = Pos.Center()
            };

            var toAddressInput = new TextField("")
            {
                X = 20,
                Y = Pos.Top(toAddressLabel),
                Width = Dim.Percent(25),
            };

            var descriptionLabel = new Label("Description:")
            {
                X = 3,
                Y = Pos.AnchorEnd() - 2
            };

            var descriptionInput = new TextView()
            {
                X = 20,
                Y = Pos.Top(descriptionLabel),
                Width = Dim.Percent(25),
                Height = 2,
                Text = "",
            };

            var dateLabel = new Label("Date:")
            {
                X = Pos.Percent(60),
                Y = 1
            };

            var dateInput = new TextField("")
            {
                X = 120,
                Y = Pos.Top(dateLabel),
                Width = 50,
            };

            var userLabel = new Label("User:")
            {
                X = 110,
                Y = Pos.Bottom(dateLabel) + 2
            };

            var userInput = new TextField("")
            {
                X = 120,
                Y = Pos.Top(userLabel),
                Width = 50,
            };

            var statusLabel = new Label("Status:")
            {
                X = 110,
                Y = Pos.Bottom(userLabel) + 2
            };

            var options = new string[] { "Processing", "Completed" };

            var statusBox = new ComboBox(options)
            {
                X = 120,
                Y = Pos.Top(statusLabel),
                Width = 50,
                Height = 4
            };


            var tableContainer = new FrameView()
            {
                X = Pos.Center(),
                Y = Pos.Bottom(container) + 2,
                Width = Dim.Percent(95),
                Height = Dim.Fill(7),
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
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(2, "Bob", 25);

            var tableView = UIComponent.Table(dataTable);

            //Tạo nút save
            var saveButton = new Button("Save", is_default: true)
            {
                X = Pos.Center(),
                Y = Pos.Bottom(tableContainer) + 2
            };

            //Khi nhấn nút save
            saveButton.Clicked += () =>
            {
                MessageBox.Query("Save", "", "OK");
            };


            tableContainer.Add(tableView);
            container.Add(fromWarehouseLabel, fromWarehouseInput, descriptionLabel, dateLabel, dateInput, userLabel, userInput, statusLabel, statusBox, toAddressLabel, toAddressInput, descriptionInput);
            mainWindow.Add(container, tableContainer, separatorLine, errorLabel, userPermissionLabel, saveButton);
        }

    }
}