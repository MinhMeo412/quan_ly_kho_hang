using System.Data;
using Terminal.Gui;
using WarehouseManager.UI.Utility;
using WarehouseManager.Core.Pages;
using WarehouseManager.Core.Utility;

namespace WarehouseManager.UI.Pages
{
    public static class ProductList
    {
        private static CancellationTokenSource? _cancellationTokenSource;
        private static Task? _sortingTask;

        public static void Display()
        {

            Application.Top.RemoveAll();
            var mainWindow = UIComponent.LoggedInMainWindow("Products");
            Application.Top.Add(mainWindow);

            var errorLabel = UIComponent.AnnounceLabel();

            var userPermissionLabel = UIComponent.UserPermissionLabel();

            var separatorLine = UIComponent.SeparatorLine();

            var refreshButton = UIComponent.RefreshButton();

            var searchLabel = UIComponent.SearchLabel();

            var searchInput = UIComponent.SearchInput();

            var deleteButton = UIComponent.DeleteButton();
            deleteButton.Visible = UIComponent.CanExecuteMenu(2);

            var addButton = UIComponent.AddButton("Add New Product");

            var exportImportFormButton = UIComponent.AddButton("Export Excel Import Form");
            exportImportFormButton.X = Pos.Left(addButton) - 29;

            var importVariantButton = UIComponent.AddButton("Import From Excel");
            importVariantButton.X = Pos.Left(exportImportFormButton) - 22;

            addButton.Visible = UIComponent.CanExecuteMenu(3);
            exportImportFormButton.Visible = UIComponent.CanExecuteMenu(3);
            importVariantButton.Visible = UIComponent.CanExecuteMenu(3);


            var tableContainer = new FrameView()
            {
                X = 3,
                Y = Pos.Bottom(searchLabel) + 2,
                Width = Dim.Fill(3),
                Height = Dim.Fill(6),
                Border = new Border() { BorderStyle = BorderStyle.None }
            };

            // Create a TableView and set its data source
            var tableView = UIComponent.Table(ProductListLogic.GetData());

            // Khi người dùng bấm nút refresh sẽ tải lại trang
            refreshButton.Clicked += () =>
            {
                Display();
            };

            // Khi người dùng search một chuỗi gì đó
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
                    var sortedTable = ProductListLogic.SortProductBySearchTerm(tableView.Table, $"{searchInput.Text}"); ;

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

                    tableView.Table = ProductListLogic.SortProductByColumn(tableView.Table, columnClicked, sortColumnInDescendingOrder);
                }
            };

            // khi bấm vào 1 ô trong bảng
            tableView.CellActivated += args =>
            {
                int productID = (int)tableView.Table.Rows[args.Row][0];
                EditProduct.Display(productID);
            };

            deleteButton.Clicked += () =>
            {
                // khi nút Delete được bấm
                DataRow selectedRow = tableView.Table.Rows[tableView.SelectedRow];
                int productID = (int)selectedRow[0];

                int result = MessageBox.Query("Delete", "Are you sure you want to delete this product?", "No", "Yes");
                if (result == 1) // "Yes" button was pressed
                {
                    try
                    {
                        tableView.Table = ProductListLogic.DeleteProduct(tableView.Table, productID);
                        errorLabel.Text = $"Successfully deleted product#{productID}";
                        errorLabel.ColorScheme = UIComponent.AnnounceLabelSuccessColor();
                    }
                    catch (Exception ex)
                    {
                        errorLabel.Text = $"Error: {ex.Message}";
                        errorLabel.ColorScheme = UIComponent.AnnounceLabelErrorColor();
                    }
                }
            };

            addButton.Clicked += () =>
            {
                AddProduct.Display();
            };

            exportImportFormButton.Clicked += () =>
            {
                try
                {
                    string? filePath = UIComponent.ChooseFileSaveLocation();
                    if (filePath != null)
                    {
                        DataTable productList = ProductListLogic.GetImportForm();
                        DataTable fileInformation = ProductListLogic.GetFileInformation();

                        filePath = Excel.GetExcelFileName(filePath);
                        Excel.Export(filePath, productList, "Product List", fileInformation, "File Information");

                        errorLabel.Text = $"Successfully exported import form to {filePath}";
                        errorLabel.ColorScheme = UIComponent.AnnounceLabelSuccessColor();
                    }
                }
                catch (Exception ex)
                {
                    errorLabel.Text = $"Error: {ex.Message}";
                    errorLabel.ColorScheme = UIComponent.AnnounceLabelErrorColor();
                }
            };

            importVariantButton.Clicked += () =>
            {
                string? filePath = UIComponent.OpenFile();
                if (filePath != null)
                {
                    try
                    {
                        ProductListLogic.ImportFromExcel(filePath);

                        errorLabel.Text = $"Successfully imported products from {filePath}";
                        errorLabel.ColorScheme = UIComponent.AnnounceLabelSuccessColor();
                    }
                    catch (Exception ex)
                    {
                        errorLabel.Text = $"Error: {ex.Message}";
                        errorLabel.ColorScheme = UIComponent.AnnounceLabelErrorColor();
                    }
                }
            };

            tableContainer.Add(tableView);
            mainWindow.Add(searchLabel, searchInput, tableContainer, addButton, deleteButton, errorLabel, userPermissionLabel, separatorLine, refreshButton, exportImportFormButton, importVariantButton);
        }
    }
}