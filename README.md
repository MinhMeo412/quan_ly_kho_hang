# Quản lý kho hàng

Repo này chứa chương trình quản lí kho hàng được viết bằng C#.

## Mục lục

1. [Tổng quan](#tổng-quan)
2. [Cách dùng](#cách-dùng)

## Tổng quan

Trong repo này:
* src/
    * WarehouseManager/: source code của chương trình
* test/
    * WarehouseManagerTest/: source code của unit test
* db/
    * scripts/: chứa các file sql 
* docs/
    * STYLE_GUIDE.md: quy ước chung
* ProjectReport/: Các file báo cáo của nhóm
    * Examples/: Mẫu báo cáo
## Cách dùng

Thiết lập database:
```bash
cd db/scripts
mysql -uroot -p
source main.sql;
```

Chạy trương trình:
```bash
cd src/WarehouseManager
dotnet run
```