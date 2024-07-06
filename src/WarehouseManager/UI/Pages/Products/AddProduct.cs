using Terminal.Gui;
using WarehouseManager.UI.Utility;
using WarehouseManager.Core.Pages;

namespace WarehouseManager.UI.Pages
{
    public static class AddProduct
    {
        public static void Display()
        {
            Application.Top.RemoveAll();
            var mainWindow = UIComponent.LoggedInMainWindow("Add New Product");
            Application.Top.Add(mainWindow);

            var errorLabel = UIComponent.ErrorMessageLabel();

            var userPermissionLabel = UIComponent.UserPermissionLabel();

            var separatorLine = UIComponent.SeparatorLine();

            var middleContainer = new FrameView()
            {
                X = 1,
                Y = 2,
                Width = Dim.Fill(1),
                Height = Dim.Fill(3),
                Border = new Border() { BorderStyle = BorderStyle.None }
            };

            var leftContainer = new FrameView()
            {
                Width = Dim.Percent(33),
                Height = Dim.Fill(3),
                Border = new Border() { BorderStyle = BorderStyle.None }
            };

            var rightContainer = new FrameView()
            {
                X = Pos.Right(leftContainer),
                Width = Dim.Fill(),
                Height = Dim.Fill(3),
                Border = new Border() { BorderStyle = BorderStyle.None }
            };

            var productNameLabel = new Label("Product Name:")
            {
                X = 2,
                Y = 1
            };

            var priceLabel = new Label("Price:")
            {
                X = 2,
                Y = Pos.Bottom(productNameLabel) + 1
            };

            var categoryLabel = new Label("Category:")
            {
                X = 2,
                Y = Pos.Bottom(priceLabel) + 1
            };

            var productNameInput = new TextField("")
            {
                X = Pos.Right(productNameLabel) + 1,
                Y = Pos.Top(productNameLabel),
                Width = Dim.Fill(1)
            };

            var priceInput = new TextField("")
            {
                X = Pos.Right(productNameLabel) + 1,
                Y = Pos.Top(priceLabel),
                Width = Dim.Fill(1)
            };

            var categoryDropDown = new ComboBox()
            {
                X = Pos.Right(productNameLabel) + 1,
                Y = Pos.Top(categoryLabel),
                Width = Dim.Fill(1),
                Height = Dim.Fill(1),
                ReadOnly = true
            };
            var categories = AddProductLogic.GetCategoryList();
            categoryDropDown.SetSource(categories);
            categoryDropDown.SelectedItem = 0;


            var descriptionLabel = new Label("Description:")
            {
                X = 2,
                Y = Pos.Bottom(categoryLabel) + 2
            };

            var descriptionInput = new TextView()
            {
                X = 2,
                Y = Pos.Bottom(descriptionLabel) + 1,
                Width = Dim.Fill(1),
                Height = Dim.Fill(1),
                Text = ""
            };

            var saveButton = new Button("Save")
            {
                X = Pos.Center(),
                Y = Pos.Bottom(rightContainer) + 1
            };

            // Create a TableView and set its data source
            var tableView = UIComponent.Table(AddProductLogic.GetDataTable());
            tableView.Height = Dim.Fill(5);
            tableView.Width = Dim.Fill(2);
            tableView.X = 1;
            tableView.Y = 1;

            var deleteButton = new Button("Delete")
            {
                X = 1,
                Y = Pos.Bottom(tableView)
            };

            var addVariantButton = new Button("Add variant")
            {
                Y = Pos.AnchorEnd(2)
            };
            addVariantButton.X = Pos.AnchorEnd(addVariantButton.Text.Length + 6);

            var variantInputContainer = new FrameView()
            {
                X = 1,
                Y = Pos.Bottom(deleteButton),
                Width = Dim.Fill(addVariantButton.Text.Length + 6),
                Height = Dim.Fill(),
                Border = new Border() { BorderStyle = BorderStyle.None }
            };

            var imageURLInput = new TextField("")
            {
                X = 1,
                Y = Pos.AnchorEnd(2),
                Width = Dim.Percent(33)
            };

            var colorInput = new TextField("")
            {
                X = Pos.Right(imageURLInput) + 1,
                Y = Pos.AnchorEnd(2),
                Width = Dim.Percent(32)
            };

            var sizeInput = new TextField("")
            {
                X = Pos.Right(colorInput) + 1,
                Y = Pos.AnchorEnd(2),
                Width = Dim.Fill(1)
            };

            var imageURLLabel = new Label("Image URL:")
            {
                X = Pos.Left(imageURLInput),
                Y = Pos.Top(imageURLInput) - 1
            };

            var colorLabel = new Label("Color:")
            {
                X = Pos.Left(colorInput),
                Y = Pos.Top(colorInput) - 1
            };

            var sizeLabel = new Label("Size:")
            {
                X = Pos.Left(sizeInput),
                Y = Pos.Top(sizeInput) - 1
            };

            addVariantButton.Clicked += () =>
            {
                if (imageURLInput.Text != "" || colorInput.Text != "" || sizeInput.Text != "")
                {
                    tableView.Table = AddProductLogic.AddVariantToDataTable(tableView.Table, $"{imageURLInput.Text}", $"{colorInput.Text}", $"{sizeInput.Text}");
                    imageURLInput.Text = "";
                    colorInput.Text = "";
                    sizeInput.Text = "";
                }
            };

            deleteButton.Clicked += () =>
            {
                tableView.Table = AddProductLogic.DeleteVariantFromDataTable(tableView.Table, tableView.SelectedRow);
            };

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

            saveButton.Clicked += () =>
            {
                // khi nút Save được bấm
                try
                {
                    AddProductLogic.Save(
                        productName: $"{productNameInput.Text}",
                        productDescription: $"{descriptionInput.Text}",
                        productPrice: int.Parse($"{priceInput.Text}"),
                        categoryName: $"{categoryDropDown.Text}",
                        variantDataTable: tableView.Table
                    );

                    MessageBox.Query("Success", $"Product created successfully.", "OK");
                    Display();
                }
                catch (Exception ex)
                {
                    errorLabel.Text = $"Error: {ex.Message}";
                }
            };

            variantInputContainer.Add(imageURLInput, colorInput, sizeInput, imageURLLabel, colorLabel, sizeLabel);
            leftContainer.Add(productNameLabel, priceLabel, categoryLabel, descriptionLabel, productNameInput, priceInput, categoryDropDown, descriptionInput);
            rightContainer.Add(tableView, deleteButton, variantInputContainer, addVariantButton);
            middleContainer.Add(leftContainer, rightContainer, saveButton);

            mainWindow.Add(middleContainer, separatorLine, errorLabel, userPermissionLabel);
        }

    }
}