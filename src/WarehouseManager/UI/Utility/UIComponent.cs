using System.Data;
using Terminal.Gui;
using WarehouseManager.UI.Pages;
using WarehouseManager.Core.Pages;

namespace WarehouseManager.UI.Utility
{
    public static class UIComponent
    {

        /*
        Thanh menu.
        */
        private static MenuBar WarehouseMenuBar()
        {
            var menuBar = new MenuBar(new MenuBarItem[] {
                new MenuBarItem("_Menu", new MenuItem[] {
                    new MenuItem("_Home", "", Home.Display, () => canExecuteMenu(4)),
                    new MenuItem("_Company Information", "", CompanyInformation.Display, () => canExecuteMenu(4)),
                    new MenuItem("_Switch Theme", "Light/Dark", UI.SwitchTheme, () => canExecuteMenu(4)),
                    new MenuItem("_Exit Program", "Ctrl+Q", () => Application.RequestStop(), () => canExecuteMenu(4))
                }),
                new MenuBarItem("_Account", new MenuItem[] {
                    new MenuItem("_Change Password", "", ChangePassword.Display, () => canExecuteMenu(4)),
                    new MenuItem("_Accounts", "", AccountList.Display, () => canExecuteMenu(4))
                }),
                new MenuBarItem("_Inventory", new MenuItem[] {
                    new MenuItem("_Warehouse Stock", "", WarehouseStock.Display, () => canExecuteMenu(4)),
                    new MenuItem("_Products", "", ProductList.Display, () => canExecuteMenu(4)),
                    new MenuItem("_Categories", "", CategoryList.Display, () => canExecuteMenu(4))
                }),
                new MenuBarItem("_Operations", new MenuItem[] {
                    new MenuItem("_Warehouses", "", WarehouseList.Display, () => canExecuteMenu(4)),
                    new MenuItem("_Shipments", "", ShipmentList.Display, () => canExecuteMenu(4)),
                    new MenuItem("_Inventory Audits", "", InventoryAuditList.Display, () => canExecuteMenu(4))
                }),
                new MenuBarItem("_Suppliers", new MenuItem[] {
                    new MenuItem("_Suppliers", "", SupplierList.Display, () => canExecuteMenu(4))
                }),
                new MenuBarItem("_Report", new MenuItem[] {
                    new MenuItem("_Generate Report", "", Home.Display, () => canExecuteMenu(4))
                })
            });

            return menuBar;
        }

        private static bool canExecuteMenu(int requiredPermission)
        {
            int permissionLevel = Program.Warehouse.PermissionLevel ?? 4;

            if (permissionLevel <= requiredPermission)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

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
                mainWindow.ColorScheme = Theme.Dark;
            }
            else
            {
                mainWindow.ColorScheme = Theme.Light;
            }

            return mainWindow;
        }

        /*
        Cửa sổ chính nhưng có thanh menu
        */
        public static Window LoggedInMainWindow(string? title = null)
        {
            var mainWindow = MainWindow(title);

            var menuBar = WarehouseMenuBar();
            Application.Top.Add(menuBar);

            return mainWindow;
        }

        public static Label AnnounceLabel(string labelMessage = "")
        {
            var announceLabel = new Label(labelMessage)
            {
                X = 1,
                Y = Pos.AnchorEnd(1)
            };
            return announceLabel;
        }

        public static ColorScheme AnnounceLabelSuccessColor()
        {
            ColorScheme color = new ColorScheme
            {
                Normal = Application.Driver.MakeAttribute(Color.BrightGreen, MainWindow().ColorScheme.Normal.Background),
            };
            return color;
        }

        public static ColorScheme AnnounceLabelErrorColor()
        {
            ColorScheme color = new ColorScheme
            {
                Normal = Application.Driver.MakeAttribute(Color.BrightRed, MainWindow().ColorScheme.Normal.Background),
            };
            return color;
        }

        /*
        Hiển thị tên người dùng và quyền
        */
        public static Label UserPermissionLabel()
        {
            string username = $"{Program.Warehouse.Username}";
            string permission = UIComponentLogic.PermissionName();

            string text = $"{username} - {permission}";
            var userPermissionLabel = new Label(text)
            {
                X = Pos.AnchorEnd(text.Length + 1),
                Y = Pos.AnchorEnd(1)
            };

            return userPermissionLabel;
        }

        //Đường kẻ trên UserPermissionLabel và ErrorMessageLabel
        public static LineView SeparatorLine()
        {
            var separatorLine = new LineView()
            {
                X = 0,
                Y = Pos.AnchorEnd(2),
                Width = Dim.Fill()
            };

            return separatorLine;
        }

        // Bảng để dùng trong các menu hiển thị
        public static TableView Table(DataTable dataTable)
        {
            var tableView = new TableView()
            {
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                FullRowSelect = true
            };
            tableView.Style.AlwaysShowHeaders = true;
            tableView.Style.ExpandLastColumn = true;
            tableView.Style.ShowHorizontalHeaderOverline = false;
            tableView.Table = dataTable;

            return tableView;
        }

        // Nút xóa trong các menu hiển thị
        public static Button DeleteButton()
        {
            var deleteButton = new Button("Delete")
            {
                X = 3,
                Y = Pos.AnchorEnd(4)
            };
            return deleteButton;
        }

        // Nút thêm trong các menu hiển thị
        public static Button AddButton(string addPrompt)
        {
            var addButton = new Button(addPrompt)
            {
                X = Pos.AnchorEnd(addPrompt.Length + 7),
                Y = Pos.AnchorEnd(4)
            };
            return addButton;
        }


        // để dùng trong các menu hiện
        public static Label SearchLabel()
        {
            string text = "Search:";
            var searchLabel = new Label(text)
            {
                X = Pos.Percent(50),
                Y = 1,
            };

            return searchLabel;
        }

        // để dùng trong các menu hiện
        public static TextField SearchInput()
        {
            string text = "Search:";
            var searchInput = new TextField()
            {
                X = Pos.Percent(50) + text.Length + 1,
                Y = 1,
                Width = Dim.Fill(3)
            };

            return searchInput;
        }


        // load lại dữ liệu trong menu hiện
        public static Button RefreshButton()
        {
            var refreshButton = new Button("Refresh")
            {
                X = 3,
                Y = 1
            };

            return refreshButton;
        }

        public static SaveDialog ExportToExcelDialog(DataTable dataTable)
        {
            var saveDialog = new SaveDialog("Export to Excel", "Choose export location");
            Application.Run(saveDialog);
            if (!saveDialog.Canceled && saveDialog.DirectoryPath != null)
            {
                string selectedPath = $"{saveDialog.FilePath}";
                UIComponentLogic.ExportToExcel(dataTable, selectedPath);
                MessageBox.Query("Export", $" Successfully exported {saveDialog.FileName} to {saveDialog.DirectoryPath} ", "OK");
            }
            return saveDialog;
        }
    }
}