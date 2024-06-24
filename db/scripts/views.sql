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

-- View cho chức năng xem danh sách phiếu xuất - nhập - chuyển kho
CREATE VIEW shipment_view AS
SELECT
  inbound_shipment_id AS shipment_id,
  warehouse_id,
  inbound_shipment_starting_date AS shipment_date,
  user_id,
  inbound_shipment_status AS shipment_status,
  'Inbound Shipment' AS shipment_type
FROM
  inbound_shipment
UNION ALL
SELECT
  outbound_shipment_id AS shipment_id,
  warehouse_id,
  outbound_shipment_starting_date AS shipment_date,
  user_id,
  outbound_shipment_status AS shipment_status,
  'Outbound Shipment' AS shipment_type
FROM
  outbound_shipment
UNION ALL
SELECT
	stock_transfer_id AS shipment_id,
    to_warehouse_id AS warehouse_id,
    stock_transfer_starting_date AS shipment_date,
    user_id,
    stock_transfer_status AS shipment_status,
    'Transfer Shipment' AS shipment_type
FROM
	stock_transfer;