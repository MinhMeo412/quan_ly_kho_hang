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

            string time =
            @"    _        _ " + "\n" +
            @"/| |_  o /| / \" + "\n" +
            @" | |_) o  | \_/" + "\n";

            var timeLabel = new TextView()
            {
                X = Pos.Center(),
                Y = 1,
                Width = 15,
                Height = 3,
                ReadOnly = true,
                CanFocus = false,
                Text = time
            };

            string calendar =
            @"         July 2024          " + "\n" +
            @" Sun Mon Tue Wed Thu Fri Sat" + "\n" +
            @"       1   2   3   4   5   6" + "\n" +
            @"   7   8   9  10  11  12  13" + "\n" +
            @"  14  15  16 <17> 18  19  20" + "\n" +
            @"  21  22  23  24  25  26  27" + "\n" +
            @"  28  29  30  31" + "\n";
            var calendarLabel = new TextView()
            {
                X = Pos.Center(),
                Y = Pos.Bottom(timeLabel) + 1,
                Width = 28,
                Height = 7,
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

            string weather =
            @" ________________________________________ " + "\n" +
            @"/ You have Egyptian flu: you're going to \" + "\n" +
            @"\ be a mummy.                            /" + "\n" +
            @" ---------------------------------------- " + "\n" +
            @"        \   ^__^                          " + "\n" +
            @"         \  (oo)\_______" + "\n" +
            @"            (__)\       )\/\" + "\n" +
            @"                ||----w |" + "\n" +
            @"                ||     ||" + "\n";
            var weatherLabel = new TextView()
            {
                X = 3,
                Y = Pos.Bottom(leftContainerMiddleLine) + 1,
                Width = Dim.Fill(3),
                Height = Dim.Fill(1),
                ReadOnly = true,
                CanFocus = false,
                Text = weather
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
                Text = HomeLogic.GetSystemInformation()
            };

            var rightLabel = new TextView()
            {
                X = 3,
                Y = 1,
                Width = Dim.Fill(3),
                Height = Dim.Fill(1),
                ReadOnly = true,
                CanFocus = false,
                Text = HomeLogic.GetDatabaseInformation()
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
        }
    }
}