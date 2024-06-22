using Terminal.Gui;
using WarehouseManager.UI.Utility;

namespace WarehouseManager.UI.Menu
{
    public static class EditStockTransfer
    {
        /*
             Todo.
             Sửa phiếu nhập.
         */
        public static void Display()
        {
            Application.Top.RemoveAll();
            var mainWindow = UIComponent.LoggedInMainWindow("Edit Stock Transfer");
            Application.Top.Add(mainWindow);

            var errorLabel = UIComponent.ErrorMessageLabel("Error Message Here");

            var userPermissionLabel = UIComponent.UserPermissionLabel("Username", "Permission");

            var separatorLine = UIComponent.SeparatorLine();

            mainWindow.Add(errorLabel, userPermissionLabel, separatorLine);
        }

    }
}