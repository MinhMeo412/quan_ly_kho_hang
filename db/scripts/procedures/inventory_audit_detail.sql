DELIMITER //

CREATE PROCEDURE create_inventory_audit_detail(
    IN input_token VARCHAR(36),
    IN input_inventory_audit_id INT,
    IN input_product_variant_id INT,
    IN input_inventory_audit_detail_actual_quantity INT
)
BEGIN
    DECLARE required_level INT DEFAULT 3;
    IF sufficient_permission(input_token, required_level) THEN
        INSERT INTO inventory_audit_detail(inventory_audit_id, product_variant_id, inventory_audit_detail_actual_quantity) VALUES
        (input_inventory_audit_id, input_product_variant_id, input_inventory_audit_detail_actual_quantity);
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE read_inventory_audit_detail(
    IN input_token VARCHAR(36)
)
BEGIN
    DECLARE required_level INT DEFAULT 4;
    IF sufficient_permission(input_token, required_level) THEN
        SELECT * FROM inventory_audit_detail;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE update_inventory_audit_detail(
    IN input_token VARCHAR(36),
    IN input_inventory_audit_id INT,
    IN input_product_variant_id INT,
    IN input_inventory_audit_detail_actual_quantity INT
)
BEGIN
    DECLARE required_level INT DEFAULT 3;
    IF sufficient_permission(input_token, required_level) THEN
        UPDATE inventory_audit_detail
        SET inventory_audit_detail_actual_quantity = input_inventory_audit_detail_actual_quantity
        WHERE inventory_audit_id = input_inventory_audit_id AND product_variant_id = input_product_variant_id;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE delete_inventory_audit_detail(
    IN input_token VARCHAR(36),
    IN input_inventory_audit_id INT,
    IN input_product_variant_id INT
)
BEGIN
    DECLARE required_level INT DEFAULT 1;
    IF sufficient_permission(input_token, required_level) THEN
        DELETE FROM inventory_audit_detail WHERE inventory_audit_id = input_inventory_audit_id AND product_variant_id = input_product_variant_id;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

DELIMITER ;
