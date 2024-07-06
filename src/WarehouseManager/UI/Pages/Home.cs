using Terminal.Gui;
using WarehouseManager.UI.Utility;

namespace WarehouseManager.UI.Pages
{
    public static class Home
    {
        public static void Display()
        {
            Application.Top.RemoveAll();
            var mainWindow = UIComponent.LoggedInMainWindow("Home");
            Application.Top.Add(mainWindow);

            string errorMessage = "";

            var errorLabel = UIComponent.ErrorMessageLabel(errorMessage);

            var userPermissionLabel = UIComponent.UserPermissionLabel();

            var separatorLine = UIComponent.SeparatorLine();

            string speechBubble =
            @"⠀⠀⠀⠀⠀⠀⠀⢀⣠⠤⠴⠒⠒⠒⠒⠒⠒⠒⠦⢤⣀⡀⠀⠀⠀⠀⠀⠀⠀" + "\n" +
            @"⠀⠀⠀⠀⣀⡴⠚⠉⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠉⠳⢦⡀⠀⠀⠀⠀" + "\n" +
            @"⠀⠀⢠⠞⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠙⢳⡀⠀⠀" + "\n" +
            @"⠀⣰⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠙⣆⠀" + "\n" +
            @"⢰⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠸⡆" + "\n" +
            @"⡟       Use the menu        ⣷" + "\n" +
            @"⣧     above to navigate!  ⠀⠀⡿" + "\n" +
            @"⢸⡄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢠⠇" + "\n" +
            @"⠀⢳⡄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢠⡯⠁" + "\n" +
            @"⠀⠀⠙⢦⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⡴⠋⠁⠀" + "\n" +
            @"⠀⠀⠀⠀⠙⠦⣄⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣠⠶⠋⠀⠀⠀⠀" + "\n" +
            @"⠀⠀⠀⠀⠀⠀⠀⠉⠛⠲⠤⢤⣀⠀⠀⠀⠀⠀⢶⠶⠛⠉⠀⠀⠀⠀⠀⠀⠀" + "\n" +
            @"⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠙⠳⢦⣄⡀⠈⠳⣄⠀⠀⠀⠀⠀⠀⠀⠀" + "\n" +
            @"⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠙⠓⢦⣝⣦⡀⠀⠀⠀⠀⠀⠀" + "\n" +
            @"⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠙⣓⠀⠀⠀⠀⠀⠀" + "\n";

            string tf2Heavy =
            @"⠄⠂⠄⠐⠄⠄⠄⠄⠄⢀⣠⣶⣶⣿⣿⣿⣷⣶⣶⣤⣀⡀⠄⠄⠄⠄⠄⠄⠂⠄⠐⠄⠄⠂⠄⠐⠄" + "\n" +
            @"⠄⠐⠈⠄⠄⠄⠄⣀⣴⣿⣿⣿⣿⣿⢿⣿⢿⣿⣿⣿⣿⣿⣿⣷⣦⡀⠄⠄⠄⠄⠁⢀⠈⠄⠈⠄⡀" + "\n" +
            @"⠐⠄⠄⠄⠄⢀⣴⣿⣿⡿⣿⢽⣾⣽⢿⣺⡯⣷⣻⣽⣻⣟⣿⣿⣿⣿⣦⡀⠄⠄⠄⠄⡀⠐⠈⠄⠄" + "\n" +
            @"⠄⠄⠄⠄⢀⣾⣿⣿⢿⡽⡯⣿⢾⣟⣿⣳⢿⡽⣾⣺⣳⣻⣺⣽⣻⡿⣿⣿⣦⠄⠄⠄⠄⠄⠄⠄⠂" + "\n" +
            @"⠄⠄⠄⠄⣿⣿⣿⣯⢿⣽⣻⣽⣿⣿⣯⣿⣿⣿⣷⣻⢮⣗⡯⣞⡾⡹⡵⣻⣿⣇⠄⠄⠄⠂⠄⠄⠠" + "\n" +
            @"⠄⠄⠄⣸⣿⣿⣿⣿⡿⡾⣳⢿⢿⢿⠿⠿⠟⠟⠟⠿⣯⡾⣝⣗⣯⢪⢎⢗⣯⣿⣇⠄⠄⠄⠄⢀⠄" + "\n" +
            @"⠄⠄⠄⠋⠉⠁⠑⠁⢉⣁⡁⠁⠁⠄⠄⠄⠄⠄⠄⠄⢉⢻⢽⣞⢾⣕⢕⢝⢎⣿⣿⡀⠄⠄⠄⠄⢀" + "\n" +
            @"⠄⠄⠄⡧⠠⡀⠐⠂⣸⣿⢿⢔⢔⢤⢈⠡⡱⣩⢤⢴⣞⣾⣽⢾⣽⣺⡕⡕⡕⡽⣿⣿⠟⢶⠄⠄⠄" + "\n" +
            @"⠄⠄⠄⣿⡳⡄⡢⡂⣿⣿⢯⣫⢗⣽⣳⡣⣗⢯⣟⣿⣿⢿⡽⣳⢗⡷⣻⡎⢎⢎⣿⡇⠻⣦⠃⠄⠄" + "\n" +
            @"⠄⠄⠄⡿⡝⡜⣜⣬⣿⣿⣿⣷⣯⢺⠻⡻⣜⢔⠡⢓⢝⢕⢏⢗⢏⢯⡳⡝⡸⡸⣸⣧⡀⣹⣠⠄⠄" + "\n" +
            @"⠄⠄⠄⣇⢪⢎⡧⡛⠛⠋⠋⠉⠙⣨⣮⣦⢅⡃⠇⡕⡌⡪⡨⢸⢨⢣⠫⡨⢪⢸⠰⣿⣇⣾⡞⠄⠄" + "\n" +
            @"⠄⠄⠄⢑⡕⡵⡻⣕⠄⠄⠄⠔⡜⡗⡟⣟⢿⢮⢆⡑⢕⣕⢎⢮⡪⡎⡪⡐⢅⢇⢣⠹⡛⣿⡅⠄⠄" + "\n" +
            @"⠄⠄⠄⢸⢎⠪⡊⣄⣰⣰⣵⣕⣮⣢⣳⡸⡨⠪⡨⠂⠄⠑⢏⠗⢍⠪⡢⢣⢃⠪⡂⣹⣽⣿⣷⡄⠄" + "\n" +
            @"⠄⠄⠄⡸⠐⠝⠋⠃⠡⡕⠬⠎⠬⠩⠱⢙⣘⣑⣁⡈⠄⠄⡕⢌⢊⢪⠸⡘⡜⢌⠢⣸⣾⢿⣿⣿⡀" + "\n" +
            @"⠄⠄⠄⡎⣐⠲⢒⢚⢛⢛⢛⢛⠛⠝⡋⡫⢉⠪⡱⠡⠄⠠⢣⢑⠱⡨⡊⡎⢜⢐⠅⢼⡾⣟⣿⣿⣷" + "\n" +
            @"⠄⠄⣠⡃⡢⠨⢀⢂⢐⢐⢄⠑⠌⢌⢂⠢⠡⡑⡘⢌⠠⡘⡌⢎⠜⡌⢎⠜⡌⠢⠨⡸⣿⡽⣿⣿⣿" + "\n" +
            @"⢴⠋⠁⡢⡑⡨⢐⢐⢌⠢⣂⢣⠩⡂⡢⡑⡑⡌⢜⠰⡨⢪⢘⠔⡱⢘⠔⡑⠨⢈⠐⢼⡷⣿⣻⢷⢯" + "\n" +
            @"⠂⠄⠄⡢⡃⡢⢊⢔⢢⠣⡪⡢⢣⠪⡢⡑⡕⡜⡜⡌⢎⢢⠱⠨⡂⡑⠨⠄⠁⡂⡨⣺⡽⡯⡫⠣⠡" + "\n" +
            @"⠄⠄⣰⡸⠐⠌⠆⢇⠎⡎⢎⢎⢎⢎⢎⢎⠎⡎⡪⠘⠌⠂⠁⠁⠄⢀⠄⢄⢢⢚⢮⢏⠞⡨⢂⠕⠉" + "\n" +
            @"⡀⢀⡯⡃⡌⠈⠈⠄⠄⠈⠄⠄⠄⠄⠄⠂⠁⠄⠄⠄⠄⠄⠄⢄⢂⢢⢱⢱⢱⠱⢡⢑⠌⢂⠡⠠⠡" + "\n" +
            @"⣀⡸⠨⢂⠌⡊⢄⢂⠠⠄⠄⠄⠄⠄⠄⠄⠄⠄⢀⠠⢐⢨⣘⢔⢵⠱⡃⡃⡕⡸⠐⡁⠌⡐⡨⠨⢊" + "\n" +
            @"⢣⢎⠨⠄⠄⠨⢊⢪⢪⡫⡪⡊⡐⡐⡐⡌⡬⡪⡪⣎⢗⡕⡎⡣⠃⢅⠊⠆⠡⠠⡁⠢⡑⡐⢌⢌⠢" + "\n" +
            @"⡎⡎⠄⠈⠄⠡⢂⠂⡕⡕⡕⠕⢌⢌⢢⢱⢸⡸⣪⢮⡣⡓⠌⠠⢑⠡⢈⠌⢌⢂⠪⠨⡂⣊⠢⡢⢣" + "\n" +
            @"⢢⢣⠊⢀⠨⠨⢐⠐⡸⡸⡪⡱⡑⡌⡆⡇⡇⣏⢮⢪⢪⠊⠄⢑⢐⠨⡐⢌⠢⡪⡘⢌⠢⡢⢣⠪⠊" + "\n" +
            @"⡣⡑⢅⠄⠄⠨⢐⠨⢰⢱⢣⢣⢪⢸⢨⡚⣞⢜⢎⢎⢎⠪⠐⠄⡆⡣⡪⡊⡪⡂⡪⡘⡌⡎⠊⠄⡐" + "\n" +
            @"⡣⠊⠄⣷⣄⠄⠄⠌⢸⢸⠱⡱⡡⡣⡣⡳⡕⡇⡇⡇⠥⠑⠄⢡⢑⠕⢔⢑⠔⡌⡆⠇⠁⡀⠄⠁⠄" + "\n" +
            @"⡊⠌⠄⣿⣿⣷⣤⣤⣂⣅⣑⠰⠨⠢⢑⣕⣜⣘⣨⣦⣥⣬⠄⢐⢅⢊⢢⢡⠣⠃⠄⡐⠠⠄⡂⠡⢈" + "\n" +
            @"⢨⠨⠄⢹⢛⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⢿⠟⠄⢰⢰⢱⠑⠁⢀⠐⡀⠂⠄⠡⠐⠐⡀" + "\n" +
            @"⡱⢐⠄⢸⡲⡠⠄⠉⠙⠻⠿⣿⠿⠿⢛⠫⡩⡳⣸⠾⠁⢀⢢⠣⠃⠄⠠⠐⡀⠂⠄⠡⠈⠄⡁⠂⠄" + "\n" +
            @"⢌⠆⢕⠈⣗⢥⢣⢡⢑⢌⡢⡢⢅⢇⢇⢯⢾⡽⠃⢀⠔⡅⠁⠄⠄⠨⢀⠡⠐⠈⠄⠡⢈⠐⡀⠅⠌" + "\n" +
            @"⠢⠭⢆⠦⡿⡷⡷⡵⡷⡷⣵⢽⡮⣷⢽⡽⡓⠤⠤⠕⡁⠠⠄⠅⠄⠅⠄⠂⠌⠠⠡⠈⠄⢂⠐⠠⠨" + "\n";


            var speechBubbleLabel = new TextView()
            {
                X = 3,
                Y = 1,
                Width = 29,
                Height = 15,
                ReadOnly = true,
                CanFocus = false,
                Text = speechBubble
            };

            var tf2HeavyLabel = new TextView()
            {
                X = Pos.Right(speechBubbleLabel) + 1,
                Y = 1,
                Width = 37,
                Height = Dim.Fill(2),
                ReadOnly = true,
                CanFocus = false,
                Text = tf2Heavy
            };

            if (UI.DarkTheme)
            {
                speechBubbleLabel.ColorScheme = Theme.Light;
                tf2HeavyLabel.ColorScheme = Theme.Light;
            }
            else
            {
                speechBubbleLabel.ColorScheme = Theme.Dark;
                tf2HeavyLabel.ColorScheme = Theme.Dark;
            }

            mainWindow.Add(speechBubbleLabel, tf2HeavyLabel, separatorLine, errorLabel, userPermissionLabel);

            // 5% to get secret menu
            if (new Random().Next(100) > 95)
            {
                mainWindow.Title = "The end";
                List<FrameView> containers = new List<FrameView>();
                for (int i = 0; i < 25; i++)
                {
                    containers.Add(new FrameView("is never the end")
                    {
                        Width = Dim.Fill(),
                        Height = Dim.Fill(1)
                    });
                }
                for (int i = 24; i > 0; i--)
                {
                    containers[i - 1].Add(containers[i], UIComponent.ErrorMessageLabel(errorMessage), UIComponent.UserPermissionLabel());
                }
                mainWindow.Add(containers[0]);
            }
        }
    }
}