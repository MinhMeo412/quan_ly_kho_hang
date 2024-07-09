using System.Data;
using Terminal.Gui;
using WarehouseManager.UI.Utility;
using WarehouseManager.Core.Pages;

namespace WarehouseManager.UI.Pages
{
    public static class AccountList
    {
        public static void Display()
        {

            Application.Top.RemoveAll();
            var mainWindow = UIComponent.LoggedInMainWindow("Accounts");
            Application.Top.Add(mainWindow);

            var errorLabel = UIComponent.ErrorMessageLabel();

            var userPermissionLabel = UIComponent.UserPermissionLabel();

            var separatorLine = UIComponent.SeparatorLine();

            var refreshButton = UIComponent.RefreshButton();

            var searchLabel = UIComponent.SearchLabel();

            var searchInput = UIComponent.SearchInput();

            var deleteButton = UIComponent.DeleteButton();

            var addButton = UIComponent.AddButton("Add New Account");

            var tableContainer = new FrameView()
            {
                X = 1,
                Y = Pos.Bottom(searchLabel) + 1,
                Width = Dim.Fill(1),
                Height = Dim.Fill(4),
                Border = new Border() { BorderStyle = BorderStyle.None }
            };

       
            var tableView = UIComponent.Table(UserListLogic.GetData());

           
            refreshButton.Clicked += () =>
            {
                Display();
            };

           
            searchInput.TextChanged += args =>
            {
                tableView.Table = UserListLogic.SortUserBySearchTerm(tableView.Table, $"{searchInput.Text}"); ;
            };

            int columnCurrentlySortBy = -1;
            bool sortColumnInDescendingOrder = false;
            
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

                    tableView.Table = UserListLogic.SortUserByColumn(tableView.Table, columnClicked, sortColumnInDescendingOrder);
                }
            };

           
            tableView.CellActivated += args =>
            {
                int column = args.Col;
                int row = args.Row;

                var currentValue = tableView.Table.Rows[row][column].ToString();

                
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
                    
                    tableView.Table.Rows[row][column] = newValue.Text.ToString();

                    int userID = (int)tableView.Table.Rows[row][0];
                    string userName = $"{tableView.Table.Rows[row][1]}";
                    string userPassword = $"{tableView.Table.Rows[row][2]}";
                    string userFullName = $"{tableView.Table.Rows[row][3]}";
                    string userEmail =$"{tableView.Table.Rows[row][4]}";
                    string userPhoneNumber = $"{tableView.Table.Rows[row][5]}";
                    int permissionLevel =(int)tableView.Table.Rows[row][6];


                    
                    try
                    {
                        UserListLogic.UpdateUser(userID, userName, userPassword,userFullName,userEmail,userPhoneNumber,permissionLevel);
                    }
                    catch (Exception ex)
                    {
                        //tableView.Table.Rows[row][column] = currentValue;
                        errorLabel.Text = $"Error: {ex.Message}";
                    }


                    Application.RequestStop();
                };

                editDialog.Add(newValue);
                editDialog.AddButton(cancelButton);
                editDialog.AddButton(okButton);

                
                if (column != 0 && column !=1)
                {
                    Application.Run(editDialog);
                }
            };

            deleteButton.Clicked += () =>
            {
                
                DataRow selectedRow = tableView.Table.Rows[tableView.SelectedRow];
                int userID = (int)selectedRow[0];

                int result = MessageBox.Query("Delete", "Are you sure you want to delete this item?", "No", "Yes");
                if (result == 1) 
                {
                    tableView.Table = UserListLogic.DeleteUser(tableView.Table, userID);
                }
            };

            addButton.Clicked += () =>
            {
                
                AddAccount.Display();
            };

            tableContainer.Add(tableView);
            mainWindow.Add(refreshButton, searchLabel, searchInput, tableContainer, addButton, deleteButton, errorLabel, userPermissionLabel, separatorLine);
        }
    }
}