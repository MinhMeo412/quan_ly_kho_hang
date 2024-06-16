using WarehouseManager.Data;
using WarehouseManager.Data.Entity;

class Program
{
    public static void Main(String[] args)
    {
        // Ví dụ cách tạo:

        // Tạo 1 thực thể database
        WarehouseDatabase warehouse = new WarehouseDatabase("localhost", "root", "7777", "warehouse");

        // Đăng nhập
        warehouse.Login("admin", "1234");

        // Tải dữ liệu database vào C#
        warehouse.Initialize();





        // Ví dụ cách dùng:

        // Thêm vào bảng category
        warehouse.AddCategory(1001, "Thể loại mới 1", "Mô tả 1");
        warehouse.AddCategory(1002, "Thể loại mới 2", "Mô tả 2");
        warehouse.AddCategory(1003, "Thể loại mới 3", "Mô tả 3");

        // Sửa trong bảng category
        warehouse.UpdateCategory(1001, "Thể loại mới 1", "3 người bạn chuối mặc pyjama");

        // Xóa trong bảng category
        warehouse.DeleteCategory(1002);


        // Lấy về danh sách category
        List<Category>? categories = warehouse.GetCategories();

        // In danh sách category
        foreach (Category category in categories)
        {
            Console.WriteLine($"Category ID: {category.CategoryID}, Category Name: {category.CategoryName}");
        }
    }

}