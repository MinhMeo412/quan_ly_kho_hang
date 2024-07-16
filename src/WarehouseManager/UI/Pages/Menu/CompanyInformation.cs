using Terminal.Gui;
using WarehouseManager.Core.Pages;
using WarehouseManager.UI.Utility;

namespace WarehouseManager.UI.Pages
{
    public static class CompanyInformation
    {
        public static void Display()
        {
            Application.Top.RemoveAll();
            var mainWindow = UIComponent.LoggedInMainWindow("Company Information");
            Application.Top.Add(mainWindow);

            var separatorLine = UIComponent.SeparatorLine();

            // Chỉnh thành true nếu người dùng được sửa.
            bool sufficientPermission = false;

            var infoContainer = new FrameView()
            {
                X = Pos.Center(),
                Y = Pos.Center(),
                Width = Dim.Percent(75),
                Height = Dim.Percent(75)
            };
            mainWindow.Add(infoContainer);

            var leftCollumnContainer = new FrameView()
            {
                X = 3,
                Width = Dim.Percent(50),
                Height = 8,
                Border = new Border() { BorderStyle = BorderStyle.None }
            };

            var rightCollumnContainer = new FrameView()
            {
                X = Pos.Right(leftCollumnContainer),
                Width = Dim.Fill(3),
                Height = 8,
                Border = new Border()
                {
                    BorderStyle = BorderStyle.None
                }
            };

            var companyNameLabel = new Label("Company Name:")
            {
                X = 0,
                Y = 1
            };
            var addressLabel = new Label("Address:")
            {
                X = 0,
                Y = Pos.Bottom(companyNameLabel) + 1
            };
            var phoneNumberLabel = new Label("Phone Number:")
            {
                X = 0,
                Y = Pos.Bottom(addressLabel) + 1
            };

            var companyNameInput = new TextField(CompanyInformationLogic.GetCompanyName())
            {
                X = Pos.Right(companyNameLabel) + 1,
                Y = Pos.Top(companyNameLabel),
                Width = Dim.Fill(3),
                ReadOnly = !sufficientPermission
            };
            var addressInput = new TextField(CompanyInformationLogic.GetAddresss())
            {
                X = Pos.Right(companyNameLabel) + 1,
                Y = Pos.Top(addressLabel),
                Width = Dim.Fill(3),
                ReadOnly = !sufficientPermission
            };
            var phoneNumberInput = new TextField(CompanyInformationLogic.GetPhoneNumber())
            {
                X = Pos.Right(companyNameLabel) + 1,
                Y = Pos.Top(phoneNumberLabel),
                Width = Dim.Fill(3),
                ReadOnly = !sufficientPermission
            };

            var emailLabel = new Label("Email:")
            {
                X = 0,
                Y = 1
            };
            var representativeLabel = new Label("Represenstative:")
            {
                X = 0,
                Y = Pos.Bottom(emailLabel) + 1
            };

            var emailInput = new TextField(CompanyInformationLogic.GetEmail())
            {
                X = Pos.Right(representativeLabel) + 1,
                Y = Pos.Top(emailLabel),
                Width = Dim.Fill(),
                ReadOnly = !sufficientPermission
            };
            var representativeInput = new TextField(CompanyInformationLogic.GetRepresentative())
            {
                X = Pos.Right(representativeLabel) + 1,
                Y = Pos.Top(representativeLabel),
                Width = Dim.Fill(),
                ReadOnly = !sufficientPermission
            };

            var descriptionLabel = new Label("Description:")
            {
                X = 3,
                Y = Pos.Bottom(leftCollumnContainer),
                Width = Dim.Fill(3)
            };

            var descriptionInput = new TextView()
            {
                X = 3,
                Y = Pos.Bottom(descriptionLabel) + 1,
                Width = Dim.Fill(3),
                Height = Dim.Fill(3),
                Text = CompanyInformationLogic.GetDescription(),
                ReadOnly = !sufficientPermission
            };

            var saveButton = new Button("Save", is_default: true)
            {
                X = Pos.Center(),
                Y = Pos.Bottom(descriptionInput) + 1,
                Visible = sufficientPermission
            };

            var errorLabel = UIComponent.AnnounceLabel();

            var userPermissionLabel = UIComponent.UserPermissionLabel();

            saveButton.Clicked += () =>
            {
                // khi nút save được bấm
                try
                {
                    CompanyInformationLogic.Save(
                        companyName: $"{companyNameInput.Text}",
                        address: $"{addressInput.Text}",
                        phoneNumber: $"{phoneNumberInput.Text}",
                        email: $"{emailInput.Text}",
                        representative: $"{representativeInput.Text}",
                        description: $"{descriptionInput.Text}"
                    );

                    errorLabel.Text = $"Successfully updated company information.";
                    errorLabel.ColorScheme = UIComponent.AnnounceLabelSuccessColor();
                }
                catch (Exception ex)
                {
                    errorLabel.Text = $"Error: {ex.Message}";
                    errorLabel.ColorScheme = UIComponent.AnnounceLabelErrorColor();
                }
            };

            leftCollumnContainer.Add(companyNameLabel, addressLabel, phoneNumberLabel, companyNameInput, addressInput, phoneNumberInput);
            rightCollumnContainer.Add(emailLabel, representativeLabel, emailInput, representativeInput);
            infoContainer.Add(leftCollumnContainer, rightCollumnContainer, descriptionLabel, descriptionInput, saveButton);
            mainWindow.Add(errorLabel, userPermissionLabel, separatorLine);
        }
    }
}