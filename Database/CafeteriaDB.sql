CREATE DATABASE CafeteriaDB;
GO
USE CafeteriaDB;
GO

CREATE TABLE Categories (
    Id int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Categories PRIMARY KEY,
    Name nvarchar(100) NOT NULL,
    Description nvarchar(500) NULL,
    CONSTRAINT UQ_Categories_Name UNIQUE (Name)
);
CREATE TABLE Suppliers (
    Id int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Suppliers PRIMARY KEY,
    Name nvarchar(150) NOT NULL,
    Email nvarchar(254) NULL,
    Phone nvarchar(30) NULL
);
CREATE TABLE Products (
    Id int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Products PRIMARY KEY,
    Name nvarchar(150) NOT NULL,
    Price decimal(10,2) NOT NULL CONSTRAINT CK_Products_Price CHECK (Price >= 0),
    Quantity int NOT NULL CONSTRAINT CK_Products_Quantity CHECK (Quantity >= 0),
    CategoryId int NOT NULL,
    SupplierId int NOT NULL,
    CONSTRAINT FK_Products_Categories FOREIGN KEY (CategoryId) REFERENCES Categories(Id),
    CONSTRAINT FK_Products_Suppliers FOREIGN KEY (SupplierId) REFERENCES Suppliers(Id)
);
CREATE TABLE PurchaseOrders (
    Id int IDENTITY(1,1) NOT NULL CONSTRAINT PK_PurchaseOrders PRIMARY KEY,
    OrderDate datetime2 NOT NULL,
    Status nvarchar(20) NOT NULL CONSTRAINT DF_PurchaseOrders_Status DEFAULT ('Pending'),
    SupplierId int NOT NULL,
    CONSTRAINT FK_PurchaseOrders_Suppliers FOREIGN KEY (SupplierId) REFERENCES Suppliers(Id),
    CONSTRAINT CK_PurchaseOrders_Status CHECK (Status IN ('Pending','Received','Cancelled'))
);
CREATE TABLE PurchaseOrderItems (
    Id int IDENTITY(1,1) NOT NULL CONSTRAINT PK_PurchaseOrderItems PRIMARY KEY,
    PurchaseOrderId int NOT NULL,
    ProductId int NOT NULL,
    Quantity int NOT NULL CONSTRAINT CK_PurchaseOrderItems_Quantity CHECK (Quantity > 0),
    UnitPrice decimal(10,2) NOT NULL CONSTRAINT CK_PurchaseOrderItems_UnitPrice CHECK (UnitPrice >= 0),
    CONSTRAINT FK_PurchaseOrderItems_PurchaseOrders FOREIGN KEY (PurchaseOrderId) REFERENCES PurchaseOrders(Id) ON DELETE CASCADE,
    CONSTRAINT FK_PurchaseOrderItems_Products FOREIGN KEY (ProductId) REFERENCES Products(Id)
);
