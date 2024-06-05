CREATE DATABASE warehouse;
USE warehouse;

-- Execute the script for creating tables
SELECT 'Executing tables.sql: ' as 'INFO';
SOURCE tables.sql;

-- Execute the script for creating triggers
SELECT 'Executing triggers.sql: ' as 'INFO';
SOURCE triggers.sql;

-- Execute the script for creating stored procedures
SELECT 'Executing procedures.sql: ' as 'INFO';
SOURCE procedures.sql;

-- Execute the script for creating views
SELECT 'Executing views.sql: ' as 'INFO';
SOURCE views.sql;

-- Execute the script for creating indexes
SELECT 'Executing indexes.sql: ' as 'INFO';
SOURCE indexes.sql;

-- Execute the script for inserting dummy data
SELECT 'Executing insert_dummy_data.sql: ' as 'INFO';
SOURCE insert_dummy_data.sql;

SELECT 'Database creation completed.' as 'INFO';