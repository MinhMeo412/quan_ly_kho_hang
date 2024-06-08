public class InboundShipment
{
    public int InboundShipmentID {get;set;}
    public int SuplierID {get;set;}
    public int WarehouseID {get;set;}
    public DateTime InboundShipmentStartingDate {get;set;}
    public string InboundShipmentStatus {get;set;}
    public string InboundShipmentDescription {get;set;}
    public int UserID {get;set;}
}