DELIMITER //

CREATE PROCEDURE create_warehouse_stock(
    IN input_token VARCHAR(36),
    IN input_warehouse_id INT, 
    IN input_product_variant_id INT, 
    IN input_warehouse_stock_quantity INT
)
BEGIN
    DECLARE required_level INT DEFAULT 3;
    IF sufficient_permission(input_token, required_level) THEN
        INSERT INTO warehouse_stock(warehouse_id, product_variant_id, warehouse_stock_quantity) VALUES
        (input_warehouse_id, input_product_variant_id, input_warehouse_stock_quantity);
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE read_warehouse_stock(
    IN input_token VARCHAR(36)
)
BEGIN
    DECLARE required_level INT DEFAULT 4;
    IF sufficient_permission(input_token, required_level) THEN
        SELECT * FROM warehouse_stock;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE update_warehouse_stock(
    IN input_token VARCHAR(36),
    IN input_warehouse_id INT,
    IN input_product_variant_id INT,
    IN input_warehouse_stock_quantity INT
)
BEGIN
    DECLARE required_level INT DEFAULT -1;
    IF sufficient_permission(input_token, required_level) THEN
        UPDATE warehouse_stock
        SET warehouse_stock_quantity = input_warehouse_stock_quantity
        WHERE warehouse_id = input_warehouse_id AND product_variant_id = input_product_variant_id;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE delete_warehouse_stock(
    IN input_token VARCHAR(36),
    IN input_warehouse_id INT,
    IN input_product_variant_id INT
)
BEGIN
    DECLARE required_level INT DEFAULT -1;
    IF sufficient_permission(input_token, required_level) THEN
        DELETE FROM warehouse_stock WHERE warehouse_id = input_warehouse_id AND product_variant_id = input_product_variant_id;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

DELIMITER ;
