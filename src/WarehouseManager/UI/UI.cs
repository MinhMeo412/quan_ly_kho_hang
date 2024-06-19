using Terminal.Gui;
using WarehouseManager.UI.Utility;

namespace WarehouseManager.UI
{
    class UI
    {
        /*
        Call this to start the interface.
        */
        public static void Start()
        {
            // Initialize the application
            Application.Init();

            // Create and show the main window
            Login();

            // Run the application
            Application.Run();

            // Cleanup before exiting
            Application.Shutdown();
        }

        /*
            Login Menu. Call this to display Login menu.
        */
        private static void Login()
        {
            var mainWindow = new Window("Warehouse Manager")
            {
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                ColorScheme = Theme.SerikaDark
            };
            Application.Top.Add(mainWindow);

            // Create a FrameView (container) and style it
            var loginContainer = new FrameView("Sign in")
            {
                X = Pos.Center(),
                Y = Pos.Center(),
                Width = 40,
                Height = 10
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
                Y = Pos.Center()
            };
            quitButtonContainer.Add(quitButton);

            // Create the Login and Quit buttons
            var loginButton = new Button("Sign in")
            {
                X = Pos.Center(),
                Y = Pos.Center()
            };
            loginButtonContainer.Add(loginButton);



            // Handle the click events
            loginButton.Clicked += () =>
            {
                // cái này thành false nếu đăng nhập ko thành cong
                bool success = true;

                if (success)
                {
                    
                    MessageBox.Query("Sign in", $"Username: {usernameInput.Text}\nPassword: {passwordInput.Text}", "OK");
                    
                    // Chuyển sang menu mới
                    Application.Top.Remove(mainWindow);
                    MainMenu();
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

        private static void MainMenu()
        {
            var mainWindow = new Window("Main Menu")
            {
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                ColorScheme = Theme.SerikaDark
            };
            Application.Top.Add(mainWindow);

            var menu = new MenuBar(new MenuBarItem[] {
                new MenuBarItem("_File", new MenuItem[] {
                    new MenuItem("_Open", "", () => Application.RequestStop()),
                    new MenuItem("_Save", "", () => Application.RequestStop()),
                    new MenuItem("_Quit", "", () => Application.RequestStop())
                }),
                new MenuBarItem("_Help", new MenuItem[] {
                    new MenuItem("_About", "", () => Application.RequestStop())
                })
            });

            Application.Top.Add(menu);

        }
    }
}