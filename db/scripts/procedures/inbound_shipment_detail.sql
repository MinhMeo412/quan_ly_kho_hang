DELIMITER //

CREATE PROCEDURE create_inbound_shipment_detail(
    IN input_token VARCHAR(36),
    IN input_inbound_shipment_id INT,
    IN input_product_variant_id INT,
    IN input_inbound_shipment_detail_amount INT
)
BEGIN
    DECLARE required_level INT DEFAULT 3;
    IF sufficient_permission(input_token, required_level) THEN
        INSERT INTO inbound_shipment_detail(inbound_shipment_id, product_variant_id, inbound_shipment_detail_amount) VALUES
        (input_inbound_shipment_id, input_product_variant_id, input_inbound_shipment_detail_amount);
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE read_inbound_shipment_detail(
    IN input_token VARCHAR(36)
)
BEGIN
    DECLARE required_level INT DEFAULT 4;
    IF sufficient_permission(input_token, required_level) THEN
        SELECT * FROM inbound_shipment_detail;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE update_inbound_shipment_detail(
    IN input_token VARCHAR(36),
    IN input_inbound_shipment_id INT,
    IN input_product_variant_id INT,
    IN input_inbound_shipment_detail_amount INT
)
BEGIN
    DECLARE required_level INT DEFAULT 3;
    IF sufficient_permission(input_token, required_level) THEN
        UPDATE inbound_shipment_detail
        SET inbound_shipment_detail_amount = input_inbound_shipment_detail_amount
        WHERE inbound_shipment_id = input_inbound_shipment_id AND product_variant_id = input_product_variant_id;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE delete_inbound_shipment_detail(
    IN input_token VARCHAR(36),
    IN input_inbound_shipment_id INT,
    IN input_product_variant_id INT
)
BEGIN
    DECLARE required_level INT DEFAULT 1;
    IF sufficient_permission(input_token, required_level) THEN
        DELETE FROM inbound_shipment_detail WHERE inbound_shipment_id = input_inbound_shipment_id AND product_variant_id = input_product_variant_id;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

DELIMITER ;
