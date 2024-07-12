using Terminal.Gui;
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

            // Chỉnh thành true nếu là admin. nếu ko là admin thì sẽ không sửa đươc.
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
                X = 1,
                Width = Dim.Percent(50),
                Height = 8,
                Border = new Border()
                {
                    BorderStyle = BorderStyle.None
                }
            };

            var rightCollumnContainer = new FrameView()
            {
                X = Pos.Right(leftCollumnContainer),
                Width = Dim.Fill(1),
                Height = 8,
                Border = new Border()
                {
                    BorderStyle = BorderStyle.None
                }
            };

            var companyNameLabel = new Label("Company Name:")
            {
                X = 1,
                Y = 1
            };
            var addressLabel = new Label("Address:")
            {
                X = 1,
                Y = Pos.Bottom(companyNameLabel) + 1
            };
            var phoneNumberLabel = new Label("Phone Number:")
            {
                X = 1,
                Y = Pos.Bottom(addressLabel) + 1
            };

            var companyNameInput = new TextField("Aperture Science")
            {
                X = Pos.Right(companyNameLabel) + 1,
                Y = Pos.Top(companyNameLabel),
                Width = Dim.Fill() - 1,
                ReadOnly = !sufficientPermission
            };
            var addressInput = new TextField("123 Enrichment Center Way, Upper Michigan, USA")
            {
                X = Pos.Right(companyNameLabel) + 1,
                Y = Pos.Top(addressLabel),
                Width = Dim.Fill() - 1,
                ReadOnly = !sufficientPermission
            };
            var phoneNumberInput = new TextField("555-9876")
            {
                X = Pos.Right(companyNameLabel) + 1,
                Y = Pos.Top(phoneNumberLabel),
                Width = Dim.Fill() - 1,
                ReadOnly = !sufficientPermission
            };

            var emailLabel = new Label("Email:")
            {
                X = 1,
                Y = 1
            };
            var representativeLabel = new Label("Represenstative:")
            {
                X = 1,
                Y = Pos.Bottom(emailLabel) + 1
            };

            var emailInput = new TextField("contact@aperturescience.com")
            {
                X = Pos.Right(representativeLabel) + 1,
                Y = Pos.Top(emailLabel),
                Width = Dim.Fill() - 1,
                ReadOnly = !sufficientPermission
            };
            var representativeInput = new TextField("Cave Johnson")
            {
                X = Pos.Right(representativeLabel) + 1,
                Y = Pos.Top(representativeLabel),
                Width = Dim.Fill() - 1,
                ReadOnly = !sufficientPermission
            };

            var descriptionLabel = new Label("Description:")
            {
                X = 2,
                Y = Pos.Bottom(leftCollumnContainer)
            };

            var descriptionInput = new TextView()
            {
                X = 2,
                Y = Pos.Bottom(descriptionLabel) + 1,
                Width = Dim.Fill(2),
                Height = Dim.Fill() - 3,
                Text = "Aperture Science is a leading innovator in the field of science and technology. \nKnown for its cutting-edge research and development, Aperture Science is dedicated to pushing the boundaries of what is possible.\nOur state-of-the-art Enrichment Center provides a safe and controlled environment for testing revolutionary new products and ideas.",
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
            };

            leftCollumnContainer.Add(companyNameLabel, addressLabel, phoneNumberLabel, companyNameInput, addressInput, phoneNumberInput);
            rightCollumnContainer.Add(emailLabel, representativeLabel, emailInput, representativeInput);
            infoContainer.Add(leftCollumnContainer, rightCollumnContainer, descriptionLabel, descriptionInput, saveButton);
            mainWindow.Add(errorLabel, userPermissionLabel, separatorLine);
        }
    }
}