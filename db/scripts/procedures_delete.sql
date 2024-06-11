DELIMITER //

CREATE PROCEDURE delete_supplier(IN input_token VARCHAR(36), IN target_supplier_id INT)
BEGIN
    DECLARE required_level INT DEFAULT 0;
    IF sufficient_permission(input_token, required_level) THEN
        DELETE FROM supplier WHERE supplier_id = target_supplier_id;
        IF ROW_COUNT() = 0 THEN
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Supplier not found.';
        END IF;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //


create procedure delete_product(in input_token VARCHAR(36),in target_product_id int)
begin 
declare required_level	int default 2;
if sufficient_permission(input_token, required_level) then
delete from product where product_id = target_product_id;
if ROW_COUNT() = 0 then 
signal sqlstate '45000' set message_text='Product not found,';
end if;
else 
signal sqlstate '45000' set message_text='Insufficient permission.';
end if ;
end//


create procedure delete_product_variant(in input_token VARCHAR(36),in target_product_id int)
begin 
declare required_level	int default 2;
if sufficient_permission(input_token, required_level) then
delete from product_variant where product_variant_id = target_variant_id;
if ROW_COUNT() = 0 then 
signal sqlstate '45000' set message_text='Product variant not found.';
end if;
else 
signal sqlstate '45000' set message_text='Insufficient permission.';
end if ;
end//

create procedure delete_category(in input_token VARCHAR(36),in target_product_id int)
begin 
declare required_level	int default 2;
if sufficient_permission(input_token, required_level) then
delete from category  where category_id  = target_category_id;
if ROW_COUNT() = 0 then 
signal sqlstate '45000' set message_text='Category  not found,';
end if;
else 
signal sqlstate '45000' set message_text='Insufficient permission.';
end if ;
end//

create procedure delete_warehouse(in input_token VARCHAR(36),in target_product_id int)
begin 
declare required_level	int default 1;
if sufficient_permission(input_token, required_level) then
delete from warehouse  where warehouse_id = target_warehouse_id;
if ROW_COUNT() = 0 then 
signal sqlstate '45000' set message_text='Warehouse not found.';
end if;
else 
signal sqlstate '45000' set message_text='Insufficient permission.';
end if ;
end//


create procedure delete_warehouse_address(in input_token VARCHAR(36),in target_product_id int)
begin 
declare required_level	int default 1;
if sufficient_permission(input_token, required_level) then
delete from warehouse_address   where warehouse_address_id  = target_address_id;
if ROW_COUNT() = 0 then 
signal sqlstate '45000' set message_text='Warehouse address not found.';
end if;
else 
signal sqlstate '45000' set message_text='Insufficient permission.';
end if ;
end//



create procedure delete_inventory_audit (in input_token VARCHAR(36),in target_product_id int)
begin 
declare required_level	int default 1;
if sufficient_permission(input_token, required_level) then
delete from inventory_audit    where inventory_audit_id   = target_audit_id;
if ROW_COUNT() = 0 then 
signal sqlstate '45000' set message_text='Inventory audit  not found.';
end if;
else 
signal sqlstate '45000' set message_text='Insufficient permission.';
end if ;
end//



create procedure delete_inbound_shipment (in input_token VARCHAR(36),in target_product_id int)
begin 
declare required_level	int default 1;
if sufficient_permission(input_token, required_level) then
delete from inbound_shipment   where inbound_shipment_id  = target_shipment_id;
if ROW_COUNT() = 0 then 
signal sqlstate '45000' set message_text='Warehouse not found.';
end if;
else 
signal sqlstate '45000' set message_text='Insufficient permission.';
end if ;
end//


create procedure delete_outbound_shipment (in input_token VARCHAR(36),in target_product_id int)
begin 
declare required_level	int default 1;
if sufficient_permission(input_token, required_level) then
delete from outbound_shipment    where outbound_shipment_id  = target_shipment_id;
if ROW_COUNT() = 0 then 
signal sqlstate '45000' set message_text='Warehouse not found.';
end if;
else 
signal sqlstate '45000' set message_text='Insufficient permission.';
end if ;
end//

create procedure delete_stock_transfer (in input_token VARCHAR(36),in target_product_id int)
begin 
declare required_level	int default 1;
if sufficient_permission(input_token, required_level) then
delete from stock_transfer   where stock_transfer_id   = target_transfer_id;
if ROW_COUNT() = 0 then 
signal sqlstate '45000' set message_text='Warehouse not found.';
end if;
else 
signal sqlstate '45000' set message_text='Insufficient permission.';
end if ;
end//

DELIMITER ;

