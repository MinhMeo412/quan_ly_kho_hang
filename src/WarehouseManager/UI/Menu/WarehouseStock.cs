using System.Data;
using Terminal.Gui;
using WarehouseManager.UI.Utility;
using WarehouseManager.Core;

namespace WarehouseManager.UI.Menu
{
    public static class WarehouseStock
    {

        public static void Display()
        {

            Application.Top.RemoveAll();
            var mainWindow = UIComponent.LoggedInMainWindow("Warehouse Stock");
            Application.Top.Add(mainWindow);

            var errorLabel = UIComponent.ErrorMessageLabel();

            var userPermissionLabel = UIComponent.UserPermissionLabel();

            var separatorLine = UIComponent.SeparatorLine();

            var warehouseCheckList = new Button("Select Warehouse")
            {
                X = 1,
                Y = 1
            };

            var searchLabel = UIComponent.SearchLabel();

            var searchInput = UIComponent.SearchInput();

            var exportButton = UIComponent.AddButton("Export to Excel");

            var tableContainer = new FrameView()
            {
                X = 1,
                Y = Pos.Bottom(searchLabel) + 1,
                Width = Dim.Fill(1),
                Height = Dim.Fill(4),
                Border = new Border() { BorderStyle = BorderStyle.None }
            };

            // Create a TableView and set its data source
            var tableView = UIComponent.Table(CategoryListLogic.GetData());

            // Khi người dùng bấm nút select warehouse 
            warehouseCheckList.Clicked += () =>
            {
                var dialog = new Dialog("Warehouses")
                {
                    X = Pos.Center(),
                    Y = Pos.Center(),
                    Width = Dim.Percent(50),
                    Height = Dim.Percent(50)
                };
                var checkList = new List<CheckBox>
                {
                new CheckBox(1, 1, "Warehouse 1"),
                new CheckBox(1, 2, "Warehouse 2"),
                new CheckBox(1, 3, "Warehouse 3")
                };
                foreach (var checkBox in checkList)
                {
                    dialog.Add(checkBox);
                }
                var ok = new Button("Ok", is_default: true);
                ok.Clicked += () =>
                {
                    List<string> selectedWarehouses = new List<string>();
                    foreach (var checkBox in checkList)
                    {
                        if (checkBox.Checked)
                        {
                            selectedWarehouses.Add($"{checkBox.Text}");
                        }
                    }
                    // Print the selected warehouses or do something with the list
                    MessageBox.Query("Selected Warehouses", string.Join("\n", selectedWarehouses), "Ok");
                    Application.RequestStop();
                };
                dialog.AddButton(ok);
                Application.Run(dialog);
            };

            // Khi người dùng search một chuỗi gì đó
            searchInput.TextChanged += args =>
            {
                tableView.Table = CategoryListLogic.SortCategoryBySearchTerm(tableView.Table, $"{searchInput.Text}"); ;
            };

            int columnCurrentlySortBy = -1;
            bool sortColumnInDescendingOrder = false;
            // khi sort cột
            tableView.MouseClick += e =>
            {
                tableView.ScreenToCell(e.MouseEvent.X, e.MouseEvent.Y, out DataColumn clickedCol);
                if (clickedCol != null)
                {
                    int columnClicked = tableView.Table.Columns.IndexOf(clickedCol);
                    if (columnClicked == columnCurrentlySortBy)
                    {
                        sortColumnInDescendingOrder = !sortColumnInDescendingOrder;
                    }

                    columnCurrentlySortBy = columnClicked;
                    searchInput.Text = "";

                    tableView.Table = CategoryListLogic.SortCategoryByColumn(tableView.Table, columnClicked, sortColumnInDescendingOrder);
                }
            };

            exportButton.Clicked += () =>
            {
                // khi nút category đc click
                Display();
            };

            tableContainer.Add(tableView);
            mainWindow.Add(warehouseCheckList, searchLabel, searchInput, tableContainer, exportButton, errorLabel, userPermissionLabel, separatorLine);
        }

    }


}