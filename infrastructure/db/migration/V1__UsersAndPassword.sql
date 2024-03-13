DROP SCHEMA IF EXISTS allergenedb;
CREATE SCHEMA allergenedb;

CREATE TABLE IF NOT EXISTS allergenedb.users (
                                                 id SERIAL PRIMARY KEY,
                                                 username VARCHAR(50) NOT NULL,
    tlfnumber INT,
    email VARCHAR(50) NOT NULL UNIQUE
    );

CREATE TABLE allergenedb.password_hash (
                                           user_id SERIAL PRIMARY KEY,
                                           hash VARCHAR(350) NOT NULL,
                                           salt VARCHAR(180) NOT NULL,
                                           algorithm VARCHAR(12) NOT NULL,
                                           FOREIGN KEY (user_id) REFERENCES allergenedb.users (id)
);

CREATE TABLE allergenedb.products (
                                      barcode VARCHAR(50) PRIMARY KEY,
                                      language VARCHAR(50),
                                      name VARCHAR(100),
                                      productName VARCHAR(100),
                                      declaration VARCHAR(10380)
);

CREATE TABLE allergenedb.categories (
                                        id INT AUTO_INCREMENT PRIMARY KEY,
                                        category_name VARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE allergenedb.allergens (
                                       id INT AUTO_INCREMENT PRIMARY KEY,
                                       allergen_name VARCHAR(100) NOT NULL UNIQUE,
                                       category_id INT,
                                       FOREIGN KEY (category_id) REFERENCES allergenedb.categories(id)
);


INSERT INTO allergenedb.users (username, tlfnumber, email)
VALUES ('nybruger', 123456789, 'nybruger@example.com');
