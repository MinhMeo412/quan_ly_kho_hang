using Terminal.Gui;
using WarehouseManager.UI.Utility;

namespace WarehouseManager.UI.Menu
{
    public static class CompanyInformation
    {
        /*
             Todo.
             Thông tin công ty.
         */
        public static void Display()
        {
            Application.Top.RemoveAll();
            var mainWindow = UIComponent.LoggedInMainWindow("Company Information");
            Application.Top.Add(mainWindow);


            // Chỉnh thành true nếu là admin. nếu ko là admin thì sẽ không sửa đươc.
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

            var companyNameInput = new TextField("Tiêu khiết thanh")
            {
                X = Pos.Right(companyNameLabel) + 1,
                Y = Pos.Top(companyNameLabel),
                Width = Dim.Fill() - 1,
                ReadOnly = !sufficientPermission
            };
            var addressInput = new TextField("18 Tam Trinh, P. Minh Khai, Q. Hai Bà Trưng, Hà Nội")
            {
                X = Pos.Right(companyNameLabel) + 1,
                Y = Pos.Top(addressLabel),
                Width = Dim.Fill() - 1,
                ReadOnly = !sufficientPermission
            };
            var phoneNumberInput = new TextField("113")
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

            var emailInput = new TextField("superman@email.com")
            {
                X = Pos.Right(representativeLabel) + 1,
                Y = Pos.Top(emailLabel),
                Width = Dim.Fill() - 1,
                ReadOnly = !sufficientPermission
            };
            var representativeInput = new TextField("Tập cận bình")
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
                Text = "Thực phẩm bảo vệ sức khỏe (TPBVSK) Tiêu Khiết Thanh là lựa chọn ưu việt cho người bị khản tiếng, mất tiếng, viêm thanh quản: Giúp giọng nói trong sáng và khỏe mạnh hơn.\nTiêu khiết thanh thực phẩm bảo vệ sức khỏe\nGiúp giảm các triệu chứng như thanh quản mất tiếng để\nđể giọng nói trong sáng hơn nhờ có tiêu khiết thanh giọng nói trong sáng hơn nhờ có tiêu khiết thanh\n",
                ReadOnly = !sufficientPermission
            };

            var saveButton = new Button("Save")
            {
                X = Pos.Center(),
                Y = Pos.Bottom(descriptionInput) + 1,
                Visible = sufficientPermission
            };

            var errorLabel = UIComponent.ErrorMessageLabel("Error Message Here");

            var userPermissionLabel = UIComponent.UserPermissionLabel("Username", "Permission");

            saveButton.Clicked += () =>
            {
                // khi nút save được bấm
                MessageBox.Query("Company Information", $"Superman", "OK");
            };

            leftCollumnContainer.Add(companyNameLabel, addressLabel, phoneNumberLabel, companyNameInput, addressInput, phoneNumberInput);
            rightCollumnContainer.Add(emailLabel, representativeLabel, emailInput, representativeInput);
            infoContainer.Add(leftCollumnContainer, rightCollumnContainer, descriptionLabel, descriptionInput, saveButton);
            mainWindow.Add(errorLabel, userPermissionLabel);
        }
    }
}