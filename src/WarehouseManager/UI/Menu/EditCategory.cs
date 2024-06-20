using Terminal.Gui;
using WarehouseManager.UI.Utility;

namespace WarehouseManager.UI.Menu
{
    public static class EditCategory
    {
        /*
           Todo.
           Sửa danh mục sản phẩm.
       */
        public static void Display()
        {
            Application.Top.RemoveAll();
            var mainWindow = UIComponent.LoggedInMainWindow("Edit Category");
            Application.Top.Add(mainWindow);

            var errorLabel = new Label("Error message here")
            {
                X = 1,
                Y = Pos.AnchorEnd(1),
                ColorScheme = new ColorScheme
                {
                    Normal = Application.Driver.MakeAttribute(Color.BrightRed, Color.Black),
                }
            };

            var userPermissionLabel = new Label("Username - Permission")
            {
                X = 1,
                Y = Pos.AnchorEnd(1)
            };
            userPermissionLabel.X = Pos.Right(mainWindow) - userPermissionLabel.Text.Length - 3;


            mainWindow.Add(errorLabel, userPermissionLabel);
        }


    }
}