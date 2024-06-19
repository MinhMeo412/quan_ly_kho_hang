using WarehouseManager.UI;
using Terminal.Gui;
using WarehouseManager.Data.Entity;

namespace WarehouseManager.UI.Utility
{
    public static class UIComponent
    {
        private static MenuBar WarehouseMenuBar = new MenuBar(new MenuBarItem[] {
            new MenuBarItem("_Menu", new MenuItem[] {
                new MenuItem("_Back to main menu", "", UI.MainMenu),
                new MenuItem("_Company information", "", UI.CompanyInformation),
                new MenuItem("_Switch theme", "Light/Dark", () => Application.RequestStop()),
                new MenuItem("_Exit program", "Ctrl+Q", () => Application.RequestStop())
            }),
            new MenuBarItem("_Account", new MenuItem[] {
                new MenuItem("_Change password", "", UI.ChangePassword),
                new MenuItem("_Show all accounts", "", UI.AccountList),
                new MenuItem("_Create new account", "", UI.AddAccount)
            }),
            new MenuBarItem("_Suppliers", new MenuItem[] {
                new MenuItem("_Show all suppliers", "", UI.SupplierList),
                new MenuItem("_Add new supplier", "", UI.AddSupplier)
            }),
            new MenuBarItem("_Products", new MenuItem[] {
                new MenuItem("_Show all categories", "", UI.CategoryList),
                new MenuItem("_Create new category", "", UI.AddCategory),
                new MenuItem("_Show all products", "", UI.ProductList),
                new MenuItem("_Add new product", "", UI.AddProduct),
                new MenuItem("_Show stock quantity", "", UI.WarehouseStock)
            }),
            new MenuBarItem("_Warehouses", new MenuItem[] {
                new MenuItem("_Show all shipments", "", UI.ShipmentList),
                new MenuItem("_Add outbound shipment", "", UI.AddOutboundShipment),
                new MenuItem("_Add inbound shipment", "", UI.AddInboundShipment),
                new MenuItem("_Show all inventory audits", "", UI.InventoryAuditList),
                new MenuItem("_Add new inventory audit", "", UI.AddInventoryAudit),
                new MenuItem("_Show all warehouses", "", UI.WarehouseList),
                new MenuItem("_Add new warehouse", "", UI.AddWarehouse)
            }),
            new MenuBarItem("_Report", new MenuItem[] {
                new MenuItem("_Generate Report", "", UI.MainMenu)
            })
        });

        public static Window MainWindow(string? title = null)
        {
            Window mainWindow = new Window(title)
            {
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                ColorScheme = Theme.SerikaDark
            };

            return mainWindow;
        }

        public static Window LoggedInMainWindow(string? title = null)
        {
            var mainWindow = UIComponent.MainWindow(title);

            var menuBar = UIComponent.WarehouseMenuBar;
            mainWindow.Add(menuBar);

            return mainWindow;
        }
    }
}