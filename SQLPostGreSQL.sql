CREATE ROLE etzjaim LOGIN
  ENCRYPTED PASSWORD 'md52cdbb078fa5f0010eaba723f63d19b7f'
  NOSUPERUSER INHERIT NOCREATEDB CREATEROLE REPLICATION;

  CREATE DATABASE etzjaim
  WITH OWNER = etzjaim
       ENCODING = 'UTF8'
       TABLESPACE = pg_default
       LC_COLLATE = 'Spanish_Mexico.1252'
       LC_CTYPE = 'Spanish_Mexico.1252'
       CONNECTION LIMIT = -1;
GRANT ALL ON DATABASE etzjaim TO etzjaim;
GRANT ALL ON DATABASE etzjaim TO public;


CREATE TABLE administradores
(
  admin_id serial NOT NULL,
  admin_username character varying(255),
  admin_password character varying(255),
  CONSTRAINT pk_admin PRIMARY KEY (admin_id)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE administradores
  OWNER TO etzjaim;
GRANT ALL ON TABLE administradores TO etzjaim;
GRANT ALL ON TABLE administradores TO public;


  CREATE TABLE notificaciones
(
  noti_id serial NOT NULL,
  noti_difusion character varying(255),
  noti_estado integer,
  CONSTRAINT pk_notifications PRIMARY KEY (noti_id)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE notificaciones
  OWNER TO etzjaim;
GRANT ALL ON TABLE notificaciones TO etzjaim;
GRANT ALL ON TABLE notificaciones TO public;


CREATE TABLE productos
(
  pro_id serial NOT NULL,
  pro_nombre character varying(255),
  pro_precio integer,
  pro_estado integer,
  pro_foto bytea,
  CONSTRAINT pk_product PRIMARY KEY (pro_id)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE productos
  OWNER TO etzjaim;
GRANT ALL ON TABLE productos TO etzjaim;
GRANT ALL ON TABLE productos TO public;


  CREATE TABLE clientes
(
  cl_id serial NOT NULL,
  cl_username character varying(255),
  cl_password character varying(255),
  cl_nombre character varying(255),
  cl_apellidos character varying(255),
  CONSTRAINT pk_client PRIMARY KEY (cl_id)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE clientes
  OWNER TO etzjaim;
GRANT ALL ON TABLE clientes TO etzjaim;
GRANT ALL ON TABLE clientes TO public;



CREATE TABLE citas
(
  cita_id serial NOT NULL,
  cl_id integer,
  cita_fecha timestamp with time zone,
  cita_estado integer,
  CONSTRAINT pk_meeting PRIMARY KEY (cita_id),
  CONSTRAINT fk_client FOREIGN KEY (cl_id)
      REFERENCES clientes (cl_id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE citas
  OWNER TO etzjaim;
GRANT ALL ON TABLE citas TO etzjaim;
GRANT ALL ON TABLE citas TO public;



CREATE TABLE alarmas
(
  alarm_id serial NOT NULL,
  cita_id integer,
  alarm_fecha timestamp with time zone,
  alarm_estado integer,
  CONSTRAINT pk_alarm PRIMARY KEY (alarm_id),
  CONSTRAINT fk_meeting FOREIGN KEY (cita_id)
      REFERENCES citas (cita_id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE alarmas
  OWNER TO etzjaim;
GRANT ALL ON TABLE alarmas TO etzjaim;
GRANT ALL ON TABLE alarmas TO public;

CREATE TABLE conversaciones
(
  conv_id serial NOT NULL,
  cl_id integer,
  admin_id integer,
  conv_spam_cliente integer,
  conv_estado_cliente integer,
  conv_spam_admin integer,
  conv_estado_admin integer,
  CONSTRAINT pk_conver PRIMARY KEY (conv_id),
  CONSTRAINT fk_admin FOREIGN KEY (admin_id)
      REFERENCES administradores (admin_id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_clientconver FOREIGN KEY (cl_id)
      REFERENCES clientes (cl_id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE conversaciones
  OWNER TO etzjaim;
GRANT ALL ON TABLE conversaciones TO etzjaim;
GRANT ALL ON TABLE conversaciones TO public;

CREATE TABLE mensajes
(
  mes_id serial NOT NULL,
  conv_id integer,
  mes_entrada timestamp with time zone,
  mes_salida timestamp with time zone,
  mes_estado integer,
  CONSTRAINT pk_messaje PRIMARY KEY (mes_id),
  CONSTRAINT fk_conver FOREIGN KEY (conv_id)
      REFERENCES conversaciones (conv_id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE mensajes
  OWNER TO etzjaim;
GRANT ALL ON TABLE mensajes TO etzjaim;
GRANT ALL ON TABLE mensajes TO public;



