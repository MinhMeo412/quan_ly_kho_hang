using Terminal.Gui;
using WarehouseManager.UI.Utility;

namespace WarehouseManager.UI.Pages
{
    public static class AddAccount
    {
        public static void Display()
        {
            Application.Top.RemoveAll();
            var mainWindow = UIComponent.LoggedInMainWindow("Add New Account");
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

            var leftCollumnContainer = new FrameView()
            {
                X = 1,
                Width = Dim.Percent(50),
                Height = Dim.Fill(),
                Border = new Border()
                {
                    BorderStyle = BorderStyle.None
                }
            };

            var rightCollumnContainer = new FrameView()
            {
                X = Pos.Right(leftCollumnContainer),
                Width = Dim.Fill(1),
                Height = Dim.Fill(),
                Border = new Border()
                {
                    BorderStyle = BorderStyle.None
                }
            };

            var userNameLabel = new Label("User Name:")
            {
                X = 1,
                Y = 1
            };

            var userPasswordLabel = new Label("User Password:")
            {
                X = 1,
                Y = 5
            };

            var fullNameLabel = new Label("Full Name:")
            {
                X = 1,
                Y = 9
            };

            var emailLabel = new Label("Email:")
            {
                X = 1,
                Y = 1
            };

            var phoneNumberLabel = new Label("Phone Number:")
            {
                X = 1,
                Y = 5
            };

            var permissionNameLabel = new Label("Permission Name:")
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

            var userPasswordInput = new TextField("")
            {
                X = Pos.Right(userPasswordLabel) + 1,
                Y = Pos.Top(userPasswordLabel),
                Width = Dim.Fill() - 1,
            };

            var fullNameInput = new TextField("")
            {
                X = Pos.Right(fullNameLabel) + 1,
                Y = Pos.Top(fullNameLabel),
                Width = Dim.Fill() - 1,
            };

            var emailInput = new TextField("")
            {
                X = Pos.Right(emailLabel) + 1,
                Y = Pos.Top(emailLabel),
                Width = Dim.Fill() - 1,
            };

            var phoneNumberInput = new TextField("")
            {
                X = Pos.Right(phoneNumberLabel) + 1,
                Y = Pos.Top(phoneNumberLabel),
                Width = Dim.Fill() - 1,
            };

            var permissionDropDown = new ComboBox()
            {
                X = Pos.Right(permissionNameLabel) + 1,
                Y = Pos.Top(permissionNameLabel),
                Width = Dim.Fill(),
                ReadOnly = true
            };

            var items = new List<string> { "Admin", "User", "Guest" };
            permissionDropDown.SetSource(items);
            permissionDropDown.SelectedItem = 0;

            var saveButton = new Button("Save")
            {
                X = Pos.Center(),
                Y = Pos.Bottom(infoContainer) - 3
            };

            saveButton.Clicked += () =>
            {
                MessageBox.Query("Save Account", "Account saved successfully!", "OK");
            };

            leftCollumnContainer.Add(userNameLabel, userPasswordLabel, fullNameLabel, userNameInput, userPasswordInput, fullNameInput);
            rightCollumnContainer.Add(emailLabel, phoneNumberLabel, permissionNameLabel, emailInput, phoneNumberInput, permissionDropDown);
            infoContainer.Add(leftCollumnContainer, rightCollumnContainer);
            mainWindow.Add(infoContainer, saveButton, errorLabel, userPermissionLabel);
        }
    }
}
