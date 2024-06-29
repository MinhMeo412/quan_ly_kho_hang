CREATE USER 'warehouse_app_user'@'%' IDENTIFIED BY '1234';

GRANT USAGE ON warehouse.* TO 'warehouse_app_user'@'%';

GRANT EXECUTE ON PROCEDURE warehouse.user_login TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.change_password TO 'warehouse_app_user'@'%';

GRANT EXECUTE ON PROCEDURE warehouse.create_permission TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.read_permission TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.update_permission TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.delete_permission TO 'warehouse_app_user'@'%';

GRANT EXECUTE ON PROCEDURE warehouse.create_user TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.read_user TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.update_user TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.delete_user TO 'warehouse_app_user'@'%';

GRANT EXECUTE ON PROCEDURE warehouse.create_token TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.read_token TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.update_token TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.delete_token TO 'warehouse_app_user'@'%';

GRANT EXECUTE ON PROCEDURE warehouse.create_supplier TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.read_supplier TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.update_supplier TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.delete_supplier TO 'warehouse_app_user'@'%';

GRANT EXECUTE ON PROCEDURE warehouse.create_category TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.read_category TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.update_category TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.delete_category TO 'warehouse_app_user'@'%';

GRANT EXECUTE ON PROCEDURE warehouse.create_product TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.read_product TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.update_product TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.delete_product TO 'warehouse_app_user'@'%';

GRANT EXECUTE ON PROCEDURE warehouse.create_product_variant TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.read_product_variant TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.update_product_variant TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.delete_product_variant TO 'warehouse_app_user'@'%';

GRANT EXECUTE ON PROCEDURE warehouse.create_warehouse_address TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.read_warehouse_address TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.update_warehouse_address TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.delete_warehouse_address TO 'warehouse_app_user'@'%';

GRANT EXECUTE ON PROCEDURE warehouse.create_warehouse TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.read_warehouse TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.update_warehouse TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.delete_warehouse TO 'warehouse_app_user'@'%';

GRANT EXECUTE ON PROCEDURE warehouse.create_warehouse_stock TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.read_warehouse_stock TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.update_warehouse_stock TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.delete_warehouse_stock TO 'warehouse_app_user'@'%';

GRANT EXECUTE ON PROCEDURE warehouse.create_inventory_audit TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.read_inventory_audit TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.update_inventory_audit TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.delete_inventory_audit TO 'warehouse_app_user'@'%';

GRANT EXECUTE ON PROCEDURE warehouse.create_inventory_audit_detail TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.read_inventory_audit_detail TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.update_inventory_audit_detail TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.delete_inventory_audit_detail TO 'warehouse_app_user'@'%';

GRANT EXECUTE ON PROCEDURE warehouse.create_inbound_shipment TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.read_inbound_shipment TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.update_inbound_shipment TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.delete_inbound_shipment TO 'warehouse_app_user'@'%';

GRANT EXECUTE ON PROCEDURE warehouse.create_inbound_shipment_detail TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.read_inbound_shipment_detail TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.update_inbound_shipment_detail TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.delete_inbound_shipment_detail TO 'warehouse_app_user'@'%';

GRANT EXECUTE ON PROCEDURE warehouse.create_outbound_shipment TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.read_outbound_shipment TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.update_outbound_shipment TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.delete_outbound_shipment TO 'warehouse_app_user'@'%';

GRANT EXECUTE ON PROCEDURE warehouse.create_outbound_shipment_detail TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.read_outbound_shipment_detail TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.update_outbound_shipment_detail TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.delete_outbound_shipment_detail TO 'warehouse_app_user'@'%';

GRANT EXECUTE ON PROCEDURE warehouse.create_stock_transfer TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.read_stock_transfer TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.update_stock_transfer TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.delete_stock_transfer TO 'warehouse_app_user'@'%';

GRANT EXECUTE ON PROCEDURE warehouse.create_stock_transfer_detail TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.read_stock_transfer_detail TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.update_stock_transfer_detail TO 'warehouse_app_user'@'%';
GRANT EXECUTE ON PROCEDURE warehouse.delete_stock_transfer_detail TO 'warehouse_app_user'@'%';

REVOKE ALL PRIVILEGES, GRANT OPTION FROM 'warehouse_app_user'@'%';
FLUSH PRIVILEGES;