create database [Furnitura4coursed]
go
use [Furnitura4coursed]
go
create table [Role]
(
	RoleID int primary key identity not null,
	RoleName nvarchar(100) not null
)
go
create table [User]
(
	UserID int primary key identity not null,
	UserSurname nvarchar(100) not null,
	UserName nvarchar(100) not null,
	UserPatronymic nvarchar(100) not null,
	UserLogin nvarchar(max) not null,
	UserPassword nvarchar(max) not null,
	UserRole int foreign key references [Role](RoleID) not null
)
go
create table [Status]
(
	StatusID int primary key identity not null,
	StatusName nvarchar(50) not null
)
go
create table [Order]
(
	OrderID int primary key identity not null,
	OrderName nvarchar(60) not null,
	OrderStatus int foreign key references [Status](StatusID) not null,
	OrderDate datetime not null,
	OrderDeliveryDate datetime not null,
	OrderPickupPoint nvarchar(max) not null
)
go
create table [Manufacturer]
(
	ManufacturerID int primary key identity  not null,
	ManufacturerName nvarchar(50) not null
)
go
create table [Tovar]
(
	TovarID int identity primary key not null,
	TovarName nvarchar(60) not null,
	TovarManufacturer int foreign key references Manufacturer(ManufacturerID) not null,
	TovarQuantityInStock int not null,
	TovarDiscountAmount decimal(19,2) not null,
	TovarCost decimal(19,2) not null,
	TovarPhoto nvarchar(max) not null,
	TovarDescription nvarchar(200) not null
)
