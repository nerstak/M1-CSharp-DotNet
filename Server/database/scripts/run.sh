#!/bin/sh
DB_USER=postgres
DB_PWD=admin
DB_SERVER=localhost
DB_NAME=postgres

psql "postgresql://$DB_USER:$DB_PWD@$DB_SERVER/$DB_NAME" -f DB_user.sql
psql "postgresql://$DB_USER:$DB_PWD@$DB_SERVER/$DB_NAME" -f DB_create.sql

DB_NAME=csharp_db
psql "postgresql://$DB_USER:$DB_PWD@$DB_SERVER/$DB_NAME" -f DB_table.sql
psql "postgresql://$DB_USER:$DB_PWD@$DB_SERVER/$DB_NAME" -f DB_populate.sql