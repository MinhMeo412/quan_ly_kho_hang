DELIMITER //

CREATE PROCEDURE create_outbound_shipment_detail(
    IN input_token VARCHAR(36),
    IN input_outbound_shipment_id INT,
    IN input_product_variant_id INT,
    IN input_outbound_shipment_detail_amount INT
)
BEGIN
    DECLARE required_level INT DEFAULT 3;
    IF sufficient_permission(input_token, required_level) THEN
        INSERT INTO outbound_shipment_detail(outbound_shipment_id, product_variant_id, outbound_shipment_detail_amount) VALUES
        (input_outbound_shipment_id, input_product_variant_id, input_outbound_shipment_detail_amount);
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE read_outbound_shipment_detail(
    IN input_token VARCHAR(36)
)
BEGIN
    DECLARE required_level INT DEFAULT 4;
    IF sufficient_permission(input_token, required_level) THEN
        SELECT * FROM outbound_shipment_detail;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE update_outbound_shipment_detail(
    IN input_token VARCHAR(36),
    IN input_outbound_shipment_id INT,
    IN input_product_variant_id INT,
    IN input_outbound_shipment_detail_amount INT
)
BEGIN
    DECLARE required_level INT DEFAULT 2;
    IF sufficient_permission(input_token, required_level) THEN
        UPDATE outbound_shipment_detail
        SET outbound_shipment_detail_amount = input_outbound_shipment_detail_amount
        WHERE outbound_shipment_id = input_outbound_shipment_id AND product_variant_id = input_product_variant_id;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE delete_outbound_shipment_detail(
    IN input_token VARCHAR(36),
    IN input_outbound_shipment_id INT,
    IN input_product_variant_id INT
)
BEGIN
    DECLARE required_level INT DEFAULT 2;
    IF sufficient_permission(input_token, required_level) THEN
        DELETE FROM outbound_shipment_detail WHERE outbound_shipment_id = input_outbound_shipment_id AND product_variant_id = input_product_variant_id;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

DELIMITER ;
