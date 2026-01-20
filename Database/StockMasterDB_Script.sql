-- =============================================
-- StockMaster Pro - Database Creation Script
-- =============================================

USE master;
GO

-- Create database if it doesn't exist
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'StockMasterDB')
BEGIN
    CREATE DATABASE StockMasterDB;
END
GO

USE StockMasterDB;
GO

-- =============================================
-- Tables Creation
-- =============================================

-- Categories Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Categories]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Categories] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [Name] NVARCHAR(100) NOT NULL,
        [Description] NVARCHAR(500) NULL,
        [Icon] NVARCHAR(50) NOT NULL DEFAULT 'fa-box',
        [Color] NVARCHAR(20) NOT NULL DEFAULT 'blue',
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETDATE(),
        [IsActive] BIT NOT NULL DEFAULT 1
    );
END
GO

-- Suppliers Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Suppliers]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Suppliers] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [Name] NVARCHAR(200) NOT NULL,
        [ContactName] NVARCHAR(200) NULL,
        [Email] NVARCHAR(200) NULL,
        [Phone] NVARCHAR(50) NULL,
        [Address] NVARCHAR(500) NULL,
        [City] NVARCHAR(100) NULL,
        [Country] NVARCHAR(100) NOT NULL DEFAULT 'Argentina',
        [IsActive] BIT NOT NULL DEFAULT 1,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETDATE()
    );
END
GO

-- Products Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Products]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Products] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [SKU] NVARCHAR(50) NOT NULL UNIQUE,
        [Name] NVARCHAR(200) NOT NULL,
        [Description] NVARCHAR(1000) NULL,
        [CategoryId] INT NOT NULL,
        [SupplierId] INT NOT NULL,
        [Price] DECIMAL(18,2) NOT NULL,
        [Stock] INT NOT NULL DEFAULT 0,
        [MinStock] INT NOT NULL DEFAULT 0,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETDATE(),
        [UpdatedAt] DATETIME2 NULL,
        [IsActive] BIT NOT NULL DEFAULT 1,
        CONSTRAINT FK_Products_Categories FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Categories]([Id]),
        CONSTRAINT FK_Products_Suppliers FOREIGN KEY ([SupplierId]) REFERENCES [dbo].[Suppliers]([Id])
    );
END
GO

-- Movements Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Movements]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Movements] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [ProductId] INT NOT NULL,
        [Type] NVARCHAR(50) NOT NULL, -- 'entrada', 'salida', 'ajuste'
        [Quantity] INT NOT NULL,
        [Reason] NVARCHAR(500) NULL,
        [User] NVARCHAR(100) NOT NULL DEFAULT 'Administrador',
        [Date] DATETIME2 NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_Movements_Products FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products]([Id]) ON DELETE CASCADE
    );
END
GO

-- =============================================
-- Seed Data
-- =============================================

-- Insert Categories
IF NOT EXISTS (SELECT * FROM Categories)
BEGIN
    SET IDENTITY_INSERT [dbo].[Categories] ON;

    INSERT INTO [dbo].[Categories] ([Id], [Name], [Description], [Icon], [Color], [CreatedAt], [IsActive])
    VALUES
        (1, N'Electrónica', N'Productos electrónicos', 'fa-laptop', 'blue', GETDATE(), 1),
        (2, N'Ropa', N'Vestimenta y accesorios', 'fa-tshirt', 'purple', GETDATE(), 1),
        (3, N'Alimentos', N'Productos alimenticios', 'fa-apple-alt', 'green', GETDATE(), 1),
        (4, N'Herramientas', N'Herramientas y equipos', 'fa-tools', 'orange', GETDATE(), 1),
        (5, N'Deportes', N'Artículos deportivos', 'fa-futbol', 'red', GETDATE(), 1),
        (6, N'Librería', N'Libros y útiles escolares', 'fa-book', 'cyan', GETDATE(), 1);

    SET IDENTITY_INSERT [dbo].[Categories] OFF;
END
GO

-- Insert Suppliers
IF NOT EXISTS (SELECT * FROM Suppliers)
BEGIN
    SET IDENTITY_INSERT [dbo].[Suppliers] ON;

    INSERT INTO [dbo].[Suppliers] ([Id], [Name], [ContactName], [Email], [Phone], [Address], [City], [Country], [IsActive], [CreatedAt])
    VALUES
        (1, N'Tech Supplies SA', N'Juan Pérez', 'ventas@techsupplies.com', '+54 11 4567-8900', 'Av. Corrientes 1234', 'Buenos Aires', 'Argentina', 1, GETDATE()),
        (2, N'Distribuidora Central', N'María González', 'info@distcentral.com', '+54 11 5432-1098', 'Calle San Martín 567', 'Córdoba', 'Argentina', 1, GETDATE()),
        (3, N'Importadora Andina', N'Carlos Rodríguez', 'compras@impandina.com', '+54 261 423-9876', 'Av. San Martín 890', 'Mendoza', 'Argentina', 1, GETDATE()),
        (4, N'Almacén Logístico', N'Ana Martínez', 'contacto@almalog.com', '+54 341 456-7890', 'Bv. Oroño 234', 'Rosario', 'Argentina', 0, GETDATE());

    SET IDENTITY_INSERT [dbo].[Suppliers] OFF;
END
GO

-- Insert Products
IF NOT EXISTS (SELECT * FROM Products)
BEGIN
    SET IDENTITY_INSERT [dbo].[Products] ON;

    INSERT INTO [dbo].[Products] ([Id], [SKU], [Name], [Description], [CategoryId], [SupplierId], [Price], [Stock], [MinStock], [CreatedAt], [IsActive])
    VALUES
        (1, 'PROD-001', N'Laptop Dell XPS 13', N'Laptop de alta gama con procesador Intel i7', 1, 1, 1200.00, 15, 5, GETDATE(), 1),
        (2, 'PROD-002', N'Mouse Logitech MX Master', N'Mouse inalámbrico premium con sensor de precisión', 1, 1, 89.99, 45, 10, GETDATE(), 1),
        (3, 'PROD-003', N'Teclado Mecánico RGB', N'Teclado gaming con switches mecánicos', 1, 1, 149.99, 30, 10, GETDATE(), 1),
        (4, 'PROD-004', N'Camiseta Deportiva', N'Camiseta de algodón para actividades deportivas', 2, 2, 29.99, 120, 30, GETDATE(), 1),
        (5, 'PROD-005', N'Pantalón Jean', N'Jean clásico de mezclilla', 2, 2, 59.99, 80, 20, GETDATE(), 1),
        (6, 'PROD-006', N'Monitor LG 27"', N'Monitor Full HD 27 pulgadas', 1, 1, 299.99, 20, 5, GETDATE(), 1),
        (7, 'PROD-007', N'Zapatillas Running', N'Zapatillas para correr con amortiguación', 5, 2, 89.99, 60, 15, GETDATE(), 1),
        (8, 'PROD-008', N'Taladro Eléctrico', N'Taladro percutor 850W', 4, 3, 149.99, 25, 8, GETDATE(), 1);

    SET IDENTITY_INSERT [dbo].[Products] OFF;
END
GO

-- =============================================
-- Indexes for Performance
-- =============================================

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Products_SKU' AND object_id = OBJECT_ID('Products'))
BEGIN
    CREATE INDEX IX_Products_SKU ON Products(SKU);
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Products_CategoryId' AND object_id = OBJECT_ID('Products'))
BEGIN
    CREATE INDEX IX_Products_CategoryId ON Products(CategoryId);
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Products_SupplierId' AND object_id = OBJECT_ID('Products'))
BEGIN
    CREATE INDEX IX_Products_SupplierId ON Products(SupplierId);
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Movements_ProductId' AND object_id = OBJECT_ID('Movements'))
BEGIN
    CREATE INDEX IX_Movements_ProductId ON Movements(ProductId);
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Movements_Date' AND object_id = OBJECT_ID('Movements'))
BEGIN
    CREATE INDEX IX_Movements_Date ON Movements(Date DESC);
END
GO

PRINT 'Database StockMasterDB created successfully!';
GO
