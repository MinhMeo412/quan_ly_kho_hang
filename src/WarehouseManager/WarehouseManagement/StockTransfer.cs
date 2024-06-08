public class StockTransfer
{
    public int StockTransferID {get;set;}
    public int StockTransferFromWarehouseID {get;set;}
    public int StockTransferToWarehouseID {get;set;}
    public DateTime StockTransferStartingDate {get;set;}
    public string StockTransferStatus {get;set;}
    public string StockTransferDescription {get;set;}
    public int UserID {get;set;}
}