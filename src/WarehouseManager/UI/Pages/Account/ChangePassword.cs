using Terminal.Gui;
using WarehouseManager.Core.Pages;
using WarehouseManager.UI.Utility;

namespace WarehouseManager.UI.Pages
{
    public static class ChangePassword
    {
        public static void Display()
        {
            Application.Top.RemoveAll();
            var mainWindow = UIComponent.LoggedInMainWindow("Change Password");
            Application.Top.Add(mainWindow);

            var errorLabel = UIComponent.AnnounceLabel();

            var userPermissionLabel = UIComponent.UserPermissionLabel();

            var separatorLine = UIComponent.SeparatorLine();

            var infoContainer = new FrameView()
            {
                X = Pos.Center(),
                Y = Pos.Center(),
                Width = 75,
                Height = 14
            };

            var userNameLabel = new Label("Username:")
            {
                X = 3,
                Y = 1
            };

            var oldPasswordLabel = new Label("Current Password:")
            {
                X = 3,
                Y = Pos.Bottom(userNameLabel) + 1
            };


            var newPasswordLabel = new Label("New Password:")
            {
                X = 3,
                Y = Pos.Bottom(oldPasswordLabel) + 2
            };

            var repeatNewPasswordLabel = new Label("Confirm New Password:")
            {
                X = 3,
                Y = Pos.Bottom(newPasswordLabel) + 1
            };


            var userNameInput = new TextField(ChangePasswordLogic.GetUsername())
            {
                X = Pos.Right(repeatNewPasswordLabel) + 1,
                Y = Pos.Top(userNameLabel),
                Width = Dim.Fill(3),
                ReadOnly = true
            };

            var oldPasswordInput = new TextField("")
            {
                X = Pos.Right(repeatNewPasswordLabel) + 1,
                Y = Pos.Top(oldPasswordLabel),
                Width = Dim.Fill(3),
                Secret = true
            };

            var newPasswordInput = new TextField("")
            {
                X = Pos.Right(repeatNewPasswordLabel) + 1,
                Y = Pos.Top(newPasswordLabel),
                Width = Dim.Fill(3),
                Secret = true
            };

            var repeatNewPasswordInput = new TextField("")
            {
                X = Pos.Right(repeatNewPasswordLabel) + 1,
                Y = Pos.Top(repeatNewPasswordLabel),
                Width = Dim.Fill(3),
                Secret = true
            };

            var saveButton = new Button("Save", is_default: true)
            {
                X = Pos.Center(),
                Y = Pos.Bottom(repeatNewPasswordLabel) + 1
            };

            saveButton.Clicked += () =>
            {
                bool success = false;

                try
                {
                    if ($"{newPasswordInput.Text}" == $"{repeatNewPasswordInput.Text}")
                    {
                        success = ChangePasswordLogic.ChangePassword($"{oldPasswordInput.Text}", $"{newPasswordInput.Text}");

                        if (success)
                        {
                            errorLabel.Text = $"Password changed successfully!";
                            errorLabel.ColorScheme = UIComponent.AnnounceLabelSuccessColor();
                            
                            oldPasswordInput.Text = "";
                            newPasswordInput.Text = "";
                            repeatNewPasswordInput.Text = "";
                        }
                        else
                        {
                            errorLabel.Text = $"Error: Current password is incorrect";
                            errorLabel.ColorScheme = UIComponent.AnnounceLabelErrorColor();
                        }
                    }
                    else
                    {
                        errorLabel.Text = $"Error: Passwords do not match";
                        errorLabel.ColorScheme = UIComponent.AnnounceLabelErrorColor();
                    }
                }
                catch (Exception ex)
                {
                    errorLabel.Text = $"Error: {ex.Message}";
                    errorLabel.ColorScheme = UIComponent.AnnounceLabelErrorColor();
                }
            };

            infoContainer.Add(userNameLabel, userNameInput, oldPasswordLabel, oldPasswordInput, newPasswordLabel, newPasswordInput, repeatNewPasswordLabel, repeatNewPasswordInput, saveButton);
            mainWindow.Add(infoContainer, errorLabel, userPermissionLabel, separatorLine);
        }
    }
}
