using Terminal.Gui;
using WarehouseManager.Core.Pages;
using WarehouseManager.UI.Utility;

namespace WarehouseManager.UI.Pages
{
    public static class EditInventoryAudit
    {
        public static void Display(int inventoryAuditID)
        {
            Application.Top.RemoveAll();
            var mainWindow = UIComponent.LoggedInMainWindow("Edit Inventory Audit");
            Application.Top.Add(mainWindow);

            bool allowEditInventoryAudit = UIComponent.CanExecuteMenu(2);
            bool allowAddInventoryAuditDetail = UIComponent.CanExecuteMenu(3);
            bool allowUpdateInventoryAuditDetail = UIComponent.CanExecuteMenu(2);

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
                Y = Pos.Bottom(rightContainer) + 1,
                Visible = UIComponent.CanExecuteMenu(3)
            };

            var unsavedLabel = new Label("You have unsaved changes")
            {
                X = Pos.Center(),
                Y = Pos.Bottom(saveButton),
                Visible = false
            };

            var warehouseLabel = new Label("Warehouse:")
            {
                X = 0,
                Y = 1
            };

            var warehouseDropDown = new TextField(EditInventoryAuditLogic.GetWarehouseName(inventoryAuditID))
            {
                X = Pos.Left(warehouseLabel),
                Y = Pos.Bottom(warehouseLabel),
                Width = Dim.Fill(1),
                ReadOnly = true,
            };

            var statusLabel = new Label("Status:")
            {
                X = 0,
                Y = Pos.Bottom(warehouseDropDown) + 1
            };

            var statusDropDown = new ComboBox(EditInventoryAuditLogic.GetStatusList(inventoryAuditID))
            {
                X = Pos.Left(statusLabel),
                Y = Pos.Bottom(statusLabel),
                Width = Dim.Fill(1),
                Height = Dim.Fill(),
                ReadOnly = true,
                SelectedItem = EditInventoryAuditLogic.GetStatusIndex(inventoryAuditID)
            };

            var userLabel = new Label("Created By:")
            {
                X = 0,
                Y = Pos.Bottom(statusLabel) + 2
            };

            var userInput = new TextField(EditInventoryAuditLogic.GetUsername(inventoryAuditID))
            {
                X = Pos.Left(userLabel),
                Y = Pos.Bottom(userLabel),
                Width = Dim.Fill(1),
                ReadOnly = true,
            };

            var timeLabel = new Label("Date:")
            {
                X = 0,
                Y = Pos.Bottom(userInput) + 1
            };

            var timeInput = new TextField($"{EditInventoryAuditLogic.GetCreationDate(inventoryAuditID)}")
            {
                X = Pos.Left(timeLabel),
                Y = Pos.Bottom(timeLabel),
                Width = Dim.Fill(1),
                ReadOnly = true,
            };


            var descriptionLabel = new Label("Description:")
            {
                X = Pos.Left(timeLabel),
                Y = Pos.Bottom(timeLabel) + 2
            };

            var descriptionInput = new TextView()
            {
                X = Pos.Left(descriptionLabel),
                Y = Pos.Bottom(descriptionLabel),
                Width = Dim.Fill(1),
                Height = Dim.Fill(),
                Text = EditInventoryAuditLogic.GetDescription(inventoryAuditID),
                ReadOnly = !allowEditInventoryAudit
            };

            var variantIDLabel = new Label("Variant ID")
            {
                X = 1,
                Y = 1,
                Visible = allowAddInventoryAuditDetail
            };

            var variantIDInput = new TextField()
            {
                X = Pos.Left(variantIDLabel),
                Y = Pos.Bottom(variantIDLabel),
                Width = 10,
                Visible = allowAddInventoryAuditDetail
            };

            Dictionary<int, string> variantDictionary = new Dictionary<int, string>();
            var variantNameDropDown = new ComboBox(variantDictionary.Values.ToList())
            {
                X = Pos.Right(variantIDInput) + 1,
                Y = Pos.Top(variantIDInput),
                Width = Dim.Percent(25),
                Height = Dim.Fill(),
                ReadOnly = true,
                SelectedItem = 0,
                Visible = allowAddInventoryAuditDetail
            };
            variantIDInput.Text = $"{EditInventoryAuditLogic.GetVariantID($"{variantNameDropDown.Text}", variantDictionary)}";

            var variantNameLabel = new Label("Name")
            {
                X = Pos.Left(variantNameDropDown),
                Y = Pos.Top(variantNameDropDown) - 1,
                Visible = allowAddInventoryAuditDetail
            };

            var quantityInput = new TextField()
            {
                X = Pos.Right(variantNameDropDown) + 1,
                Y = Pos.Top(variantNameDropDown),
                Width = 8,
                Visible = allowAddInventoryAuditDetail
            };

            var quantityLabel = new Label("Quantity")
            {
                X = Pos.Left(quantityInput),
                Y = Pos.Top(quantityInput) - 1,
                Visible = allowAddInventoryAuditDetail
            };


            var addBUtton = new Button("Add")
            {
                X = Pos.Right(quantityInput) + 1,
                Y = Pos.Top(quantityInput),
                Visible = allowAddInventoryAuditDetail
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

            var tableView = UIComponent.Table(EditInventoryAuditLogic.GetData(inventoryAuditID));
            tableView.X = 1;
            tableView.Y = Pos.Bottom(variantIDInput) + 2;
            tableView.Height = Dim.Fill(2);

            var deleteButton = new Button("Delete")
            {
                X = Pos.Left(tableView),
                Y = Pos.Bottom(tableView) + 1,
                Visible = allowEditInventoryAudit
            };

            var getAllStockButton = new Button("Get All Stock")
            {
                X = Pos.AnchorEnd(17),
                Y = Pos.Top(deleteButton),
                Visible = allowAddInventoryAuditDetail
            };

            // Behaviors:

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
                    unsavedLabel.Visible = true;

                    Application.RequestStop();
                };

                editDialog.Add(newValue);
                editDialog.AddButton(cancelButton);
                editDialog.AddButton(okButton);

                if (column == 3 && $"{statusDropDown.Text}" == "Processing" && allowEditInventoryAudit)
                {
                    Application.Run(editDialog);
                }
            };

            returnButton.Clicked += () =>
            {
                InventoryAuditList.Display();
            };

            statusDropDown.OpenSelectedItem += (a) =>
            {
                unsavedLabel.Visible = true;
            };

            descriptionInput.TextChanged += () =>
            {
                unsavedLabel.Visible = true;
            };

            variantIDInput.TextChanged += args =>
            {
                variantNameDropDown.SelectedItem = EditInventoryAuditLogic.GetVariantIndex($"{variantIDInput.Text}", variantDictionary);
            };

            variantNameDropDown.OpenSelectedItem += (a) =>
            {
                variantIDInput.Text = $"{EditInventoryAuditLogic.GetVariantID($"{variantNameDropDown.Text}", variantDictionary)}";
            };

            addBUtton.Clicked += () =>
            {
                tableView.Table = EditInventoryAuditLogic.AddVariant($"{warehouseDropDown.Text}", $"{variantIDInput.Text}", $"{quantityInput.Text}", tableView.Table);
                variantNameDropDown.SelectedItem = 0;
                variantIDInput.Text = $"{EditInventoryAuditLogic.GetVariantID($"{variantNameDropDown.Text}", variantDictionary)}";
                quantityInput.Text = "";
                unsavedLabel.Visible = true;
            };

            searchInput.TextChanged += args =>
            {
                tableView.Table = EditInventoryAuditLogic.Search(tableView.Table, $"{searchInput.Text}");
            };

            deleteButton.Clicked += () =>
            {
                tableView.Table = EditInventoryAuditLogic.DeleteVariant(tableView.Table, tableView.SelectedRow);
                unsavedLabel.Visible = true;
            };

            getAllStockButton.Clicked += () =>
            {
                tableView.Table = EditInventoryAuditLogic.GetAllStock($"{warehouseDropDown.Text}", tableView.Table);
                unsavedLabel.Visible = true;
            };

            saveButton.Clicked += () =>
            {
                try
                {
                    EditInventoryAuditLogic.Save(inventoryAuditID, $"{statusDropDown.Text}", $"{descriptionInput.Text}", tableView.Table);

                    statusDropDown.SetSource(EditInventoryAuditLogic.GetStatusList(inventoryAuditID));
                    statusDropDown.SelectedItem = EditInventoryAuditLogic.GetStatusIndex(inventoryAuditID);
                    tableView.Table = EditInventoryAuditLogic.GetData(inventoryAuditID);

                    errorLabel.Text = $"Successfully saved inventory audit.";
                    errorLabel.ColorScheme = UIComponent.AnnounceLabelSuccessColor();

                    unsavedLabel.Visible = false;
                }
                catch (Exception ex)
                {
                    errorLabel.Text = $"Error: {ex}";
                    errorLabel.ColorScheme = UIComponent.AnnounceLabelErrorColor();
                    // MessageBox.Query("", $"{ex}", "ok");
                }
            };

            leftContainer.Add(warehouseLabel, descriptionLabel, statusLabel, userLabel, timeLabel, warehouseDropDown, statusDropDown, userInput, timeInput, descriptionInput);
            rightContainer.Add(searchLabel, searchInput, variantIDLabel, variantNameLabel, variantIDInput, variantNameDropDown, quantityLabel, quantityInput, addBUtton, tableView, deleteButton, getAllStockButton);
            mainWindow.Add(returnButton, errorLabel, userPermissionLabel, separatorLine, leftContainer, rightContainer, saveButton, unsavedLabel);

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