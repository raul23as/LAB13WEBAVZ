-- =============================================
--  VeterinariaDB - Script de creación (Database First)
--  Semana 13 - API en capas con DTOs y AutoMapper
--  Relación 1:N  ->  Un Dueño tiene muchas Mascotas
-- =============================================

IF DB_ID(N'VeterinariaDB') IS NULL
    CREATE DATABASE VeterinariaDB;
GO

USE VeterinariaDB;
GO

IF OBJECT_ID(N'dbo.Mascotas', N'U') IS NOT NULL DROP TABLE dbo.Mascotas;
IF OBJECT_ID(N'dbo.Duenos', N'U') IS NOT NULL DROP TABLE dbo.Duenos;
GO

-- Tabla padre
CREATE TABLE dbo.Duenos
(
    Id        INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Duenos PRIMARY KEY,
    Nombres   NVARCHAR(100)     NOT NULL,
    Dni       NVARCHAR(8)       NOT NULL CONSTRAINT UQ_Duenos_Dni UNIQUE,
    Telefono  NVARCHAR(9)       NULL,
    Correo    NVARCHAR(100)     NULL
);
GO

-- Tabla hija (clave foránea DuenoId)
CREATE TABLE dbo.Mascotas
(
    Id               INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Mascotas PRIMARY KEY,
    Nombre           NVARCHAR(50)      NOT NULL,
    Especie          NVARCHAR(30)      NOT NULL,
    Raza             NVARCHAR(50)      NULL,
    FechaNacimiento  DATE              NOT NULL,
    Peso             DECIMAL(5,2)      NOT NULL CONSTRAINT CK_Mascotas_Peso CHECK (Peso > 0),
    DuenoId          INT               NOT NULL
        CONSTRAINT FK_Mascotas_Duenos FOREIGN KEY REFERENCES dbo.Duenos(Id)
);
GO

-- Datos de ejemplo
INSERT INTO dbo.Duenos (Nombres, Dni, Telefono, Correo) VALUES
 (N'Ana Torres Quispe',   N'70123456', N'987654321', N'ana.torres@correo.com'),
 (N'Bruno Díaz Ramos',    N'71234567', N'912345678', N'bruno.diaz@correo.com'),
 (N'Carla Mendoza Ríos',  N'72345678', NULL,         N'carla.mendoza@correo.com'),
 (N'Diego Salazar Vega',  N'73456789', N'956781234', NULL);
GO

-- Ana tiene 5 mascotas (sirve para probar la regla del máximo de 5)
-- Diego no tiene mascotas (sirve para probar un DELETE correcto)
INSERT INTO dbo.Mascotas (Nombre, Especie, Raza, FechaNacimiento, Peso, DuenoId) VALUES
 (N'Firulais', N'Perro', N'Labrador',        '2020-03-15', 28.50, 1),
 (N'Michi',    N'Gato',  N'Siamés',          '2021-07-02',  4.20, 1),
 (N'Rocky',    N'Perro', N'Bulldog',         '2019-11-20', 22.00, 1),
 (N'Pelusa',   N'Gato',  N'Persa',           '2022-01-10',  3.80, 1),
 (N'Kiwi',     N'Ave',   N'Periquito',       '2023-05-05',  0.05, 1),
 (N'Max',      N'Perro', N'Pastor Alemán',   '2018-09-30', 32.00, 2),
 (N'Luna',     N'Gato',  NULL,               '2023-02-14',  3.50, 2),
 (N'Toby',     N'Conejo', N'Cabeza de León', '2022-08-25',  1.90, 3);
GO

SELECT * FROM dbo.Duenos;
SELECT * FROM dbo.Mascotas;
GO
