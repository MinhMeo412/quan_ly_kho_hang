CREATE TABLE permission(
	permission_level INT,
	permission_name VARCHAR(32),
	permission_description TEXT,
	PRIMARY KEY (permission_level)
);

CREATE TABLE user(
	user_id INT AUTO_INCREMENT,
	user_name VARCHAR(32) NOT NULL,
	user_password VARCHAR(128) NOT NULL,
	user_full_name VARCHAR(64) NOT NULL,
	user_email VARCHAR(320),
	user_phone_number VARCHAR(24),
	permission_level INT NOT NULL,
	PRIMARY KEY (user_id),
	FOREIGN KEY (permission_level) REFERENCES permission(permission_level),
	UNIQUE (user_name, user_email, user_phone_number)
);

CREATE TABLE token(
	token_uuid VARCHAR(36),
	user_id INT NOT NULL,
	token_last_activity_timestamp DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP(),
	PRIMARY KEY (token_uuid),
	FOREIGN KEY (user_id) REFERENCES user(user_id) ON DELETE CASCADE
);

CREATE TABLE supplier(
	supplier_id INT AUTO_INCREMENT,
	supplier_name VARCHAR(32) NOT NULL,
	supplier_description TEXT,
	supplier_address TEXT,
	supplier_email VARCHAR(320),
	supplier_phone_number VARCHAR(24),
	supplier_website TEXT,
	PRIMARY KEY (supplier_id)
);

CREATE TABLE category(
	category_id INT AUTO_INCREMENT,
	category_name VARCHAR(32) NOT NULL,
	category_description TEXT,
	PRIMARY KEY (category_id),
	UNIQUE (category_name)
);

CREATE TABLE product(
	product_id INT AUTO_INCREMENT,
	product_name varchar(32) NOT NULL,
	product_description TEXT,
	product_price INT DEFAULT 0,
	category_id INT,
	PRIMARY KEY (product_id),
	FOREIGN KEY (category_id) REFERENCES category(category_id) ON DELETE SET NULL,
	UNIQUE (product_name),
	CHECK (product_price >= 0)
);

CREATE TABLE product_variant(
	product_variant_id INT AUTO_INCREMENT,
	product_id INT NOT NULL,
	product_variant_image_url TEXT,
	product_variant_color VARCHAR(32),
	product_variant_size VARCHAR(16),
	PRIMARY KEY (product_variant_id),
	FOREIGN KEY (product_id) REFERENCES product(product_id) ON DELETE CASCADE
);

CREATE TABLE warehouse_address(
	warehouse_address_id INT AUTO_INCREMENT,
	warehouse_address_address VARCHAR(128) NOT NULL,
	warehouse_address_district VARCHAR(64),
	warehouse_address_postal_code VARCHAR(16),
	warehouse_address_city VARCHAR(32),
	warehouse_address_country VARCHAR(64),
	PRIMARY KEY (warehouse_address_id)
);

CREATE TABLE warehouse(
	warehouse_id INT AUTO_INCREMENT,
	warehouse_name VARCHAR(32) NOT NULL,
	warehouse_address_id INT,
	PRIMARY KEY (warehouse_id),
	FOREIGN KEY (warehouse_address_id) REFERENCES warehouse_address(warehouse_address_id) ON DELETE SET NULL
);

CREATE TABLE warehouse_stock(
	warehouse_id INT,
	product_variant_id INT,
	warehouse_stock_quantity INT NOT NULL DEFAULT 0,
	FOREIGN KEY (warehouse_id) REFERENCES warehouse(warehouse_id),
	FOREIGN KEY (product_variant_id) REFERENCES product_variant(product_variant_id),
	PRIMARY KEY (warehouse_id, product_variant_id),
	CHECK (warehouse_stock_quantity >= 0)
);

CREATE TABLE inventory_audit(
	inventory_audit_id INT AUTO_INCREMENT,
	warehouse_id INT NOT NULL,
	user_id INT NOT NULL,
	inventory_audit_time DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP(),
	PRIMARY KEY (inventory_audit_id),
	FOREIGN KEY (warehouse_id) REFERENCES warehouse(warehouse_id) ON DELETE CASCADE,
	FOREIGN KEY (user_id) REFERENCES user(user_id)
);

CREATE TABLE inventory_audit_detail(
	inventory_audit_id INT,
	product_variant_id INT,
	inventory_audit_detail_actual_quantity INT NOT NULL DEFAULT 0,
	FOREIGN KEY (inventory_audit_id) REFERENCES inventory_audit(inventory_audit_id) ON DELETE CASCADE,
	FOREIGN KEY (product_variant_id) REFERENCES product_variant(product_variant_id) ON DELETE CASCADE,
	PRIMARY KEY (inventory_audit_id, product_variant_id),
	CHECK (inventory_audit_detail_actual_quantity >= 0)
);

CREATE TABLE inbound_shipment(
	inbound_shipment_id INT AUTO_INCREMENT,
	supplier_id INT NOT NULL,
	warehouse_id INT NOT NULL,
	inbound_shipment_starting_date DATETIME DEFAULT CURRENT_TIMESTAMP(),
	inbound_shipment_status ENUM('Processing','Completed') NOT NULL DEFAULT 'Processing',
	inbound_shipment_description TEXT,
	user_id INT NOT NULL,
	PRIMARY KEY (inbound_shipment_id),
	FOREIGN KEY (supplier_id) REFERENCES supplier(supplier_id),
	FOREIGN KEY (warehouse_id) REFERENCES warehouse(warehouse_id) ON DELETE CASCADE,
	FOREIGN KEY (user_id) REFERENCES user(user_id)
);

CREATE TABLE inbound_shipment_detail(
	inbound_shipment_id INT,
	product_variant_id INT,
	inbound_shipment_detail_amount INT NOT NULL DEFAULT 1,
	FOREIGN KEY (inbound_shipment_id) REFERENCES inbound_shipment(inbound_shipment_id) ON DELETE CASCADE,
	FOREIGN KEY (product_variant_id) REFERENCES product_variant(product_variant_id) ON DELETE CASCADE,
	PRIMARY KEY (inbound_shipment_id, product_variant_id),
	CHECK (inbound_shipment_detail_amount >= 0)
);

CREATE TABLE outbound_shipment(
	outbound_shipment_id INT AUTO_INCREMENT,
	warehouse_id INT NOT NULL,
	outbound_shipment_address TEXT NOT NULL,
	outbound_shipment_starting_date DATETIME DEFAULT CURRENT_TIMESTAMP(),
	outbound_shipment_status ENUM('Processing','Completed') NOT NULL DEFAULT 'Processing',
	outbound_shipment_description TEXT,
	user_id INT NOT NULL,
	PRIMARY KEY (outbound_shipment_id),
	FOREIGN KEY (warehouse_id) REFERENCES warehouse(warehouse_id) ON DELETE CASCADE,
	FOREIGN KEY (user_id) REFERENCES user(user_id)
);

CREATE TABLE outbound_shipment_detail(
	outbound_shipment_id INT,
	product_variant_id INT,
	outbound_shipment_detail_amount INT NOT NULL DEFAULT 1,
	FOREIGN KEY (outbound_shipment_id) REFERENCES outbound_shipment(outbound_shipment_id) ON DELETE CASCADE,
	FOREIGN KEY (product_variant_id) REFERENCES product_variant(product_variant_id) ON DELETE CASCADE,
	PRIMARY KEY (outbound_shipment_id, product_variant_id),
	CHECK (outbound_shipment_detail_amount >= 0)
);

CREATE TABLE stock_transfer(
	stock_transfer_id INT AUTO_INCREMENT,
	from_warehouse_id INT NOT NULL,
	to_warehouse_id INT NOT NULL,
	stock_transfer_starting_date DATETIME DEFAULT CURRENT_TIMESTAMP(),
	stock_transfer_status ENUM('Processing','Completed') NOT NULL DEFAULT 'Processing',
	stock_transfer_description TEXT,
	user_id INT NOT NULL,
	PRIMARY KEY (stock_transfer_id),
	FOREIGN KEY (from_warehouse_id) REFERENCES warehouse(warehouse_id) ON DELETE CASCADE,
	FOREIGN KEY (to_warehouse_id) REFERENCES warehouse(warehouse_id) ON DELETE CASCADE,
	FOREIGN KEY (user_id) REFERENCES user(user_id)
);

CREATE TABLE stock_transfer_detail(
	stock_transfer_id INT,
	product_variant_id INT,
	stock_transfer_detail_amount INT NOT NULL DEFAULT 1,
	FOREIGN KEY (stock_transfer_id) REFERENCES stock_transfer(stock_transfer_id) ON DELETE CASCADE,
	FOREIGN KEY (product_variant_id) REFERENCES product_variant(product_variant_id) ON DELETE CASCADE,
	PRIMARY KEY (stock_transfer_id, product_variant_id),
	CHECK (stock_transfer_detail_amount >= 0)
);