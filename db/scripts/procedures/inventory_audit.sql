DELIMITER //

CREATE PROCEDURE create_inventory_audit(
    IN input_token VARCHAR(36),
    IN input_inventory_audit_id INT,
    IN input_warehouse_id INT, 
    IN input_user_id INT, 
    IN input_inventory_audit_time DATETIME
)
BEGIN
    DECLARE required_level INT DEFAULT 3;
    IF sufficient_permission(input_token, required_level) THEN
        INSERT INTO inventory_audit(inventory_audit_id, warehouse_id, user_id, inventory_audit_time) VALUES
        (input_inventory_audit_id, input_warehouse_id, input_user_id, input_inventory_audit_time);
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE read_inventory_audit(
    IN input_token VARCHAR(36)
)
BEGIN
    DECLARE required_level INT DEFAULT 4;
    IF sufficient_permission(input_token, required_level) THEN
        SELECT * FROM inventory_audit;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE update_inventory_audit(
    IN input_token VARCHAR(36),
    IN input_inventory_audit_id INT,
    IN input_warehouse_id INT, 
    IN input_user_id INT, 
    IN input_inventory_audit_time DATETIME
)
BEGIN
    DECLARE required_level INT DEFAULT 3;
    IF sufficient_permission(input_token, required_level) THEN
        UPDATE inventory_audit
        SET warehouse_id = input_warehouse_id, 
            user_id = input_user_id, 
            inventory_audit_time = input_inventory_audit_time
        WHERE inventory_audit_id = input_inventory_audit_id;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE delete_inventory_audit(
    IN input_token VARCHAR(36),
    IN input_inventory_audit_id INT
)
BEGIN
    DECLARE required_level INT DEFAULT 1;
    IF sufficient_permission(input_token, required_level) THEN
        DELETE FROM inventory_audit WHERE inventory_audit_id = input_inventory_audit_id;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

DELIMITER ;
