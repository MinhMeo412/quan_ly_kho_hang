DELIMITER //

CREATE PROCEDURE create_stock_transfer(
    IN input_token VARCHAR(36),
    IN input_stock_transfer_id INT,
    IN input_from_warehouse_id INT,
    IN input_to_warehouse_id INT,
    IN input_stock_transfer_starting_date DATETIME,
    IN input_stock_transfer_status ENUM('Processing','Completed'),
    IN input_stock_transfer_description TEXT,
    IN input_user_id INT
)
BEGIN
    DECLARE required_level INT DEFAULT 3;
    IF sufficient_permission(input_token, required_level) THEN
        INSERT INTO stock_transfer(stock_transfer_id, from_warehouse_id, to_warehouse_id, stock_transfer_starting_date, stock_transfer_status, stock_transfer_description, user_id) VALUES
        (input_stock_transfer_id, input_from_warehouse_id, input_to_warehouse_id, input_stock_transfer_starting_date, input_stock_transfer_status, input_stock_transfer_description, input_user_id);
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE read_stock_transfer(
    IN input_token VARCHAR(36)
)
BEGIN
    DECLARE required_level INT DEFAULT 4;
    IF sufficient_permission(input_token, required_level) THEN
        SELECT * FROM stock_transfer;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE update_stock_transfer(
    IN input_token VARCHAR(36),
    IN input_stock_transfer_id INT,
    IN input_from_warehouse_id INT,
    IN input_to_warehouse_id INT,
    IN input_stock_transfer_starting_date DATETIME,
    IN input_stock_transfer_status ENUM('Processing','Completed'),
    IN input_stock_transfer_description TEXT,
    IN input_user_id INT
)
BEGIN
    DECLARE required_level INT DEFAULT 4;
    IF sufficient_permission(input_token, required_level) THEN
        UPDATE stock_transfer
        SET from_warehouse_id = input_from_warehouse_id,
            to_warehouse_id = input_to_warehouse_id,
            stock_transfer_starting_date = input_stock_transfer_starting_date,
            stock_transfer_status = input_stock_transfer_status,
            stock_transfer_description = input_stock_transfer_description,
            user_id = input_user_id
        WHERE stock_transfer_id = input_stock_transfer_id;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE delete_stock_transfer(
    IN input_token VARCHAR(36),
    IN input_stock_transfer_id INT
)
BEGIN
    DECLARE required_level INT DEFAULT 1;
    IF sufficient_permission(input_token, required_level) THEN
        DELETE FROM stock_transfer WHERE stock_transfer_id = input_stock_transfer_id;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

DELIMITER ;
