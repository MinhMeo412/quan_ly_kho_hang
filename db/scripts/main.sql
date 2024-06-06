CREATE DATABASE warehouse;
USE warehouse;

SELECT 'Executing tables.sql' as 'INFO';
SOURCE tables.sql;

SELECT 'Executing triggers.sql' as 'INFO';
SOURCE triggers.sql;

SELECT 'Executing procedures.sql' as 'INFO';
SOURCE procedures.sql;

SELECT 'Executing views.sql' as 'INFO';
SOURCE views.sql;

SELECT 'Executing indexes.sql' as 'INFO';
SOURCE indexes.sql;

SELECT 'Executing insert_dummy_data.sql' as 'INFO';
SOURCE insert_dummy_data.sql;

SELECT 'Database creation completed.' as 'INFO';