using System.Data;
using Terminal.Gui;
using WarehouseManager.Core.Pages;
using WarehouseManager.UI.Utility;
using WarehouseManager.Data.Entity;

namespace WarehouseManager.UI.Pages
{
    public static class AddOutboundShipment
    {
        public static void Display()
        {
            Application.Top.RemoveAll();
            var mainWindow = UIComponent.LoggedInMainWindow("Add Outbound Shipment");
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
            var warehouses = AddOutboundShipmentLogic.GetWarehouseList();
            warehouseDropDown.SetSource(warehouses);

            var dateLabel = new Label("Date:")
            {
                X = 3,
                Y = Pos.Bottom(warehouseLabel) + 2
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
            var addressLabel = new Label("To Address:")
            {
                X = 3,
                Y = 1
            };

            var addressInput = new TextField("")
            {
                X = 20,
                Y = Pos.Top(addressLabel),
                Width = Dim.Percent(60),
            };

            var userLabel = new Label("User:")
            {
                X = 3,
                Y = Pos.Bottom(addressLabel) + 2
            };

            var userInput = new TextField(AddOutboundShipmentLogic.GetUserFullName())
            {
                X = 20,
                Y = Pos.Top(userLabel),
                Width = Dim.Percent(60),
                ReadOnly = true
            };

            var statusLabel = new Label("Status:")
            {
                X = 3,
                Y = Pos.Bottom(userLabel) + 2,
                Visible = false
            };

            var options = new string[] { "Processing", "Completed" };

            var statusBox = new ComboBox(options)
            {
                X = 20,
                Y = Pos.Top(statusLabel),
                Width = Dim.Percent(60),
                Height = 3,
                SelectedItem = 0,
                Visible = false
            };


            //
            var tableView = UIComponent.Table(AddOutboundShipmentLogic.GetDataTable());


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


            //Khi nhấn nút save (Đổi Date thành thời gian lưu mới nhất)
            saveButton.Clicked += () =>
            {
                try
                {
                    AddOutboundShipmentLogic.Save(
                        outboundShipmentAddress: $"{addressInput.Text}",
                        warehouseName: $"{warehouseDropDown.Text}",
                        outboundShipmentStartingDate: DateTime.Now,
                        outboundShipmentStatus: $"{statusBox.Text}",
                        outboundShipmentDescription: $"{descriptionInput.Text}",
                        userName: $"{userInput.Text}",
                        dataTable: tableView.Table
                    );

                    errorLabel.Text = $"Successfully created Outbound Shipment";
                    errorLabel.ColorScheme = UIComponent.AnnounceLabelSuccessColor();
                }
                catch (Exception ex)
                {
                    errorLabel.Text = $"Error: {ex.Message}";
                    errorLabel.ColorScheme = UIComponent.AnnounceLabelErrorColor();
                }
            };


            //Khi nhấn nút Back
            returnButton.Clicked += () =>
            {
                ShipmentList.Display();
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
                string warehouseText = warehouseDropDown.Text.ToString() ?? "";

                try
                {
                    if (!string.IsNullOrEmpty($"{warehouseDropDown.Text}"))
                    {
                        // Kiểm tra nếu các TextField không trống
                        if (!string.IsNullOrEmpty(productVariantIDText) && !string.IsNullOrEmpty(quantityText))
                        {
                            // Chuyển đổi giá trị TextField từ chuỗi sang số nguyên
                            if (int.TryParse(productVariantIDText, out int productVariantID) && int.TryParse(quantityText, out int quantity))
                            {
                                string result = AddOutboundShipmentLogic.CheckVariantAdded(warehouseText, productVariantID, quantity);

                                if (!string.IsNullOrEmpty(result))
                                {
                                    // Nếu có thông báo lỗi từ CheckVariantAdded, hiển thị lỗi và dừng thực hiện
                                    errorLabel.Text = $"Error: {result}";
                                    errorLabel.ColorScheme = UIComponent.AnnounceLabelErrorColor();
                                    return;
                                }

                                DataTable updatedTable = AddOutboundShipmentLogic.AddOutboundShipmentDetailToDataTable(tableView.Table, productVariantID, quantity);

                                if (updatedTable != null)
                                {
                                    tableView.Table = updatedTable;

                                    AddOutboundShipmentLogic.Save(
                                        outboundShipmentAddress: $"{addressInput.Text}",
                                        warehouseName: $"{warehouseDropDown.Text}",
                                        outboundShipmentStartingDate: DateTime.Now,
                                        outboundShipmentStatus: $"{statusBox.Text}",
                                        outboundShipmentDescription: $"{descriptionInput.Text}",
                                        userName: $"{userInput.Text}",
                                        dataTable: tableView.Table);

                                    EditOutboundShipment.Display(AddOutboundShipmentLogic.GetCurrentHighestOutboundShipmentID());
                                }
                                else
                                {
                                    // Thông báo lỗi khi không thể thêm chi tiết vào DataTable
                                    errorLabel.Text = "Error: Unable to add details to the DataTable.";
                                    errorLabel.ColorScheme = UIComponent.AnnounceLabelErrorColor();
                                }
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
                    }
                    else
                    {
                        // Xử lý lỗi khi các trường warehouseDropDown và supplierDropDown trống
                        errorLabel.Text = "Error: Warehouse cannot be blank.";
                        errorLabel.ColorScheme = UIComponent.AnnounceLabelErrorColor();
                    }
                }
                catch (Exception ex)
                {
                    errorLabel.Text = $"Error: {ex.Message}";
                    errorLabel.ColorScheme = UIComponent.AnnounceLabelErrorColor();
                }
            };

            //Add display object
            itemInputContainer.Add(addItemButton, productVariantIDLabel, productVariantIDInput, quantityLabel, quantityInput);
            tableContainer.Add(tableView);
            leftContainer.Add(warehouseLabel, warehouseDropDown, descriptionLabel, descriptionInput, dateLabel, dateInput);
            rightContainer.Add(userLabel, userInput, statusLabel, statusBox, addressLabel, addressInput);
            container.Add(leftContainer, rightContainer);
            mainWindow.Add(container, tableContainer, separatorLine, errorLabel, userPermissionLabel, saveButton, returnButton, itemInputContainer);
        }
    }
}