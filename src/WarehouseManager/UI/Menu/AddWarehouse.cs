using Terminal.Gui;
using WarehouseManager.UI.Utility;

namespace WarehouseManager.UI.Menu
{
    public static class AddWarehouse
    {
        public static void Display()
        {
            Application.Top.RemoveAll();
            var mainWindow = UIComponent.LoggedInMainWindow("Add New Warehouse");
            Application.Top.Add(mainWindow);

            var errorLabel = UIComponent.ErrorMessageLabel("Error Message Here");

            var userPermissionLabel = UIComponent.UserPermissionLabel();

            var separatorLine = UIComponent.SeparatorLine();

            var container = new FrameView()
            {
                X = Pos.Center(),
                Y = Pos.Center(),
                Width = Dim.Percent(65),
                Height = 20
            };

            var warehouseNameLabel = new Label("Warehouse Name:")
            {
                X = 3,
                Y = 1
            };
            var warehouseNameInput = new TextField("")
            {
                X = 25,
                Y = Pos.Top(warehouseNameLabel),
                Width = 50,
            };

            var warehouseAddressLabel = new Label("Warehouse Address:")
            {
                X = 3,
                Y = Pos.Bottom(warehouseNameLabel) + 1
            };
            var warehouseAddressInput = new TextField("")
            {
                X = 25,
                Y = Pos.Top(warehouseAddressLabel),
                Width = 50,
            };

            var districtLabel = new Label("District:")
            {
                X = 3,
                Y = Pos.Bottom(warehouseNameLabel) + 3
            };
            var districtInput = new TextField("")
            {
                X = 25,
                Y = Pos.Top(districtLabel),
                Width = 50,
            };

            var cityLabel = new Label("City:")
            {
                X = 3,
                Y = Pos.Bottom(warehouseNameLabel) + 5
            };
            var cityInput = new TextField("")
            {
                X = 25,
                Y = Pos.Top(cityLabel),
                Width = 50,
            };

            var countryLabel = new Label("Country:")
            {
                X = 3,
                Y = Pos.Bottom(warehouseNameLabel) + 7
            };
            var countryInput = new TextField("")
            {
                X = 25,
                Y = Pos.Top(countryLabel),
                Width = 50,
            };

            var postalCodeLabel = new Label("Postal Code:")
            {
                X = 3,
                Y = Pos.Bottom(warehouseNameLabel) + 9
            };
            var postalCodeInput = new TextField("")
            {
                X = 25,
                Y = Pos.Top(postalCodeLabel),
                Width = 50,
            };

            //Tạo nút save
            var saveButton = new Button("Save", is_default: true)
            {
                X = Pos.Center(),
                Y = Pos.AnchorEnd(3)
            };

            //Khi nhấn nút save
            saveButton.Clicked += () =>
            {
                var message = $"Name: {warehouseNameInput.Text}\nAddress: {warehouseAddressInput.Text}\nDistrict: {districtInput.Text}\nCity: {cityInput.Text}\nCountry: {countryInput.Text}\nPostal Code: {postalCodeInput.Text}";
                var dialog = new Dialog("Save", 60, 20);

                var messageLabel = new Label(message)
                {
                    X = 1,
                    Y = 1,
                    Width = Dim.Fill() - 2,
                    Height = Dim.Fill() - 2,
                    TextAlignment = TextAlignment.Left,
                };

                var okButton = new Button("OK", is_default: true)
                {
                    X = Pos.Center(),
                    Y = Pos.Bottom(messageLabel) + 3
                };
                okButton.Clicked += () => Application.RequestStop();

                dialog.Add(messageLabel, okButton);
                Application.Run(dialog);
            };

            container.Add(warehouseNameLabel, warehouseNameInput, warehouseAddressLabel, warehouseAddressInput, districtLabel, districtInput, cityLabel, cityInput, countryLabel, countryInput, postalCodeLabel, postalCodeInput, saveButton);
            mainWindow.Add(container, errorLabel, userPermissionLabel, separatorLine);
        }
    }
}