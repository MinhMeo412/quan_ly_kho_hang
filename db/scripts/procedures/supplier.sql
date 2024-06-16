DELIMITER //

CREATE PROCEDURE create_supplier(
    IN input_token VARCHAR(36),
    IN input_supplier_id INT,
    IN input_supplier_name VARCHAR(32), 
    IN input_supplier_description TEXT, 
    IN input_supplier_address TEXT, 
    IN input_supplier_email VARCHAR(320), 
    IN input_supplier_phone_number VARCHAR(24), 
    IN input_supplier_website TEXT
)
BEGIN
    DECLARE required_level INT DEFAULT 3;
    IF sufficient_permission(input_token, required_level) THEN
        INSERT INTO supplier(supplier_id, supplier_name, supplier_description, supplier_address, supplier_email, supplier_phone_number, supplier_website) VALUES
        (input_supplier_id, input_supplier_name, input_supplier_description, input_supplier_address, input_supplier_email, input_supplier_phone_number, input_supplier_website);
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE read_supplier(
    IN input_token VARCHAR(36)
)
BEGIN
    DECLARE required_level INT DEFAULT 4;
    IF sufficient_permission(input_token, required_level) THEN
        SELECT * FROM supplier;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE update_supplier(
    IN input_token VARCHAR(36),
    IN input_supplier_id INT,
    IN input_supplier_name VARCHAR(32), 
    IN input_supplier_description TEXT, 
    IN input_supplier_address TEXT, 
    IN input_supplier_email VARCHAR(320), 
    IN input_supplier_phone_number VARCHAR(24), 
    IN input_supplier_website TEXT
)
BEGIN
    DECLARE required_level INT DEFAULT 2;
    IF sufficient_permission(input_token, required_level) THEN
        UPDATE supplier
        SET supplier_name = input_supplier_name, 
            supplier_description = input_supplier_description, 
            supplier_address = input_supplier_address, 
            supplier_email = input_supplier_email, 
            supplier_phone_number = input_supplier_phone_number, 
            supplier_website = input_supplier_website
        WHERE supplier_id = input_supplier_id;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE delete_supplier(
    IN input_token VARCHAR(36),
    IN input_supplier_id INT
)
BEGIN
    DECLARE required_level INT DEFAULT 1;
    IF sufficient_permission(input_token, required_level) THEN
        DELETE FROM supplier WHERE supplier_id = input_supplier_id;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

DELIMITER ;
