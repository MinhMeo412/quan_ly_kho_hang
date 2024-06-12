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

CREATE PROCEDURE delete_product(IN input_token VARCHAR(36), IN target_product_id INT)
BEGIN 
    DECLARE required_level INT DEFAULT 2;
    IF sufficient_permission(input_token, required_level) THEN
        DELETE FROM product WHERE product_id = target_product_id;
        IF ROW_COUNT() = 0 THEN 
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Product not found.';
        END IF;
    ELSE 
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE delete_product_variant(IN input_token VARCHAR(36), IN target_variant_id INT)
BEGIN 
    DECLARE required_level INT DEFAULT 2;
    IF sufficient_permission(input_token, required_level) THEN
        DELETE FROM product_variant WHERE product_variant_id = target_variant_id;
        IF ROW_COUNT() = 0 THEN 
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Product variant not found.';
        END IF;
    ELSE 
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE delete_category(IN input_token VARCHAR(36), IN target_category_id INT)
BEGIN 
    DECLARE required_level INT DEFAULT 2;
    IF sufficient_permission(input_token, required_level) THEN
        DELETE FROM category WHERE category_id = target_category_id;
        IF ROW_COUNT() = 0 THEN 
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Category not found.';
        END IF;
    ELSE 
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE delete_warehouse(IN input_token VARCHAR(36), IN target_warehouse_id INT)
BEGIN 
    DECLARE required_level INT DEFAULT 1;
    IF sufficient_permission(input_token, required_level) THEN
        DELETE FROM warehouse WHERE warehouse_id = target_warehouse_id;
        IF ROW_COUNT() = 0 THEN 
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Warehouse not found.';
        END IF;
    ELSE 
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE delete_warehouse_address(IN input_token VARCHAR(36), IN target_address_id INT)
BEGIN 
    DECLARE required_level INT DEFAULT 1;
    IF sufficient_permission(input_token, required_level) THEN
        DELETE FROM warehouse_address WHERE warehouse_address_id = target_address_id;
        IF ROW_COUNT() = 0 THEN 
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Warehouse address not found.';
        END IF;
    ELSE 
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE delete_inventory_audit(IN input_token VARCHAR(36), IN target_audit_id INT)
BEGIN 
    DECLARE required_level INT DEFAULT 1;
    IF sufficient_permission(input_token, required_level) THEN
        DELETE FROM inventory_audit WHERE inventory_audit_id = target_audit_id;
        IF ROW_COUNT() = 0 THEN 
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Inventory audit not found.';
        END IF;
    ELSE 
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE delete_inventory_audit_detail(IN input_token VARCHAR(36), IN target_audit_id INT, IN target_variant_id INT)
BEGIN 
    DECLARE required_level INT DEFAULT 1;
    IF sufficient_permission(input_token, required_level) THEN
        DELETE FROM inventory_audit_detail WHERE inventory_audit_id = target_audit_id AND product_variant_id = target_variant_id;
        IF ROW_COUNT() = 0 THEN 
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Inventory audit detail not found.';
        END IF;
    ELSE 
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE delete_inbound_shipment(IN input_token VARCHAR(36), IN target_shipment_id INT)
BEGIN 
    DECLARE required_level INT DEFAULT 1;
    IF sufficient_permission(input_token, required_level) THEN
        DELETE FROM inbound_shipment WHERE inbound_shipment_id = target_shipment_id;
        IF ROW_COUNT() = 0 THEN 
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Inbound shipment not found.';
        END IF;
    ELSE 
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE delete_inbound_shipment_detail(IN input_token VARCHAR(36), IN target_shipment_id INT, IN target_variant_id INT)
BEGIN 
    DECLARE required_level INT DEFAULT 1;
    IF sufficient_permission(input_token, required_level) THEN
        DELETE FROM inbound_shipment_detail WHERE inbound_shipment_id = target_shipment_id AND product_variant_id = target_variant_id;
        IF ROW_COUNT() = 0 THEN 
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Inbound shipment detail not found.';
        END IF;
    ELSE 
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE delete_outbound_shipment(IN input_token VARCHAR(36), IN target_shipment_id INT)
BEGIN 
    DECLARE required_level INT DEFAULT 1;
    IF sufficient_permission(input_token, required_level) THEN
        DELETE FROM outbound_shipment WHERE outbound_shipment_id = target_shipment_id;
        IF ROW_COUNT() = 0 THEN 
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Outbound shipment not found.';
        END IF;
    ELSE 
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE delete_outbound_shipment_detail(IN input_token VARCHAR(36), IN target_shipment_id INT, IN target_variant_id INT)
BEGIN 
    DECLARE required_level INT DEFAULT 1;
    IF sufficient_permission(input_token, required_level) THEN
        DELETE FROM outbound_shipment_detail WHERE outbound_shipment_id = target_shipment_id AND product_variant_id = target_variant_id;
        IF ROW_COUNT() = 0 THEN 
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Outbound shipment detail not found.';
        END IF;
    ELSE 
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE delete_stock_transfer(IN input_token VARCHAR(36), IN target_transfer_id INT)
BEGIN 
    DECLARE required_level INT DEFAULT 1;
    IF sufficient_permission(input_token, required_level) THEN
        DELETE FROM stock_transfer WHERE stock_transfer_id = target_transfer_id;
        IF ROW_COUNT() = 0 THEN 
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Stock transfer not found.';
        END IF;
    ELSE 
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE delete_stock_transfer_detail(IN input_token VARCHAR(36), IN target_transfer_id INT, IN target_variant_id INT)
BEGIN 
    DECLARE required_level INT DEFAULT 1;
    IF sufficient_permission(input_token, required_level) THEN
        DELETE FROM stock_transfer WHERE stock_transfer_id = target_transfer_id AND product_variant_id = target_variant_id;
        IF ROW_COUNT() = 0 THEN 
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Stock transfer detail not found.';
        END IF;
    ELSE 
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

DELIMITER ;
.