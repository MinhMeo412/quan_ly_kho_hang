using Terminal.Gui;
using WarehouseManager.UI.Utility;

namespace WarehouseManager.UI.Pages
{
    public static class ChangePassword
    {
        /*
            Todo.
            Đổi mật khẩu.
        */
        public static void Display()
        {
            Application.Top.RemoveAll();
            var mainWindow = UIComponent.LoggedInMainWindow("Change Password");
            Application.Top.Add(mainWindow);

            var errorLabel = UIComponent.ErrorMessageLabel("Error Message Here");

            var userPermissionLabel = UIComponent.UserPermissionLabel();

            var infoContainer = new FrameView()
            {
                X = Pos.Center(),
                Y = Pos.Center(),
                Width = Dim.Percent(75),
                Height = Dim.Percent(75)
            };

            var userNameLabel = new Label("User Name:")
            {
                X = 1,
                Y = 1
            };

            var oldPasswordLabel = new Label("Old Password:")
            {
                X = 1,
                Y = 5
            };

            var newPasswordLabel = new Label("New Password:")
            {
                X = 1,
                Y = 9
            };

            var userNameInput = new TextField("")
            {
                X = Pos.Right(userNameLabel) + 1,
                Y = Pos.Top(userNameLabel),
                Width = Dim.Fill() - 1,
            };

            var oldPasswordInput = new TextField("")
            {
                X = Pos.Right(oldPasswordLabel) + 1,
                Y = Pos.Top(oldPasswordLabel),
                Width = Dim.Fill() - 1,
                Secret = true
            };

            var newPasswordInput = new TextField("")
            {
                X = Pos.Right(newPasswordLabel) + 1,
                Y = Pos.Top(newPasswordLabel),
                Width = Dim.Fill() - 1,
                Secret = true
            };

            var saveButton = new Button("Save")
            {
                X = Pos.Center(),
                Y = Pos.Bottom(infoContainer) - 3
            };

            saveButton.Clicked += () =>
            {
                MessageBox.Query("Save Password", "Password changed successfully!", "OK");
            };

            infoContainer.Add(userNameLabel, userNameInput, oldPasswordLabel, oldPasswordInput, newPasswordLabel, newPasswordInput);
            mainWindow.Add(infoContainer, saveButton, errorLabel, userPermissionLabel);
        }
    }
}
