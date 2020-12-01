-- You need to be connected to postgres (the DB) to execute this part
drop database IF EXISTS csharp_db; -- No need to put everything in capital, it is not case sensitive here
CREATE DATABASE csharp_db OWNER adm ENCODING DEFAULT CONNECTION LIMIT -1;


