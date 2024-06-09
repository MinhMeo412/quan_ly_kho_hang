namespace WarehouseManager.Data.Entity
{
    class User
    {
        public int UserID { get; set; }
        public string UserName { get; set; } = "";
        public string UserPassword { get; set; } = "";
        public string? UserFullName { get; set; }
        public string? UserEmail { get; set; }
        public string? UserPhoneNumber { get; set; }
        public int PermissionLevel { get; set; }
    }
}