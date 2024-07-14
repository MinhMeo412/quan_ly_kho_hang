using Terminal.Gui;
using WarehouseManager.UI.Utility;
using WarehouseManager.Core.Pages;
using System.Data;

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
                DataTable fileInformation = ProductListReportLogic.GetFileInformation();
                DataTable productList = ProductListReportLogic.GetProductAndVariantList();
                UIComponent.ExportToExcelDialog(productList, "Product List", fileInformation, "File Information");
            };

            mainWindow.Add(errorLabel, userPermissionLabel, separatorLine, exportButton);
        }
    }
}