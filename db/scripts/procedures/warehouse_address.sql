DELIMITER //

CREATE PROCEDURE create_warehouse_address(
    IN input_token VARCHAR(36),
    IN input_warehouse_address_id INT,
    IN input_warehouse_address_address VARCHAR(128), 
    IN input_warehouse_address_district VARCHAR(64), 
    IN input_warehouse_address_postal_code VARCHAR(16), 
    IN input_warehouse_address_city VARCHAR(32), 
    IN input_warehouse_address_country VARCHAR(64)
)
BEGIN
    DECLARE required_level INT DEFAULT 1;
    IF sufficient_permission(input_token, required_level) THEN
        INSERT INTO warehouse_address(warehouse_address_id, warehouse_address_address, warehouse_address_district, warehouse_address_postal_code, warehouse_address_city, warehouse_address_country) VALUES
        (input_warehouse_address_id, input_warehouse_address_address, input_warehouse_address_district, input_warehouse_address_postal_code, input_warehouse_address_city, input_warehouse_address_country);
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE read_warehouse_address(
    IN input_token VARCHAR(36)
)
BEGIN
    DECLARE required_level INT DEFAULT 4;
    IF sufficient_permission(input_token, required_level) THEN
        SELECT * FROM warehouse_address;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE update_warehouse_address(
    IN input_token VARCHAR(36),
    IN input_warehouse_address_id INT,
    IN input_warehouse_address_address VARCHAR(128), 
    IN input_warehouse_address_district VARCHAR(64), 
    IN input_warehouse_address_postal_code VARCHAR(16), 
    IN input_warehouse_address_city VARCHAR(32), 
    IN input_warehouse_address_country VARCHAR(64)
)
BEGIN
    DECLARE required_level INT DEFAULT 1;
    IF sufficient_permission(input_token, required_level) THEN
        UPDATE warehouse_address
        SET warehouse_address_address = input_warehouse_address_address, 
            warehouse_address_district = input_warehouse_address_district, 
            warehouse_address_postal_code = input_warehouse_address_postal_code, 
            warehouse_address_city = input_warehouse_address_city, 
            warehouse_address_country = input_warehouse_address_country
        WHERE warehouse_address_id = input_warehouse_address_id;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE delete_warehouse_address(
    IN input_token VARCHAR(36),
    IN input_warehouse_address_id INT
)
BEGIN
    DECLARE required_level INT DEFAULT 1;
    IF sufficient_permission(input_token, required_level) THEN
        DELETE FROM warehouse_address WHERE warehouse_address_id = input_warehouse_address_id;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

DELIMITER ;
