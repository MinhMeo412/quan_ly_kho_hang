using Terminal.Gui;
using WarehouseManager.UI.Utility;
using WarehouseManager.Core.Pages;

namespace WarehouseManager.UI.Pages
{
    public static class WarehouseShipmentsReport
    {
        public static void Display()
        {
            Application.Top.RemoveAll();
            var mainWindow = UIComponent.LoggedInMainWindow("Warehouse Shipments Report");
            Application.Top.Add(mainWindow);

            var errorLabel = UIComponent.AnnounceLabel();
            var userPermissionLabel = UIComponent.UserPermissionLabel();
            var separatorLine = UIComponent.SeparatorLine();


            // nhập/xuất/product

            // checkbox inventory audit

            // - Choose 1 warehouse (id if product)
            // - time (from time to time)
            var warehouseContainer = new FrameView()
            {
                X = Pos.Center(),
                Y = Pos.Center(),
                Width = Dim.Percent(50),
                Height = 14,
                Visible = true
            };

            var warehouseExportOptionLabel = new Label("Report shipments by:")
            {
                X = 3,
                Y = 1
            };

            var warehouseExportOptionDropDown = new ComboBox(WarehouseShipmentsReportLogic.GetReportOptions())
            {
                X = Pos.Right(warehouseExportOptionLabel) + 1,
                Y = Pos.Top(warehouseExportOptionLabel),
                Width = Dim.Fill(3),
                Height = Dim.Fill(),
                ReadOnly = true,
                SelectedItem = 0
            };

            var warehouseLabel = new Label("Warehouse:")
            {
                X = 3,
                Y = Pos.Bottom(warehouseExportOptionLabel) + 1
            };

            var warehouseDropDown = new ComboBox(WarehouseShipmentsReportLogic.GetWarehouseList())
            {
                X = Pos.Right(warehouseExportOptionLabel) + 1,
                Y = Pos.Top(warehouseLabel),
                Width = Dim.Fill(3),
                Height = Dim.Fill(),
                ReadOnly = true,
                SelectedItem = 0
            };

            var warehouseFromDateLabel = new Label("Start date:")
            {
                X = 3,
                Y = Pos.Bottom(warehouseLabel) + 1
            };

            var warehouseFromDateField = new DateField(WarehouseShipmentsReportLogic.GetDefaultStartDate())
            {
                X = Pos.Right(warehouseExportOptionLabel) + 1,
                Y = Pos.Top(warehouseFromDateLabel),
                Width = Dim.Fill(3)
            };

            var warehouseToDateLabel = new Label("End date:")
            {
                X = 3,
                Y = Pos.Bottom(warehouseFromDateLabel) + 1
            };

            var warehouseToDateField = new DateField(WarehouseShipmentsReportLogic.GetDefaultEndDate())
            {
                X = Pos.Right(warehouseExportOptionLabel) + 1,
                Y = Pos.Top(warehouseToDateLabel),
                Width = Dim.Fill(3)
            };

            var warehouseExportButton = new Button("Export Warehouse Shipments", is_default: true)
            {
                X = Pos.Center(),
                Y = Pos.Bottom(warehouseToDateLabel) + 2
            };





            var productContainer = new FrameView()
            {
                X = Pos.Center(),
                Y = Pos.Center(),
                Width = Dim.Percent(50),
                Height = 14,
                Visible = false
            };

            var productExportOptionLabel = new Label("Report shipments by:")
            {
                X = 3,
                Y = 1
            };

            var productExportOptionDropDown = new ComboBox(WarehouseShipmentsReportLogic.GetReportOptions())
            {
                X = Pos.Right(productExportOptionLabel) + 1,
                Y = Pos.Top(productExportOptionLabel),
                Width = Dim.Fill(3),
                Height = Dim.Fill(),
                ReadOnly = true,
                SelectedItem = 2
            };

            var productLabel = new Label("Product:")
            {
                X = 3,
                Y = Pos.Bottom(productExportOptionLabel) + 1
            };

            var productDropDown = new ComboBox(WarehouseShipmentsReportLogic.GetProductList())
            {
                X = Pos.Right(productExportOptionLabel) + 1,
                Y = Pos.Top(productLabel),
                Width = Dim.Fill(3),
                Height = Dim.Fill(),
                ReadOnly = true,
                SelectedItem = 0
            };

            var productFromDateLabel = new Label("Start date:")
            {
                X = 3,
                Y = Pos.Bottom(productLabel) + 1
            };

            var productFromDateField = new DateField(WarehouseShipmentsReportLogic.GetDefaultStartDate())
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

            var productToDateField = new DateField(WarehouseShipmentsReportLogic.GetDefaultEndDate())
            {
                X = Pos.Right(productExportOptionLabel) + 1,
                Y = Pos.Top(productToDateLabel),
                Width = Dim.Fill(3)
            };

            var productExportButton = new Button("Export Warehouse Shipments", is_default: true)
            {
                X = Pos.Center(),
                Y = Pos.Bottom(productToDateLabel) + 2
            };


            warehouseExportOptionDropDown.OpenSelectedItem += (e) =>
            {
                if (warehouseExportOptionDropDown.SelectedItem == 2)
                {
                    warehouseContainer.Visible = false;
                    productContainer.Visible = true;
                }
            };


            productExportOptionDropDown.OpenSelectedItem += (e) =>
            {
                if (productExportOptionDropDown.SelectedItem != 2)
                {
                    warehouseContainer.Visible = true;
                    productContainer.Visible = false;
                }
            };

            warehouseExportButton.Clicked += () =>
            {
                try
                {
                    string? filePath = UIComponent.ChooseFileSaveLocation();

                    if (filePath != null)
                    {
                        // KeyValuePair<string, int?> selectedWarehouse = new KeyValuePair<string, int?>(
                        //     warehouseNameID.Keys.ToList()[warehouseDropDown.SelectedItem],
                        //     warehouseNameID[warehouseNameID.Keys.ToList()[warehouseDropDown.SelectedItem]]
                        // );

                        // DataTable warehouseStock = WarehouseStockReportLogic.GetWarehouseStock(selectedWarehouse);
                        // DataTable fileInformation = WarehouseStockReportLogic.GetFileInformation(selectedWarehouse);

                        // filePath = Excel.GetExcelFileName(filePath);
                        // Excel.Export(filePath, warehouseStock, "Warehouse Stock", fileInformation, "File Information");

                        // errorLabel.Text = $"Successfully exported warehouse stock to {filePath}";
                        // errorLabel.ColorScheme = UIComponent.AnnounceLabelSuccessColor();
                    }
                }
                catch (Exception ex)
                {
                    errorLabel.Text = $"Error: {ex}";
                    errorLabel.ColorScheme = UIComponent.AnnounceLabelErrorColor();
                }
            };



            productExportButton.Clicked += () =>
            {
                try
                {
                    string? filePath = UIComponent.ChooseFileSaveLocation();

                    if (filePath != null)
                    {

                    }
                }
                catch (Exception ex)
                {
                    errorLabel.Text = $"Error: {ex}";
                    errorLabel.ColorScheme = UIComponent.AnnounceLabelErrorColor();
                }
            };



            productContainer.Add(productExportOptionLabel, productExportOptionDropDown, productLabel, productDropDown, productFromDateLabel, productFromDateField, productToDateLabel, productToDateField, productExportButton);
            warehouseContainer.Add(warehouseExportOptionLabel, warehouseExportOptionDropDown, warehouseLabel, warehouseDropDown, warehouseFromDateLabel, warehouseFromDateField, warehouseToDateLabel, warehouseToDateField, warehouseExportButton);
            mainWindow.Add(errorLabel, userPermissionLabel, separatorLine, warehouseContainer, productContainer);
        }
    }
}