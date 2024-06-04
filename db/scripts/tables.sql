/*
- users
    - user_id (pk)
    - username
    - password
    - full_name
    - email
    - phone_number
    - level (fk)

- permission
    - level (pk)
    - name
    - description



- suppliers
    - supplier_id (pk)
    - supplier_name
    - supplier_address
    - supplier_email
    - supplier_phone
    - supplier_website
    - description



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

- product_variant
    - variant_id (pk)
    - product_id (fk)
    - image_url
    - color
    - size




- warehouse address
    - id (pk)
    - address
    - district
    - city
    - postal code
    - country

- warehouse
    - warehouse_id (pk)
    - warehouse_name
    - address (fk)

- warehouse stock
    - warehouse_id (fk) (composite key)
    - variant_id (fk) (composite key)
    - quantity


    
- inventory check
    - check_id (pk)
    - datetime
    - warehouse_id (fk)
    - check_by_user_id (fk)
    
- checklist details
    - check_id (fk) (composite key)
    - variant_id (fk) (composite key)
    - expected_quantity (fk) ref warehouse stock
    - actual_quantity




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


- outbound shipment
    - id (pk)
    - from warehouse id (fk)
    - create_date
    - create by user (fk)
    - to address
    - status
    - description

- outbound product details
    - product variant id (fk) (composite key)
    - outbound shipment id (fk) (composite key)
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
    - stock transfer id (fk) (composite key)
    - amount

!!!
- procedure sẽ yêu cầu 1 permission level tối thiểu nào đó để thực hiện?
    (procedure sẽ được gọi dựa trên chức năng được hiển thị qua menu của user, cách của anh là 
    tạo cho mỗi user 1 menu được hiển thị riêng, level user nào thì tương ứng với các chức năng được
    hiển thị và sử dụng chức năng đó, admin có đầy đủ menu chức năng, các level user thấp hơn chỉ cần
    menu không hiển thị các chức năng level đó không được sử dụng là được)

TODO:
    - thanh toán???
*/