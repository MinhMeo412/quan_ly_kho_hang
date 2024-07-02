/*
 * Trigger to auto update warehouse stock when inbound shipment status is set to Completed
 */
DELIMITER //
Create trigger update_warehouse_stock_after_inbound_shipment
After update on inbound_shipment
for each row
Begin
	if new.inbound_shipment_status = 'Completed' and old.inbound_shipment_status <> 'Completed' then
	-- Update stock for each variant
    update warehouse_stock ws
    join inbound_shipment_detail isd on ws.product_variant_id = isd.product_variant_id
    Set ws.warehouse_stock_quatity = ws.warehouse_stock_quantity + isd.inbound_shipment_detail_amount
	where isd.inbound_shipment_id = new.inbound_shipment_id
    and ws.warehouse_id = new.warehouse_id;
    
    -- Add product if not exits in stock
    insert into warehouse_stock (warehouse_id, product_variant_id, warehouse_stock_quantity)
    select new.warehouse_id, isd.product_variant_id, isd.inbound_shipment_detail_amount
    from inbound_shipment_detail isd
    left join warehouse_stock ws on ws.warehouse_id = new.warehouse_id and ws.product_variant_id = isd.product_variant_id
    where isd.inbound_shipment_id = new.inbound_shipment_id
    and ws.product_variant_id is null;
    end if;
End //
Delimiter ;
/*
 * Trigger to prevent update on inbound shipment when inbound shipment status is Completed
 */
DELIMITER //
CREATE TRIGGER prevent_update_completed_inbound_shipment
BEFORE UPDATE ON inbound_shipment
FOR EACH ROW
BEGIN
    IF OLD.inbound_shipment_status = 'Completed' THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Cannot update inbound shipment when status is Completed';
    END IF;
END//
DELIMITER ;
/*
 * Trigger to prevent update on inbound shipment detail when inbound shipment status is Completed
 */
DELIMITER //
CREATE TRIGGER prevent_update_completed_inbound_shipment_detail
BEFORE UPDATE ON inbound_shipment_detail
FOR EACH ROW
BEGIN
    DECLARE shipment_status VARCHAR(32);
    
    SELECT inbound_shipment_status INTO shipment_status
    FROM inbound_shipment
    WHERE inbound_shipment_id = NEW.inbound_shipment_id;
    
    IF shipment_status = 'Completed' THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Cannot update inbound shipment detail when shipment status is Completed';
    END IF;
END//
DELIMITER ;



/*
 * Trigger to auto update warehouse stock when outbound shipment status is set to Completed
 */
Delimiter //
create trigger update_warehouse_stock_after_outbound_shipment
before update on outbound_shipment
for each row
begin
	if new.outbound_shipment_status = 'Completed' and old.outbound_shipment_status <> 'Completed' then
		update warehouse_stock ws
        join outbound_shipment_detail osd on ws.warehouse_id = new.warehouse_id and ws.product_variand_id = osd.product_variant_id
        set ws.warehouse_stock_quatity = ws.warehouse_stock_quatity - osd.outbound_shipment_detail_amount
        where osd.outbound_shipment_id = new.outbound_shipment_id;
	end if;
end//
delimiter ;
/*
 * Trigger to prevent update on outbound shipment when outbound shipment status is Completed
 */
DELIMITER //
CREATE TRIGGER prevent_update_completed_outbound_shipment
BEFORE UPDATE ON outbound_shipment
FOR EACH ROW
BEGIN
    IF OLD.outbound_shipment_status = 'Completed' THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Cannot update outbound shipment when status is Completed';
    END IF;
END//
DELIMITER ;
/*
 * Trigger to prevent update on outbound shipment detail when outbound shipment status is Completed
 */
DELIMITER //
CREATE TRIGGER prevent_update_completed_outbound_shipment_detail
BEFORE UPDATE ON outbound_shipment_detail
FOR EACH ROW
BEGIN
    DECLARE shipment_status VARCHAR(32);
    
    SELECT outbound_shipment_status INTO shipment_status
    FROM outbound_shipment
    WHERE outbound_shipment_id = NEW.outbound_shipment_id;
    
    IF shipment_status = 'Completed' THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Cannot update outbound shipment detail when shipment status is Completed';
    END IF;
END//
DELIMITER ;



/*
 * Trigger to auto update warehouse stock when stock transfer status is set to Completed
 */
Delimiter //
create trigger update_warehouse_stock_after_stock_transfer
before update on stock_transfer
for each row
begin
	if new.stock_transfer_status = 'Completed' and old.stock_transfer_status <> 'Completed' then
		-- Update destination warehouse stock
		update warehouse_stock ws_dest
        join stock_transfer_detail std on ws_dest.warehouse_id = new.to_warehouse_id and ws_dest.product_variant_id = std.product_variant_id
        set ws_dest.warehouse_stock_quantity = ws_dest.warehouse_stock_quantity + std.stock_transfer_detail_amount
        where std.stock_transfer_id = new.stock_transfer_id;
        -- Update source warehouse stock
        update warehouse_stock ws_src
        join stock_transfer_detail std on ws_src.warehouse_id = new.from_warehouse_id and ws_src.product_variant_id = std.product_variant_id
        set ws_src.warehouse_stock_quantity = ws_src.warehouse_stock_quantity - std.stock_transfer_detail_amount
        where std.stock_transfer_id = new.stock_transfer_id;
	end if;
end//
delimiter ;
/*
 * Trigger to prevent update on stock transfer when stock transfer status is Completed
 */
DELIMITER //
CREATE TRIGGER prevent_update_completed_stock_transfer
BEFORE UPDATE ON stock_transfer
FOR EACH ROW
BEGIN
    IF OLD.stock_transfer_status = 'Completed' THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Cannot update stock transfer when status is Completed';
    END IF;
END//
DELIMITER ;
/*
 * Trigger to prevent update on stock transfer detail when stock transfer status is Completed
 */
DELIMITER //
CREATE TRIGGER prevent_update_completed_stock_transfer_detail
BEFORE UPDATE ON stock_transfer_detail
FOR EACH ROW
BEGIN
    DECLARE shipment_status VARCHAR(32);
    
    SELECT stock_transfer_status INTO shipment_status
    FROM stock_transfer
    WHERE stock_transfer_id = NEW.stock_transfer_id;
    
    IF shipment_status = 'Completed' THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Cannot update stock transfer detail when shipment status is Completed';
    END IF;
END//
DELIMITER ;
