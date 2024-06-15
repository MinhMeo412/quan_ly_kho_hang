DELIMITER //

/*
Get the permission level of the user associated with the inputted token.

token: token of the user in question
return: permission level of that user
*/
CREATE FUNCTION get_permission_level(token VARCHAR(36))
RETURNS INT
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
    
    SET success = FALSE;
    SET token = NULL;
    
    SELECT user_password, user_id INTO actual_password, actual_user_id
    FROM user WHERE user_name = inputted_usename LIMIT 1;
    
    IF actual_user_id IS NOT NULL THEN
		IF inputted_password = actual_password THEN
			SELECT UUID() INTO token_uuid;
            INSERT INTO token(token_uuid, user_id) VALUES (token_uuid, actutal_user_id);
            
            SET success = TRUE;
            SET token = token_uuid;
		END IF;
    END IF;
END //

DELIMITER ;