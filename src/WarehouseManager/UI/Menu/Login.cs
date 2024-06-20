using Terminal.Gui;
using WarehouseManager.UI.Utility;

namespace WarehouseManager.UI.Menu
{

    public static class Login
    {
        /*
            Login Menu. Call this to display Login menu.
        */
        public static void Display()
        {
            Application.Top.RemoveAll();
            var mainWindow = UIComponent.MainWindow("Warehouse Manager");

            Application.Top.Add(mainWindow);

            // Create a FrameView (container) and style it
            var loginContainer = new FrameView("Sign in")
            {
                X = Pos.Center(),
                Y = Pos.Center(),
                Width = 40,
                Height = 8
            };
            mainWindow.Add(loginContainer);

            // Create a label and input box for username
            var usernameLabel = new Label("Username:")
            {
                X = 1,
                Y = 1
            };
            var usernameInput = new TextField("")
            {
                X = Pos.Right(usernameLabel) + 1,
                Y = Pos.Top(usernameLabel),
                Width = Dim.Fill() - 1
            };

            // Create a label and input box for password
            var passwordLabel = new Label("Password:")
            {
                X = 1,
                Y = Pos.Bottom(usernameLabel) + 1
            };
            var passwordInput = new TextField("")
            {
                X = Pos.Right(passwordLabel) + 1,
                Y = Pos.Top(passwordLabel),
                Width = Dim.Fill() - 1,
                Secret = true // Mask the input for password
            };

            var quitButtonContainer = new FrameView()
            {
                Y = Pos.Bottom(passwordInput),
                Width = Dim.Percent(50),
                Height = Dim.Fill(),
                Border = new Border()
                {
                    BorderStyle = BorderStyle.None,
                    DrawMarginFrame = false,
                    Effect3D = false,
                }
            };

            var loginButtonContainer = new FrameView()
            {
                X = Pos.Right(quitButtonContainer),
                Y = Pos.Bottom(passwordInput),
                Width = Dim.Percent(50),
                Height = Dim.Fill(),
                Border = new Border()
                {
                    BorderStyle = BorderStyle.None,
                    DrawMarginFrame = false,
                    Effect3D = false,
                }
            };

            var quitButton = new Button("Quit")
            {
                X = Pos.Center(),
                Y = Pos.AnchorEnd(1)
            };
            quitButtonContainer.Add(quitButton);

            // Create the Login and Quit buttons
            var loginButton = new Button("Sign in")
            {
                X = Pos.Center(),
                Y = Pos.AnchorEnd(1)
            };
            loginButtonContainer.Add(loginButton);



            // Handle the click events
            loginButton.Clicked += () =>
            {
                // cái này thành false nếu đăng nhập ko thành cong
                bool success = true;

                if (success)
                {
                    // Pop up username va password:
                    // MessageBox.Query("Sign in", $"Username: {usernameInput.Text}\nPassword: {passwordInput.Text}", "OK");

                    // Chuyển sang menu mới
                    Home.Display();
                }
                else
                {
                    MessageBox.Query("Login Failed", $"Username or password is incorrect", "OK");
                }
            };

            quitButton.Clicked += () => Application.RequestStop();

            // Add all the elements to the container
            loginContainer.Add(usernameLabel, usernameInput, passwordLabel, passwordInput, quitButtonContainer, loginButtonContainer);
        }

    }
}