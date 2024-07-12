using Terminal.Gui;
using WarehouseManager.UI.Utility;
using WarehouseManager.Core.Pages;

namespace WarehouseManager.UI.Pages
{
    public static class Home
    {
        public static void Display()
        {
            Application.Top.RemoveAll();
            var mainWindow = UIComponent.LoggedInMainWindow("Home");
            Application.Top.Add(mainWindow);

            var errorLabel = UIComponent.AnnounceLabel();
            var userPermissionLabel = UIComponent.UserPermissionLabel();
            var separatorLine = UIComponent.SeparatorLine();

            var leftContainer = new FrameView()
            {
                X = 2,
                Y = 1,
                Width = Dim.Percent(23),
                Height = Dim.Fill(3),
                Border = new Border() { BorderStyle = BorderStyle.Rounded }
            };

            var middleContainer = new FrameView()
            {
                X = Pos.Right(leftContainer) + 2,
                Y = 1,
                Width = Dim.Percent(50),
                Height = Dim.Fill(3),
                Border = new Border() { BorderStyle = BorderStyle.Rounded }
            };

            var rightContainer = new FrameView()
            {
                X = Pos.Right(middleContainer) + 2,
                Y = 1,
                Width = Dim.Fill(2),
                Height = Dim.Fill(3),
                Border = new Border() { BorderStyle = BorderStyle.Rounded }
            };

            var leftLabel = new TextView()
            {
                X = 1,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill(1),
                ReadOnly = true,
                CanFocus = false,
                Text = HomeLogic.GetTimeInformation()
            };

            var logoLabel = new TextView()
            {
                X = 1,
                Y = 1,
                Width = Dim.Fill(),
                Height = 13,
                ReadOnly = true,
                CanFocus = false,
                Text = HomeLogic.GetLogo()
            };

            var systemInformationLabel = new TextView()
            {
                X = 1,
                Y = Pos.Bottom(logoLabel) + 1,
                Width = Dim.Fill(),
                Height = Dim.Fill(1),
                ReadOnly = true,
                CanFocus = false,
                Text = HomeLogic.GetSystemInformation()
            };

            var rightLabel = new TextView()
            {
                X = 1,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill(1),
                ReadOnly = true,
                CanFocus = false,
                Text = HomeLogic.GetDatabaseInformation()
            };

            if (UI.DarkTheme)
            {
                leftLabel.ColorScheme = Theme.Light;
                logoLabel.ColorScheme = Theme.Light;
                systemInformationLabel.ColorScheme = Theme.Light;
                rightLabel.ColorScheme = Theme.Light;
            }
            else
            {
                leftLabel.ColorScheme = Theme.Dark;
                logoLabel.ColorScheme = Theme.Light;
                systemInformationLabel.ColorScheme = Theme.Dark;
                rightLabel.ColorScheme = Theme.Light;
            }

            leftContainer.Add(leftLabel);
            middleContainer.Add(logoLabel, systemInformationLabel);
            rightContainer.Add(rightLabel);

            mainWindow.Add(errorLabel, userPermissionLabel, separatorLine, leftContainer, middleContainer, rightContainer);
        }
    }
}