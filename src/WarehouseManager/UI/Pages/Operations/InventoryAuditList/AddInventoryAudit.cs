using Terminal.Gui;
using WarehouseManager.Core.Pages;
using WarehouseManager.UI.Utility;

namespace WarehouseManager.UI.Pages
{
    public static class AddInventoryAudit
    {
        public static void Display()
        {
            Application.Top.RemoveAll();
            var mainWindow = UIComponent.LoggedInMainWindow("Add New Inventory Audit");
            Application.Top.Add(mainWindow);

            var errorLabel = UIComponent.AnnounceLabel();

            var userPermissionLabel = UIComponent.UserPermissionLabel();

            var separatorLine = UIComponent.SeparatorLine();


            var returnButton = new Button("Back")
            {
                X = 3,
                Y = 1
            };

            var leftContainer = new FrameView()
            {
                X = 3,
                Y = 2,
                Width = Dim.Percent(25),
                Height = Dim.Fill(5),
                Border = new Border() { BorderStyle = BorderStyle.None }
            };

            var rightContainer = new FrameView()
            {
                X = Pos.Right(leftContainer) + 1,
                Y = Pos.Top(leftContainer),
                Width = Dim.Fill(3),
                Height = Dim.Fill(5),
                Border = new Border() { BorderStyle = BorderStyle.None }
            };


            var saveButton = new Button("Save", is_default: true)
            {
                X = Pos.Center(),
                Y = Pos.Bottom(rightContainer) + 1
            };

            var warehouseLabel = new Label("Warehouse:")
            {
                X = 0,
                Y = 1
            };

            var warehouseDropDown = new ComboBox(AddInventoryAuditLogic.GetWarehouseNames())
            {
                X = Pos.Left(warehouseLabel),
                Y = Pos.Bottom(warehouseLabel),
                Width = Dim.Fill(1),
                Height = Dim.Fill(),
                ReadOnly = true,
                SelectedItem = 0
            };


            var descriptionLabel = new Label("Description:")
            {
                X = Pos.Left(warehouseLabel),
                Y = Pos.Bottom(warehouseLabel) + 2
            };

            var descriptionInput = new TextView()
            {
                X = Pos.Left(descriptionLabel),
                Y = Pos.Bottom(descriptionLabel),
                Width = Dim.Fill(1),
                Height = Dim.Fill(),
                Text = ""
            };

            var variantIDLabel = new Label("Variant ID")
            {
                X = 1,
                Y = 1,
            };

            var variantIDInput = new TextField()
            {
                X = Pos.Left(variantIDLabel),
                Y = Pos.Bottom(variantIDLabel),
                Width = 10
            };

            Dictionary<int, string> variantDictionary = new Dictionary<int, string>();
            var variantNameDropDown = new ComboBox(variantDictionary.Values.ToList())
            {
                X = Pos.Right(variantIDInput) + 1,
                Y = Pos.Top(variantIDInput),
                Width = Dim.Percent(25),
                Height = Dim.Fill(),
                ReadOnly = true,
                SelectedItem = 0
            };
            variantIDInput.Text = $"{AddInventoryAuditLogic.GetVariantID($"{variantNameDropDown.Text}", variantDictionary)}";

            var variantNameLabel = new Label("Name")
            {
                X = Pos.Left(variantNameDropDown),
                Y = Pos.Top(variantNameDropDown) - 1,
            };

            var quantityInput = new TextField()
            {
                X = Pos.Right(variantNameDropDown) + 1,
                Y = Pos.Top(variantNameDropDown),
                Width = 8
            };

            var quantityLabel = new Label("Quantity")
            {
                X = Pos.Left(quantityInput),
                Y = Pos.Top(quantityInput) - 1,
            };


            var addBUtton = new Button("Add")
            {
                X = Pos.Right(quantityInput) + 1,
                Y = Pos.Top(quantityInput)
            };


            var searchLabel = new Label("Search")
            {
                X = Pos.Right(quantityLabel) + 32,
                Y = 1,
            };

            var searchInput = new TextField()
            {
                X = Pos.Left(searchLabel),
                Y = Pos.Bottom(searchLabel),
                Width = Dim.Fill()
            };

            var tableView = UIComponent.Table(AddInventoryAuditLogic.GetDataTable());
            tableView.X = 1;
            tableView.Y = Pos.Bottom(variantIDInput) + 2;
            tableView.Height = Dim.Fill(2);

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
                    tableView.Table = EditInventoryAuditLogic.EditVariant(row, $"{newValue.Text}", tableView.Table);

                    Application.RequestStop();
                };

                editDialog.Add(newValue);
                editDialog.AddButton(cancelButton);
                editDialog.AddButton(okButton);

                if (column == 3)
                {
                    Application.Run(editDialog);
                }
            };

            var deleteButton = new Button("Delete")
            {
                X = Pos.Left(tableView),
                Y = Pos.Bottom(tableView) + 1
            };

            var getAllStockButton = new Button("Get All Stock")
            {
                X = Pos.AnchorEnd(17),
                Y = Pos.Top(deleteButton)
            };

            returnButton.Clicked += () =>
            {
                InventoryAuditList.Display();
            };

            warehouseDropDown.OpenSelectedItem += (e) =>
            {
                tableView.Table = AddInventoryAuditLogic.GetDataTable();
            };

            variantIDInput.TextChanged += args =>
            {
                variantNameDropDown.SelectedItem = AddInventoryAuditLogic.GetVariantIndex($"{variantIDInput.Text}", variantDictionary);
            };

            variantNameDropDown.OpenSelectedItem += (a) =>
            {
                variantIDInput.Text = $"{AddInventoryAuditLogic.GetVariantID($"{variantNameDropDown.Text}", variantDictionary)}";
            };

            addBUtton.Clicked += () =>
            {
                tableView.Table = AddInventoryAuditLogic.AddVariant($"{warehouseDropDown.Text}", $"{variantIDInput.Text}", $"{quantityInput.Text}", tableView.Table);
                variantNameDropDown.SelectedItem = 0;
                variantIDInput.Text = $"{AddInventoryAuditLogic.GetVariantID($"{variantNameDropDown.Text}", variantDictionary)}";
                quantityInput.Text = "";
            };

            searchInput.TextChanged += args =>
            {
                tableView.Table = AddInventoryAuditLogic.Search(tableView.Table, $"{searchInput.Text}");
            };

            deleteButton.Clicked += () =>
            {
                tableView.Table = AddInventoryAuditLogic.DeleteVariant(tableView.Table, tableView.SelectedRow);
            };

            getAllStockButton.Clicked += () =>
            {
                tableView.Table = AddInventoryAuditLogic.GetAllStock($"{warehouseDropDown.Text}", tableView.Table);
            };

            saveButton.Clicked += () =>
            {
                try
                {
                    AddInventoryAuditLogic.Save($"{warehouseDropDown.Text}", $"{descriptionInput.Text}", tableView.Table);

                    warehouseDropDown.SelectedItem = 0;
                    descriptionInput.Text = "";
                    variantNameDropDown.SelectedItem = 0;
                    variantIDInput.Text = $"{AddInventoryAuditLogic.GetVariantID($"{variantNameDropDown.Text}", variantDictionary)}";
                    quantityInput.Text = "";
                    searchInput.Text = "";
                    tableView.Table = AddInventoryAuditLogic.GetDataTable();

                    errorLabel.Text = $"Successfully saved inventory audit.";
                    errorLabel.ColorScheme = UIComponent.AnnounceLabelSuccessColor();
                }
                catch (Exception ex)
                {
                    errorLabel.Text = $"Error: {ex}";
                    errorLabel.ColorScheme = UIComponent.AnnounceLabelErrorColor();
                    // MessageBox.Query("", $"{ex}", "ok");
                }
            };

            leftContainer.Add(warehouseLabel, descriptionLabel, warehouseDropDown, descriptionInput);
            rightContainer.Add(searchLabel, searchInput, variantIDLabel, variantNameLabel, variantIDInput, variantNameDropDown, quantityLabel, quantityInput, addBUtton, tableView, deleteButton, getAllStockButton);
            mainWindow.Add(returnButton, errorLabel, userPermissionLabel, separatorLine, leftContainer, rightContainer, saveButton);

            Task.Run(() =>
            {
                variantDictionary = AddInventoryAuditLogic.GetVariantList();
                Application.MainLoop.Invoke(() =>
                {
                    variantNameDropDown.SetSource(variantDictionary.Values.ToList());
                    variantNameDropDown.SelectedItem = 0;
                    variantIDInput.Text = $"{AddInventoryAuditLogic.GetVariantID($"{variantNameDropDown.Text}", variantDictionary)}";
                });
            });
        }
    }
}