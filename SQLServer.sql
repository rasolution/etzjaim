use master
go

drop database etzjaim
go

create database etzjaim
go

use etzjaim
go

create table administradores(
	admin_id int identity not null,
	admin_username varchar(255) not null,
	admin_password varchar(255) not null
)
alter table administradores add constraint pk_admin primary key(admin_id)
go
create table notificaciones(
	noti_id int identity not null,
	noti_difusion varchar(255) not null,
	noti_estado int not null
)
alter table notificaciones add constraint pk_notifications primary key(noti_id)
go

create table productos(
	pro_id int identity not null,
	pro_nombre varchar(255) not null,
	pro_precio int not null,
	pro_estado int not null
)
alter table productos add constraint pk_product primary key(pro_id)
go
create table clientes(
	cl_id int not null,
	cl_username varchar(255) not null,
	cl_passworrd varchar(255) not null
)
alter table clientes add constraint pk_client primary key(cl_id)
go
create table citas(
	cita_id int identity not null,
	cl_id int not null,
	cita_fecha datetime not null,
	cita_estado int not null,
)
alter table citas add constraint pk_meeting primary key(cita_id)
alter table citas add constraint fk_client foreign key(cl_id) references clientes(cl_id)
go
create table alarmas(
	alarm_id int identity not null,
	cita_id int not null,
	alarm_fecha datetime not null,
	alarm_estado int not null,
)
alter table alarmas add constraint pk_alarm primary key(alarm_id)
alter table alarmas add constraint fk_meeting foreign key(cita_id) references citas(cita_id)
go

create table conversaciones(
	conv_id int identity not null,
	cl_id int not null,
	admin_id int not null,
	conv_spam int not null,
	conv_estado int not null,
)
alter table conversaciones add constraint pk_conver primary key(conv_id)
alter table conversaciones add constraint fk_clientconver foreign key(cl_id) references clientes(cl_id)
alter table conversaciones add constraint fk_admin foreign key(admin_id) references administradores(admin_id)
go
create table mensajes(
	mes_id int identity not null,
	conv_id int not null,
	mes_entrada datetime not null,
	mes_salida datetime not null,
	mes_estado int not null
)
alter table mensajes add constraint pk_messaje primary key (mes_id)
alter table mensajes add constraint fk_conver foreign key(conv_id) references conversaciones(conv_id)
go
 