using Terminal.Gui;
using WarehouseManager.UI.Utility;
using WarehouseManager.Core.Pages;
using WarehouseManager.Core.Utility;

namespace WarehouseManager.UI.Pages
{
    public static class WarehouseShipmentsReportProduct
    {
        public static void Display()
        {
            Application.Top.RemoveAll();
            var mainWindow = UIComponent.LoggedInMainWindow("Warehouse Shipments Report");
            Application.Top.Add(mainWindow);

            var errorLabel = UIComponent.AnnounceLabel();
            var userPermissionLabel = UIComponent.UserPermissionLabel();
            var separatorLine = UIComponent.SeparatorLine();

            var productContainer = new FrameView()
            {
                X = Pos.Center(),
                Y = Pos.Center(),
                Width = Dim.Percent(50),
                Height = 14,
                Visible = true
            };

            var productExportOptionLabel = new Label("Report shipments by:")
            {
                X = 3,
                Y = 1
            };

            var productExportOptionDropDown = new ComboBox(WarehouseShipmentsReportProductLogic.GetReportOptions())
            {
                X = Pos.Right(productExportOptionLabel) + 1,
                Y = Pos.Top(productExportOptionLabel),
                Width = Dim.Fill(3 + 26),
                Height = 4,
                ReadOnly = true,
                SelectedItem = 2
            };

            var stockTransferCheckbox = new CheckBox("Include stock transfers")
            {
                X = Pos.Right(productExportOptionDropDown) + 1,
                Y = Pos.Top(productExportOptionLabel),
                Checked = true
            };

            var productFromDateLabel = new Label("Start date:")
            {
                X = 3,
                Y = Pos.Bottom(productExportOptionLabel) + 1
            };

            var productFromDateField = new DateField(WarehouseShipmentsReportProductLogic.GetDefaultStartDate())
            {
                X = Pos.Right(productExportOptionLabel) + 1,
                Y = Pos.Top(productFromDateLabel),
                Width = Dim.Fill(3)
            };

            var productToDateLabel = new Label("End date:")
            {
                X = 3,
                Y = Pos.Bottom(productFromDateLabel) + 1
            };

            var productToDateField = new DateField(WarehouseShipmentsReportProductLogic.GetDefaultEndDate())
            {
                X = Pos.Right(productExportOptionLabel) + 1,
                Y = Pos.Top(productToDateLabel),
                Width = Dim.Fill(3)
            };


            var productLabel = new Label("Product ID:")
            {
                X = 3,
                Y = Pos.Bottom(productToDateLabel) + 1
            };

            var productIDInput = new TextField()
            {
                X = Pos.Right(productExportOptionLabel) + 1,
                Y = Pos.Top(productLabel),
                Width = 7,
            };

            Dictionary<int, string> productDictionary = WarehouseShipmentsReportProductLogic.GetProductList();
            var productDropDown = new ComboBox(productDictionary.Values.ToList())
            {
                X = Pos.Right(productIDInput) + 1,
                Y = Pos.Top(productLabel),
                Width = Dim.Fill(3),
                Height = Dim.Fill(),
                ReadOnly = true,
                SelectedItem = 0
            };
            productIDInput.Text = $"{WarehouseShipmentsReportProductLogic.GetProductID($"{productDropDown.Text}", productDictionary)}";

            var productExportButton = new Button("Export Warehouse Shipments", is_default: true)
            {
                X = Pos.Center(),
                Y = Pos.Bottom(productLabel) + 2
            };

            productExportOptionDropDown.OpenSelectedItem += (e) =>
            {
                if (productExportOptionDropDown.SelectedItem != 2)
                {
                    WarehouseShipmentsReportWarehouse.Display(productExportOptionDropDown.SelectedItem);
                }
            };

            productIDInput.TextChanged += args =>
            {
                productDropDown.SelectedItem = WarehouseShipmentsReportProductLogic.GetProductIndex($"{productIDInput.Text}", productDictionary);
            };

            productDropDown.OpenSelectedItem += (a) =>
            {
                productIDInput.Text = $"{WarehouseShipmentsReportProductLogic.GetProductID($"{productDropDown.Text}", productDictionary)}";
            };

            productExportButton.Clicked += () =>
            {
                try
                {
                    string? filePath = UIComponent.ChooseFileSaveLocation();

                    if (filePath != null)
                    {
                        filePath = Excel.GetExcelFileName(filePath);
                        Excel.Export(
                            filePath,
                            WarehouseShipmentsReportProductLogic.GetProductExportData(
                                $"{productIDInput.Text}",
                                productFromDateField.Date,
                                productToDateField.Date,
                                stockTransferCheckbox.Checked),
                            "Warehouse Shipments",
                            WarehouseShipmentsReportProductLogic.GetProductFileInformation(
                                $"{productIDInput.Text}",
                                productFromDateField.Date,
                                productToDateField.Date),
                            "File Information");

                        errorLabel.Text = $"Successfully exported warehouse shipments to {filePath}";
                        errorLabel.ColorScheme = UIComponent.AnnounceLabelSuccessColor();
                    }
                }
                catch (Exception ex)
                {
                    errorLabel.Text = $"Error: {ex}";
                    errorLabel.ColorScheme = UIComponent.AnnounceLabelErrorColor();
                }
            };

            productContainer.Add(productExportOptionLabel, productExportOptionDropDown, stockTransferCheckbox, productFromDateLabel, productFromDateField, productToDateLabel, productToDateField, productLabel, productIDInput, productDropDown, productExportButton);
            mainWindow.Add(errorLabel, userPermissionLabel, separatorLine, productContainer);
        }
    }
}