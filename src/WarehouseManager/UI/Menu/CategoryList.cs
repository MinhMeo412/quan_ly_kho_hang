using System.Data;
using Terminal.Gui;
using WarehouseManager.UI.Utility;
using WarehouseManager.Core;

namespace WarehouseManager.UI.Menu
{
    public static class CategoryList
    {
        public static void Display()
        {

            Application.Top.RemoveAll();
            var mainWindow = UIComponent.LoggedInMainWindow("Categories");
            Application.Top.Add(mainWindow);

            var errorLabel = UIComponent.ErrorMessageLabel("Error Message Here");

            var userPermissionLabel = UIComponent.UserPermissionLabel();

            var separatorLine = UIComponent.SeparatorLine();

            var refreshButton = UIComponent.RefreshButton();

            var searchLabel = UIComponent.SearchLabel();

            var searchInput = UIComponent.SearchInput();

            var deleteButton = UIComponent.DeleteButton();

            var addButton = UIComponent.AddButton("Add New Category");

            var tableContainer = new FrameView()
            {
                X = 1,
                Y = Pos.Bottom(searchLabel) + 1,
                Width = Dim.Fill(1),
                Height = Dim.Fill(4),
                Border = new Border() { BorderStyle = BorderStyle.None }
            };

            // Create a TableView and set its data source
            var tableView = UIComponent.Table(CategoryListLogic.GetSortedCategoryList());

            int columnCurrentlySortBy = -1;
            bool sortColumnInDescendingOrder = false;
            
            refreshButton.Clicked += () =>
            {

            };

            searchInput.TextChanged += args =>
            {
                if (searchInput.Text == "")
                {
                    tableView.Table = CategoryListLogic.GetSortedCategoryList(columnCurrentlySortBy, sortColumnInDescendingOrder);
                }
                else
                {
                    tableView.Table = CategoryListLogic.GetSearchedCategory($"{searchInput.Text}");
                }
            };

            // khi bấm vào cột
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

                    tableView.Table = CategoryListLogic.GetSortedCategoryList(columnCurrentlySortBy, sortColumnInDescendingOrder);

                    int direction = 0;
                    if (!sortColumnInDescendingOrder)
                    {
                        direction = 1;
                    }
                    else
                    {
                        direction = 2;
                    }
                    tableView.Table.Columns[columnClicked].ColumnName = CategoryListLogic.ShowCurrentSortingDirection(tableView.Table.Columns[columnClicked].ColumnName, direction);
                }
            };

            // khi bấm vào 1 ô trong bảng
            tableView.CellActivated += args =>
            {
                int column = args.Col;
                int row = args.Row;

                // Retrieve the current value of the cell
                var currentValue = tableView.Table.Rows[row][column].ToString();

                // Create a dialog box with an input field for editing the cell value
                var editDialog = new Dialog("Edit Cell")
                {
                    X = Pos.Center(),
                    Y = Pos.Center(),
                    Width = Dim.Percent(50),
                    Height = Dim.Percent(50)
                };

                var newValue = new TextView()
                {
                    X = Pos.Center(),
                    Y = Pos.Center(),
                    Width = Dim.Fill(),
                    Height = Dim.Fill(),
                    Text = currentValue
                };

                var cancelButton = new Button("Cancel");
                cancelButton.Clicked += () =>
                {
                    Application.RequestStop();
                };

                var okButton = new Button("OK", is_default: true);
                okButton.Clicked += () =>
                {
                    // Update the table with the new value
                    tableView.Table.Rows[row][column] = newValue.Text.ToString();

                    int categoryID = (int)tableView.Table.Rows[row][0];
                    string categoryName = $"{tableView.Table.Rows[row][1]}";
                    string categoryDescription = $"{tableView.Table.Rows[row][2]}";
                    CategoryListLogic.UpdateCategory(categoryID, categoryName, categoryDescription);

                    Application.RequestStop();
                };

                editDialog.Add(newValue);
                editDialog.AddButton(cancelButton);
                editDialog.AddButton(okButton);

                if (column != 0)
                {
                    Application.Run(editDialog);
                }
            };

            deleteButton.Clicked += () =>
            {
                // khi nút Delete được bấm

                DataRow selectedRow = tableView.Table.Rows[tableView.SelectedRow];
                int categoryID = (int)selectedRow[0];

                int result = MessageBox.Query("Delete", "Are you sure you want to delete this item?", "No", "Yes");
                if (result == 1) // "Yes" button was pressed
                {
                    CategoryListLogic.DeleteCategory(categoryID);
                    if (searchInput.Text == "")
                    {
                        tableView.Table = CategoryListLogic.GetSortedCategoryList(columnCurrentlySortBy, sortColumnInDescendingOrder);
                    }
                    else
                    {
                        tableView.Table = CategoryListLogic.GetSearchedCategory($"{searchInput.Text}");
                    }
                }
            };

            addButton.Clicked += () =>
            {
                // khi nút category đc click
                AddCategory.Display();
            };

            tableContainer.Add(tableView);
            mainWindow.Add(refreshButton, searchLabel, searchInput, tableContainer, addButton, deleteButton, errorLabel, userPermissionLabel, separatorLine);
        }
    }
}