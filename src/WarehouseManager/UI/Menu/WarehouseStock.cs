using System.Data;
using Terminal.Gui;
using WarehouseManager.UI.Utility;
using WarehouseManager.Core;

namespace WarehouseManager.UI.Menu
{
    public static class WarehouseStock
    {
        private static CancellationTokenSource? _cancellationTokenSource;
        private static Task? _sortingTask;


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
                X = 2,
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


            Dictionary<CheckBox, int> warehouseChecklistDict = WarehouseStockLogic.GetWarehouseChecklistDict();
            var tableView = UIComponent.Table(WarehouseStockLogic.GetData(warehouseChecklistDict));

            // Khi người dùng bấm nút select warehouse 
            warehouseCheckList.Clicked += () =>
            {
                int widestWarehouseName = warehouseChecklistDict.Keys.Max(w => w.Text.Length);

                var dialog = new Dialog("Warehouses")
                {
                    X = Pos.Center(),
                    Y = Pos.Center(),
                    Width = widestWarehouseName + 8,
                    Height = Dim.Percent(50)
                };

                var scrollView = new ScrollView()
                {
                    X = 1,
                    Y = 1,
                    Width = Dim.Fill(1),
                    Height = Dim.Fill(2),
                    ShowVerticalScrollIndicator = false,
                    ShowHorizontalScrollIndicator = false,
                    ContentSize = new Size(widestWarehouseName + 2, warehouseChecklistDict.Count) // Adjust size based on content
                };

                int y = 0;
                foreach (var checkBox in warehouseChecklistDict.Keys)
                {
                    checkBox.Y = y++;
                    scrollView.Add(checkBox);
                }

                dialog.Add(scrollView);

                var ok = new Button("Ok", is_default: true)
                {
                    X = Pos.Center(),
                    Y = Pos.AnchorEnd(1)
                };
                ok.Clicked += () =>
                {
                    tableView.Table = WarehouseStockLogic.GetData(warehouseChecklistDict);
                    Application.RequestStop();
                };

                dialog.Add(ok);
                Application.Run(dialog);
            };


            // Phải dùng 1 thread khác cho sort của menu này vì nó quá chậm.
            searchInput.TextChanged += async args =>
            {
                // Cancel the previous sorting task if it's still running
                _cancellationTokenSource?.Cancel();

                // Create a new CancellationTokenSource for the current sorting task
                _cancellationTokenSource = new CancellationTokenSource();
                var token = _cancellationTokenSource.Token;

                // Capture the current search term
                string searchTerm = $"{searchInput.Text}";

                // Offload the sorting operation to a background task
                _sortingTask = Task.Run(() =>
                {
                    // Perform the sorting operation
                    var sortedTable = WarehouseStockLogic.SortWarehouseStockBySearchTerm(tableView.Table, searchTerm);

                    // If the task is not canceled, update the table on the UI thread
                    if (!token.IsCancellationRequested)
                    {
                        Application.MainLoop.Invoke(() =>
                        {
                            tableView.Table = sortedTable;
                        });
                    }
                }, token);

                try
                {
                    // Await the completion of the sorting task
                    await _sortingTask;
                }
                catch (TaskCanceledException)
                {
                    // Handle the cancellation if needed (optional)
                }
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

                    tableView.Table = WarehouseStockLogic.SortWarehouseStockByColumn(tableView.Table, columnClicked, sortColumnInDescendingOrder);
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