using Terminal.Gui;
using WarehouseManager.UI.Utility;
using WarehouseManager.Core.Pages;
using WarehouseManager.Core.Utility;

namespace WarehouseManager.UI.Pages
{
    public static class WarehouseShipmentsReportWarehouse
    {
        public static void Display(int chosenOption = 0)
        {
            Application.Top.RemoveAll();
            var mainWindow = UIComponent.LoggedInMainWindow("Warehouse Shipments Report");
            Application.Top.Add(mainWindow);

            var errorLabel = UIComponent.AnnounceLabel();
            var userPermissionLabel = UIComponent.UserPermissionLabel();
            var separatorLine = UIComponent.SeparatorLine();

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

            var warehouseExportOptionDropDown = new ComboBox(WarehouseShipmentsReportWarehouseLogic.GetReportOptions())
            {
                X = Pos.Right(warehouseExportOptionLabel) + 1,
                Y = Pos.Top(warehouseExportOptionLabel),
                Width = Dim.Fill(3),
                Height = 4,
                ReadOnly = true,
                SelectedItem = chosenOption
            };

            var warehouseFromDateLabel = new Label("Start date:")
            {
                X = 3,
                Y = Pos.Bottom(warehouseExportOptionLabel) + 1
            };

            var warehouseFromDateField = new DateField(WarehouseShipmentsReportWarehouseLogic.GetDefaultStartDate())
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

            var warehouseToDateField = new DateField(WarehouseShipmentsReportWarehouseLogic.GetDefaultEndDate())
            {
                X = Pos.Right(warehouseExportOptionLabel) + 1,
                Y = Pos.Top(warehouseToDateLabel),
                Width = Dim.Fill(3)
            };


            var warehouseLabel = new Label("Warehouse:")
            {
                X = 3,
                Y = Pos.Bottom(warehouseToDateLabel) + 1
            };

            var warehouseDropDown = new ComboBox(WarehouseShipmentsReportWarehouseLogic.GetWarehouseList())
            {
                X = Pos.Right(warehouseExportOptionLabel) + 1,
                Y = Pos.Top(warehouseLabel),
                Width = Dim.Fill(3),
                Height = Dim.Fill(),
                ReadOnly = true,
                SelectedItem = 0
            };

            var warehouseExportButton = new Button("Export Warehouse Shipments", is_default: true)
            {
                X = Pos.Center(),
                Y = Pos.Bottom(warehouseLabel) + 2
            };

            warehouseExportOptionDropDown.OpenSelectedItem += (e) =>
            {
                if (warehouseExportOptionDropDown.SelectedItem == 2)
                {
                    WarehouseShipmentsReportProduct.Display();
                }
            };

            warehouseExportButton.Clicked += () =>
            {
                try
                {
                    string? filePath = UIComponent.ChooseFileSaveLocation();

                    if (filePath != null)
                    {
                        filePath = Excel.GetExcelFileName(filePath);
                        Excel.Export(
                            filePath,
                            WarehouseShipmentsReportWarehouseLogic.GetWarehouseExportData(
                                $"{warehouseExportOptionDropDown.Text}",
                                $"{warehouseDropDown.Text}",
                                warehouseFromDateField.Date,
                                warehouseToDateField.Date),
                            "Warehouse Shipments",
                            WarehouseShipmentsReportWarehouseLogic.GetWarehouseFileInformation(
                                $"{warehouseExportOptionDropDown.Text}",
                                $"{warehouseDropDown.Text}",
                                warehouseFromDateField.Date,
                                warehouseToDateField.Date),
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

            warehouseContainer.Add(warehouseExportOptionLabel, warehouseExportOptionDropDown, warehouseLabel, warehouseFromDateField, warehouseToDateLabel, warehouseToDateField, warehouseDropDown, warehouseFromDateLabel, warehouseExportButton);
            mainWindow.Add(errorLabel, userPermissionLabel, separatorLine, warehouseContainer);
        }
    }
}