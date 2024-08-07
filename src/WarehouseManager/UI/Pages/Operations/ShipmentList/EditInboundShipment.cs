using System.Data;
using Terminal.Gui;
using WarehouseManager.Core.Pages;
using WarehouseManager.UI.Utility;

namespace WarehouseManager.UI.Pages
{
    public static class EditInboundShipment
    {
        public static void Display(int shipmentID)
        {
            Application.Top.RemoveAll();
            var mainWindow = UIComponent.LoggedInMainWindow("Edit Inbound Shipment");
            Application.Top.Add(mainWindow);

            bool allowUpdateInboundShipment = UIComponent.CanExecuteMenu(2);
            bool allowCreateDetail = UIComponent.CanExecuteMenu(3);
            bool allowEditDetail = UIComponent.CanExecuteMenu(2);
            bool allowDeleteDetail = UIComponent.CanExecuteMenu(2);

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
            var warehouseLabel = new Label("Warehouse:")
            {
                X = 3,
                Y = 1
            };

            var warehouseDropDown = new ComboBox()
            {
                X = 20,
                Y = Pos.Top(warehouseLabel),
                Width = Dim.Percent(60),
                Height = Dim.Fill(1),
                ReadOnly = true
            };
            var warehouses = EditInboundShipmentLogic.GetWarehouseList();
            warehouseDropDown.SetSource(warehouses);
            warehouseDropDown.SelectedItem = EditInboundShipmentLogic.GetInboundShipmentWarehouse(shipmentID);

            var dateLabel = new Label("Date:")
            {
                X = 3,
                Y = Pos.Bottom(warehouseLabel) + 2
            };

            var dateInput = new TextField(EditInboundShipmentLogic.GetInboundShipmentDate(shipmentID).ToString("dd/MM/yyyy h:mm:ss tt"))//(DateTime.Now.ToString("dd/MM/yyyy h:mm:ss tt"))
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
                Text = EditInboundShipmentLogic.GetInboundShipmentDescription(shipmentID),
                ReadOnly = !allowUpdateInboundShipment
            };

            //Right Label/Input
            var supplierNameLabel = new Label("Supplier Name:")
            {
                X = 3,
                Y = 1
            };

            var supplierDropDown = new ComboBox()
            {
                X = 20,
                Y = Pos.Top(supplierNameLabel),
                Width = Dim.Percent(60),
                Height = Dim.Fill(1),
                ReadOnly = true
            };
            var suppliers = EditInboundShipmentLogic.GetSupplierList();
            supplierDropDown.SetSource(suppliers);
            supplierDropDown.SelectedItem = EditInboundShipmentLogic.GetInboundShipmentSupplier(shipmentID);

            var userLabel = new Label("User:")
            {
                X = 3,
                Y = Pos.Bottom(supplierNameLabel) + 2
            };

            var userInput = new TextField(EditInboundShipmentLogic.GetInboundShipmentUserName(shipmentID))
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

            var statusBox = new ComboBox()
            {
                X = 20,
                Y = Pos.Top(statusLabel),
                Width = Dim.Percent(60),
                Height = 3,
                Text = EditInboundShipmentLogic.GetInboundShipmentStatus(shipmentID),
            };
            statusBox.SetSource(options);



            //Item table data (??)
            var dataTable = new DataTable();

            var tableView = UIComponent.Table(EditInboundShipmentLogic.GetInboundShipmentDetailData(shipmentID));

            //Button
            var saveButton = new Button("Save")
            {
                X = Pos.Percent(95),
                Y = Pos.Bottom(tableContainer) + 4,
                Visible = UIComponent.CanExecuteMenu(3)
            };

            var returnButton = new Button("Back")
            {
                X = Pos.Left(saveButton) - 10,
                Y = Pos.Bottom(tableContainer) + 4
            };

            var deleteButton = new Button("Delete")
            {
                X = 1,
                Y = Pos.Bottom(tableContainer) + 4,
                Visible = allowDeleteDetail
            };


            //Khi nhấn nút save (Đổi Date thành thời gian lưu mới nhất)
            saveButton.Clicked += () =>
            {
                try
                {
                    EditInboundShipmentLogic.Save(
                        inboundShipmentID: shipmentID,
                        supplierName: $"{supplierDropDown.Text}",
                        warehouseName: $"{warehouseDropDown.Text}",
                        inboundShipmentStartingDate: DateTime.Now,
                        inboundShipmentStatus: $"{statusBox.Text}",
                        inboundShipmentDescription: $"{descriptionInput.Text}",
                        userName: $"{userInput.Text}"
                    );

                    tableView.Table = EditInboundShipmentLogic.GetInboundShipmentDetailData(shipmentID);
                    errorLabel.Text = $"Successfully saved Inbound Shipment";
                    errorLabel.ColorScheme = UIComponent.AnnounceLabelSuccessColor();
                }
                catch (Exception ex)
                {
                    errorLabel.Text = $"Error: {ex.Message}";
                    errorLabel.ColorScheme = UIComponent.AnnounceLabelErrorColor();
                    tableView.Table = EditInboundShipmentLogic.GetInboundShipmentDetailData(shipmentID);
                }
            };

            //Khi nhấn nút Delete(cho Item)
            deleteButton.Clicked += () =>
            {
                int selectedRowIndex = tableView.SelectedRow;

                // Lấy giá trị từ cột đầu tiên của hàng được chọn
                var selectedRow = tableView.Table.Rows[selectedRowIndex];
                if (int.TryParse(selectedRow[0].ToString(), out int firstColumnValue))
                {
                    // Gọi phương thức DeleteInboundShipmentDetail với giá trị từ cột đầu tiên
                    tableView.Table = EditInboundShipmentLogic.DeleteInboundShipmentDetail(tableView.Table, selectedRowIndex, firstColumnValue, shipmentID);
                }
                else
                {
                    errorLabel.Text = $"Error: Invalid value in first column";
                    errorLabel.ColorScheme = UIComponent.AnnounceLabelErrorColor();
                }
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
                    var quantityString = tableView.Table.Rows[row][column].ToString(); ;
                    int quantity = int.Parse(quantityString ?? "");
                    EditInboundShipmentLogic.UpdateInboundShipmentDetail(tableView.Table, variantID, quantity, shipmentID);
                    Application.RequestStop();
                };

                editDialog.Add(newValue);
                editDialog.AddButton(cancelButton);
                editDialog.AddButton(okButton);

                if (column != 0 && column != 1 && allowEditDetail)
                {
                    Application.Run(editDialog);
                }
            };

            //Item Label/Input
            var productVariantIDLabel = new Label("Product Variant ID:")
            {
                X = 1,
                Y = 0,
                Width = Dim.Percent(40),
                Visible = allowCreateDetail
            };

            var quantityLabel = new Label("Quantity:")
            {
                X = Pos.Right(productVariantIDLabel) + 1,
                Y = 0,
                Visible = allowCreateDetail
            };

            var productVariantIDInput = new TextField("")
            {
                X = Pos.Bottom(productVariantIDLabel),
                Y = 1,
                Width = Dim.Percent(40),
                Visible = allowCreateDetail
            };

            var quantityInput = new TextField("")
            {
                X = Pos.Right(productVariantIDInput) + 1,
                Y = 1,
                Width = Dim.Percent(40),
                Visible = allowCreateDetail
            };
            //Add Item Button
            var addItemButton = new Button("Add Item")
            {
                X = Pos.Right(quantityInput),
                Y = 1,
                Width = Dim.Percent(20),
                Visible = allowCreateDetail
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
                        tableView.Table = EditInboundShipmentLogic.AddInboundShipmentDetail(tableView.Table, productVariantID, quantity, shipmentID);

                        productVariantIDInput.Text = "";
                        quantityInput.Text = "";
                    }
                    else
                    {
                        // Xử lý lỗi khi chuyển đổi thất bại
                        // MessageBox.Query("Lỗi", "Vui lòng nhập giá trị hợp lệ cho Product Variant ID và Quantity.", "OK");
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
            leftContainer.Add(warehouseLabel, warehouseDropDown, descriptionLabel, descriptionInput, dateLabel, dateInput);
            rightContainer.Add(userLabel, userInput, statusLabel, statusBox, supplierNameLabel, supplierDropDown);
            container.Add(leftContainer, rightContainer);
            mainWindow.Add(container, tableContainer, separatorLine, errorLabel, userPermissionLabel, saveButton, deleteButton, returnButton, itemInputContainer);
        }
    }
}