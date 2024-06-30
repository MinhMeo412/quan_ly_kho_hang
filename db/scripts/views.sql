CREATE VIEW v_user_blank_password AS
SELECT 
    user_id,
    user_name,
    '' AS user_password,
    user_full_name,
    user_email,
    user_phone_number,
    permission_level
FROM user;