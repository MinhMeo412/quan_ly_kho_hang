DELIMITER //

CREATE PROCEDURE create_inbound_shipment(
    IN input_token VARCHAR(36),
    IN input_inbound_shipment_id INT,
    IN input_supplier_id INT,
    IN input_warehouse_id INT,
    IN input_inbound_shipment_starting_date DATETIME,
    IN input_inbound_shipment_status ENUM('Processing','Completed'),
    IN input_inbound_shipment_description TEXT,
    IN input_user_id INT
)
BEGIN
    DECLARE required_level INT DEFAULT 3;
    IF sufficient_permission(input_token, required_level) THEN
        INSERT INTO inbound_shipment(inbound_shipment_id, supplier_id, warehouse_id, inbound_shipment_starting_date, inbound_shipment_status, inbound_shipment_description, user_id) VALUES
        (input_inbound_shipment_id, input_supplier_id, input_warehouse_id, input_inbound_shipment_starting_date, input_inbound_shipment_status, input_inbound_shipment_description, input_user_id);
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE read_inbound_shipment(
    IN input_token VARCHAR(36)
)
BEGIN
    DECLARE required_level INT DEFAULT 4;
    IF sufficient_permission(input_token, required_level) THEN
        SELECT * FROM inbound_shipment;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE update_inbound_shipment(
    IN input_token VARCHAR(36),
    IN input_inbound_shipment_id INT,
    IN input_supplier_id INT,
    IN input_warehouse_id INT,
    IN input_inbound_shipment_starting_date DATETIME,
    IN input_inbound_shipment_status ENUM('Processing','Completed'),
    IN input_inbound_shipment_description TEXT,
    IN input_user_id INT
)
BEGIN
    DECLARE required_level INT DEFAULT 2;
    IF sufficient_permission(input_token, required_level) THEN
        UPDATE inbound_shipment
        SET supplier_id = input_supplier_id,
            warehouse_id = input_warehouse_id,
            inbound_shipment_starting_date = input_inbound_shipment_starting_date,
            inbound_shipment_status = input_inbound_shipment_status,
            inbound_shipment_description = input_inbound_shipment_description,
            user_id = input_user_id
        WHERE inbound_shipment_id = input_inbound_shipment_id;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE delete_inbound_shipment(
    IN input_token VARCHAR(36),
    IN input_inbound_shipment_id INT
)
BEGIN
    DECLARE required_level INT DEFAULT 2;
    IF sufficient_permission(input_token, required_level) THEN
        DELETE FROM inbound_shipment WHERE inbound_shipment_id = input_inbound_shipment_id;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

DELIMITER ;
