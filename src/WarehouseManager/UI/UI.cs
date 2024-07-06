using Terminal.Gui;
using WarehouseManager.UI.Pages;

namespace WarehouseManager.UI
{
    public static class UI
    {
        public static bool DarkTheme { get; private set; } = true;

        /*
        Call this to start the interface.
        */
        public static void Start()
        {
            // Initialize the application
            Application.Init();

            // Show the login menu
            Login.Display();

            // Run the application
            Application.Run();

            // Cleanup before exiting
            Application.Shutdown();
        }

        // Đổi theme 
        public static void SwitchTheme()
        {
            int result = MessageBox.Query("Switch Theme", "A reload is required. Proceed?", "No", "Yes");

            if (result == 1) // "Yes" button was pressed
            {
                DarkTheme = !DarkTheme;
                Home.Display();
            }
        }
    }
}