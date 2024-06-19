using Terminal.Gui;

namespace WarehouseManager.UI
{
    class UI
    {
        public static void Login()
        {
            var mainWindow = new Window("Login")
            {
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            Application.Top.Add(mainWindow);

            // Create a button to open the secondary window
            var usernameLabel = new Label("Username:")
            {
                X = Pos.Center() - 12,
                Y = Pos.Center() - 4
            };
            var usernameInput = new TextField("")
            {
                X = Pos.Center(),
                Y = Pos.Top(usernameLabel),
                Width = 20
            };

            // Create a label and input box for password
            var passwordLabel = new Label("Password:")
            {
                X = Pos.Center() - 12,
                Y = Pos.Bottom(usernameLabel) + 2
            };
            var passwordInput = new TextField("")
            {
                X = Pos.Center(),
                Y = Pos.Top(passwordLabel),
                Width = 20,
                Secret = true // Mask the input for password
            };

            // Create the Login and Quit buttons
            var signInButton = new Button("Sign in")
            {
                X = Pos.Percent(50) + 2,
                Y = Pos.Bottom(passwordInput) + 2

            };
            var quitButton = new Button("Quit")
            {
                X = Pos.Percent(50) - 10,
                Y = Pos.Bottom(passwordInput) + 2
            };

            signInButton.Clicked += () =>
            {
                // Implement your login logic here
                MessageBox.Query("Login", $"Username: {usernameInput.Text}\nPassword: {passwordInput.Text}", "OK");
            };

            quitButton.Clicked += () => Application.RequestStop();

            // Add all the elements to the main window
            mainWindow.Add(usernameLabel, usernameInput, passwordLabel, passwordInput, signInButton, quitButton);
        }
    }
}