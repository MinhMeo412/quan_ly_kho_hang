using Terminal.Gui;
using WarehouseManager.UI.Utility;
using WarehouseManager.Core.Pages;
using System.Data;
using WarehouseManager.Core.Utility;

namespace WarehouseManager.UI.Pages
{
    public static class WarehouseStockReport
    {
        public static void Display()
        {
            Application.Top.RemoveAll();
            var mainWindow = UIComponent.LoggedInMainWindow("Warehouse Stock Report");
            Application.Top.Add(mainWindow);

            var errorLabel = UIComponent.AnnounceLabel();
            var userPermissionLabel = UIComponent.UserPermissionLabel();
            var separatorLine = UIComponent.SeparatorLine();

            var container = new FrameView()
            {
                X = Pos.Center(),
                Y = Pos.Center(),
                Width = Dim.Percent(50),
                Height = 8
            };

            var selectWarehouseLabel = new Label("Select Warehouse:")
            {
                X = 3,
                Y = 1
            };

            var warehouseDropDown = new ComboBox()
            {
                X = Pos.Right(selectWarehouseLabel) + 1,
                Y = Pos.Top(selectWarehouseLabel),
                Width = Dim.Fill(3),
                Height = Dim.Fill(),
                ReadOnly = true
            };
            Dictionary<string, int?> warehouseNameID = WarehouseStockReportLogic.GetWarehousesNames();
            warehouseDropDown.SetSource(warehouseNameID.Keys.ToList());
            warehouseDropDown.SelectedItem = 0;


            var exportButton = new Button("Export Warehouse Stock", is_default: true)
            {
                X = Pos.Center(),
                Y = Pos.Bottom(selectWarehouseLabel) + 2
            };

            exportButton.Clicked += () =>
            {
                try
                {
                    string? filePath = UIComponent.ChooseFileSaveLocation();

                    if (filePath != null)
                    {
                        errorLabel.Text = $"Processing Warehouse Stock...";
                        errorLabel.ColorScheme = UIComponent.AnnounceLabelErrorColor();

                        KeyValuePair<string, int?> selectedWarehouse = new KeyValuePair<string, int?>(
                            warehouseNameID.Keys.ToList()[warehouseDropDown.SelectedItem],
                            warehouseNameID[warehouseNameID.Keys.ToList()[warehouseDropDown.SelectedItem]]
                        );

                        DataTable warehouseStock = WarehouseStockReportLogic.GetWarehouseStock(selectedWarehouse);
                        DataTable fileInformation = WarehouseStockReportLogic.GetFileInformation(selectedWarehouse);

                        filePath = Excel.GetExcelFileName(filePath);
                        Excel.Export(filePath, warehouseStock, "Warehouse Stock", fileInformation, "File Information");

                        errorLabel.Text = $"Successfully exported warehouse stock to {filePath}";
                        errorLabel.ColorScheme = UIComponent.AnnounceLabelSuccessColor();
                    }
                }
                catch (Exception ex)
                {
                    errorLabel.Text = $"Error: {ex}";
                    errorLabel.ColorScheme = UIComponent.AnnounceLabelErrorColor();
                }
            };

            container.Add(selectWarehouseLabel, warehouseDropDown, exportButton);
            mainWindow.Add(errorLabel, userPermissionLabel, separatorLine, container);
        }
    }
}