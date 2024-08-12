DELIMITER //

-- Không thể tạo thêm permission level qua các user (chỉ có thể tạo qua sửa CSDL trực tiếp) và không cần thiết (bỏ procedure này / để quyền = -1)
CREATE PROCEDURE create_permission(
    IN input_token VARCHAR(36),
    IN input_permission_level INT, 
    IN input_permission_name VARCHAR(32), 
    IN input_permission_description TEXT
)
BEGIN
    DECLARE required_level INT DEFAULT -1;
    IF sufficient_permission(input_token, required_level) THEN
        INSERT INTO permission(permission_level, permission_name, permission_description) VALUES
        (input_permission_level, input_permission_name, input_permission_description);
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

-- Có thể đọc/xem danh sách các permission level nhưng không cần thiết (có thể giữ hoặc bỏ / để quyền = -1)
CREATE PROCEDURE read_permission(
    IN input_token VARCHAR(36)
)
BEGIN
    DECLARE required_level INT DEFAULT 4;
    IF sufficient_permission(input_token, required_level) THEN
        SELECT * FROM permission;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

-- Tương tự create_permission (bỏ procedure này / để quyền = -1)
CREATE PROCEDURE update_permission(
    IN input_token VARCHAR(36),
    IN input_permission_level INT,
    IN input_permission_name VARCHAR(32),
    IN input_permission_description TEXT
)
BEGIN
    DECLARE required_level INT DEFAULT -1;
    IF sufficient_permission(input_token, required_level) THEN
        UPDATE permission
        SET permission_name = input_permission_name, permission_description = input_permission_description
        WHERE permission_level = input_permission_level;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

-- Tương tự create_permission (bỏ procedure này / để quyền = -1)
CREATE PROCEDURE delete_permission(
    IN input_token VARCHAR(36),
    IN input_permission_level INT
)
BEGIN
    DECLARE required_level INT DEFAULT -1; 
    IF sufficient_permission(input_token, required_level) THEN
        DELETE FROM permission WHERE permission_level = input_permission_level;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //

DELIMITER ;