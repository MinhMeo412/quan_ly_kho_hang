namespace WarehouseManager.Data.Entity
{
    class Permission(int permissionLevel, string? permissionName, string? permissionDescription)
    {
        public int PermissionLevel { get; set; } = permissionLevel;
        public string? PermissionName { get; set; } = permissionName;
        public string? PermissionDescription { get; set; } = permissionDescription;
    }
}