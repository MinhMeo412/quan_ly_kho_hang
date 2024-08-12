DELIMITER //

CREATE PROCEDURE create_stock_transfer_detail(
    IN input_token VARCHAR(36),
    IN input_stock_transfer_id INT,
    IN input_product_variant_id INT,
    IN input_stock_transfer_detail_amount INT
)
BEGIN
    DECLARE required_level INT DEFAULT 3;
    IF sufficient_permission(input_token, required_level) THEN
        INSERT INTO stock_transfer_detail(stock_transfer_id, product_variant_id, stock_transfer_detail_amount) VALUES
        (input_stock_transfer_id, input_product_variant_id, input_stock_transfer_detail_amount);
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE read_stock_transfer_detail(
    IN input_token VARCHAR(36)
)
BEGIN
    DECLARE required_level INT DEFAULT 4;
    IF sufficient_permission(input_token, required_level) THEN
        SELECT * FROM stock_transfer_detail;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE update_stock_transfer_detail(
    IN input_token VARCHAR(36),
    IN input_stock_transfer_id INT,
    IN input_product_variant_id INT,
    IN input_stock_transfer_detail_amount INT
)
BEGIN
    DECLARE required_level INT DEFAULT 2;
    IF sufficient_permission(input_token, required_level) THEN
        UPDATE stock_transfer_detail
        SET stock_transfer_detail_amount = input_stock_transfer_detail_amount
        WHERE stock_transfer_id = input_stock_transfer_id AND product_variant_id = input_product_variant_id;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE delete_stock_transfer_detail(
    IN input_token VARCHAR(36),
    IN input_stock_transfer_id INT,
    IN input_product_variant_id INT
)
BEGIN
    DECLARE required_level INT DEFAULT 2;
    IF sufficient_permission(input_token, required_level) THEN
        DELETE FROM stock_transfer_detail WHERE stock_transfer_id = input_stock_transfer_id AND product_variant_id = input_product_variant_id;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

DELIMITER ;
