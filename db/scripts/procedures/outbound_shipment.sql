DELIMITER //

CREATE PROCEDURE create_outbound_shipment(
    IN input_token VARCHAR(36),
    IN input_outbound_shipment_id INT,
    IN input_warehouse_id INT,
    IN input_outbound_shipment_address TEXT,
    IN input_outbound_shipment_starting_date DATETIME,
    IN input_outbound_shipment_status ENUM('Processing','Completed'),
    IN input_outbound_shipment_description TEXT,
    IN input_user_id INT
)
BEGIN
    DECLARE required_level INT DEFAULT 3;
    IF sufficient_permission(input_token, required_level) THEN
        INSERT INTO outbound_shipment(outbound_shipment_id, warehouse_id, outbound_shipment_address, outbound_shipment_starting_date, outbound_shipment_status, outbound_shipment_description, user_id) VALUES
        (input_outbound_shipment_id, input_warehouse_id, input_outbound_shipment_address, input_outbound_shipment_starting_date, input_outbound_shipment_status, input_outbound_shipment_description, input_user_id);
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE read_outbound_shipment(
    IN input_token VARCHAR(36)
)
BEGIN
    DECLARE required_level INT DEFAULT 4;
    IF sufficient_permission(input_token, required_level) THEN
        SELECT * FROM outbound_shipment;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE update_outbound_shipment(
    IN input_token VARCHAR(36),
    IN input_outbound_shipment_id INT,
    IN input_warehouse_id INT,
    IN input_outbound_shipment_address TEXT,
    IN input_outbound_shipment_starting_date DATETIME,
    IN input_outbound_shipment_status ENUM('Processing','Completed'),
    IN input_outbound_shipment_description TEXT,
    IN input_user_id INT
)
BEGIN
    DECLARE required_level INT DEFAULT 3;
    IF sufficient_permission(input_token, required_level) THEN
        UPDATE outbound_shipment
        SET warehouse_id = input_warehouse_id,
            outbound_shipment_address = input_outbound_shipment_address,
            outbound_shipment_starting_date = input_outbound_shipment_starting_date,
            outbound_shipment_status = input_outbound_shipment_status,
            outbound_shipment_description = input_outbound_shipment_description,
            user_id = input_user_id
        WHERE outbound_shipment_id = input_outbound_shipment_id;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE delete_outbound_shipment(
    IN input_token VARCHAR(36),
    IN input_outbound_shipment_id INT
)
BEGIN
    DECLARE required_level INT DEFAULT 1;
    IF sufficient_permission(input_token, required_level) THEN
        DELETE FROM outbound_shipment WHERE outbound_shipment_id = input_outbound_shipment_id;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

DELIMITER ;
