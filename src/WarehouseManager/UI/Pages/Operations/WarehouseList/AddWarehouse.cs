using Terminal.Gui;
using WarehouseManager.Core.Pages;
using WarehouseManager.UI.Utility;

namespace WarehouseManager.UI.Pages
{
    public static class AddWarehouse
    {
        public static void Display()
        {
            Application.Top.RemoveAll();
            var mainWindow = UIComponent.LoggedInMainWindow("Add New Warehouse");
            Application.Top.Add(mainWindow);

            var errorLabel = UIComponent.AnnounceLabel();

            var userPermissionLabel = UIComponent.UserPermissionLabel();

            var separatorLine = UIComponent.SeparatorLine();
            
            var refreshButton = UIComponent.RefreshButton();

            var container = new FrameView()
            {
                X = Pos.Center(),
                Y = Pos.Center(),
                Width = Dim.Percent(75),
                Height = 17
            };

            var warehouseNameLabel = new Label("Warehouse Name:")
            {
                X = 3,
                Y = 1
            };


            var warehouseAddressLabel = new Label("Warehouse Address:")
            {
                X = 3,
                Y = Pos.Bottom(warehouseNameLabel) + 1
            };

            var warehouseNameInput = new TextField("")
            {
                X = Pos.Right(warehouseAddressLabel) + 1,
                Y = Pos.Top(warehouseNameLabel),
                Width = Dim.Fill(3)
            };

            var warehouseAddressInput = new TextField("")
            {
                X = Pos.Right(warehouseAddressLabel) + 1,
                Y = Pos.Top(warehouseAddressLabel),
                Width = Dim.Fill(3),
            };


            var districtLabel = new Label("District:")
            {
                X = 3,
                Y = Pos.Bottom(warehouseAddressLabel) + 1
            };
            var districtInput = new TextField("")
            {
                X = Pos.Right(warehouseAddressLabel) + 1,
                Y = Pos.Top(districtLabel),
                Width = Dim.Fill(3)
            };

            var postalCodeLabel = new Label("Postal Code:")
            {
                X = 3,
                Y = Pos.Bottom(districtLabel) + 1
            };
            var postalCodeInput = new TextField("")
            {
                X = Pos.Right(warehouseAddressLabel) + 1,
                Y = Pos.Top(postalCodeLabel),
                Width = Dim.Fill(3)
            };

            var cityLabel = new Label("City:")
            {
                X = 3,
                Y = Pos.Bottom(postalCodeLabel) + 1
            };
            var cityInput = new TextField("")
            {
                X = Pos.Right(warehouseAddressLabel) + 1,
                Y = Pos.Top(cityLabel),
                Width = Dim.Fill(3)
            };

            var countryLabel = new Label("Country:")
            {
                X = 3,
                Y = Pos.Bottom(cityLabel) + 1
            };
            var countryInput = new TextField("")
            {
                X = Pos.Right(warehouseAddressLabel) + 1,
                Y = Pos.Top(countryLabel),
                Width = Dim.Fill(3)
            };

            //Tạo nút save
            var saveButton = new Button("Save", is_default: true)
            {
                X = Pos.Center(),
                Y = Pos.Bottom(countryLabel) + 1
            };

            refreshButton.Text = "Back";
            refreshButton.Clicked += () =>
            {
                WarehouseList.Display();
            };
            //Khi nhấn nút save
            saveButton.Clicked += () =>
            {
                try
                {
                    AddWarehouseLogic.Save(
                        $"{warehouseNameInput.Text}",
                        $"{warehouseAddressInput.Text}",
                        $"{districtInput.Text}",
                        $"{postalCodeInput.Text}",
                        $"{cityInput.Text}",
                        $"{countryInput.Text}"
                    );

                    errorLabel.Text = $"Warehouse created successfully!";
                    errorLabel.ColorScheme = UIComponent.AnnounceLabelSuccessColor();

                    warehouseNameInput.Text = "";
                    warehouseAddressInput.Text = "";
                    districtInput.Text = "";
                    postalCodeInput.Text = "";
                    cityInput.Text = "";
                    countryInput.Text = "";
                }
                catch (Exception ex)
                {
                    errorLabel.Text = $"Error: {ex.Message}";
                    errorLabel.ColorScheme = UIComponent.AnnounceLabelErrorColor();
                }
            };

            container.Add(warehouseNameLabel, warehouseNameInput, warehouseAddressLabel, warehouseAddressInput, districtLabel, districtInput, postalCodeLabel, postalCodeInput, cityLabel, cityInput, countryLabel, countryInput, saveButton);
            mainWindow.Add(container, errorLabel, userPermissionLabel, separatorLine, refreshButton);
        }
    }
}