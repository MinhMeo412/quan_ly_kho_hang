DELIMITER //

CREATE PROCEDURE create_token(
    IN input_token VARCHAR(36),
	IN input_token_uuid VARCHAR(36),
    IN input_user_id INT,
    IN input_token_last_activity_timestamp DATETIME
)
BEGIN
    DECLARE required_level INT DEFAULT -1;
    IF sufficient_permission(input_token, required_level) THEN
        INSERT INTO token(token_uuid, user_id, token_last_activity_timestamp) VALUES (input_token_uuid, input_user_id, input_token_last_activity_timestamp);
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

-- Không user nào nên có quyền truy cập để xem user token (bỏ procedure này)
CREATE PROCEDURE read_token(
    IN input_token VARCHAR(36)
)
BEGIN
    DECLARE required_level INT DEFAULT -1;
    IF sufficient_permission(input_token, required_level) THEN
        SELECT * FROM token;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE update_token(
    IN input_token VARCHAR(36),
	IN input_token_uuid VARCHAR(36),
    IN input_user_id INT,
    IN input_token_last_activity_timestamp DATETIME
)
BEGIN
    DECLARE required_level INT DEFAULT -1;
    IF sufficient_permission(input_token, required_level) THEN
        UPDATE token
        SET user_id = input_user_id,
            token_last_activity_timestamp = input_token_last_activity_timestamp
        WHERE token_uuid = input_token_uuid;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE delete_token(
    IN input_token VARCHAR(36),
    IN input_token_uuid VARCHAR(36)
)
BEGIN
    DECLARE required_level INT DEFAULT -1;
    IF sufficient_permission(input_token, required_level) THEN
        DELETE FROM token WHERE token_uuid = input_token_uuid;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

DELIMITER ;
