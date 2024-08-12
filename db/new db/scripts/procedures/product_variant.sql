DELIMITER //

CREATE PROCEDURE create_product_variant(
    IN input_token VARCHAR(36),
    IN input_product_variant_id INT,
    IN input_product_id INT, 
    IN input_product_variant_image_url TEXT, 
    IN input_product_variant_color VARCHAR(32), 
    IN input_product_variant_size VARCHAR(16)
)
BEGIN
    DECLARE required_level INT DEFAULT 3;
    IF sufficient_permission(input_token, required_level) THEN
        INSERT INTO product_variant(product_variant_id, product_id, product_variant_image_url, product_variant_color, product_variant_size) VALUES
        (input_product_variant_id, input_product_id, input_product_variant_image_url, input_product_variant_color, input_product_variant_size);
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE read_product_variant(
    IN input_token VARCHAR(36)
)
BEGIN
    DECLARE required_level INT DEFAULT 4;
    IF sufficient_permission(input_token, required_level) THEN
        SELECT * FROM product_variant;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE update_product_variant(
    IN input_token VARCHAR(36),
    IN input_product_variant_id INT,
    IN input_product_id INT, 
    IN input_product_variant_image_url TEXT, 
    IN input_product_variant_color VARCHAR(32), 
    IN input_product_variant_size VARCHAR(16)
)
BEGIN
    DECLARE required_level INT DEFAULT 2;
    IF sufficient_permission(input_token, required_level) THEN
        UPDATE product_variant
        SET product_id = input_product_id, 
            product_variant_image_url = input_product_variant_image_url, 
            product_variant_color = input_product_variant_color, 
            product_variant_size = input_product_variant_size
        WHERE product_variant_id = input_product_variant_id;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE delete_product_variant(
    IN input_token VARCHAR(36),
    IN input_product_variant_id INT
)
BEGIN
    DECLARE required_level INT DEFAULT 2; 
    IF sufficient_permission(input_token, required_level) THEN
        DELETE FROM product_variant WHERE product_variant_id = input_product_variant_id;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

DELIMITER ;
