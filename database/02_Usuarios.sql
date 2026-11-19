-- =============================================
--  VeterinariaDB - Semana 14: tabla Usuarios (autenticación JWT)
--  Ejecutar DESPUÉS de VeterinariaDB.sql
--  Roles del negocio: Recepcionista y Veterinario
-- =============================================

USE VeterinariaDB;
GO

IF OBJECT_ID(N'dbo.Usuarios', N'U') IS NOT NULL DROP TABLE dbo.Usuarios;
GO

CREATE TABLE dbo.Usuarios
(
    Id         INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Usuarios PRIMARY KEY,
    Correo     NVARCHAR(100)     NOT NULL CONSTRAINT UQ_Usuarios_Correo UNIQUE,
    ClaveHash  NVARCHAR(100)     NOT NULL,   -- nunca la contraseña en texto, solo su hash BCrypt
    Rol        NVARCHAR(30)      NOT NULL
        CONSTRAINT CK_Usuarios_Rol CHECK (Rol IN (N'Recepcionista', N'Veterinario'))
);
GO

-- Un usuario por cada rol (las claves de prueba se guardan como hash BCrypt)
--   recepcion@veterinaria.com   clave de prueba: Recepcion123*
--   veterinario@veterinaria.com clave de prueba: Veterinario123*
INSERT INTO dbo.Usuarios (Correo, ClaveHash, Rol) VALUES
 (N'recepcion@veterinaria.com',   N'$2a$11$FYIcaTLmuO2lVWq.oy9N2O72dFhYziVd6y6YnWeSWcuBxAwBWJeB2', N'Recepcionista'),
 (N'veterinario@veterinaria.com', N'$2a$11$c8YY2kQ4aex01rNs2l743Og0FlxjzOwoEI8V2SUDGMoBERTIHdwaW', N'Veterinario');
GO

-- Para promover a Veterinario a un usuario registrado por la API (el registro siempre asigna Recepcionista):
-- UPDATE dbo.Usuarios SET Rol = N'Veterinario' WHERE Correo = N'correo@ejemplo.com';

SELECT Id, Correo, Rol FROM dbo.Usuarios;
GO
