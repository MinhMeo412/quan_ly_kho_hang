using Terminal.Gui;
using WarehouseManager.UI.Utility;

namespace WarehouseManager.UI.Pages
{
    public static class AddSupplier
    {
        /*
            Todo.
            Thêm nhà cung cấp.
        */
        public static void Display()
        {
            Application.Top.RemoveAll();
            var mainWindow = UIComponent.LoggedInMainWindow("Add New Supplier");
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

            // Left column labels and inputs
            var supplierNameLabel = new Label("Supplier Name:")
            {
                X = 1,
                Y = 1
            };

            var addressLabel = new Label("Address:")
            {
                X = 1,
                Y = 5
            };

            var emailLabel = new Label("Email:")
            {
                X = 1,
                Y = 9
            };

            var supplierNameInput = new TextField("")
            {
                X = Pos.Right(supplierNameLabel) + 1,
                Y = Pos.Top(supplierNameLabel),
                Width = Dim.Fill() - 1,
            };

            var addressInput = new TextField("")
            {
                X = Pos.Right(addressLabel) + 1,
                Y = Pos.Top(addressLabel),
                Width = Dim.Fill() - 1,
            };

            var emailInput = new TextField("")
            {
                X = Pos.Right(emailLabel) + 1,
                Y = Pos.Top(emailLabel),
                Width = Dim.Fill() - 1,
            };

            // Right column labels and inputs
            var phoneLabel = new Label("Phone:")
            {
                X = 1,
                Y = 1
            };

            var websiteLabel = new Label("Website:")
            {
                X = 1,
                Y = 5
            };

            var descriptionLabel = new Label("Description:")
            {
                X = 1,
                Y = 9
            };

            var phoneInput = new TextField("")
            {
                X = Pos.Right(phoneLabel) + 1,
                Y = Pos.Top(phoneLabel),
                Width = Dim.Fill() - 1,
            };

            var websiteInput = new TextField("")
            {
                X = Pos.Right(websiteLabel) + 1,
                Y = Pos.Top(websiteLabel),
                Width = Dim.Fill() - 1,
            };

            var descriptionInput = new TextView()
            {
                X = Pos.Right(descriptionLabel) + 1,
                Y = Pos.Top(descriptionLabel),
                Width = Dim.Fill() - 1,
                Height = 5 // Adjust height as needed
            };

            var saveButton = new Button("Save")
            {
                X = Pos.Center(),
                Y = Pos.Bottom(infoContainer) - 3
            };

            saveButton.Clicked += () =>
            {
                MessageBox.Query("Save Supplier", "Supplier added successfully!", "OK");
            };

            leftCollumnContainer.Add(supplierNameLabel, supplierNameInput, addressLabel, addressInput, emailLabel, emailInput);
            rightCollumnContainer.Add(phoneLabel, phoneInput, websiteLabel, websiteInput, descriptionLabel, descriptionInput);
            infoContainer.Add(leftCollumnContainer, rightCollumnContainer);
            mainWindow.Add(infoContainer, saveButton, errorLabel, userPermissionLabel);
        }
    }
}
