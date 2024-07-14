using Terminal.Gui;
using WarehouseManager.UI.Utility;
using WarehouseManager.Core.Pages;

namespace WarehouseManager.UI.Pages
{
    public static class AddAccount
    {
        public static void Display()
        {
            Application.Top.RemoveAll();
            var mainWindow = UIComponent.LoggedInMainWindow("Add New Account");
            Application.Top.Add(mainWindow);

            var errorLabel = UIComponent.AnnounceLabel();
            var userPermissionLabel = UIComponent.UserPermissionLabel();
            var separatorLine = UIComponent.SeparatorLine();
            var refreshButton = UIComponent.RefreshButton();

            var infoContainer = new FrameView()
            {
                X = Pos.Center(),
                Y = Pos.Center(),
                Width = Dim.Percent(75),
                Height = 11
            };

            var leftCollumnContainer = new FrameView()
            {
                X = 3,
                Width = Dim.Percent(50) - 3,
                Height = Dim.Fill(),
                Border = new Border() { BorderStyle = BorderStyle.None }
            };

            var rightCollumnContainer = new FrameView()
            {
                X = Pos.Right(leftCollumnContainer) + 1,
                Width = Dim.Fill(3),
                Height = 6,
                Border = new Border() { BorderStyle = BorderStyle.None }
            };

            var userNameLabel = new Label("Username:")
            {
                X = 0,
                Y = 1
            };

            var userPasswordLabel = new Label("Password:")
            {
                X = 0,
                Y = Pos.Bottom(userNameLabel) + 1
            };

            var permissionNameLabel = new Label("Permission:")
            {
                X = 0,
                Y = Pos.Bottom(userPasswordLabel) + 1
            };

            var fullNameLabel = new Label("Full Name:")
            {
                X = 0,
                Y = 1
            };

            var emailLabel = new Label("Email:")
            {
                X = 0,
                Y = Pos.Bottom(fullNameLabel) + 1
            };

            var phoneNumberLabel = new Label("Phone Number:")
            {
                X = 0,
                Y = Pos.Bottom(emailLabel) + 1
            };


            var userNameInput = new TextField("")
            {
                X = Pos.Right(permissionNameLabel) + 1,
                Y = Pos.Top(userNameLabel),
                Width = Dim.Fill(1),
            };

            var userPasswordInput = new TextField("")
            {
                X = Pos.Right(permissionNameLabel) + 1,
                Y = Pos.Top(userPasswordLabel),
                Width = Dim.Fill(1),
            };

            var permissionDropDown = new ComboBox()
            {
                X = Pos.Right(permissionNameLabel) + 1,
                Y = Pos.Top(permissionNameLabel),
                Width = Dim.Fill(1),
                Height = Dim.Fill(),
                ReadOnly = true
            };

            var fullNameInput = new TextField("")
            {
                X = Pos.Right(phoneNumberLabel) + 1,
                Y = Pos.Top(fullNameLabel),
                Width = Dim.Fill(),
            };

            var emailInput = new TextField("")
            {
                X = Pos.Right(phoneNumberLabel) + 1,
                Y = Pos.Top(emailLabel),
                Width = Dim.Fill(),
            };

            var phoneNumberInput = new TextField("")
            {
                X = Pos.Right(phoneNumberLabel) + 1,
                Y = Pos.Top(phoneNumberLabel),
                Width = Dim.Fill(),
            };

            var items = AddAccountLogic.GetPermissionNames();
            permissionDropDown.SetSource(items);
            permissionDropDown.SelectedItem = 4;

            var saveButton = new Button("Save", is_default: true)
            {
                X = Pos.Center(),
                Y = Pos.Bottom(rightCollumnContainer) + 1
            };

            refreshButton.Text = "Back";
            refreshButton.Clicked += () =>
            {
                AccountList.Display();
            };

            saveButton.Clicked += () =>
            {
                try
                {
                    AddAccountLogic.Save
                    (
                        userName: $"{userNameInput.Text}",
                        userPassword: $"{userPasswordInput.Text}",
                        userFullName: $"{fullNameInput.Text}",
                        userEmail: $"{emailInput.Text} ",
                        userPhoneNumber: $"{phoneNumberInput.Text}",
                        permissionName: $"{permissionDropDown.Text}"
                    );

                    errorLabel.Text = $"User created successfully!";
                    errorLabel.ColorScheme = UIComponent.AnnounceLabelSuccessColor();

                    userNameInput.Text = "";
                    userPasswordInput.Text = "";
                    fullNameInput.Text = "";
                    emailInput.Text = "";
                    phoneNumberInput.Text = "";
                    permissionDropDown.SelectedItem = 4;
                }
                catch (Exception ex)
                {
                    errorLabel.Text = $"Error: {ex.Message}";
                    errorLabel.ColorScheme = UIComponent.AnnounceLabelErrorColor();
                }
            };



            leftCollumnContainer.Add(userNameLabel, userPasswordLabel, permissionNameLabel, userNameInput, userPasswordInput, permissionDropDown);
            rightCollumnContainer.Add(fullNameLabel, emailLabel, phoneNumberLabel, fullNameInput, emailInput, phoneNumberInput);
            infoContainer.Add(leftCollumnContainer, rightCollumnContainer, saveButton);
            mainWindow.Add(infoContainer, errorLabel, userPermissionLabel, separatorLine, refreshButton);
        }
    }
}
