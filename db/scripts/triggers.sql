
/*
Trigger to auto update warehouse stock when inbound shipment status is set to delivered
*/
DELIMITER //
CREATE TRIGGER tg_before_update_inbound_shipment_detail_delivered BEFORE UPDATE ON inbound_shipment_detail FOR EACH ROW
BEGIN
    update Product_Variant
    set stock_quantity = stock_quantity - new.quantity
    where variant_id = new.variant_id;

    UPDATE warehouse_stock
    set warehouse_stock_quantity = warehouse_stock_quantity + new.inbound_shipment_detail_amount
    WHERE warehouse_stock_
END //
DELIMITER ;

/*
 * Trigger to auto update warehouse stock when inbound shipment status reverted from Delivered to undelivered
 */

 /*
 * Trigger to auto update warehouse stock before an outbound shipment is created
 */

 /*
 * Trigger to auto update warehouse stock before an outbound shipment detail amount is modified
 */

 /*
 * Trigger to auto update warehouse stock before a stock transfer order is created
 */

 /*
 * Trigger to auto update warehouse stock before a stock transfer detail amount is modified
 */