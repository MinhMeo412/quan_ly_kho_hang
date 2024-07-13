using Terminal.Gui;
using WarehouseManager.UI.Utility;
using WarehouseManager.Core.Pages;

namespace WarehouseManager.UI.Pages
{
    public static class AddCategory
    {

        public static void Display()
        {
            Application.Top.RemoveAll();
            var mainWindow = UIComponent.LoggedInMainWindow("Add New Category");
            Application.Top.Add(mainWindow);

            var errorLabel = UIComponent.AnnounceLabel();
            var userPermissionLabel = UIComponent.UserPermissionLabel();
            var separatorLine = UIComponent.SeparatorLine();
            var refreshButton = UIComponent.RefreshButton();

            var infoContainer = new FrameView()
            {
                X = Pos.Center(),
                Y = Pos.Center(),
                Width = Dim.Percent(75),
                Height = Dim.Percent(75)
            };

            var categorynameLabel = new Label("Category Name:")
            {
                X = 3,
                Y = 1
            };

            var categoryNameInput = new TextField("")
            {
                X = Pos.Right(categorynameLabel) + 1,
                Y = Pos.Top(categorynameLabel),
                Width = Dim.Fill(3),
            };

            var descriptionLabel = new Label("Description:")
            {
                X = 3,
                Y = Pos.Bottom(categorynameLabel) + 2
            };

            var descriptionInput = new TextView()
            {
                X = Pos.Left(descriptionLabel),
                Y = Pos.Bottom(descriptionLabel) + 1,
                Width = Dim.Fill(3),
                Height = Dim.Fill(3),
                Text = "",
            };

            var saveButton = new Button("Save", is_default: true)
            {
                X = Pos.Center(),
                Y = Pos.Bottom(descriptionInput) + 1
            };

            refreshButton.Text = "To category list";
            refreshButton.Clicked += () =>
            {
                CategoryList.Display();
            };

            saveButton.Clicked += () =>
            {
                try
                {
                    AddCategoryLogic.AddCategory($"{categoryNameInput.Text}", $"{descriptionInput.Text}");

                    errorLabel.Text = $"Category created successfully!";
                    errorLabel.ColorScheme = UIComponent.AnnounceLabelSuccessColor();

                    categoryNameInput.Text = "";
                    descriptionInput.Text = "";
                }
                catch (Exception ex)
                {
                    errorLabel.Text = $"Error: {ex.Message}";
                    errorLabel.ColorScheme = UIComponent.AnnounceLabelErrorColor();
                }
            };

            infoContainer.Add(categorynameLabel, descriptionLabel, categoryNameInput, descriptionInput, saveButton);
            mainWindow.Add(infoContainer, errorLabel, userPermissionLabel, separatorLine, refreshButton);
        }

    }
}