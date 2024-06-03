/*
- user
    - id (pk)
    - username
    - password
    - email
    - phone number
    - permission level (fk)
- permission
    - level (pk)
    - name
    - description



- category
    - id (pk)
    - name
    - description
- product
    - id (pk)
    - name
    - description
    - category
- product_variant
    - id (pk)
    - product id (fk)
    - image id (fk)
    - color
    - size
    - price
- image
    - image id (fk)
    - image url



- address
    - id (pk)
    - address line 1
    - address line 2
    - district
    - city
    - postal code
    - country



- supplier 
    - id (pk)
    - name
- supplier address
    - supplier id (fk) (composite key)
    - address id (fk) (composite key)
    - primary address (boolean)
- supplier product
    - product variant id (fk) (composite key)
    - supplier id (fk) (composite key)



- warehouse
    - id (pk)
    - name
    - address (fk)
- warehouse stock
    - warehouse id (fk)
    - product variant id (fk)
    - stock quantity



- inbound shipment
    - id (pk)
    - supplier id (fk)
    - warehouse id (fk)
    - order date
    - arrival date
    - status
- inbound product
    - product variant id (fk) (composite key)
    - inbound shipment id (fk) (composite key)
    - amount

- outbound shipment
    - id (pk)
    - order date
    - arrival date
    - status
    - address id (fk)
- outbound product
    - product variant id (fk) (composite key)
    - warehouse id (fk) (composite key)
    - outbound shipment id (fk) (composite key)
    - amount


!!!
- procedure sẽ yêu cầu 1 permission level tối thiểu nào đó để thực hiện?
- 1 product có thể có nhiều variant
- 1 supplier có thể có nhiều address
- 1 variant có thể có nhiều supplier
- 1 inbound/outbound shipment có thể có nhiều product variant

TODO:
    - thanh toán???
*/