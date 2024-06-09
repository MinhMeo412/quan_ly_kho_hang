namespace WarehouseManager.Data.Entity
{
    class Token
    {
        public string TokenUUID { get; set; } = "";
        public int UserID { get; set; }
        public DateTime TokenLastActivityTimeStamp { get; set; }
    }
}