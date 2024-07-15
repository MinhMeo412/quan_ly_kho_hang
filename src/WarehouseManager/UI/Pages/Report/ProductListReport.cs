using Terminal.Gui;
using WarehouseManager.UI.Utility;
using WarehouseManager.Core.Pages;
using System.Data;
using WarehouseManager.Core.Utility;

namespace WarehouseManager.UI.Pages
{
    public static class ProductListReport
    {
        public static void Display()
        {
            Application.Top.RemoveAll();
            var mainWindow = UIComponent.LoggedInMainWindow("Product List Report");
            Application.Top.Add(mainWindow);

            var errorLabel = UIComponent.AnnounceLabel();
            var userPermissionLabel = UIComponent.UserPermissionLabel();
            var separatorLine = UIComponent.SeparatorLine();

            var exportButton = new Button("Export Product List", is_default: true)
            {
                X = Pos.Center(),
                Y = Pos.Center()
            };

            exportButton.Clicked += () =>
            {
                try
                {
                    string? filePath = UIComponent.ChooseFileSaveLocation();
                    if (filePath != null)
                    {
                        // Progress bar starts showing from here and stops when the last line of this if is reached
                        DataTable productList = ProductListReportLogic.GetProductAndVariantList();
                        DataTable fileInformation = ProductListReportLogic.GetFileInformation();

                        filePath = Excel.GetExcelFileName(filePath);
                        Excel.Export(filePath, productList, "Product List", fileInformation, "File Information");

                        errorLabel.Text = $"Successfully exported product list to {filePath}";
                        errorLabel.ColorScheme = UIComponent.AnnounceLabelSuccessColor();
                    }
                }
                catch (Exception ex)
                {
                    errorLabel.Text = $"Error: {ex.Message}";
                    errorLabel.ColorScheme = UIComponent.AnnounceLabelErrorColor();
                }
            };

            mainWindow.Add(errorLabel, userPermissionLabel, separatorLine, exportButton);
        }
    }
}