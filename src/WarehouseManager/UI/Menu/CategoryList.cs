using System.Data;
using Terminal.Gui;
using WarehouseManager.UI.Utility;

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

            var userPermissionLabel = UIComponent.UserPermissionLabel("Username", "Permission");

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

            var searchButton = new Button("Search")
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

            var dataTable = new DataTable();
            dataTable.Columns.Add("ID", typeof(int));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Age", typeof(int));
            dataTable.Rows.Add(1, "Alice", 30);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(3, "Charlie", 35);
            dataTable.Rows.Add(1, "Alice", 30);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(3, "Charlie", 35);
            dataTable.Rows.Add(1, "Alice", 30);
            dataTable.Rows.Add(1, "Alice", 30);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(3, "Charlie", 35);
            dataTable.Rows.Add(1, "Alice", 30);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(3, "Charlie", 35);
            dataTable.Rows.Add(1, "Alice", 30);
            dataTable.Rows.Add(1, "Alice", 30);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(3, "Charlie", 35);
            dataTable.Rows.Add(1, "Alice", 30);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(3, "Charlie", 35);
            dataTable.Rows.Add(1, "Alice", 30);
            dataTable.Rows.Add(1, "Alice", 30);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(3, "Charlie", 35);
            dataTable.Rows.Add(1, "Alice", 30);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(3, "Charlie", 35);
            dataTable.Rows.Add(1, "Alice", 30);
            dataTable.Rows.Add(1, "Alice", 30);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(3, "Charlie", 35);
            dataTable.Rows.Add(1, "Alice", 30);
            dataTable.Rows.Add(2, "Bob", 25);
            dataTable.Rows.Add(3, "Charlie", 35);
            dataTable.Rows.Add(1, "Alice", 30);


            // Create a TableView and set its data source
            var tableView = UIComponent.Table(dataTable);

            searchButton.Clicked += () =>
            {
                // khi nút save được bấm
                MessageBox.Query("Search", $"Query: {searchInput.Text}", "OK");
            };

            // khi bấm vào cột
            tableView.MouseClick += e =>
            {
                tableView.ScreenToCell(e.MouseEvent.X, e.MouseEvent.Y, out DataColumn clickedCol);
                if (clickedCol != null)
                {
                    MessageBox.Query("Column Clicked", $"Column: {clickedCol}", "OK");
                }
            };

            // khi bấm vào 1 ô trong bảng
            tableView.CellActivated += args =>
            {
                int column = args.Col;
                int row = args.Row;
                MessageBox.Query("Cell Clicked", $"Column: {column}, Row: {row}", "OK");
            };

            deleteButton.Clicked += () =>
            {
                // khi nút Delete được bấm
                MessageBox.Query("Delete", $"Row: {tableView.SelectedRow}", "OK");
            };

            addButton.Clicked += () =>
            {
                // khi nút category đc click
                AddCategory.Display();
            };

            tableContainer.Add(tableView);
            searchContainer.Add(searchLabel, searchInput, searchButton);
            mainWindow.Add(searchContainer, tableContainer, addButton, deleteButton, errorLabel, userPermissionLabel);
        }
    }
}