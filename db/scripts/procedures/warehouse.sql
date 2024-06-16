DELIMITER //

CREATE PROCEDURE create_warehouse(
    IN input_token VARCHAR(36),
    IN input_warehouse_id INT,
    IN input_warehouse_name VARCHAR(32), 
    IN input_warehouse_address_id INT
)
BEGIN
    DECLARE required_level INT DEFAULT 1;
    IF sufficient_permission(input_token, required_level) THEN
        INSERT INTO warehouse(warehouse_id, warehouse_name, warehouse_address_id) VALUES
        (input_warehouse_id, input_warehouse_name, input_warehouse_address_id);
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE read_warehouse(
    IN input_token VARCHAR(36)
)
BEGIN
    DECLARE required_level INT DEFAULT 4;
    IF sufficient_permission(input_token, required_level) THEN
        SELECT * FROM warehouse;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE update_warehouse(
    IN input_token VARCHAR(36),
    IN input_warehouse_id INT,
    IN input_warehouse_name VARCHAR(32), 
    IN input_warehouse_address_id INT
)
BEGIN
    DECLARE required_level INT DEFAULT 1;
    IF sufficient_permission(input_token, required_level) THEN
        UPDATE warehouse
        SET warehouse_name = input_warehouse_name, 
            warehouse_address_id = input_warehouse_address_id
        WHERE warehouse_id = input_warehouse_id;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE delete_warehouse(
    IN input_token VARCHAR(36),
    IN input_warehouse_id INT
)
BEGIN
    DECLARE required_level INT DEFAULT 1;
    IF sufficient_permission(input_token, required_level) THEN
        DELETE FROM warehouse WHERE warehouse_id = input_warehouse_id;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

DELIMITER ;
