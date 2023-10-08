create database mockup
go

use mockup
go


create table Rol(
idRol int primary key,
nombreRol varchar(100) not null
)
go



create table Usuario(
idUsuario int primary key identity,
nombres varchar(100) not null,
apellidos varchar(200) not null,
correo varchar(200) not null unique,
contrasenia varchar(250) not null,
fechaRegistro datetime default getdate() not null,
idRol int references rol(idRol) not null
)
go

create table Producto(
idProducto int primary key identity(1000,1),
titulo varchar(250) not null,
precio decimal(10,2) not null,
estado bit not null,
descuento int,
precioDescuento decimal(10,2)
)
go

create table Contenido(
idContenido int primary key identity,
idCloudinary varchar(500),
urlContenido varchar(500),
tipo bit,
idProducto int references producto(idProducto)
)
go





create table Venta(
idVenta int primary key identity (100,1),
totalVenta decimal(10,2) not null,
fechaCompra datetime,
comisionPaypal decimal(10,2),
idTransaccionPaypal varchar(500) not null,
idUsuario int references Usuario(idUsuario)
)
go

create table DetalleVenta(
idDetalleVenta int primary key identity(1,1),
idVenta int references venta(idVenta),
idProducto int references Producto(idProducto),
subtotal decimal(10,2)
)
go




insert into Rol values
(1, 'administrador'),
(2, 'cliente')
go

--Password: 12345678
insert into Usuario values
('Cesar','Aguilar','cesar@gmail.com','73l8gRjwLftklgfdXT+MdiMEjJwGPVMsyVxe16iYpk8=',default,1),
('Ronald','Aguilar','ronald@gmail.com','73l8gRjwLftklgfdXT+MdiMEjJwGPVMsyVxe16iYpk8=',default,1)
go


--Procedimiento almacenado para logeo de usuario
create or alter proc sp_LoginUsuario
(@correo varchar(200),
@contrasenia varchar(250)
)
as
begin
select u.idUsuario, u.nombres, u.apellidos,
u.correo, u.idRol, r.nombreRol 
from usuario u inner join rol r 
on u.idRol = r.idRol
where u.correo = @correo and u.contrasenia = @contrasenia
end
go


--procedimiento almacenado para listar solo productos
create or alter procedure sp_ListarProductos
as
begin
select * from producto
end 
go



--procedimiento almacenado para listar productos para la tienda
create or alter proc sp_ListarProductosStore
as
begin
select p.idProducto, p.titulo, p.precio,
p.descuento, p.precioDescuento, c.urlContenido
from producto p join contenido c
on p.idProducto = c.idProducto
where p.estado = 1 and c.tipo = 1
end
go


--procedimiento almacenado para listar reportes de la tienda
create or alter proc sp_ReportesVenta
as
begin
select count(*) as cantidad,
SUM(totalVenta) as total,
SUM(comisionPaypal) as paypal,
(SUM(totalVenta) - SUM(comisionPaypal)) as ganancia
from Venta
end
go

