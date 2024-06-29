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

            var searchContainer = new FrameView()
            {
                X = Pos.Center(),
                Width = Dim.Percent(75),
                Height = 3,
                Border = new Border() { BorderStyle = BorderStyle.None }
            };

            var searchLabel = new Label("Enter search term:")
            {
                X = 1,
                Y = 1
            };

            var searchInput = new TextField("")
            {
                X = Pos.Right(searchLabel) + 1,
                Y = Pos.Top(searchLabel),
                Width = Dim.Fill(12)
            };

            var clearButton = new Button("Clear")
            {
                X = Pos.AnchorEnd(11),
                Y = Pos.Top(searchLabel)
            };

            var deleteButton = UIComponent.DeleteButton();

            var addButton = UIComponent.AddButton("Add New Category");

            var tableContainer = new FrameView()
            {
                X = 1,
                Y = Pos.Bottom(searchContainer),
                Width = Dim.Fill(1),
                Height = Dim.Fill(4),
                Border = new Border() { BorderStyle = BorderStyle.None }
            };

            // Create a TableView and set its data source
            var tableView = UIComponent.Table(CategoryListLogic.GetSortedCategoryList());
            int columnCurrentlySortBy = -1;
            bool sortColumnInDescendingOrder = false;

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

            clearButton.Clicked += () =>
            {
                // khi nút category đc click
                searchInput.Text = "";
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
                    Application.RequestStop();
                };

                editDialog.Add(newValue);
                editDialog.AddButton(cancelButton);
                editDialog.AddButton(okButton);
                Application.Run(editDialog);
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
            searchContainer.Add(searchLabel, searchInput, clearButton);
            mainWindow.Add(searchContainer, tableContainer, addButton, deleteButton, errorLabel, userPermissionLabel, separatorLine);
        }
    }
}