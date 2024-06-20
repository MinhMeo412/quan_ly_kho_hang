using Terminal.Gui;
using WarehouseManager.UI.Utility;

namespace WarehouseManager.UI.Menu
{
    public static class CategoryList
    {
       /*
            Todo.
            Danh sách danh mục sản phẩm.
        */
        public static void Display()
        {
            Application.Top.RemoveAll();
            var mainWindow = UIComponent.LoggedInMainWindow("Categories");
            Application.Top.Add(mainWindow);

            var errorLabel = UIComponent.ErrorMessageLabel("Error Message Here");

            var userPermissionLabel = UIComponent.UserPermissionLabel("Username", "Permission");

            mainWindow.Add(errorLabel, userPermissionLabel);
        }

    }
}