  CREATE DATABASE etzjaim
  WITH OWNER = etzjaim
       ENCODING = 'UTF8'
       TABLESPACE = pg_default
       LC_COLLATE = 'Spanish_Mexico.1252'
       LC_CTYPE = 'Spanish_Mexico.1252'
       CONNECTION LIMIT = -1;
GRANT ALL ON DATABASE etzjaim TO etzjaim;
GRANT ALL ON DATABASE etzjaim TO public;