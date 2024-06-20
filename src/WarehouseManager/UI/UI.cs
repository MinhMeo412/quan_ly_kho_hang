using Terminal.Gui;
using WarehouseManager.UI.Menu;

namespace WarehouseManager.UI
{
    public static class UI
    {
        /*
        Call this to start the interface.
        */
        public static bool DarkTheme { get; set; } = true;

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


        public static void SwitchTheme()
        {
            int result = MessageBox.Query("Switch Theme", "Reload is required. Proceed?", "No", "Yes");

            if (result == 1) // "Yes" button was pressed
            {
                DarkTheme = !DarkTheme;
                Home.Display();
            }
        }

    }
}