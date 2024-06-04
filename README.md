# Quản lý kho hàng

Repo này chứa chương trình quản lí kho hàng được viết bằng C#.

## Mục lục

1. [Tổng quan](#tổng-quan)
2. [Việc cần làm](#việc-cần-làm)

## Tổng quan

Trong repo này:
* src/
    * WarehouseManager/: source code của chương trình
* test/
    * WarehouseManagerTest/: source code của unit test
* db/
    * scripts/: chứa các file sql 
* Project_Slide.pptx: file thuyết trình của nhóm
* Projects_Report.docx: file báo cáo của nhóm

## Việc cần làm

```
Đăng nhập
    - phân quyền user
    - 3 cấp user khác nhau
        - Admin
        - User 1
        - User 2

*Menu Admin
    1. Quản lí nhà cung cấp:
        1.1 Xem danh sách NCC
        1.2 Thêm nhà cung cấp
        1.3 Sửa thông tin nhà cung cấp(Bao gồm xóa)

    2. Quản lí sản phẩm
        3.1 Danh mục sản phẩm
            3.1.1 Xem danh mục
            3.1.2 Thêm danh mục sản phẩm
            3.1.3 Sửa danh mục sản phẩm (Bao gồm xóa)
        3.2 Danh sách sản phẩm
            3.2.1 Xem danh sách
            3.2.2 Tìm sản phẩm
            3.2.3 Thêm sản phẩm mới
            3.2.4 Sửa thông tin sản phẩm (Bao gồm xóa)
        3.3 Tồn kho sản phẩm

    3. Quản lý kho
        4.1 Xuất/nhập kho
            4.1.1 Xem danh sách phiếu xuất/nhập
            4.1.2 Tìm/Xem phiếu xuất/nhập
            4.1.3 Tạo phiếu xuất/nhập (1 kiểu nhập duy nhất: Nhập mua hàng, 2 kiểu xuất: xuất chuyển kho và xuất giao hàng)
        4.2 Kiểm kê
            4.3.1 Tạo phiếu kiểm kê
            4.3.2 Tìm/Xem phiểu kiểm kê
        4.3 Kho hàng
            4.3.1 Xem danh sách kho
            4.3.2 Thêm kho mới
            4.3.3 Sửa thông tin kho


    4. Báo cáo
        Nhiều dạng báo cáo (Làm sau cùng)


    5. Quản lí tài khoản
        2.1 Xem danh sách tài khoản
        2.2 Tạo tài khoản mới
        2.3 Sửa thông tin tài khoản
            - Sửa thông tin người dùng()\Phân quyền
```
