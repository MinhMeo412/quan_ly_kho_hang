DELIMITER //

CREATE PROCEDURE create_product(
    IN input_token VARCHAR(36),
    IN input_product_id INT,
    IN input_product_name VARCHAR(64), 
    IN input_product_description TEXT, 
    IN input_product_price INT, 
    IN input_category_id INT
)
BEGIN
    DECLARE required_level INT DEFAULT 3;
    IF sufficient_permission(input_token, required_level) THEN
        INSERT INTO product(product_id, product_name, product_description, product_price, category_id) VALUES
        (input_product_id, input_product_name, input_product_description, input_product_price, input_category_id);
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE read_product(
    IN input_token VARCHAR(36)
)
BEGIN
    DECLARE required_level INT DEFAULT 4;
    IF sufficient_permission(input_token, required_level) THEN
        SELECT * FROM product;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE update_product(
    IN input_token VARCHAR(36),
    IN input_product_id INT,
    IN input_product_name VARCHAR(64), 
    IN input_product_description TEXT, 
    IN input_product_price INT, 
    IN input_category_id INT
)
BEGIN
    DECLARE required_level INT DEFAULT 2;
    IF sufficient_permission(input_token, required_level) THEN
        UPDATE product
        SET product_name = input_product_name, 
            product_description = input_product_description, 
            product_price = input_product_price, 
            category_id = input_category_id
        WHERE product_id = input_product_id;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE delete_product(
    IN input_token VARCHAR(36),
    IN input_product_id INT
)
BEGIN
    DECLARE required_level INT DEFAULT 2; 
    IF sufficient_permission(input_token, required_level) THEN
        DELETE FROM product WHERE product_id = input_product_id;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

DELIMITER ;
