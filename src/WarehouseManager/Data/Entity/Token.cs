namespace WarehouseManager.Data.Entity
{
    class Token(string tokenUUID, int userID, DateTime tokenLastActivityTimeStamp)
    {
        public string TokenUUID { get; set; } = tokenUUID;
        public int UserID { get; set; } = userID;
        public DateTime TokenLastActivityTimeStamp { get; set; } = tokenLastActivityTimeStamp;
    }
}