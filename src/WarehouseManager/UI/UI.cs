using Terminal.Gui;
using WarehouseManager.UI.Menu;

namespace WarehouseManager.UI
{
    public static class UI
    {
        /*
        Call this to start the interface.
        */
        public static void Start()
        {
            // Initialize the application
            Application.Init();

            // Create and show the login menu
            Login.Display();

            // Run the application
            Application.Run();

            // Cleanup before exiting
            Application.Shutdown();
        }
    }
}