using Terminal.Gui;
using WarehouseManager.UI.Utility;

namespace WarehouseManager.UI.Menu
{
    public static class AddStockTransfer
    {
<<<<<<< HEAD:src/WarehouseManager/UI/Menu/AddStockTransfer.cs
       /*
            Todo.
            Sửa nhà cung cấp.
        */
=======
>>>>>>> refs/remotes/origin/vu_UI:src/WarehouseManager/UI/Menu/EditCategory.cs
        public static void Display()
        {
            Application.Top.RemoveAll();
            var mainWindow = UIComponent.LoggedInMainWindow("Add New Stock Transfer");
            Application.Top.Add(mainWindow);

            var errorLabel = UIComponent.ErrorMessageLabel("Error Message Here");

            var userPermissionLabel = UIComponent.UserPermissionLabel("Username", "Permission");

            var infoContainer = new FrameView()
            {
                X = Pos.Center(),
                Y = Pos.Center(),
                Width = Dim.Percent(75),
                Height = Dim.Percent(75)
            };

            var categorynameLabel = new Label("Category Name:")
            {
                X = 2,
                Y = 1
            };

            var categoryNameInput = new TextField("")
            {
                X = Pos.Right(categorynameLabel) + 1,
                Y = Pos.Top(categorynameLabel),
                Width = Dim.Fill() - 2,
            };

            var descriptionLabel = new Label("Description:")
            {
                X = 2,
                Y = Pos.Bottom(categorynameLabel) + 2
            };

            var descriptionInput = new TextView()
            {
                X = 2,
                Y = Pos.Bottom(descriptionLabel) + 1,
                Width = Dim.Fill(2),
                Height = Dim.Fill() - 3,
                Text = "",
            };

            var saveButton = new Button("Save", is_default: true)
            {
                X = Pos.Center(),
                Y = Pos.Bottom(descriptionInput) + 1
            };

            saveButton.Clicked += () =>
            {
                // khi nút save được bấm
                MessageBox.Query("Save", $"name: {categoryNameInput.Text}, desc: {descriptionInput.Text}", "OK");
            };

            infoContainer.Add(categorynameLabel, descriptionLabel, categoryNameInput, descriptionInput, saveButton);
            mainWindow.Add(infoContainer, errorLabel, userPermissionLabel);
        }

<<<<<<< HEAD:src/WarehouseManager/UI/Menu/AddStockTransfer.cs
=======


>>>>>>> refs/remotes/origin/vu_UI:src/WarehouseManager/UI/Menu/EditCategory.cs
    }
}