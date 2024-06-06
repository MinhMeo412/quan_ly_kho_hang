SELECT 'Loading permissions' as 'INFO';
SOURCE dummy_data/load_permission.dump;

SELECT 'Loading users' as 'INFO';
SOURCE dummy_data/load_user.dump;

SELECT 'Loading tokens' as 'INFO';
SOURCE dummy_data/load_token.dump;

SELECT 'Loading suppliers' as 'INFO';
SOURCE dummy_data/load_supplier.dump;

SELECT 'Loading categories' as 'INFO';
SOURCE dummy_data/load_category.dump;

SELECT 'Loading products' as 'INFO';
SOURCE dummy_data/load_product.dump;

SELECT 'Loading product variants' as 'INFO';
SOURCE dummy_data/load_product_variant.dump;

SELECT 'Loading warehouse addresses' as 'INFO';
SOURCE dummy_data/load_warehouse_address.dump;

SELECT 'Loading warehouses' as 'INFO';
SOURCE dummy_data/load_warehouse.dump;

SELECT 'Loading warehouse stocks' as 'INFO';
SOURCE dummy_data/load_warehouse_stock.dump;

SELECT 'Loading inventory audits' as 'INFO';
SOURCE dummy_data/load_inventory_audit.dump;

SELECT 'Loading inventory audit details' as 'INFO';
SOURCE dummy_data/load_inventory_audit_detail.dump;

SELECT 'Loading inbound shipments' as 'INFO';
SOURCE dummy_data/load_inbound_shipment.dump;

SELECT 'Loading inbound shipment details' as 'INFO';
SOURCE dummy_data/load_inbound_shipment_detail.dump;

SELECT 'Loading outbound shipments' as 'INFO';
SOURCE dummy_data/load_outbound_shipment.dump;

SELECT 'Loading outbound shipment details' as 'INFO';
SOURCE dummy_data/load_outbound_shipment_detail.dump;

SELECT 'Loading stock transfers' as 'INFO';
SOURCE dummy_data/load_stock_transfer.dump;

SELECT 'Loading stock transfer details' as 'INFO';
SOURCE dummy_data/load_stock_transfer_detail.dump;