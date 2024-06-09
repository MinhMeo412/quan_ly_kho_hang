namespace WarehouseManager.Data.Entities
{
    class Token
    {
        public string TokenUUID { get; set; } = "";
        public int TokenUserID { get; set; }
        public DateTime TokenLastActivityTimeStamp { get; set; }
    }
}