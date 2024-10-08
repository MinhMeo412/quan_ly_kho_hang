DROP DATABASE IF EXISTS warehouse;
CREATE DATABASE warehouse;
USE warehouse;

SELECT 'Executing tables.sql' AS 'INFO';
SOURCE tables.sql;

SELECT 'Executing triggers.sql' AS 'INFO';
SOURCE triggers.sql;

SELECT 'Executing views.sql' AS 'INFO';
SOURCE views.sql;

SELECT 'Executing procedures.sql' AS 'INFO';
SOURCE procedures.sql;

SELECT 'Executing indexes.sql' AS 'INFO';
SOURCE indexes.sql;

SELECT 'Executing insert_required_data.sql' AS 'INFO';
SOURCE insert_required_data.sql;

SELECT 'Executing user.sql' AS 'INFO';
SOURCE user.sql;

SELECT 'Executing insert_dummy_data.sql' AS 'INFO';
SOURCE insert_dummy_data.sql;

SELECT 'Database creation completed.' AS 'INFO';