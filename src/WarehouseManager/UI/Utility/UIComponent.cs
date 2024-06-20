using Terminal.Gui;
using WarehouseManager.UI.Menu;

namespace WarehouseManager.UI.Utility
{
    public static class UIComponent
    {

        /*
        Thanh menu.
        */
        private static MenuBar WarehouseMenuBar = new MenuBar(new MenuBarItem[] {
            new MenuBarItem("_Menu", new MenuItem[] {
                new MenuItem("_Back to home", "", Home.Display),
                new MenuItem("_Company information", "", CompanyInformation.Display),
                new MenuItem("_Switch theme", "Light/Dark", UI.SwitchTheme),
                new MenuItem("_Exit program", "Ctrl+Q", () => Application.RequestStop())
            }),
            new MenuBarItem("_Account", new MenuItem[] {
                new MenuItem("_Change password", "", ChangePassword.Display),
                new MenuItem("_Show all accounts", "", AccountList.Display),
                new MenuItem("_Create new account", "", AddAccount.Display)
            }),
            new MenuBarItem("_Suppliers", new MenuItem[] {
                new MenuItem("_Show all suppliers", "", SupplierList.Display),
                new MenuItem("_Add new supplier", "", AddSupplier.Display)
            }),
            new MenuBarItem("_Products", new MenuItem[] {
                new MenuItem("_Show all categories", "", CategoryList.Display),
                new MenuItem("_Create new category", "", AddCategory.Display),
                new MenuItem("_Show all products", "", ProductList.Display),
                new MenuItem("_Add new product", "", AddProduct.Display),
                new MenuItem("_Show stock quantity", "", WarehouseStock.Display)
            }),
            new MenuBarItem("_Warehouses", new MenuItem[] {
                new MenuItem("_Show all shipments", "", ShipmentList.Display),
                new MenuItem("_Add outbound shipment", "", AddOutboundShipment.Display),
                new MenuItem("_Add inbound shipment", "", AddInboundShipment.Display),
                new MenuItem("_Show all inventory audits", "", InventoryAuditList.Display),
                new MenuItem("_Add new inventory audit", "", AddInventoryAudit.Display),
                new MenuItem("_Show all warehouses", "", WarehouseList.Display),
                new MenuItem("_Add new warehouse", "", AddWarehouse.Display)
            }),
            new MenuBarItem("_Report", new MenuItem[] {
                new MenuItem("_Generate Report", "", Home.Display)
            })
        });


        /*
        Cửa sổ chính nhưng ko có thanh menu
        */
        public static Window MainWindow(string? title = null)
        {
            Window mainWindow = new Window(title)
            {
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            if (UI.DarkTheme)
            {
                mainWindow.ColorScheme = Theme.SerikaDark;
            }
            else
            {
                mainWindow.ColorScheme = Theme.GenericLight;
            }

            return mainWindow;
        }

        /*
        Cửa sổ chính nhưng có thanh menu
        */
        public static Window LoggedInMainWindow(string? title = null)
        {
            var mainWindow = UIComponent.MainWindow(title);

            var menuBar = UIComponent.WarehouseMenuBar;
            mainWindow.Add(menuBar);

            return mainWindow;
        }

        /*
        Thông báo lỗi
        */
        public static Label ErrorMessageLabel(string errorMessage = "")
        {
            var errorLabel = new Label(errorMessage)
            {
                X = 1,
                Y = Pos.AnchorEnd(1),
                ColorScheme = new ColorScheme
                {
                    Normal = Application.Driver.MakeAttribute(Color.BrightRed, MainWindow().ColorScheme.Normal.Background),
                }
            };
            return errorLabel;
        }

        public static Label UserPermissionLabel(string username = "Username", string permission = "Permission")
        {
            string text = $"{username} - {permission}";
            var userPermissionLabel = new Label(text)
            {
                X = Pos.AnchorEnd(text.Length + 1),
                Y = Pos.AnchorEnd(1)
            };

            return userPermissionLabel;
        }
    }
}