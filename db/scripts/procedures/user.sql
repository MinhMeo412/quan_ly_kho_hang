DELIMITER //

CREATE PROCEDURE create_user(
    IN input_token VARCHAR(36),
    IN input_user_id INT, 
    IN input_user_name VARCHAR(32), 
    IN input_user_password VARCHAR(128), 
    IN input_user_full_name VARCHAR(64), 
    IN input_user_email VARCHAR(320), 
    IN input_user_phone_number VARCHAR(24),
    IN input_permission_level INT
)
BEGIN
    DECLARE required_level INT DEFAULT 1;
    IF sufficient_permission(input_token, required_level) THEN
        INSERT INTO user(user_id, user_name, user_password, user_full_name, user_email, user_phone_number, permission_level) VALUES
        (input_user_id, input_user_name, input_user_password, input_user_full_name, input_user_email, input_user_phone_number, input_permission_level);
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

-- Xem danh sách user có bao gồm (0,1,2)
CREATE PROCEDURE read_user(IN input_token VARCHAR(36))
BEGIN
    DECLARE required_level INT DEFAULT 2;
    IF sufficient_permission(input_token, required_level) THEN
        SELECT * FROM v_user_blank_password;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

-- Quyền hạn update user thay đổi theo mức level, tất cả các level thay đổi được password còn 0,1 sửa được thông tin user
CREATE PROCEDURE update_user(
    IN input_token VARCHAR(36),
    IN input_user_id INT, 
    IN input_user_name VARCHAR(32), 
    IN input_user_password VARCHAR(128), 
    IN input_user_full_name VARCHAR(64), 
    IN input_user_email VARCHAR(320), 
    IN input_user_phone_number VARCHAR(24),
    IN input_permission_level INT
)
BEGIN
    DECLARE required_level INT DEFAULT 1;
    IF sufficient_permission(input_token, required_level) THEN
        UPDATE user
        SET user_name = input_user_name,
            user_full_name = input_user_full_name,
            user_email = input_user_email,
            user_phone_number = input_user_phone_number,
            permission_level = input_permission_level
        WHERE user_id = input_user_id;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

CREATE PROCEDURE delete_user(IN input_token VARCHAR(36), IN input_user_id INT)
BEGIN
    DECLARE required_level INT DEFAULT 0;
    IF sufficient_permission(input_token, required_level) THEN
        DELETE FROM user WHERE user_id = input_user_id;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

DELIMITER ;