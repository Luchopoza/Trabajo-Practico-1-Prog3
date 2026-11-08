
IF DB_ID('Programacion3') IS NULL
    CREATE DATABASE Programacion3;
GO

USE Programacion3;
GO

IF OBJECT_ID('dbo.Productos', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Productos (
        Id     INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        Nombre VARCHAR(200)      NOT NULL,
        Precio DECIMAL(10,2)     NOT NULL,
        Stock  INT               NOT NULL
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Productos)
BEGIN
    INSERT INTO dbo.Productos (Nombre, Precio, Stock) VALUES
    ('Notebook Lenovo',     850000.00, 10),
    ('Mouse Logitech',       25000.00, 25),
    ('Teclado Redragon',     45000.00, 15),
    ('Monitor Samsung 24"', 180000.00,  8),
    ('Auriculares JBL',      65000.00, 20);
END
GO

SELECT Id, Nombre, Precio, Stock FROM dbo.Productos;
GO