using Terminal.Gui;
using WarehouseManager.UI;

class Program
{
    public static void Main(String[] args)
    {
        // Initialize the application
        Application.Init();

        // Create and show the main window
        UI.Login();

        // Run the application
        Application.Run();

        // Cleanup before exiting
        Application.Shutdown();
    }

}