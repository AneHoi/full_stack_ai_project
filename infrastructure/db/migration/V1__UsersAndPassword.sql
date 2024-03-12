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

INSERT INTO allergenedb.users (username, tlfnumber, email)
VALUES ('nybruger', 123456789, 'nybruger@example.com');
