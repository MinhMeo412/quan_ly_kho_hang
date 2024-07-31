using System.Data;
using Terminal.Gui;
using WarehouseManager.Core.Pages;
using WarehouseManager.UI.Utility;

namespace WarehouseManager.UI.Pages
{
    public static class AddInventoryAudit
    {
        public static void Display()
        {
            Application.Top.RemoveAll();
            var mainWindow = UIComponent.LoggedInMainWindow("Add New Inventory Audit");
            Application.Top.Add(mainWindow);

            var errorLabel = UIComponent.AnnounceLabel();

            var userPermissionLabel = UIComponent.UserPermissionLabel();

            var separatorLine = UIComponent.SeparatorLine();


            var returnButton = new Button("Back")
            {
                X = 3,
                Y = 1
            };

            var leftContainer = new FrameView()
            {
                X = 3,
                Y = 2,
                Width = Dim.Percent(25),
                Height = Dim.Fill(5),
                Border = new Border() { BorderStyle = BorderStyle.None }
            };

            var rightContainer = new FrameView()
            {
                X = Pos.Right(leftContainer) + 1,
                Y = Pos.Top(leftContainer),
                Width = Dim.Fill(3),
                Height = Dim.Fill(5),
                Border = new Border() { BorderStyle = BorderStyle.None }
            };


            var saveButton = new Button("Save", is_default: true)
            {
                X = Pos.Center(),
                Y = Pos.Bottom(rightContainer) + 1
            };

            var warehouseLabel = new Label("Warehouse:")
            {
                X = 0,
                Y = 1
            };

            var warehouseDropDown = new ComboBox()
            {
                X = Pos.Left(warehouseLabel),
                Y = Pos.Bottom(warehouseLabel),
                Width = Dim.Fill(1),
                Height = Dim.Fill(),
                ReadOnly = true,
                SelectedItem = 0
            };


            var descriptionLabel = new Label("Description:")
            {
                X = Pos.Left(warehouseLabel),
                Y = Pos.Bottom(warehouseLabel) + 2
            };

            var descriptionInput = new TextView()
            {
                X = Pos.Left(descriptionLabel),
                Y = Pos.Bottom(descriptionLabel),
                Width = Dim.Fill(1),
                Height = Dim.Fill(),
                Text = ""
            };

            var variantIDLabel = new Label("Product")
            {
                X = 1,
                Y = 1,
            };

            var variantIDInput = new TextField()
            {
                X = Pos.Left(variantIDLabel),
                Y = Pos.Bottom(variantIDLabel),
                Width = 7
            };

            var variantNameDropDown = new ComboBox()
            {
                X = Pos.Right(variantIDInput) + 1,
                Y = Pos.Top(variantIDInput),
                Width = Dim.Percent(25),
                Height = Dim.Fill(),
                ReadOnly = true,
                SelectedItem = 0
            };

            var quantityInput = new TextField()
            {
                X = Pos.Right(variantNameDropDown) + 1,
                Y = Pos.Top(variantNameDropDown),
                Width = 8
            };

            var quantityLabel = new Label("Quantity")
            {
                X = Pos.Left(quantityInput),
                Y = Pos.Top(quantityInput) - 1,
            };


            var addBUtton = new Button("Add")
            {
                X = Pos.Right(quantityInput) + 1,
                Y = Pos.Top(quantityInput)
            };


            var searchLabel = new Label("Search")
            {
                X = Pos.Right(quantityLabel) + 32,
                Y = 1,
            };

            var searchInput = new TextField()
            {
                X = Pos.Left(searchLabel),
                Y = Pos.Bottom(searchLabel),
                Width = Dim.Fill()
            };

            var tableView = UIComponent.Table(AddInventoryAuditLogic.GetDataTable());
            tableView.X = 1;
            tableView.Y = Pos.Bottom(variantIDInput) + 2;
            tableView.Height = Dim.Fill(2);

            var deleteButton = new Button("Delete")
            {
                X = Pos.Left(tableView),
                Y = Pos.Bottom(tableView) + 1
            };

            var getAllStockButton = new Button("Get All Stock")
            {
                X = Pos.AnchorEnd(17),
                Y = Pos.Top(deleteButton)
            };

            leftContainer.Add(warehouseLabel, descriptionLabel, warehouseDropDown, descriptionInput);
            rightContainer.Add(searchLabel, searchInput, variantIDLabel, variantIDInput, variantNameDropDown, quantityLabel, quantityInput, addBUtton, tableView, deleteButton, getAllStockButton);
            mainWindow.Add(returnButton, errorLabel, userPermissionLabel, separatorLine, leftContainer, rightContainer, saveButton);
        }
    }
}