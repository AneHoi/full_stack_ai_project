DROP SCHEMA IF EXISTS allergenedb CASCADE;
CREATE SCHEMA allergenedb;

create table if not exists allergenedb.users
(
    id          SERIAL      PRIMARY KEY,
    username    VARCHAR(50) NOT NULL,
    tlfnumber   INT,
    email       VARCHAR(50) NOT NULL UNIQUE
);

create table allergenedb.password_hash
(
    user_id     integer,
    hash        VARCHAR(350) NOT NULL,
    salt        VARCHAR(180) NOT NULL,
    algorithm   VARCHAR(12)  NOT NULL,
    FOREIGN KEY (user_id) REFERENCES allergenedb.users (id)
);

create table allergenedb.allergies(
    id          SERIAL      PRIMARY KEY,
    allergene   VARCHAR(250) NOT NULL
);

create table allergenedb.allergiesprPerson(
    user_id     integer,
    allergeneId integer,
    FOREIGN KEY (allergeneId) REFERENCES allergenedb.allergies (id)
);

SELECT * FROM allergenedb.users;