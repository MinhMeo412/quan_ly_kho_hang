DELIMITER //
CREATE PROCEDURE create_user(
    IN input_token VARCHAR(36),
    IN new_user_name VARCHAR(32),
    IN new_user_password VARCHAR(128),
    IN new_user_full_name VARCHAR(64),
    IN new_user_email VARCHAR(320),
    IN new_user_phone_number VARCHAR(24),
    IN new_permission_level INT
)
BEGIN
    DECLARE required_level INT DEFAULT 1;
    DECLARE user_exists INT;

    IF sufficient_permission(input_token, required_level) THEN
        SELECT COUNT(*) INTO user_exists
        FROM user
        WHERE user_name = new_user_name OR user_email = new_user_email OR user_phone_number = new_user_phone_number;

        IF user_exists > 0 THEN
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'User name, email or phone number already exists.';
        ELSE
            INSERT INTO user (user_name, user_password, user_full_name, user_email, user_phone_number, permission_level)
            VALUES (new_user_name, new_user_password, new_user_full_name, new_user_email, new_user_phone_number, new_permission_level);
        END IF;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //
DELIMITER ;


DELIMITER //
CREATE PROCEDURE create_supplier(
    IN input_token VARCHAR(36),
    IN new_supplier_name VARCHAR(32),
    IN new_supplier_description TEXT,
    IN new_supplier_address TEXT,
    IN new_supplier_email VARCHAR(320),
    IN new_supplier_phone_number VARCHAR(24),
    IN new_supplier_website TEXT
)
BEGIN
    DECLARE required_level INT DEFAULT 3;
    DECLARE supplier_exists INT;

    IF sufficient_permission(input_token, required_level) THEN
        SELECT COUNT(*) INTO supplier_exists
        FROM supplier
        WHERE supplier_name = new_supplier_name;
        
        IF supplier_exists > 0 THEN
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Supplier name already exists.';
        ELSE
            INSERT INTO supplier (supplier_name, supplier_description, supplier_address, supplier_email, supplier_phone_number, supplier_website)
            VALUES (new_supplier_name, new_supplier_description, new_supplier_address, new_supplier_email, new_supplier_phone_number, new_supplier_website);
        END IF;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //
DELIMITER ;


DELIMITER //
CREATE PROCEDURE create_category(
    IN input_token VARCHAR(36),
    IN new_category_name VARCHAR(32),
    IN new_category_description TEXT
)
BEGIN
    DECLARE required_level INT DEFAULT 1;
    DECLARE category_exists INT;

    IF sufficient_permission(input_token, required_level) THEN
        SELECT COUNT(*) INTO category_exists
        FROM category
        WHERE category_name = new_category_name;
        
        IF category_exists > 0 THEN
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Category name already exists.';
        ELSE
            INSERT INTO category (category_name, category_description)
            VALUES (new_category_name, new_category_description);
        END IF;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //
DELIMITER ;


DELIMITER //
CREATE PROCEDURE create_product(
    IN input_token VARCHAR(36),
    IN new_product_name VARCHAR(32),
    IN new_product_description TEXT,
    IN new_product_price INT,
    IN new_category_id INT
)
BEGIN
    DECLARE required_level INT DEFAULT 3;
    DECLARE product_exists INT;

    IF sufficient_permission(input_token, required_level) THEN
        SELECT COUNT(*) INTO product_exists
        FROM product
        WHERE product_name = new_product_name;
        
        IF product_exists > 0 THEN
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Product name already exists.';
        ELSE
            INSERT INTO product (product_name, product_description, product_price, category_id)
            VALUES (new_product_name, new_product_description, new_product_price, new_category_id);
        END IF;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //
DELIMITER ;


DELIMITER //
CREATE PROCEDURE create_product_variant(
    IN input_token VARCHAR(36),
    IN product_id INT,
    IN product_variant_image_url TEXT,
    IN product_variant_color VARCHAR(32),
    IN product_variant_size VARCHAR(16)
)
BEGIN
    DECLARE required_level INT DEFAULT 3;
    DECLARE product_variant_exists INT;

    IF sufficient_permission(input_token, required_level) THEN
        SELECT COUNT(*) INTO product_variant_exists
        FROM product_variant
        WHERE product_id = product_id
        AND product_variant_color = product_variant_color
        AND product_variant_size = product_variant_size;

        IF product_variant_exists > 0 THEN
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Product variant already exists.';
        ELSE
            INSERT INTO product_variant (product_id, product_variant_image_url, product_variant_color, product_variant_size)
            VALUES (product_id, product_variant_image_url, product_variant_color, product_variant_size);
        END IF;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //
DELIMITER ;


DELIMITER //
CREATE PROCEDURE create_warehouse(
    IN input_token VARCHAR(36),
    IN new_warehouse_name VARCHAR(32),
    IN new_warehouse_address_id INT
)
BEGIN
    DECLARE required_level INT DEFAULT 1;
    DECLARE address_exists INT;

    SELECT COUNT(*)
    INTO address_exists
    FROM warehouse_address
    WHERE warehouse_address_id = new_warehouse_address_id;

    IF sufficient_permission(input_token, required_level) THEN
        IF address_exists > 0 THEN
            INSERT INTO warehouse (warehouse_name, warehouse_address_id)
            VALUES (new_warehouse_name, new_warehouse_address_id);
        ELSE
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Warehouse address ID does not exist.';
        END IF;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //
DELIMITER ;


DELIMITER //
CREATE PROCEDURE create_warehouse_address(
    IN input_token VARCHAR(36),
    IN new_warehouse_address_address VARCHAR(128),
    IN new_warehouse_address_district VARCHAR(64),
    IN new_warehouse_address_postal_code VARCHAR(16),
    IN new_warehouse_address_city VARCHAR(32),
    IN new_warehouse_address_country VARCHAR(64)
)
BEGIN
    DECLARE required_level INT DEFAULT 1;
    DECLARE address_exists INT;

    IF sufficient_permission(input_token, required_level) THEN
        SELECT COUNT(*) INTO address_exists
        FROM warehouse_address
        WHERE warehouse_address_address = new_warehouse_address_address
        AND warehouse_address_district = new_warehouse_address_district
        AND warehouse_address_postal_code = new_warehouse_address_postal_code
        AND warehouse_address_city = new_warehouse_address_city
        AND warehouse_address_country = new_warehouse_address_country;

        IF address_exists > 0 THEN
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Warehouse address already exists.';
        ELSE
            INSERT INTO warehouse_address (warehouse_address_address, warehouse_address_district, warehouse_address_postal_code, warehouse_address_city, warehouse_address_country)
            VALUES (new_warehouse_address_address, new_warehouse_address_district, new_warehouse_address_postal_code, new_warehouse_address_city, new_warehouse_address_country);
        END IF;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //
DELIMITER ;



/*
	Create stock for product variant through inbound_shipment
	Only create if stock in warehouse not exists (first time only)
    If already exists - use Update
*/
DELIMITER //
CREATE PROCEDURE create_warehouse_stock(
    IN input_token VARCHAR(36),
    IN warehouse_id INT,
    IN product_variant_id INT,
    IN warehouse_stock_quantity INT
)
BEGIN
    DECLARE required_level INT DEFAULT 3;
    DECLARE warehouse_exists INT;
    DECLARE product_variant_exists INT;

    IF sufficient_permission(input_token, required_level) THEN
        SELECT COUNT(*) INTO warehouse_exists FROM warehouse WHERE warehouse_id = warehouse_id;
        SELECT COUNT(*) INTO product_variant_exists FROM product_variant WHERE product_variant_id = product_variant_id;

        IF warehouse_exists > 0 AND product_variant_exists > 0 THEN
            IF EXISTS (SELECT * FROM warehouse_stock WHERE warehouse_id = warehouse_id AND product_variant_id = product_variant_id) THEN
                SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Warehouse stock information already exists.';
            ELSE
                INSERT INTO warehouse_stock (warehouse_id, product_variant_id, warehouse_stock_quantity)
                VALUES (warehouse_id, product_variant_id, warehouse_stock_quantity);
            END IF;
        ELSE
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Warehouse or product variant does not exist.';
        END IF;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //
DELIMITER ;


DELIMITER //

CREATE PROCEDURE create_inventory_audit(
    IN input_token VARCHAR(36),
    IN warehouse_id INT,
    IN user_id INT
)
BEGIN
    DECLARE required_level INT DEFAULT 3;
    DECLARE warehouse_exists INT;
    DECLARE user_exists INT;

    IF sufficient_permission(input_token, required_level) THEN
        SELECT COUNT(*) INTO warehouse_exists FROM warehouse WHERE warehouse_id = warehouse_id;
        SELECT COUNT(*) INTO user_exists FROM user WHERE user_id = user_id;

        IF warehouse_exists > 0 AND user_exists > 0 THEN
            INSERT INTO inventory_audit (warehouse_id, user_id)
            VALUES (warehouse_id, user_id);
        ELSE
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Warehouse or user does not exist.';
        END IF;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //
DELIMITER ;


DELIMITER //
CREATE PROCEDURE create_inventory_audit_detail(
    IN input_token VARCHAR(36),
    IN inventory_audit_id INT,
    IN product_variant_id INT,
    IN inventory_audit_detail_actual_quantity INT
)
BEGIN
    DECLARE required_level INT DEFAULT 3;
    DECLARE audit_exists INT;
    DECLARE product_variant_exists INT;

    IF sufficient_permission(input_token, required_level) THEN
        SELECT COUNT(*) INTO audit_exists FROM inventory_audit WHERE inventory_audit_id = inventory_audit_id;
        SELECT COUNT(*) INTO product_variant_exists FROM product_variant WHERE product_variant_id = product_variant_id;

        IF audit_exists <= 0 THEN
			SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Inventory audit does not exist.';
		ELSEIF product_variant_exists <= 0 THEN
			SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Product variant does not exist.';
		ELSE
			INSERT INTO inventory_audit_detail (inventory_audit_id, product_variant_id, inventory_audit_detail_actual_quantity)
			VALUES (inventory_audit_id, product_variant_id, inventory_audit_detail_actual_quantity);
		END IF;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //
DELIMITER ;


DELIMITER //
CREATE PROCEDURE create_inbound_shipment(
    IN input_token VARCHAR(36),
    IN supplier_id INT,
    IN warehouse_id INT,
    IN inbound_shipment_description TEXT,
    IN user_id INT
)
BEGIN
    DECLARE required_level INT DEFAULT 3;
    DECLARE supplier_exists INT;
    DECLARE warehouse_exists INT;

    IF sufficient_permission(input_token, required_level) THEN
        SELECT COUNT(*) INTO supplier_exists FROM supplier WHERE supplier_id = supplier_id;
        SELECT COUNT(*) INTO warehouse_exists FROM warehouse WHERE warehouse_id = warehouse_id;

        IF supplier_exists > 0 AND warehouse_exists > 0 THEN
            INSERT INTO inbound_shipment (supplier_id, warehouse_id, inbound_shipment_description, user_id)
            VALUES (supplier_id, warehouse_id, inbound_shipment_description, user_id);
        ELSE
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Supplier or warehouse does not exist.';
        END IF;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //
DELIMITER ;


DELIMITER //
CREATE PROCEDURE create_inbound_shipment_detail(
    IN input_token VARCHAR(36),
    IN inbound_shipment_id INT,
    IN product_variant_id INT,
    IN inbound_shipment_detail_amount INT
)
BEGIN
    DECLARE required_level INT DEFAULT 3; 
    DECLARE shipment_exists INT;
    DECLARE variant_exists INT;

    IF sufficient_permission(input_token, required_level) THEN
        SELECT COUNT(*) INTO shipment_exists FROM inbound_shipment WHERE inbound_shipment_id = inbound_shipment_id;
        SELECT COUNT(*) INTO variant_exists FROM product_variant WHERE product_variant_id = product_variant_id;

        IF shipment_exists > 0 AND variant_exists > 0 THEN
            INSERT INTO inbound_shipment_detail (inbound_shipment_id, product_variant_id, inbound_shipment_detail_amount)
            VALUES (inbound_shipment_id, product_variant_id, inbound_shipment_detail_amount);
        ELSE
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Inbound shipment or product variant does not exist.';
        END IF;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //
DELIMITER ;


DELIMITER //

CREATE PROCEDURE create_outbound_shipment(
    IN input_token VARCHAR(36),
    IN warehouse_id INT,
    IN outbound_shipment_address TEXT,
    IN outbound_shipment_description TEXT,
    IN user_id INT
)
BEGIN
    DECLARE required_level INT DEFAULT 3;
    DECLARE warehouse_exists INT;

    IF sufficient_permission(input_token, required_level) THEN
        SELECT COUNT(*) INTO warehouse_exists FROM warehouse WHERE warehouse_id = warehouse_id;

        IF warehouse_exists > 0 THEN
            INSERT INTO outbound_shipment (warehouse_id, outbound_shipment_address, outbound_shipment_description, user_id)
            VALUES (warehouse_id, outbound_shipment_address, outbound_shipment_description, user_id);
        ELSE
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Warehouse does not exist.';
        END IF;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //
DELIMITER ;


DELIMITER //
CREATE PROCEDURE create_outbound_shipment_detail(
    IN input_token VARCHAR(36),
    IN outbound_shipment_id INT,
    IN product_variant_id INT,
    IN outbound_shipment_detail_amount INT
)
BEGIN
    DECLARE required_level INT DEFAULT 3;
    DECLARE shipment_exists INT;
    DECLARE variant_exists INT;

    IF sufficient_permission(input_token, required_level) THEN
        SELECT COUNT(*) INTO shipment_exists FROM outbound_shipment WHERE outbound_shipment_id = outbound_shipment_id;
        SELECT COUNT(*) INTO variant_exists FROM product_variant WHERE product_variant_id = product_variant_id;

        IF shipment_exists > 0 AND variant_exists > 0 THEN
            INSERT INTO outbound_shipment_detail (outbound_shipment_id, product_variant_id, outbound_shipment_detail_amount)
            VALUES (outbound_shipment_id, product_variant_id, outbound_shipment_detail_amount);
        ELSE
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Outbound shipment or product variant does not exist.';
        END IF;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //
DELIMITER ;


DELIMITER //
CREATE PROCEDURE create_stock_transfer(
    IN input_token VARCHAR(36),
    IN from_warehouse_id INT,
    IN to_warehouse_id INT,
    IN stock_transfer_description TEXT,
    IN user_id INT
)
BEGIN
    DECLARE required_level INT DEFAULT 3;
    DECLARE from_warehouse_exists INT;
    DECLARE to_warehouse_exists INT;

    IF sufficient_permission(input_token, required_level) THEN
        SELECT COUNT(*) INTO from_warehouse_exists FROM warehouse WHERE warehouse_id = from_warehouse_id;
        SELECT COUNT(*) INTO to_warehouse_exists FROM warehouse WHERE warehouse_id = to_warehouse_id;

        IF from_warehouse_exists > 0 AND to_warehouse_exists > 0 THEN
            INSERT INTO stock_transfer (from_warehouse_id, to_warehouse_id, stock_transfer_description, user_id)
            VALUES (from_warehouse_id, to_warehouse_id, stock_transfer_description, user_id);
        ELSE
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'From warehouse or to warehouse does not exist.';
        END IF;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //
DELIMITER ;


DELIMITER //
CREATE PROCEDURE create_stock_transfer_detail(
    IN input_token VARCHAR(36),
    IN stock_transfer_id INT,
    IN product_variant_id INT,
    IN stock_transfer_detail_amount INT
)
BEGIN
    DECLARE required_level INT DEFAULT 3;
    DECLARE transfer_exists INT;
    DECLARE variant_exists INT;

    IF sufficient_permission(input_token, required_level) THEN
        SELECT COUNT(*) INTO transfer_exists FROM stock_transfer WHERE stock_transfer_id = stock_transfer_id;
        SELECT COUNT(*) INTO variant_exists FROM product_variant WHERE product_variant_id = product_variant_id;

        IF transfer_exists > 0 AND variant_exists > 0 THEN
            INSERT INTO stock_transfer_detail (stock_transfer_id, product_variant_id, stock_transfer_detail_amount)
            VALUES (stock_transfer_id, product_variant_id, stock_transfer_detail_amount);
        ELSE
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Stock transfer or product variant does not exist.';
        END IF;
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Insufficient permission.';
    END IF;
END //
DELIMITER ;
