create database SecureDB

USE SecureDB;
GO

CREATE TABLE Personas (
    Identificador INT IDENTITY(1,1) PRIMARY KEY,
    Nombres NVARCHAR(100) NOT NULL,
    Apellidos NVARCHAR(100) NOT NULL,
    NumeroIdentificacion NVARCHAR(20) NOT NULL UNIQUE,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    TipoIdentificacion NVARCHAR(50) NOT NULL,
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
    NumeroIdentificacionConTipo AS (NumeroIdentificacion + ' - ' + TipoIdentificacion) PERSISTED,
    NombresApellidos AS (Nombres + ' ' + Apellidos) PERSISTED
);
GO

CREATE TABLE Usuario (
    Identificador INT IDENTITY(1,1) PRIMARY KEY,
    Usuario NVARCHAR(50) NOT NULL UNIQUE,
    Pass NVARCHAR(128) NOT NULL,
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE()
);
GO

CREATE PROCEDURE ConsultarPersonas
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SELECT * FROM Personas
    ORDER BY Identificador
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO

