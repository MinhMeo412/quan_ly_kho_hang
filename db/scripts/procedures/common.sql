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
    DECLARE actual_user_id INT;
	DECLARE actual_password VARCHAR(128);
    
    SELECT UUID() INTO token_uuid;
	SELECT user_id FROM user WHERE user_name = inputted_username LIMIT 1 INTO actual_user_id;
    SELECT user_password FROM user WHERE user_name = inputted_username LIMIT 1 INTO actual_password;

    SET success = FALSE;
    SET token = NULL;
    
    IF inputted_password = actual_password THEN
        INSERT INTO token(token_uuid, user_id) VALUES (token_uuid, actual_user_id);
        
        SET success = TRUE;
        SET token = token_uuid;
    END IF;
END //

CREATE PROCEDURE change_password(
    IN inputted_username VARCHAR(32),
    IN old_password VARCHAR(128),
    IN new_password VARCHAR(128),
    OUT success BOOLEAN,
    OUT token VARCHAR(36)
)
BEGIN
	DECLARE token_uuid VARCHAR(36);
	DECLARE actual_password VARCHAR(128);
    DECLARE actual_user_id INT;
    
    SELECT UUID() INTO token_uuid;
	SELECT user_id FROM user WHERE user_name = inputted_username LIMIT 1 INTO actual_user_id;
    SELECT user_password FROM user WHERE user_name = inputted_username LIMIT 1 INTO actual_password;

    SET success = FALSE;
    SET token = NULL;
    
    IF old_password = actual_password THEN
        UPDATE user SET user_password = new_password WHERE user_id = actual_user_id;
        DELETE FROM token WHERE user_id = actual_user_id;
        INSERT INTO token(token_uuid, user_id) VALUES (token_uuid, actual_user_id);

        SET success = TRUE;
        SET token = token_uuid;
    END IF;
END //

DELIMITER ;