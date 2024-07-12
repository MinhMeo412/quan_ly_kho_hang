using Terminal.Gui;
using WarehouseManager.UI.Utility;
using WarehouseManager.Core.Pages;

namespace WarehouseManager.UI.Pages
{
    public static class Home
    {
        public static async void Display()
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

            string time = HomeLogic.GetTime();
            int timeWidth = HomeLogic.GetStringWidth(time);
            var timeLabel = new TextView()
            {
                X = Pos.Center(),
                Y = 1,
                Width = timeWidth,
                Height = 3,
                ReadOnly = true,
                CanFocus = false,
                Text = time
            };

            string calendar = HomeLogic.GetCalendar();
            int calendarWidth = HomeLogic.GetStringWidth(calendar);
            int calendarHeight = HomeLogic.GetStringHeight(calendar);
            var calendarLabel = new TextView()
            {
                X = Pos.Center(),
                Y = Pos.Bottom(timeLabel) + 1,
                Width = calendarWidth,
                Height = calendarHeight,
                ReadOnly = true,
                CanFocus = false,
                Text = calendar
            };

            var leftContainerMiddleLine = new LineView()
            {
                X = 0,
                Y = Pos.Bottom(calendarLabel) + 1,
                Width = Dim.Fill()
            };

            var weatherLabel = new TextView()
            {
                X = 3,
                Y = Pos.Bottom(leftContainerMiddleLine) + 1,
                Width = Dim.Fill(3),
                Height = Dim.Fill(1),
                ReadOnly = true,
                CanFocus = false,
                Text = "Loading weather..."
            };

            var logoLabelTop = new TextView()
            {
                X = Pos.Center(),
                Y = 1,
                Width = 76,
                Height = 7,
                ReadOnly = true,
                CanFocus = false,
                Text = HomeLogic.GetLogoTop()
            };

            var logolabelBottom = new TextView()
            {
                X = Pos.Center(),
                Y = Pos.Bottom(logoLabelTop),
                Width = 62,
                Height = 6,
                ReadOnly = true,
                CanFocus = false,
                Text = HomeLogic.GetLogoBottom()
            };

            var middleContainerMiddleLine = new LineView()
            {
                X = 0,
                Y = Pos.Bottom(logolabelBottom) + 1,
                Width = Dim.Fill()
            };

            var systemInformationLabel = new TextView()
            {
                X = 3,
                Y = Pos.Bottom(middleContainerMiddleLine) + 1,
                Width = Dim.Fill(3),
                Height = Dim.Fill(1),
                ReadOnly = true,
                CanFocus = false,
                Text = "Loading system information..."
            };

            var rightLabel = new TextView()
            {
                X = 3,
                Y = 1,
                Width = Dim.Fill(3),
                Height = Dim.Fill(1),
                ReadOnly = true,
                CanFocus = false,
                Text = "Loading quick stats..."
            };

            if (UI.DarkTheme)
            {
                timeLabel.ColorScheme = Theme.Light;
                calendarLabel.ColorScheme = Theme.Light;
                weatherLabel.ColorScheme = Theme.Light;
                logoLabelTop.ColorScheme = Theme.Light;
                logolabelBottom.ColorScheme = Theme.Light;
                systemInformationLabel.ColorScheme = Theme.Light;
                rightLabel.ColorScheme = Theme.Light;
            }
            else
            {
                timeLabel.ColorScheme = Theme.Dark;
                calendarLabel.ColorScheme = Theme.Dark;
                weatherLabel.ColorScheme = Theme.Dark;
                logoLabelTop.ColorScheme = Theme.Dark;
                logolabelBottom.ColorScheme = Theme.Dark;
                systemInformationLabel.ColorScheme = Theme.Dark;
                rightLabel.ColorScheme = Theme.Dark;
            }

            leftContainer.Add(timeLabel, calendarLabel, leftContainerMiddleLine, weatherLabel);
            middleContainer.Add(logoLabelTop, logolabelBottom, middleContainerMiddleLine, systemInformationLabel);
            rightContainer.Add(rightLabel);

            mainWindow.Add(errorLabel, userPermissionLabel, separatorLine, leftContainer, middleContainer, rightContainer);

            weatherLabel.Text = await HomeLogic.GetWeather();
            systemInformationLabel.Text = HomeLogic.GetSystemInformation();
            rightLabel.Text = HomeLogic.GetDatabaseInformation();
        }
    }
}