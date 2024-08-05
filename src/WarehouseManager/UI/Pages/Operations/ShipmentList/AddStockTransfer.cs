using System.Data;
using Terminal.Gui;
using WarehouseManager.Core.Pages;
using WarehouseManager.UI.Utility;

namespace WarehouseManager.UI.Pages
{
    public static class AddStockTransfer
    {
        public static void Display()
        {
            Application.Top.RemoveAll();
            var mainWindow = UIComponent.LoggedInMainWindow("Add Stock Transfer");
            Application.Top.Add(mainWindow);

            var errorLabel = UIComponent.AnnounceLabel();

            var userPermissionLabel = UIComponent.UserPermissionLabel();

            var separatorLine = UIComponent.SeparatorLine();

            //Container
            var container = new FrameView()
            {
                X = Pos.Center(),
                Y = 1,
                Width = Dim.Percent(85),
                Height = Dim.Percent(30)
            };
            var leftContainer = new FrameView()
            {
                X = 0,
                Y = 0,
                Width = Dim.Percent(50),
                Height = Dim.Percent(100),
                Border = new Border() { BorderStyle = BorderStyle.None }
            };
            var rightContainer = new FrameView()
            {
                X = Pos.Percent(50),
                Y = 0,
                Width = Dim.Percent(50),
                Height = Dim.Percent(100),
                Border = new Border() { BorderStyle = BorderStyle.None }
            };

            var tableContainer = new FrameView()
            {
                X = Pos.Center(),
                Y = Pos.Bottom(container) + 1,
                Width = Dim.Percent(95),
                Height = Dim.Fill(8),
            };

            var itemInputContainer = new FrameView()
            {
                X = Pos.Percent(57),
                Y = Pos.Bottom(tableContainer),
                Width = Dim.Percent(40),
                Height = 2,
                Border = new Border() { BorderStyle = BorderStyle.None }
            };

            //Left Label/Input
            var fromWarehouseLabel = new Label("From Warehouse:")
            {
                X = 3,
                Y = 1
            };

            var fromWarehouseDropDown = new ComboBox()
            {
                X = 20,
                Y = Pos.Top(fromWarehouseLabel),
                Width = Dim.Percent(60),
                Height = Dim.Fill(1),
                ReadOnly = true
            };
            var warehouses = AddStockTransferLogic.GetWarehouseList();
            fromWarehouseDropDown.SetSource(warehouses);

            var dateLabel = new Label("Date:")
            {
                X = 3,
                Y = Pos.Bottom(fromWarehouseLabel) + 2
            };

            var dateInput = new TextField(DateTime.Now.ToString("dd/MM/yyyy h:mm:ss tt"))
            {
                X = 20,
                Y = Pos.Top(dateLabel),
                Width = Dim.Percent(60),
                ReadOnly = true
            };

            var descriptionLabel = new Label("Description:")
            {
                X = 3,
                Y = Pos.Bottom(dateLabel) + 2
            };

            var descriptionInput = new TextView()
            {
                X = 20,
                Y = Pos.Top(descriptionLabel),
                Width = Dim.Percent(60),
                Height = 3,
                Text = "",
            };

            //Right Label/Input
            var toWarehouseLabel = new Label("To Warehouse :")
            {
                X = 3,
                Y = 1
            };

            var toWarehouseDropDown = new ComboBox()
            {
                X = 20,
                Y = Pos.Top(toWarehouseLabel),
                Width = Dim.Percent(60),
                Height = Dim.Fill(1),
                ReadOnly = true
            };
            var toWarehouses = AddStockTransferLogic.GetWarehouseList();
            toWarehouseDropDown.SetSource(toWarehouses);

            var userLabel = new Label("User:")
            {
                X = 3,
                Y = Pos.Bottom(toWarehouseLabel) + 2
            };

            var userInput = new TextField(AddStockTransferLogic.GetUserFullName())
            {
                X = 20,
                Y = Pos.Top(userLabel),
                Width = Dim.Percent(60),
                ReadOnly = true
            };

            var statusLabel = new Label("Status:")
            {
                X = 3,
                Y = Pos.Bottom(userLabel) + 2
            };

            var options = new string[] { "Processing", "Completed" };

            var statusBox = new ComboBox(options)
            {
                X = 20,
                Y = Pos.Top(statusLabel),
                Width = Dim.Percent(60),
                Height = 3
            };


            //Item table data
            var dataTable = new DataTable();

            var tableView = UIComponent.Table(AddStockTransferLogic.GetDataTable());


            //Button
            var saveButton = new Button("Save")
            {
                X = Pos.Percent(95),
                Y = Pos.Bottom(tableContainer) + 4
            };

            var returnButton = new Button("Back")
            {
                X = Pos.Left(saveButton) - 10,
                Y = Pos.Bottom(tableContainer) + 4
            };

            var deleteButton = new Button("Delete")
            {
                X = 1,
                Y = Pos.Bottom(tableContainer) + 4
            };


            //Khi nhấn nút save (Đổi Date thành thời gian lưu mới nhất)
            saveButton.Clicked += () =>
            {
                try
                {
                    AddStockTransferLogic.Save(
                        fromWarehouseName: $"{fromWarehouseDropDown.Text}",
                        toWarehouseName: $"{toWarehouseDropDown.Text}",
                        stockTransferStartingDate: DateTime.Now,
                        stockTransferStatus: $"{statusBox.Text}",
                        stockTransferDescription: $"{descriptionInput.Text}",
                        userName: $"{userInput.Text}",
                        dataTable: tableView.Table
                    );

                    errorLabel.Text = $"Successfully created Stock Transfer";
                    errorLabel.ColorScheme = UIComponent.AnnounceLabelSuccessColor();
                }
                catch (Exception ex)
                {
                    errorLabel.Text = $"Error: {ex.Message}";
                    errorLabel.ColorScheme = UIComponent.AnnounceLabelErrorColor();
                }
            };

            //Khi nhấn nút Delete(cho Item)
            deleteButton.Clicked += () =>
            {
                tableView.Table = AddStockTransferLogic.DeleteStockTransferDetail(tableView.Table, tableView.SelectedRow);
            };

            //Khi nhấn nút Back
            returnButton.Clicked += () =>
            {
                ShipmentList.Display();
            };

            //Khi nhấn vào 1 ô trong bảng để sửa
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

                //Lấy Product variant ID
                var variantIDString = tableView.Table.Rows[row][0].ToString();
                int variantID = int.Parse(variantIDString ?? "");

                var okButton = new Button("OK", is_default: true);
                okButton.Clicked += () =>
                {
                    // Update the table with the new value
                    tableView.Table.Rows[row][column] = newValue.Text.ToString();
                    Application.RequestStop();
                };

                if (column != 0 && column != 1)
                {
                    Application.Run(editDialog);
                }

                editDialog.Add(newValue);
                editDialog.AddButton(cancelButton);
                editDialog.AddButton(okButton);
            };


            //Item Label/Input
            var productVariantIDLabel = new Label("Product Variant ID:")
            {
                X = 1,
                Y = 0,
                Width = Dim.Percent(40)
            };

            var quantityLabel = new Label("Quantity:")
            {
                X = Pos.Right(productVariantIDLabel) + 1,
                Y = 0,
            };

            var productVariantIDInput = new TextField("")
            {
                X = Pos.Bottom(productVariantIDLabel),
                Y = 1,
                Width = Dim.Percent(40)
            };

            var quantityInput = new TextField("")
            {
                X = Pos.Right(productVariantIDInput) + 1,
                Y = 1,
                Width = Dim.Percent(40)
            };
            //Add Item Button
            var addItemButton = new Button("Add Item")
            {
                X = Pos.Right(quantityInput),
                Y = 1,
                Width = Dim.Percent(20)
            };


            // Khi nhấn nút Add Item
            addItemButton.Clicked += () =>
            {
                string productVariantIDText = productVariantIDInput.Text.ToString() ?? "";
                string quantityText = quantityInput.Text.ToString() ?? "";
                // Kiểm tra nếu các TextField không trống
                if (!string.IsNullOrEmpty(productVariantIDText) && !string.IsNullOrEmpty(quantityText))
                {
                    // Chuyển đổi giá trị TextField từ chuỗi sang số nguyên
                    if (int.TryParse(productVariantIDText, out int productVariantID) && int.TryParse(quantityText, out int quantity))
                    {
                        tableView.Table = AddStockTransferLogic.AddStockTransferDetail(tableView.Table, productVariantID, quantity);

                        productVariantIDInput.Text = "";
                        quantityInput.Text = "";
                    }
                    else
                    {
                        // Xử lý lỗi khi chuyển đổi thất bại
                        errorLabel.Text = $"Error: Invalid values for Product Variant ID and Quantity";
                        errorLabel.ColorScheme = UIComponent.AnnounceLabelErrorColor();
                    }
                }
                else
                {
                    // Xử lý lỗi khi các trường TextField trống
                    errorLabel.Text = $"Error: Fields cannot be blank.";
                    errorLabel.ColorScheme = UIComponent.AnnounceLabelErrorColor();
                }
            };

            //Add display object
            itemInputContainer.Add(addItemButton, productVariantIDLabel, productVariantIDInput, quantityLabel, quantityInput);
            tableContainer.Add(tableView);
            leftContainer.Add(fromWarehouseLabel, fromWarehouseDropDown, descriptionLabel, descriptionInput, dateLabel, dateInput);
            rightContainer.Add(userLabel, userInput, statusLabel, statusBox, toWarehouseLabel, toWarehouseDropDown);
            container.Add(leftContainer, rightContainer);
            mainWindow.Add(container, tableContainer, separatorLine, errorLabel, userPermissionLabel, saveButton, deleteButton, returnButton, itemInputContainer);
        }
    }
}