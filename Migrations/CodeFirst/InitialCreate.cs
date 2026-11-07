using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using UniversityCafeteriaManagement.Data;

namespace UniversityCafeteriaManagement.Migrations.CodeFirst;

[DbContext(typeof(CafeteriaEmployeesDbContext))]
[Migration("202611080001_InitialCreate")]
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("CREATE TABLE Categories (Id int IDENTITY(1,1) NOT NULL PRIMARY KEY, Name nvarchar(100) NOT NULL UNIQUE, Description nvarchar(500) NULL);");
        migrationBuilder.Sql("CREATE TABLE Suppliers (Id int IDENTITY(1,1) NOT NULL PRIMARY KEY, Name nvarchar(150) NOT NULL, Email nvarchar(254) NULL, Phone nvarchar(30) NULL);");
        migrationBuilder.Sql("CREATE TABLE Products (Id int IDENTITY(1,1) NOT NULL PRIMARY KEY, Name nvarchar(150) NOT NULL, Price decimal(10,2) NOT NULL CHECK (Price >= 0), Quantity int NOT NULL CHECK (Quantity >= 0), CategoryId int NOT NULL REFERENCES Categories(Id), SupplierId int NOT NULL REFERENCES Suppliers(Id));");
        migrationBuilder.Sql("CREATE TABLE PurchaseOrders (Id int IDENTITY(1,1) NOT NULL PRIMARY KEY, OrderDate datetime2 NOT NULL, Status nvarchar(20) NOT NULL CONSTRAINT DF_PurchaseOrders_Status DEFAULT ('Pending') CHECK (Status IN ('Pending','Received','Cancelled')), SupplierId int NOT NULL REFERENCES Suppliers(Id));");
        migrationBuilder.Sql("CREATE TABLE PurchaseOrderItems (Id int IDENTITY(1,1) NOT NULL PRIMARY KEY, PurchaseOrderId int NOT NULL REFERENCES PurchaseOrders(Id) ON DELETE CASCADE, ProductId int NOT NULL REFERENCES Products(Id), Quantity int NOT NULL CHECK (Quantity > 0), UnitPrice decimal(10,2) NOT NULL CHECK (UnitPrice >= 0));");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("DROP TABLE PurchaseOrderItems; DROP TABLE Products; DROP TABLE PurchaseOrders; DROP TABLE Suppliers; DROP TABLE Categories;");
    }
}
