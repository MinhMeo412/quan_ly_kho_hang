using Hardware.Info;
using System.Diagnostics;
using WarehouseManager.Data.Entity;
using Figgle;

namespace WarehouseManager.Core.Pages
{
    public static class HomeLogic
    {
        public static string GetTime()
        {
            DateTime now = DateTime.Now;

            // Format the time as HH:mm
            string currentTime = now.ToString("HH:mm");
            
            string timeWithSpaces = "";
            foreach (char letter in currentTime)
            {
                timeWithSpaces += $"{letter} ";
            }
            timeWithSpaces = timeWithSpaces.Trim();

            string asciiArt = FiggleFonts.Mini.Render(timeWithSpaces).TrimEnd();

            return asciiArt;
        }

        public static string GetCalendar()
        {
            string calendar =
            @"         July 2024          " + "\n" +
            @" Sun Mon Tue Wed Thu Fri Sat" + "\n" +
            @"       1   2   3   4   5   6" + "\n" +
            @"   7   8   9  10  11  12  13" + "\n" +
            @"  14  15  16 <17> 18  19  20" + "\n" +
            @"  21  22  23  24  25  26  27" + "\n" +
            @"  28  29  30  31";


            DateTime now = DateTime.Now;
            // Get the current day of the week
            string currentDay = now.DayOfWeek.ToString();
            string currentDate = now.ToString("MM/dd/yyyy");

            // Print the current day and date
            Console.WriteLine("Current Day: " + currentDay);
            Console.WriteLine("Current Date: " + currentDate);

            return calendar;
        }

        public static async Task<string> GetWeather()
        {
            string url = "https://wttr.in/?0?A?d?T"; // This returns weather condition and temperature

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string weather = await client.GetStringAsync(url);
                    return weather;
                }
                catch (Exception)
                {
                    string weather =
                    @"  ________________________    " + "\n" +
                    @"/ Unable to connect to     \\ " + "\n" +
                    @"\\ https://wttr.in/?0?A?d?T / " + "\n" +
                    @"  ------------------------    " + "\n" +
                    @"         \   ^__^             " + "\n" +
                    @"          \  (oo)\_______     " + "\n" +
                    @"             (__)\       )\/\\" + "\n" +
                    @"                 ||----w |    " + "\n" +
                    @"                 ||     ||    " + "\n";
                    return weather;
                }
            }
        }


        public static string GetLogoTop()
        {
            string logoTop =
            @"██╗    ██╗ █████╗ ██████╗ ███████╗██╗  ██╗ ██████╗ ██╗   ██╗███████╗███████╗" + "\n" +
            @"██║    ██║██╔══██╗██╔══██╗██╔════╝██║  ██║██╔═══██╗██║   ██║██╔════╝██╔════╝" + "\n" +
            @"██║ █╗ ██║███████║██████╔╝█████╗  ███████║██║   ██║██║   ██║███████╗█████╗  " + "\n" +
            @"██║███╗██║██╔══██║██╔══██╗██╔══╝  ██╔══██║██║   ██║██║   ██║╚════██║██╔══╝  " + "\n" +
            @"╚███╔███╔╝██║  ██║██║  ██║███████╗██║  ██║╚██████╔╝╚██████╔╝███████║███████╗" + "\n" +
            @" ╚══╝╚══╝ ╚═╝  ╚═╝╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝ ╚═════╝  ╚═════╝ ╚══════╝╚══════╝" + "\n";

            return logoTop;
        }

        public static string GetLogoBottom()
        {
            string logoBottom =
            @"███╗   ███╗ █████╗ ███╗   ██╗ █████╗  ██████╗ ███████╗██████╗               " + "\n" +
            @"████╗ ████║██╔══██╗████╗  ██║██╔══██╗██╔════╝ ██╔════╝██╔══██╗              " + "\n" +
            @"██╔████╔██║███████║██╔██╗ ██║███████║██║  ███╗█████╗  ██████╔╝              " + "\n" +
            @"██║╚██╔╝██║██╔══██║██║╚██╗██║██╔══██║██║   ██║██╔══╝  ██╔══██╗              " + "\n" +
            @"██║ ╚═╝ ██║██║  ██║██║ ╚████║██║  ██║╚██████╔╝███████╗██║  ██║              " + "\n" +
            @"╚═╝     ╚═╝╚═╝  ╚═╝╚═╝  ╚═══╝╚═╝  ╚═╝ ╚═════╝ ╚══════╝╚═╝  ╚═╝              " + "\n";

            return logoBottom;
        }

        public static string GetSystemInformation()
        {
            string systemInformation =
            $"{GetUser()}" + "\n" +
            $"{new string('_', $"{GetUser()}".Length)}" + "\n" +
            $"OS: {GetOS()}" + "\n" +
            $"Host: {GetHost()}" + "\n" +
            $"Kernel: {GetKernel()}" + "\n" +
            $"Uptime: {GetUptime()}" + "\n" +
            $"Application Theme: {GetApplicationTheme()}" + "\n" +
            $"CPU: {GetCPU()}" + "\n" +
            $"Application Memory Usage: {GetApplicationMemoryUsage()}" + "\n" +
            $"Shell: {GetShell()}" + "\n" +
            $"Terminal: {GetTerminal()}" + "\n" +
            $"Terminal Resolution: {GetTerminalResolution()}" + "\n";

            return systemInformation;
        }

        public static string GetDatabaseInformation()
        {
            string inventoryAuditInProgressCount = $"Inventory Audits In Progress: {GetInventoryAuditInProgressCount()}";
            string inboundShipmentInProgressCount = $"Inbound Shipments In Progress: {GetInboundShipmentInProgressCount()}";
            string outboundShipmentInProgressCount = $"Outbound Shipments In Progress: {GetOutboundShipmentInProgressCount()}";
            string stockTransferInProgressCount = $"Stock Transfers In Progress: {GetStockTransferInProgressCount()}";
            string productCount = $"Products: {GetProductCount()}";
            string categoryCount = $"Categories: {GetCategoryCount()}";
            string warehouseCount = $"Warehouses: {GetWarehouseCount()}";
            string supplierCount = $"Suppliers: {GetSupplierCount()}";
            string userCount = $"Users: {GetUserCount()}";

            string databaseInformation =
            "QUICK STATS" + "\n" +
            $"{new string('_', $"QUICK STATS".Length)}" + "\n" +
            inventoryAuditInProgressCount + "\n" +
            inboundShipmentInProgressCount + "\n" +
            outboundShipmentInProgressCount + "\n" +
            stockTransferInProgressCount + "\n" +
            productCount + "\n" +
            categoryCount + "\n" +
            warehouseCount + "\n" +
            supplierCount + "\n" +
            userCount + "\n";

            return databaseInformation;
        }

        public static int GetStringWidth(string input)
        {
            List<string> phrases = input.Split('\n').ToList();
            int maxWidth = 0;
            foreach (string phrase in phrases)
            {
                if (phrase.Length > maxWidth)
                {
                    maxWidth = phrase.Length;
                }
            }
            return maxWidth;
        }

        public static int GetStringHeight(string input)
        {
            List<string> phrases = input.Split('\n').ToList();
            int height = phrases.Count;

            return height;
        }

        private static string GetUser()
        {
            return $"{Environment.UserName}@{GetHost()}";
        }

        private static string GetOS()
        {
            var hardwareInfo = new HardwareInfo();
            hardwareInfo.RefreshOperatingSystem();
            return $"{hardwareInfo.OperatingSystem.Name} {hardwareInfo.OperatingSystem.VersionString}";
        }

        private static string GetHost()
        {
            return Environment.MachineName;
        }

        private static string GetKernel()
        {
            return Environment.OSVersion.VersionString;
        }

        private static string GetUptime()
        {
            TimeSpan uptime = TimeSpan.FromSeconds(Stopwatch.GetTimestamp() / (double)Stopwatch.Frequency);
            return $"{(int)uptime.TotalDays} days, {uptime.Hours} hours, {uptime.Minutes} mins, {uptime.Seconds} seconds";
        }

        private static string GetApplicationTheme()
        {
            if (UI.UI.DarkTheme)
            {
                return "Dark";
            }
            else
            {
                return "Light";
            }
        }

        private static string GetCPU()
        {
            var hardwareInfo = new HardwareInfo();
            hardwareInfo.RefreshCPUList();

            foreach (var cpu in hardwareInfo.CpuList)
            {
                return cpu.Name;
            }

            return "Unknown";
        }

        private static string GetApplicationMemoryUsage()
        {
            Process process = Process.GetCurrentProcess();
            long memoryUsed = process.PrivateMemorySize64;
            long maxMemory = GC.GetGCMemoryInfo().TotalAvailableMemoryBytes;

            return $"{memoryUsed / 1024 / 1024}MB / {maxMemory / 1024 / 1024}MB";
        }

        private static string GetShell()
        {
            string? shell = Environment.GetEnvironmentVariable("SHELL");
            if (string.IsNullOrEmpty(shell))
            {
                shell = Environment.GetEnvironmentVariable("ComSpec"); // On Windows
            }
            return $"{shell}";
        }

        private static string GetTerminal()
        {
            string terminal = Environment.GetEnvironmentVariable("TERM") ?? "Unknown";
            return terminal;
        }

        private static string GetTerminalResolution()
        {
            int screenWidth = Console.WindowWidth;
            int screenHeight = Console.WindowHeight;
            return $"{screenWidth}x{screenHeight}";
        }

        private static int GetInventoryAuditInProgressCount()
        {
            List<InventoryAudit> outboundShipments = Program.Warehouse.InventoryAuditTable.InventoryAudits ?? new List<InventoryAudit>();
            return outboundShipments.Count(shipment => shipment.InventoryAuditStatus == "Processing");
        }

        private static int GetInboundShipmentInProgressCount()
        {

            List<InboundShipment> outboundShipments = Program.Warehouse.InboundShipmentTable.InboundShipments ?? new List<InboundShipment>();
            return outboundShipments.Count(shipment => shipment.InboundShipmentStatus == "Processing");
        }

        private static int GetOutboundShipmentInProgressCount()
        {
            List<OutboundShipment> outboundShipments = Program.Warehouse.OutboundShipmentTable.OutboundShipments ?? new List<OutboundShipment>();
            return outboundShipments.Count(shipment => shipment.OutboundShipmentStatus == "Processing");

        }

        private static int GetStockTransferInProgressCount()
        {
            List<StockTransfer> outboundShipments = Program.Warehouse.StockTransferTable.StockTransfers ?? new List<StockTransfer>();
            return outboundShipments.Count(shipment => shipment.StockTransferStatus == "Processing");

        }

        private static int GetProductCount()
        {
            List<Product> products = Program.Warehouse.ProductTable.Products ?? new List<Product>();
            return products.Count;
        }

        private static int GetCategoryCount()
        {
            List<Category> categories = Program.Warehouse.CategoryTable.Categories ?? new List<Category>();
            return categories.Count;
        }

        private static int GetWarehouseCount()
        {
            List<Warehouse> warehouses = Program.Warehouse.WarehouseTable.Warehouses ?? new List<Warehouse>();
            return warehouses.Count;
        }

        private static int GetSupplierCount()
        {
            List<Supplier> suppliers = Program.Warehouse.SupplierTable.Suppliers ?? new List<Supplier>();
            return suppliers.Count;
        }

        private static int GetUserCount()
        {
            List<User> users = Program.Warehouse.UserTable.Users ?? new List<User>();
            return users.Count;
        }
    }
}
