-- You need to be connected to st2eedb to execute this part
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE users
(
    username  varchar NOT NULL PRIMARY KEY,
    pwd       varchar
);

GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO adm;