namespace WarehouseManager.Data.Entity
{
    class User(int userID, string userName, string userPassword, string userFullName, string? userEmail, string? userPhoneNumber, int permissionLevel)
    {
        public int UserID { get; set; } = userID;
        public string UserName { get; set; } = userName;
        public string UserPassword { get; set; } = userPassword;
        public string UserFullName { get; set; } = userFullName;
        public string? UserEmail { get; set; } = userEmail;
        public string? UserPhoneNumber { get; set; } = userPhoneNumber;
        public int PermissionLevel { get; set; } = permissionLevel;
    }
}