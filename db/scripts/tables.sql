CREATE TABLE permission(
	permission_level INT,
	permission_name VARCHAR(32),
	permission_description TEXT,
	CONSTRAINT pk_permission_level PRIMARY KEY (permission_level)
);

CREATE TABLE user(
	user_id INT AUTO_INCREMENT,
	user_name VARCHAR(32) NOT NULL,
	user_password VARCHAR(128) NOT NULL,
	user_full_name VARCHAR(64),
	user_email VARCHAR(320),
	user_phone_number VARCHAR(24),
	user_permission_level INT NOT NULL,
	CONSTRAINT pk_user_id PRIMARY KEY (user_id),
	CONSTRAINT fk_user_permission_level FOREIGN KEY (user_permission_level) REFERENCES permission(permission_level),
	CONSTRAINT uc_user_name_email_phone_number UNIQUE (user_name, user_email, user_phone_number)
);

CREATE TABLE token(
	token_uuid VARCHAR(36) NOT NULL,
	token_user_id INT NOT NULL,
	token_last_activity_timestamp DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP(),
	CONSTRAINT pk_token_uuid PRIMARY KEY (token_uuid),
	CONSTRAINT fk_token_user_id FOREIGN KEY (token_user_id) REFERENCES user(user_id) ON DELETE CASCADE
);

CREATE TABLE supplier(
	supplier_id INT AUTO_INCREMENT,
	supplier_name VARCHAR(32) NOT NULL,
	supplier_description TEXT,
	supplier_address TEXT,
	supplier_email VARCHAR(320),
	supplier_phone_number VARCHAR(24),
	supplier_website TEXT,
	CONSTRAINT pk_supplier_id PRIMARY KEY (supplier_id)
);

CREATE TABLE category(
	category_id INT AUTO_INCREMENT,
	category_name VARCHAR(32) NOT NULL,
	category_description TEXT,
	CONSTRAINT pk_category_id PRIMARY KEY (category_id),
	CONSTRAINT uc_category_name UNIQUE (category_name)
);

CREATE TABLE product(
	product_id INT AUTO_INCREMENT,
	product_name varchar(32) NOT NULL,
	product_description TEXT,
	product_price INT DEFAULT 0,
	product_category_id INT,
	CONSTRAINT pk_product_id PRIMARY KEY (product_id),
	CONSTRAINT fk_product_category_id FOREIGN KEY (product_category_id) REFERENCES category(category_id) ON DELETE SET NULL,
	CONSTRAINT uc_product_name UNIQUE (product_name),
	CONSTRAINT ck_product_price CHECK (product_price >= 0)
);

CREATE TABLE product_variant(
	product_variant_id INT AUTO_INCREMENT,
	product_variant_product_id INT NOT NULL,
	product_variant_image_url TEXT,
	product_variant_color VARCHAR(32),
	product_variant_size VARCHAR(16),
	CONSTRAINT pk_product_variant_id PRIMARY KEY (product_variant_id),
	CONSTRAINT fk_product_variant_product_id FOREIGN KEY (product_variant_product_id) REFERENCES product(product_id) ON DELETE CASCADE
);

CREATE TABLE warehouse_address(
	warehouse_address_id INT AUTO_INCREMENT,
	warehouse_address_address VARCHAR(128) NOT NULL,
	warehouse_address_district VARCHAR(64),
	warehouse_address_postal_code VARCHAR(16),
	warehouse_address_city VARCHAR(32),
	warehouse_address_country VARCHAR(64),
	CONSTRAINT pk_warehouse_address_id PRIMARY KEY (warehouse_address_id)
);

CREATE TABLE warehouse(
	warehouse_id INT AUTO_INCREMENT,
	warehouse_name VARCHAR(32) NOT NULL,
	warehouse_warehouse_address_id INT NOT NULL,
	CONSTRAINT pk_warehouse_id PRIMARY KEY (warehouse_id),
	CONSTRAINT fk_warehouse_warehouse_address_id FOREIGN KEY (warehouse_warehouse_address_id) REFERENCES warehouse_address(warehouse_address_id)
);

CREATE TABLE warehouse_stock(
	warehouse_stock_warehouse_id INT,
	warehouse_stock_product_variant_id INT,
	warehouse_stock_quantity INT NOT NULL DEFAULT 0,
	CONSTRAINT fk_warehouse_stock_warehouse_id FOREIGN KEY (warehouse_stock_warehouse_id) REFERENCES warehouse(warehouse_id) ON DELETE CASCADE,
	CONSTRAINT fk_warehouse_stock_product_variant_id FOREIGN KEY (warehouse_stock_product_variant_id) REFERENCES product_variant(product_variant_id) ON DELETE CASCADE,
	CONSTRAINT pk_warehouse_stock_warehouse_id_product_variant_id PRIMARY KEY (warehouse_stock_warehouse_id, warehouse_stock_product_variant_id),
	CONSTRAINT ck_warehouse_stock_quantity CHECK (warehouse_stock_quantity >= 0)
);

CREATE TABLE inventory_audit(
	inventory_audit_id INT AUTO_INCREMENT,
	inventory_audit_warehouse_id INT NOT NULL,
	inventory_audit_user_id INT,
	inventory_audit_time DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP(),
	CONSTRAINT pk_inventory_audit_id PRIMARY KEY (inventory_audit_id),
	CONSTRAINT fk_inventory_audit_warehouse_id FOREIGN KEY (inventory_audit_warehouse_id) REFERENCES warehouse(warehouse_id) ON DELETE CASCADE,
	CONSTRAINT fk_inventory_audit_user_id FOREIGN KEY (inventory_audit_user_id) REFERENCES user(user_id) ON DELETE SET NULL
);

CREATE TABLE inventory_audit_detail(
	inventory_audit_detail_inventory_audit_id INT,
	inventory_audit_detail_product_variant_id INT,
	inventory_audit_detail_actual_quantity INT NOT NULL DEFAULT 0,
	CONSTRAINT fk_inventory_audit_detail_inventory_audit_id FOREIGN KEY (inventory_audit_detail_inventory_audit_id) REFERENCES inventory_audit(inventory_audit_id) ON DELETE CASCADE,
	CONSTRAINT fk_inventory_audit_detail_product_variant_id FOREIGN KEY (inventory_audit_detail_product_variant_id) REFERENCES product_variant(product_variant_id) ON DELETE CASCADE,
	CONSTRAINT pk_inventory_audit_detail_inventory_audit_id_product_variant_id PRIMARY KEY (inventory_audit_detail_inventory_audit_id, inventory_audit_detail_product_variant_id),
	CONSTRAINT ck_inventory_audit_detail_actual_quantity CHECK (inventory_audit_detail_actual_quantity >= 0)
);

CREATE TABLE inbound_shipment(
	inbound_shipment_id INT AUTO_INCREMENT,
	inbound_shipment_supplier_id INT,
	inbound_shipment_warehouse_id INT NOT NULL,
	inbound_shipment_starting_date DATETIME DEFAULT CURRENT_TIMESTAMP(),
	inbound_shipment_status VARCHAR(16) NOT NULL,
	inbound_shipment_description TEXT,
	inbound_shipment_user_id INT,
	CONSTRAINT pk_inbound_shipment_id PRIMARY KEY (inbound_shipment_id),
	CONSTRAINT fk_inbound_shipment_supplier_id FOREIGN KEY (inbound_shipment_supplier_id) REFERENCES supplier(supplier_id) ON DELETE SET NULL,
	CONSTRAINT fk_inbound_shipment_warehouse_id FOREIGN KEY (inbound_shipment_warehouse_id) REFERENCES warehouse(warehouse_id) ON DELETE CASCADE,
	CONSTRAINT fk_inbound_shipment_user_id FOREIGN KEY (inbound_shipment_user_id) REFERENCES user(user_id) ON DELETE SET NULL
);

CREATE TABLE inbound_shipment_detail(
	inbound_shipment_detail_inbound_shipment_id INT,
	inbound_shipment_detail_product_variant_id INT,
	inbound_shipment_detail_amount INT NOT NULL DEFAULT 1,
	CONSTRAINT fk_inbound_shipment_detail_inbound_shipment_id FOREIGN KEY (inbound_shipment_detail_inbound_shipment_id) REFERENCES inbound_shipment(inbound_shipment_id) ON DELETE CASCADE,
	CONSTRAINT fk_inbound_shipment_detail_product_variant_id FOREIGN KEY (inbound_shipment_detail_product_variant_id) REFERENCES product_variant(product_variant_id) ON DELETE CASCADE,
	CONSTRAINT pk_inbound_shipment_detail_inbound_shipment_id_product_variant_i PRIMARY KEY (inbound_shipment_detail_inbound_shipment_id, inbound_shipment_detail_product_variant_id),
	CONSTRAINT ck_inbound_shipment_detail_amount CHECK (inbound_shipment_detail_amount >= 0)
);

CREATE TABLE outbound_shipment(
	outbound_shipment_id INT AUTO_INCREMENT,
	outbound_shipment_warehouse_id INT NOT NULL,
	outbound_shipment_address TEXT NOT NULL,
	outbound_shipment_starting_date DATETIME DEFAULT CURRENT_TIMESTAMP(),
	outbound_shipment_status VARCHAR(16) NOT NULL,
	outbound_shipment_description TEXT,
	outbound_shipment_user_id INT,
	CONSTRAINT pk_outbound_shipment_id PRIMARY KEY (outbound_shipment_id),
	CONSTRAINT fk_outbound_shipment_warehouse_id FOREIGN KEY (outbound_shipment_warehouse_id) REFERENCES warehouse(warehouse_id) ON DELETE CASCADE,
	CONSTRAINT fk_outbound_shipment_user_id FOREIGN KEY (outbound_shipment_user_id) REFERENCES user(user_id) ON DELETE SET NULL
);

CREATE TABLE outbound_shipment_detail(
	outbound_shipment_detail_outbound_shipment_id INT,
	outbound_shipment_detail_product_variant_id INT,
	outbound_shipment_detail_amount INT NOT NULL DEFAULT 1,
	CONSTRAINT fk_outbound_shipment_detail_outbound_shipment_id FOREIGN KEY (outbound_shipment_detail_outbound_shipment_id) REFERENCES outbound_shipment(outbound_shipment_id) ON DELETE CASCADE,
	CONSTRAINT fk_outbound_shipment_detail_product_variant_id FOREIGN KEY (outbound_shipment_detail_product_variant_id) REFERENCES product_variant(product_variant_id) ON DELETE CASCADE,
	CONSTRAINT pk_outbound_shipment_detail_outbound_shipment_id_product_variant PRIMARY KEY (outbound_shipment_detail_outbound_shipment_id, outbound_shipment_detail_product_variant_id),
	CONSTRAINT ck_outbound_shipment_detail_amount CHECK (outbound_shipment_detail_amount >= 0)
);

CREATE TABLE stock_transfer(
	stock_transfer_id INT AUTO_INCREMENT,
	stock_transfer_from_warehouse_id INT NOT NULL,
	stock_transfer_to_warehouse_id INT NOT NULL,
	stock_transfer_starting_date DATETIME DEFAULT CURRENT_TIMESTAMP(),
	stock_transfer_status VARCHAR(16) NOT NULL,
	stock_transfer_description TEXT,
	stock_transfer_user_id INT,
	CONSTRAINT pk_stock_transfer_id PRIMARY KEY (stock_transfer_id),
	CONSTRAINT fk_stock_transfer_from_warehouse_id FOREIGN KEY (stock_transfer_from_warehouse_id) REFERENCES warehouse(warehouse_id) ON DELETE CASCADE,
	CONSTRAINT fk_stock_transfer_to_warehouse_id FOREIGN KEY (stock_transfer_to_warehouse_id) REFERENCES warehouse(warehouse_id) ON DELETE CASCADE,
	CONSTRAINT fk_stock_transfer_user_id FOREIGN KEY (stock_transfer_user_id) REFERENCES user(user_id) ON DELETE SET NULL
);

CREATE TABLE stock_transfer_detail(
	stock_transfer_detail_stock_transfer_id INT,
	stock_transfer_detail_product_variant_id INT,
	stock_transfer_detail_amount INT NOT NULL DEFAULT 1,
	CONSTRAINT fk_stock_transfer_detail_stock_transfer_id FOREIGN KEY (stock_transfer_detail_stock_transfer_id) REFERENCES stock_transfer(stock_transfer_id) ON DELETE CASCADE,
	CONSTRAINT fk_stock_transfer_detail_product_variant_id FOREIGN KEY (stock_transfer_detail_product_variant_id) REFERENCES product_variant(product_variant_id) ON DELETE CASCADE,
	CONSTRAINT pk_stock_transfer_detail_stock_transfer_id_product_variant_id PRIMARY KEY (stock_transfer_detail_stock_transfer_id, stock_transfer_detail_product_variant_id),
	CONSTRAINT ck_stock_transfer_detail_amount CHECK (stock_transfer_detail_amount >= 0)
);