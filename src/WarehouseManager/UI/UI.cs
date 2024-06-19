using Terminal.Gui;
using WarehouseManager.UI.Utility;

namespace WarehouseManager.UI
{
    class UI
    {
        public static void Login()
        {
            var mainWindow = new Window("Warehouse Manager")
            {
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                ColorScheme = Theme.SerikaDark
            };
            Application.Top.Add(mainWindow);

            // Create a FrameView (container) and style it
            var container = new FrameView("Sign in")
            {
                X = Pos.Center(),
                Y = Pos.Center(),
                Width = 40,
                Height = 10
            };
            mainWindow.Add(container);

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
                Width = 20
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
                Width = 20,
                Secret = true // Mask the input for password
            };

            // Create the Login and Quit buttons
            var loginButton = new Button("Login")
            {
                X = Pos.Percent(75) - 2,
                Y = Pos.Bottom(passwordInput) + 2

            };
            var quitButton = new Button("Quit")
            {
                X = Pos.Percent(25) - 2,
                Y = Pos.Bottom(passwordInput) + 2
            };

            // Handle the click events
            loginButton.Clicked += () =>
            {
                // Implement your login logic here
                MessageBox.Query("Login", $"Username: {usernameInput.Text}\nPassword: {passwordInput.Text}", "OK");
            };

            quitButton.Clicked += () => Application.RequestStop();

            // Add all the elements to the container
            container.Add(usernameLabel, usernameInput, passwordLabel, passwordInput, loginButton, quitButton);

        }
    }
}