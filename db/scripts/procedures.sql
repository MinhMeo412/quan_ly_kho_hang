/*
Get the permission level of the user associated with the inputted token.

token: token of the user in question
return: permission level of that user
*/
SELECT 'Loading get_permission_level' AS 'INFO';
DELIMITER //
CREATE FUNCTION get_permission_level(token VARCHAR(36))
RETURNS INT
DETERMINISTIC
BEGIN
    DECLARE permission_level INT;
    SELECT user_permission_level from user where user_id = (select token_user_id from token where token_uuid = token) into permission_level;
    RETURN permission_level;
END//
DELIMITER ;

/*
Check if the user has sufficient permission to perform an action.

token: token of the user in question
required_level: the required level to perform an action
return: true if the user has sufficient permission. false if otherwise.
*/
SELECT 'Loading sufficient_permission' AS 'INFO';
DELIMITER //
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
DELIMITER ;

/*
Authenticate an user. Returns true and a token if the login information is correct.

inputted_username: username that the user entered.
inputted_password: password that the user entered.

success: TRUE if the usernam and password is correct.
token: uniquely identifiable token if username and password is correct. null if otherwise.
*/
SELECT 'Loading user_login' AS 'INFO';
DELIMITER //
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
	
    if inputted_password = actual_password then
    	INSERT INTO token(token_uuid, token_user_id) VALUES
    	(token_uuid, actual_user_id);
    
    	SET success = TRUE;
 		SET token = token_uuid;
    ELSE
        SET success = FALSE;
        SET token = NULL;
    END IF;
END //
DELIMITER ;

























