namespace WarehouseManager.Data.Entities
{
    class User
    {
        public int UserID { get; set; }
        public string UserName { get; set; } = "";
        public string UserPassword { get; set; } = "";
        public string? UserFullName { get; set; }
        public string? UserEmail { get; set; }
        public string? UserPhoneNumber { get; set; }
        public int UserPermissionLevel { get; set; }
    }
}