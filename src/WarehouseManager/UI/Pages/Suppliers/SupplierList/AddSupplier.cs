using Terminal.Gui;
using WarehouseManager.Core.Pages;
using WarehouseManager.UI.Utility;

namespace WarehouseManager.UI.Pages
{
    public static class AddSupplier
    {
        // warning: this menu layout is copied from company information menu due to time constraints. variable names may not make sense
        public static void Display()
        {
            Application.Top.RemoveAll();
            var mainWindow = UIComponent.LoggedInMainWindow("Add New Supplier");
            Application.Top.Add(mainWindow);

            var separatorLine = UIComponent.SeparatorLine();
            var refreshButton = UIComponent.RefreshButton();

            bool sufficientPermission = true;

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

            var companyNameLabel = new Label("Supplier Name:")
            {
                X = 0,
                Y = 1
            };
            var addressLabel = new Label("Address:")
            {
                X = 0,
                Y = Pos.Bottom(companyNameLabel) + 1
            };
            var phoneNumberLabel = new Label("Website:")
            {
                X = 0,
                Y = Pos.Bottom(addressLabel) + 1
            };

            var companyNameInput = new TextField()
            {
                X = Pos.Right(companyNameLabel) + 1,
                Y = Pos.Top(companyNameLabel),
                Width = Dim.Fill(3),
                ReadOnly = !sufficientPermission
            };
            var addressInput = new TextField()
            {
                X = Pos.Right(companyNameLabel) + 1,
                Y = Pos.Top(addressLabel),
                Width = Dim.Fill(3),
                ReadOnly = !sufficientPermission
            };
            var phoneNumberInput = new TextField()
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
            var representativeLabel = new Label("Phone Number:")
            {
                X = 0,
                Y = Pos.Bottom(emailLabel) + 1
            };

            var emailInput = new TextField()
            {
                X = Pos.Right(representativeLabel) + 1,
                Y = Pos.Top(emailLabel),
                Width = Dim.Fill(),
                ReadOnly = !sufficientPermission
            };
            var representativeInput = new TextField()
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
                Text = "",
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

            refreshButton.Text = "Back";
            refreshButton.Clicked += () =>
            {
                SupplierList.Display();
            };

            saveButton.Clicked += () =>
            {
                try
                {
                    AddSupplierLogic.Save(
                        supplierName: $"{companyNameInput.Text}",
                        supplierDescription: $"{descriptionInput.Text}",
                        supplierAddress: $"{addressInput.Text}",
                        supplierEmail: $"{emailInput.Text}",
                        supplierPhoneNumber: $"{representativeInput.Text}",
                        supplierWebsite: $"{phoneNumberInput.Text}"
                    );

                    errorLabel.Text = $"Supplier added successfully!";
                    errorLabel.ColorScheme = UIComponent.AnnounceLabelSuccessColor();

                    companyNameInput.Text = "";
                    descriptionInput.Text = "";
                    addressInput.Text = "";
                    emailInput.Text = "";
                    representativeInput.Text = "";
                    phoneNumberInput.Text = "";
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
            mainWindow.Add(errorLabel, userPermissionLabel, separatorLine, refreshButton);
        }
    }
}
