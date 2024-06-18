using WarehouseManager.Data.Entity;
using WarehouseManager.Data.Utility;

namespace WarehouseManager.Data.Table
{
    class CategoryTable(string connectionString, string? token)
    {
        private string ConnectionString = connectionString;
        private string? Token = token;

        private List<Category>? _categories;
        public List<Category>? Categories
        {
            get
            {
                this.Load();
                return _categories;
            }
            private set
            {
                _categories = value;
            }
        }

        private void Load()
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token}
            };

            List<List<object?>> rawCategories = Procedure.ExecuteReader(this.ConnectionString, "read_category", inParameters);

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

        public void Add(int categoryID, string categoryName, string? categoryDescription)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_category_id", categoryID},
                {"input_category_name", categoryName},
                {"input_category_description", categoryDescription}
            };
            Procedure.ExecuteNonQuery(this.ConnectionString, "create_category", inParameters);
        }

        public void Update(int categoryID, string categoryName, string? categoryDescription)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_category_id", categoryID},
                {"input_category_name", categoryName},
                {"input_category_description", categoryDescription}
            };

            Procedure.ExecuteNonQuery(this.ConnectionString, "update_category", inParameters);
        }

        public void Delete(int categoryID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_category_id", categoryID}
            };

            Procedure.ExecuteNonQuery(this.ConnectionString, "delete_category", inParameters);
        }
    }
}
