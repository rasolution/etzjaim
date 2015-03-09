CREATE TABLE usuarios
(
  username character varying(255) NOT NULL,
  password character varying(255),
  user_nombre character varying(255),
  user_apellidos character varying(255),
  user_tipo integer NOT NULL,
  CONSTRAINT pk_client PRIMARY KEY (username)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE usuarios
  OWNER TO etzjaim;
GRANT ALL ON TABLE usuarios TO etzjaim;
GRANT ALL ON TABLE usuarios TO public;

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

CREATE TABLE conversaciones
(
  conv_id serial NOT NULL,
  username1 character varying(255),
  username2 character varying(255),
  user1_spam integer,
  user2_spam integer,
  user1_estado integer,
  user2_estado integer,
  CONSTRAINT pk_conv PRIMARY KEY (conv_id)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE conversaciones
  OWNER TO etzjaim;
GRANT ALL ON TABLE conversaciones TO etzjaim;
GRANT ALL ON TABLE conversaciones TO public;

CREATE TABLE conv_message
(
  mes_id serial NOT NULL,
  conv_id integer,
  username character varying(255),
  message character varying(255),
  mes_fecha time with time zone,
  CONSTRAINT pk_mesa PRIMARY KEY (mes_id),
  CONSTRAINT fk_conv FOREIGN KEY (conv_id)
      REFERENCES conversaciones (conv_id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_user FOREIGN KEY (username)
      REFERENCES usuarios (username) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE conv_message
  OWNER TO etzjaim;
GRANT ALL ON TABLE conv_message TO etzjaim;
GRANT ALL ON TABLE conv_message TO public;

CREATE TABLE citas
(
  cita_id serial NOT NULL,
  username character varying(255),
  cita_estado integer,
  cita_fecha character varying(255),
  cita_hora character varying(255),
  CONSTRAINT pk_meeting PRIMARY KEY (cita_id),
  CONSTRAINT fk_usuario FOREIGN KEY (username)
      REFERENCES usuarios (username) MATCH SIMPLE
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
