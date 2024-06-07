SELECT 'Loading permissions' AS 'INFO';
SOURCE dummy_data/load_permission.dump;

SELECT 'Loading users' AS 'INFO';
SOURCE dummy_data/load_user.dump;

SELECT 'Loading tokens' AS 'INFO';
SOURCE dummy_data/load_token.dump;

SELECT 'Loading suppliers' AS 'INFO';
SOURCE dummy_data/load_supplier.dump;

SELECT 'Loading categories' AS 'INFO';
SOURCE dummy_data/load_category.dump;

SELECT 'Loading products' AS 'INFO';
SOURCE dummy_data/load_product.dump;

SELECT 'Loading product variants' AS 'INFO';
SOURCE dummy_data/load_product_variant.dump;

SELECT 'Loading warehouse addresses' AS 'INFO';
SOURCE dummy_data/load_warehouse_address.dump;

SELECT 'Loading warehouses' AS 'INFO';
SOURCE dummy_data/load_warehouse.dump;

SELECT 'Loading warehouse stocks' AS 'INFO';
SOURCE dummy_data/load_warehouse_stock.dump;

SELECT 'Loading inventory audits' AS 'INFO';
SOURCE dummy_data/load_inventory_audit.dump;

SELECT 'Loading inventory audit details' AS 'INFO';
SOURCE dummy_data/load_inventory_audit_detail.dump;

SELECT 'Loading inbound shipments' AS 'INFO';
SOURCE dummy_data/load_inbound_shipment.dump;

SELECT 'Loading inbound shipment details' AS 'INFO';
SOURCE dummy_data/load_inbound_shipment_detail.dump;

SELECT 'Loading outbound shipments' AS 'INFO';
SOURCE dummy_data/load_outbound_shipment.dump;

SELECT 'Loading outbound shipment details' AS 'INFO';
SOURCE dummy_data/load_outbound_shipment_detail.dump;

SELECT 'Loading stock transfers' AS 'INFO';
SOURCE dummy_data/load_stock_transfer.dump;

SELECT 'Loading stock transfer details' AS 'INFO';
SOURCE dummy_data/load_stock_transfer_detail.dump;