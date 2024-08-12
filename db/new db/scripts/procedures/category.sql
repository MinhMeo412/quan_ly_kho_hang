DELIMITER //

CREATE PROCEDURE create_category(
    IN input_token VARCHAR(36),
    IN input_category_id INT,
    IN input_category_name VARCHAR(32), 
    IN input_category_description TEXT
)
BEGIN
    DECLARE required_level INT DEFAULT 3;
    IF sufficient_permission(input_token, required_level) THEN
        INSERT INTO category(category_id, category_name, category_description) VALUES
        (input_category_id, input_category_name, input_category_description);
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE read_category(
    IN input_token VARCHAR(36)
)
BEGIN
    DECLARE required_level INT DEFAULT 4;
    IF sufficient_permission(input_token, required_level) THEN
        SELECT * FROM category;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE update_category(
    IN input_token VARCHAR(36),
    IN input_category_id INT,
    IN input_category_name VARCHAR(32), 
    IN input_category_description TEXT
)
BEGIN
    DECLARE required_level INT DEFAULT 2;
    IF sufficient_permission(input_token, required_level) THEN
        UPDATE category
        SET category_name = input_category_name, 
            category_description = input_category_description
        WHERE category_id = input_category_id;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE delete_category(
    IN input_token VARCHAR(36),
    IN input_category_id INT
)
BEGIN
    DECLARE required_level INT DEFAULT 2;
    IF sufficient_permission(input_token, required_level) THEN
        DELETE FROM category WHERE category_id = input_category_id;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

DELIMITER ;
