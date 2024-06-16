using WarehouseManager.Data.Entity;
using WarehouseManager.Data.Utility;

namespace WarehouseManager.Data.Table
{
    class CategoryTable
    {
        public List<Category>? Categories { get; private set; }

        public void Load(string connectionString, string token)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token}
            };

            List<List<object?>> rawCategories = Procedure.ExecuteReader(connectionString, "read_category", inParameters);

            List<Category> categories = new List<Category>();
            foreach (List<object?> rawCategory in rawCategories)
            {
                Category category = new Category(
                    (int)(rawCategory[0] ?? 0),
                    (string)(rawCategory[1] ?? ""),
                    (string?)rawCategory[2]
                );
                categories.Add(category);
            }

            this.Categories = categories;
        }

        public void Add(string connectionString, string token, int categoryID, string categoryName, string? categoryDescription)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_category_id", categoryID},
                {"input_category_name", categoryName},
                {"input_category_description", categoryDescription}
            };
            Procedure.ExecuteNonQuery(connectionString, "create_category", inParameters);

            Category category = new Category(categoryID, categoryName, categoryDescription);

            this.Categories ??= new List<Category>();
            this.Categories.Add(category);
        }

        public void Update(string connectionString, string token, int categoryID, string categoryName, string? categoryDescription)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_category_id", categoryID},
                {"input_category_name", categoryName},
                {"input_category_description", categoryDescription}
            };

            Procedure.ExecuteNonQuery(connectionString, "update_category", inParameters);

            var category = this.Categories?.FirstOrDefault(c => c.CategoryID == categoryID);
            if (category != null)
            {
                category.CategoryName = categoryName;
                category.CategoryDescription = categoryDescription;
            }
        }

        public void Delete(string connectionString, string token, int categoryID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_category_id", categoryID}
            };

            Procedure.ExecuteNonQuery(connectionString, "delete_category", inParameters);

            var category = this.Categories?.FirstOrDefault(c => c.CategoryID == categoryID);
            if (category != null)
            {
                this.Categories ??= new List<Category>();
                this.Categories.Remove(category);
            }
        }
    }
}
