SELECT 'Loading procedures' AS 'INFO';
DELIMITER //

/*
Get the permission level of the user associated with the inputted token.

token: token of the user in question
return: permission level of that user
*/
CREATE FUNCTION get_permission_level(token VARCHAR(36))
RETURNS INT
DETERMINISTIC
BEGIN
    DECLARE user_permission_level INT;
    SELECT permission_level from user where user_id = (select user_id from token where token_uuid = token) into user_permission_level;
    RETURN user_permission_level;
END//

/*
Check if the user has sufficient permission to perform an action.

token: token of the user in question
required_level: the required level to perform an action
return: true if the user has sufficient permission. false if otherwise.
*/
CREATE FUNCTION sufficient_permission(token VARCHAR(36), required_level INT)
RETURNS BOOLEAN
DETERMINISTIC
BEGIN
    DECLARE user_permission_level INT;
    SELECT get_permission_level(token) into user_permission_level;

    IF user_permission_level <= required_level THEN
        RETURN TRUE;
    ELSE
        RETURN FALSE;
    END IF;
END//

/*
Authenticate an user. Returns true and a token if the login information is correct.

inputted_username: username that the user entered.
inputted_password: password that the user entered.

success: TRUE if the usernam and password is correct.
token: uniquely identifiable token if username and password is correct. null if otherwise.
*/
CREATE PROCEDURE user_login(
    IN inputted_username VARCHAR(32),
    IN inputted_password VARCHAR(128),
    OUT success BOOLEAN,
    OUT token VARCHAR(36)
)
BEGIN
	DECLARE token_uuid VARCHAR(36);
	DECLARE actual_password VARCHAR(128);
	DECLARE actual_user_id INT;

	SELECT UUID() INTO token_uuid;
	SELECT user_password FROM user WHERE user_name = inputted_username LIMIT 1 INTO actual_password;
	SELECT user_id FROM user WHERE user_name = inputted_username LIMIT 1 INTO actual_user_id;
	
    IF inputted_password = actual_password THEN
    	INSERT INTO token(token_uuid, user_id) VALUES
    	(token_uuid, actual_user_id);
    
    	SET success = TRUE;
 		SET token = token_uuid;
    ELSE
        SET success = FALSE;
        SET token = NULL;
    END IF;
END //


CREATE PROCEDURE read_permission(IN input_token VARCHAR(36))
BEGIN
    DECLARE required_level INT DEFAULT 0;
    IF sufficient_permission(input_token, required_level) THEN
        SELECT * FROM permission;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE create_permission(IN input_token VARCHAR(36),IN input_permission_level INT, IN input_permission_name VARCHAR(32), IN input_permission_description TEXT)
BEGIN
    DECLARE required_level INT DEFAULT 0;
    IF sufficient_permission(input_token, required_level) THEN
        INSERT INTO permission(permission_level, permission_name, permission_description) VALUES
        (input_permission_level, input_permission_name, input_permission_description);
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE update_permission(IN input_token VARCHAR(36),IN input_permission_level INT, IN input_permission_name VARCHAR(32), IN input_permission_description TEXT)
BEGIN
    DECLARE required_level INT DEFAULT 0;
    IF sufficient_permission(input_token, required_level) THEN
        UPDATE permission
        SET permission_name = input_permission_name, permission_description = input_permission_description
        WHERE permission_level = input_permission_level;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE delete_permission(IN input_token VARCHAR(36), IN input_permission_level INT)
BEGIN
    DECLARE required_level INT DEFAULT 0;
    IF sufficient_permission(input_token, required_level) THEN
        DELETE FROM permission
        WHERE permission_level = input_permission_level;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE read_user(IN input_token VARCHAR(36))
BEGIN
    DECLARE required_level INT DEFAULT 0;
    IF sufficient_permission(input_token, required_level) THEN
        SELECT * FROM user;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE read_token(IN input_token VARCHAR(36))
BEGIN
    DECLARE required_level INT DEFAULT 0;
    IF sufficient_permission(input_token, required_level) THEN
        SELECT * FROM token;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE read_supplier(IN input_token VARCHAR(36))
BEGIN
    DECLARE required_level INT DEFAULT 0;
    IF sufficient_permission(input_token, required_level) THEN
        SELECT * FROM supplier;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE read_category(IN input_token VARCHAR(36))
BEGIN
    DECLARE required_level INT DEFAULT 0;
    IF sufficient_permission(input_token, required_level) THEN
        SELECT * FROM category;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE read_product(IN input_token VARCHAR(36))
BEGIN
    DECLARE required_level INT DEFAULT 0;
    IF sufficient_permission(input_token, required_level) THEN
        SELECT * FROM product;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE read_product_variant(IN input_token VARCHAR(36))
BEGIN
    DECLARE required_level INT DEFAULT 0;
    IF sufficient_permission(input_token, required_level) THEN
        SELECT * FROM product_variant;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE read_warehouse_address(IN input_token VARCHAR(36))
BEGIN
    DECLARE required_level INT DEFAULT 0;
    IF sufficient_permission(input_token, required_level) THEN
        SELECT * FROM warehouse_address;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE read_warehouse(IN input_token VARCHAR(36))
BEGIN
    DECLARE required_level INT DEFAULT 0;
    IF sufficient_permission(input_token, required_level) THEN
        SELECT * FROM warehouse;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE read_warehouse_stock(IN input_token VARCHAR(36))
BEGIN
    DECLARE required_level INT DEFAULT 0;
    IF sufficient_permission(input_token, required_level) THEN
        SELECT * FROM warehouse_stock;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE read_inventory_audit(IN input_token VARCHAR(36))
BEGIN
    DECLARE required_level INT DEFAULT 0;
    IF sufficient_permission(input_token, required_level) THEN
        SELECT * FROM inventory_audit;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE read_inventory_audit_detail(IN input_token VARCHAR(36))
BEGIN
    DECLARE required_level INT DEFAULT 0;
    IF sufficient_permission(input_token, required_level) THEN
        SELECT * FROM inventory_audit_detail;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE read_inbound_shipment(IN input_token VARCHAR(36))
BEGIN
    DECLARE required_level INT DEFAULT 0;
    IF sufficient_permission(input_token, required_level) THEN
        SELECT * FROM inbound_shipment;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE read_inbound_shipment_detail(IN input_token VARCHAR(36))
BEGIN
    DECLARE required_level INT DEFAULT 0;
    IF sufficient_permission(input_token, required_level) THEN
        SELECT * FROM inbound_shipment_detail;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE read_outbound_shipment(IN input_token VARCHAR(36))
BEGIN
    DECLARE required_level INT DEFAULT 0;
    IF sufficient_permission(input_token, required_level) THEN
        SELECT * FROM outbound_shipment;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE read_outbound_shipment_detail(IN input_token VARCHAR(36))
BEGIN
    DECLARE required_level INT DEFAULT 0;
    IF sufficient_permission(input_token, required_level) THEN
        SELECT * FROM outbound_shipment_detail;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE read_stock_transfer(IN input_token VARCHAR(36))
BEGIN
    DECLARE required_level INT DEFAULT 0;
    IF sufficient_permission(input_token, required_level) THEN
        SELECT * FROM stock_transfer;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE read_stock_transfer_detail(IN input_token VARCHAR(36))
BEGIN
    DECLARE required_level INT DEFAULT 0;
    IF sufficient_permission(input_token, required_level) THEN
        SELECT * FROM stock_transfer_detail;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

























































DELIMITER ;