/*
- users
    - user_id (pk)
    - username
    - password
    - full_name
    - email
    - phone_number
    - level (fk) (các fk nên để tên giống pk của bảng tham chiếu)
- permission
    - level (pk)
    - name
    - description



- suppliers
    - supplier_id (pk)
    - supplier_name
    - supplier_address (chỉ cần sử dụng 1 địa chỉ chính đối với NCC)
    - supplier_email
    - supplier_phone
    - description (mô tả về NCC và 1 số main product của NCC)

    *(supplier address không cần tạo bảng vì địa chỉ của supplier không hoạt động
    giống địa chỉ của customer)
    *(supplier product không cần thiết, thông tin này có thể đưa vào trong description của supplier)

- category
    - category_id (pk)
    - category_name
    - description
- product
    - product_id (pk)
    - product_name
    - description
    - price
    - category_id (fk)
- product_image
    - image_id (fk)
    - image url
- product_variant
    - variant_id (pk)
    - product_id (fk) (composite key)
    - image_id (fk)
    - color (composite key)
    - size (composite key)




- warehouse address
    - id (pk)
    - address line 1 (thông thường 1 warehouse chỉ có 1 địa chỉ)
    - district
    - city
    - postal code
    - country
- warehouse
    - warehouse_id (pk)
    - warehouse_name
    - address (fk)
- warehouse stock
    - warehouse_id (fk)
    - variant_id (fk)
    - quantity
- inventory check
    - check_id (pk)
    - datetime
    - warehouse_id (fk)
    - check_by_user_id (fk)
- checklist details
    - check_id (fk)
    - checklist_details_id (pk)
    - variant_id (fk)
    - quantity (fk) ref warehouse stock
    - actual_quantity
    - quantity discrepancy




- inbound shipment
    - id (pk)
    - supplier id (fk)
    - warehouse id (fk)
    - order date
    - arrival date
    - create by user (fk)
    - status
    - description
- inbound product details
    - product variant id (fk) (composite key)
    - inbound shipment id (fk) (composite key)
    - amount

- stock transfer
    - id (pk)
    - create_date
    - create by user (fk)
    - status
    - from warehouse id (fk)
    - to warehouse id (fk)
    - description
- transfer details
    - product variant id (fk) (composite key)
    - outbound shipment id (fk) (composite key)
    - amount
Xuất chuyển kho và Xuất giao hàng nên có 2 kiểu phiếu khác nhau

- outbound shipment
    - id (pk)
    - from warehouse id (fk)
    - create_date
    - create by user (fk)
    - to customer (bảng customer hoặc tự nhập)
    - status
    - description
- outbound product details
    - product variant id (fk) (composite key)
    - outbound shipment id (fk) (composite key)
    - amount

!!!
- procedure sẽ yêu cầu 1 permission level tối thiểu nào đó để thực hiện?
    (procedure sẽ được gọi dựa trên chức năng được hiển thị qua menu của user, cách của anh là 
    tạo cho mỗi user 1 menu được hiển thị riêng, level user nào thì tương ứng với các chức năng được
    hiển thị và sử dụng chức năng đó, admin có đầy đủ menu chức năng, các level user thấp hơn chỉ cần
    menu không hiển thị các chức năng level đó không được sử dụng là được)
- 1 product có thể có nhiều variant
    (confirmed)
- 1 supplier có thể có nhiều address
    (đã trả lời ở trên)
- 1 variant có thể có nhiều supplier
    (không cần thiết)
- 1 inbound/outbound shipment có thể có nhiều product variant
    (confirmed)

TODO:
    - thanh toán???
*/