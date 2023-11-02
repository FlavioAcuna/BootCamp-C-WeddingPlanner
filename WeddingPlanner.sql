drop database if exists WeddingsDB;
create database WeddingsDB;
use WeddingsDB;

create table users(
UserId int auto_increment not null,
Nombre varchar(50) not null,
Apellido varchar(50) not null,
Email varchar(150) not null,
Password varchar(100) not null,
CreatedAt datetime	default current_timestamp,
UpdateAt datetime	default current_timestamp,
primary key(UserId)
);

create table weddings(
WeddingId int auto_increment not null,
WedderOne varchar(50) not null,
WedderTwo varchar(50) not null,
WeddingDate datetime not null,
address varchar(45) not null,
CreatedAt datetime	default current_timestamp,
UpdateAt datetime	default current_timestamp,
UserId int not null,
primary key(WeddingId),
FOREIGN KEY (`UserId`) REFERENCES users (`UserId`)
);

create table guests(
GuestId int auto_increment not null,
WeddingId int not null,
UserId int not null,
CreatedAt datetime	default current_timestamp,
UpdateAt datetime	default current_timestamp,
primary key(GuestId),
foreign key(`WeddingId`) references weddings(`WeddingId`),
FOREIGN KEY (`UserId`) REFERENCES users (`UserId`)
);
