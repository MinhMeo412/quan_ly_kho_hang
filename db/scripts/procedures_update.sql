-- Quyền hạn update user thay đổi theo mức level, tất cả các level thay đổi được password còn 0,1 sửa được thông tin user
DELIMITER //
CREATE PROCEDURE update_user(
    IN input_token VARCHAR(36),
    IN user_id INT,
    IN new_user_name VARCHAR(32),
    IN new_user_full_name VARCHAR(64),
    IN new_user_email VARCHAR(320),
    IN new_user_phone_number VARCHAR(24),
    IN new_user_password VARCHAR(128)
)
BEGIN
    DECLARE required_level INT DEFAULT 2;
    DECLARE current_permission_level INT;

    SET current_permission_level = get_permission_level(input_token);

    IF current_permission_level > required_level THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission to update user information.';
    ELSE
        UPDATE user
        SET user_name = new_user_name,
            user_full_name = new_user_full_name,
            user_email = new_user_email,
            user_phone_number = new_user_phone_number
        WHERE user_id = user_id;

        IF new_user_password IS NOT NULL THEN
            UPDATE user
            SET user_password = new_user_password
            WHERE user_id = user_id;
        END IF;
    END IF;
END //
DELIMITER ;


DELIMITER //
CREATE PROCEDURE update_supplier(
    IN input_token VARCHAR(36),
    IN supplier_id INT,
    IN new_supplier_name VARCHAR(32),
    IN new_supplier_description TEXT,
    IN new_supplier_address TEXT,
    IN new_supplier_email VARCHAR(320),
    IN new_supplier_phone_number VARCHAR(24),
    IN new_supplier_website TEXT
)
BEGIN
    DECLARE required_level INT DEFAULT 2;
    DECLARE current_permission_level INT;

    SET current_permission_level = get_permission_level(input_token);

    IF current_permission_level > required_level THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission to update supplier information.';
    ELSE
        -- Cập nhật thông tin nhà cung cấp
        UPDATE supplier
        SET supplier_name = new_supplier_name,
            supplier_description = new_supplier_description,
            supplier_address = new_supplier_address,
            supplier_email = new_supplier_email,
            supplier_phone_number = new_supplier_phone_number,
            supplier_website = new_supplier_website
        WHERE supplier_id = supplier_id;
    END IF;
END //
DELIMITER ;


DELIMITER //
CREATE PROCEDURE update_category(
    IN input_token VARCHAR(36),
    IN category_id INT,
    IN new_category_name VARCHAR(32),
    IN new_category_description TEXT
)
BEGIN
    DECLARE required_level INT DEFAULT 2;
    DECLARE current_permission_level INT;

    SET current_permission_level = get_permission_level(input_token);

    IF current_permission_level > required_level THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission to update category information.';
    ELSE
        UPDATE category
        SET category_name = new_category_name,
            category_description = new_category_description
        WHERE category_id = category_id;
    END IF;
END //
DELIMITER ;


DELIMITER //
CREATE PROCEDURE update_product(
    IN input_token VARCHAR(36),
    IN product_id INT,
    IN new_product_name VARCHAR(32),
    IN new_product_description TEXT,
    IN new_product_price INT,
    IN new_category_id INT
)
BEGIN
    DECLARE required_level INT DEFAULT 2;
    DECLARE current_permission_level INT;

    SET current_permission_level = get_permission_level(input_token);

    IF current_permission_level > required_level THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission to update product information.';
    ELSE
        UPDATE product
        SET product_name = new_product_name,
            product_description = new_product_description,
            product_price = new_product_price,
            category_id = new_category_id
        WHERE product_id = product_id;
    END IF;
END //
DELIMITER ;


DELIMITER //
CREATE PROCEDURE update_product_variant(
    IN input_token VARCHAR(36),
    IN product_variant_id INT,
    IN new_product_variant_image_url TEXT,
    IN new_product_variant_color VARCHAR(32),
    IN new_product_variant_size VARCHAR(16)
)
BEGIN
    DECLARE required_level INT DEFAULT 2;
    DECLARE current_permission_level INT;

    SET current_permission_level = get_permission_level(input_token);

    IF current_permission_level > required_level THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission to update product variant information.';
    ELSE
        UPDATE product_variant
        SET product_variant_image_url = new_product_variant_image_url,
            product_variant_color = new_product_variant_color,
            product_variant_size = new_product_variant_size
        WHERE product_variant_id = product_variant_id;
    END IF;
END //
DELIMITER ;


DELIMITER //
CREATE PROCEDURE update_warehouse(
    IN input_token VARCHAR(36),
    IN warehouse_id INT,
    IN new_warehouse_name VARCHAR(32),
    IN new_warehouse_address_id INT
)
BEGIN
    DECLARE required_level INT DEFAULT 1;
    DECLARE current_permission_level INT;

    SET current_permission_level = get_permission_level(input_token);

    IF current_permission_level > required_level THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission to update warehouse information.';
    ELSE
        UPDATE warehouse
        SET warehouse_name = new_warehouse_name,
            warehouse_address_id = new_warehouse_address_id
        WHERE warehouse_id = warehouse_id;
    END IF;
END //
DELIMITER ;


DELIMITER //
CREATE PROCEDURE update_warehouse_address(
    IN input_token VARCHAR(36),
    IN warehouse_address_id INT,
    IN new_warehouse_address_address VARCHAR(128),
    IN new_warehouse_address_district VARCHAR(64),
    IN new_warehouse_address_postal_code VARCHAR(16),
    IN new_warehouse_address_city VARCHAR(32),
    IN new_warehouse_address_country VARCHAR(64)
)
BEGIN
    DECLARE required_level INT DEFAULT 1;
    DECLARE current_permission_level INT;

    SET current_permission_level = get_permission_level(input_token);

    IF current_permission_level > required_level THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission to update warehouse address information.';
    ELSE
        UPDATE warehouse_address
        SET warehouse_address_address = new_warehouse_address_address,
            warehouse_address_district = new_warehouse_address_district,
            warehouse_address_postal_code = new_warehouse_address_postal_code,
            warehouse_address_city = new_warehouse_address_city,
            warehouse_address_country = new_warehouse_address_country
        WHERE warehouse_address_id = warehouse_address_id;
    END IF;
END //
DELIMITER ;


DELIMITER //
CREATE PROCEDURE update_warehouse_stock(
    IN input_token VARCHAR(36),
    IN warehouse_id INT,
    IN product_variant_id INT,
    IN new_warehouse_stock_quantity INT
)
BEGIN
    DECLARE required_level INT DEFAULT 2;
    DECLARE current_permission_level INT;
    DECLARE warehouse_exists INT;
    DECLARE product_variant_exists INT;
    DECLARE stock_exists INT;

    -- Check user permission level
    SET current_permission_level = get_permission_level(input_token);

    IF current_permission_level > required_level THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission to update warehouse stock information.';
    ELSE
        SELECT COUNT(*) INTO warehouse_exists
        FROM warehouse
        WHERE warehouse_id = warehouse_id;

        SELECT COUNT(*) INTO product_variant_exists
        FROM product_variant
        WHERE product_variant_id = product_variant_id;

        IF warehouse_exists = 0 THEN
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Warehouse does not exist.';
        ELSEIF product_variant_exists = 0 THEN
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Product variant does not exist.';
        ELSE
            SELECT COUNT(*) INTO stock_exists
            FROM warehouse_stock
            WHERE warehouse_id = warehouse_id AND product_variant_id = product_variant_id;

            IF stock_exists > 0 THEN
                UPDATE warehouse_stock
                SET warehouse_stock_quantity = warehouse_stock_quantity + new_warehouse_stock_quantity
                WHERE warehouse_id = warehouse_id AND product_variant_id = product_variant_id;
            ELSE
                INSERT INTO warehouse_stock (warehouse_id, product_variant_id, warehouse_stock_quantity)
                VALUES (warehouse_id, product_variant_id, new_warehouse_stock_quantity);
            END IF;
        END IF;
    END IF;
END //
DELIMITER ;


DELIMITER //
CREATE PROCEDURE update_inventory_audit(
    IN input_token VARCHAR(36),
    IN inventory_audit_id INT,
    IN new_warehouse_id INT,
    IN new_user_id INT,
    IN new_inventory_audit_time DATETIME
)
BEGIN
    DECLARE required_level INT DEFAULT 3;
    DECLARE current_permission_level INT;

    SET current_permission_level = get_permission_level(input_token);

    IF current_permission_level > required_level THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission to update inventory audit information.';
    ELSE
        UPDATE inventory_audit
        SET warehouse_id = new_warehouse_id,
            user_id = new_user_id,
            inventory_audit_time = new_inventory_audit_time
        WHERE inventory_audit_id = inventory_audit_id;
    END IF;
END //
DELIMITER ;


DELIMITER //

CREATE PROCEDURE update_inventory_audit_detail(
    IN input_token VARCHAR(36),
    IN inventory_audit_id INT,
    IN product_variant_id INT,
    IN new_inventory_audit_detail_actual_quantity INT
)
BEGIN
    DECLARE required_level INT DEFAULT 3;
    DECLARE current_permission_level INT;
	DECLARE audit_exists INT;
	DECLARE product_variant_exists INT;

    SET current_permission_level = get_permission_level(input_token);

    IF current_permission_level > required_level THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission to update inventory audit detail.';
    ELSE
        SELECT COUNT(*) INTO audit_exists
        FROM inventory_audit
        WHERE inventory_audit_id = inventory_audit_id;

        SELECT COUNT(*) INTO product_variant_exists
        FROM product_variant
        WHERE product_variant_id = product_variant_id;

        IF audit_exists = 0 THEN
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Inventory audit does not exist.';
        ELSEIF product_variant_exists = 0 THEN
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Product variant does not exist.';
        ELSE
            UPDATE inventory_audit_detail
            SET inventory_audit_detail_actual_quantity = new_inventory_audit_detail_actual_quantity
            WHERE inventory_audit_id = inventory_audit_id AND product_variant_id = product_variant_id;
        END IF;
    END IF;
END //
DELIMITER ;


DELIMITER //
CREATE PROCEDURE update_inbound_shipment(
    IN input_token VARCHAR(36),
    IN inbound_shipment_id INT,
    IN new_supplier_id INT,
    IN new_warehouse_id INT,
    IN new_inbound_shipment_starting_date DATETIME,
    IN new_inbound_shipment_status ENUM('Processing','Completed'),
    IN new_inbound_shipment_description TEXT,
    IN new_user_id INT
)
BEGIN
    DECLARE required_level INT DEFAULT 3;
    DECLARE current_permission_level INT;

    SET current_permission_level = get_permission_level(input_token);

    IF current_permission_level > required_level THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission to update inbound shipment information.';
    ELSE
        UPDATE inbound_shipment
        SET supplier_id = new_supplier_id,
            warehouse_id = new_warehouse_id,
            inbound_shipment_starting_date = new_inbound_shipment_starting_date,
            inbound_shipment_status = new_inbound_shipment_status,
            inbound_shipment_description = new_inbound_shipment_description,
            user_id = new_user_id
        WHERE inbound_shipment_id = inbound_shipment_id;
    END IF;
END //
DELIMITER ;


DELIMITER //
CREATE PROCEDURE update_inbound_shipment_detail(
    IN input_token VARCHAR(36),
    IN inbound_shipment_id INT,
    IN product_variant_id INT,
    IN new_inbound_shipment_detail_amount INT
)
BEGIN
    DECLARE required_level INT DEFAULT 3;
    DECLARE current_permission_level INT;
	DECLARE shipment_exists INT;
	DECLARE product_variant_exists INT;

    SET current_permission_level = get_permission_level(input_token);

    IF current_permission_level > required_level THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission to update inbound shipment detail.';
    ELSE
        SELECT COUNT(*) INTO shipment_exists
        FROM inbound_shipment
        WHERE inbound_shipment_id = inbound_shipment_id;

        SELECT COUNT(*) INTO product_variant_exists
        FROM product_variant
        WHERE product_variant_id = product_variant_id;

        IF shipment_exists = 0 THEN
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Inbound shipment does not exist.';
        ELSEIF product_variant_exists = 0 THEN
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Product variant does not exist.';
        ELSE
            UPDATE inbound_shipment_detail
            SET inbound_shipment_detail_amount = new_inbound_shipment_detail_amount
            WHERE inbound_shipment_id = inbound_shipment_id AND product_variant_id = product_variant_id;
        END IF;
    END IF;
END //
DELIMITER ;


DELIMITER //
CREATE PROCEDURE update_outbound_shipment(
    IN input_token VARCHAR(36),
    IN outbound_shipment_id INT,
    IN new_warehouse_id INT,
    IN new_outbound_shipment_address TEXT,
    IN new_outbound_shipment_starting_date DATETIME,
    IN new_outbound_shipment_status ENUM('Processing','Completed'),
    IN new_outbound_shipment_description TEXT,
    IN new_user_id INT
)
BEGIN
    DECLARE required_level INT DEFAULT 3;
    DECLARE current_permission_level INT;

    SET current_permission_level = get_permission_level(input_token);

    IF current_permission_level > required_level THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission to update outbound shipment information.';
    ELSE
        UPDATE outbound_shipment
        SET warehouse_id = new_warehouse_id,
            outbound_shipment_address = new_outbound_shipment_address,
            outbound_shipment_starting_date = new_outbound_shipment_starting_date,
            outbound_shipment_status = new_outbound_shipment_status,
            outbound_shipment_description = new_outbound_shipment_description,
            user_id = new_user_id
        WHERE outbound_shipment_id = outbound_shipment_id;
    END IF;
END //
DELIMITER ;


DELIMITER //

CREATE PROCEDURE update_outbound_shipment_detail(
    IN input_token VARCHAR(36),
    IN outbound_shipment_id INT,
    IN product_variant_id INT,
    IN new_outbound_shipment_detail_amount INT
)
BEGIN
    DECLARE required_level INT DEFAULT 3;
    DECLARE current_permission_level INT;
	DECLARE shipment_exists INT;
	DECLARE product_variant_exists INT;
    
    SET current_permission_level = get_permission_level(input_token);

    IF current_permission_level > required_level THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission to update outbound shipment detail.';
    ELSE
        SELECT COUNT(*) INTO shipment_exists
        FROM outbound_shipment
        WHERE outbound_shipment_id = outbound_shipment_id;

        SELECT COUNT(*) INTO product_variant_exists
        FROM product_variant
        WHERE product_variant_id = product_variant_id;

        IF shipment_exists = 0 THEN
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Outbound shipment does not exist.';
        ELSEIF product_variant_exists = 0 THEN
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Product variant does not exist.';
        ELSE
            UPDATE outbound_shipment_detail
            SET outbound_shipment_detail_amount = new_outbound_shipment_detail_amount
            WHERE outbound_shipment_id = outbound_shipment_id AND product_variant_id = product_variant_id;
        END IF;
    END IF;
END //
DELIMITER ;


DELIMITER //
CREATE PROCEDURE update_stock_transfer(
    IN input_token VARCHAR(36),
    IN stock_transfer_id INT,
    IN new_from_warehouse_id INT,
    IN new_to_warehouse_id INT,
    IN new_stock_transfer_starting_date DATETIME,
    IN new_stock_transfer_status ENUM('Processing','Completed'),
    IN new_stock_transfer_description TEXT,
    IN new_user_id INT
)
BEGIN
    DECLARE required_level INT DEFAULT 3;
    DECLARE current_permission_level INT;

    SET current_permission_level = get_permission_level(input_token);

    IF current_permission_level > required_level THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission to update stock transfer information.';
    ELSE
        -- Cập nhật thông tin chuyển kho
        UPDATE stock_transfer
        SET from_warehouse_id = new_from_warehouse_id,
            to_warehouse_id = new_to_warehouse_id,
            stock_transfer_starting_date = new_stock_transfer_starting_date,
            stock_transfer_status = new_stock_transfer_status,
            stock_transfer_description = new_stock_transfer_description,
            user_id = new_user_id
        WHERE stock_transfer_id = stock_transfer_id;
    END IF;
END //
DELIMITER ;


DELIMITER //
CREATE PROCEDURE update_stock_transfer_detail(
    IN input_token VARCHAR(36),
    IN stock_transfer_id INT,
    IN product_variant_id INT,
    IN new_stock_transfer_detail_amount INT
)
BEGIN
    DECLARE required_level INT DEFAULT 3;
    DECLARE current_permission_level INT;
	DECLARE transfer_exists INT;
	DECLARE product_variant_exists INT;

    SET current_permission_level = get_permission_level(input_token);

    IF current_permission_level > required_level THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission to update stock transfer detail.';
    ELSE
        SELECT COUNT(*) INTO transfer_exists
        FROM stock_transfer
        WHERE stock_transfer_id = stock_transfer_id;

        SELECT COUNT(*) INTO product_variant_exists
        FROM product_variant
        WHERE product_variant_id = product_variant_id;

        IF transfer_exists = 0 THEN
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Stock transfer does not exist.';
        ELSEIF product_variant_exists = 0 THEN
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Product variant does not exist.';
        ELSE
            UPDATE stock_transfer_detail
            SET stock_transfer_detail_amount = new_stock_transfer_detail_amount
            WHERE stock_transfer_id = stock_transfer_id AND product_variant_id = product_variant_id;
        END IF;
    END IF;
END //
DELIMITER ;
