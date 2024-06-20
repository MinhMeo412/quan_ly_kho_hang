using Terminal.Gui;
using WarehouseManager.UI.Utility;

namespace WarehouseManager.UI.Menu
{
    public static class EditWarehouse
    {
       /*
            Todo.
            Sửa nhà kho.
        */
        public static void Display()
        {
            Application.Top.RemoveAll();
            var mainWindow = UIComponent.LoggedInMainWindow("Edit Warehouse");
            Application.Top.Add(mainWindow);

            var errorLabel = UIComponent.ErrorMessageLabel("Error Message Here");

            var userPermissionLabel = UIComponent.UserPermissionLabel("Username", "Permission");

            mainWindow.Add(errorLabel, userPermissionLabel);
        }
   
    }
}