DROP DATABASE IF EXISTS registro_estudiantes;
CREATE DATABASE registro_estudiantes
  CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE registro_estudiantes;

CREATE TABLE Profesor (
  Id        INT AUTO_INCREMENT PRIMARY KEY,
  Documento VARCHAR(20)  NOT NULL,
  Nombre    VARCHAR(100) NOT NULL,
  Correo    VARCHAR(150) NOT NULL,
  Telefono  VARCHAR(20)  NULL,
  CONSTRAINT UQ_Profesor_Documento UNIQUE (Documento),
  CONSTRAINT UQ_Profesor_Correo    UNIQUE (Correo)
);

CREATE TABLE Materia (
  Id         INT AUTO_INCREMENT PRIMARY KEY,
  Nombre     VARCHAR(100) NOT NULL,
  Creditos   INT NOT NULL DEFAULT 3,
  ProfesorId INT NOT NULL,
  CONSTRAINT UQ_Materia_Nombre UNIQUE (Nombre),
  CONSTRAINT FK_Materia_Profesor FOREIGN KEY (ProfesorId)
    REFERENCES Profesor (Id) ON DELETE RESTRICT
);

CREATE TABLE Estudiante (
  Id            INT AUTO_INCREMENT PRIMARY KEY,
  Documento     VARCHAR(20)  NOT NULL,
  Nombre        VARCHAR(100) NOT NULL,
  Correo        VARCHAR(150) NOT NULL,
  Telefono      VARCHAR(20)  NULL,
  PasswordHash  VARCHAR(100) NOT NULL,
  Rol           VARCHAR(20)  NOT NULL DEFAULT 'Estudiante',
  Activo        BOOLEAN      NOT NULL DEFAULT TRUE,
  FechaRegistro DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT UQ_Estudiante_Documento UNIQUE (Documento),
  CONSTRAINT UQ_Estudiante_Correo    UNIQUE (Correo),
  CONSTRAINT CK_Estudiante_Rol CHECK (Rol IN ('Estudiante', 'Administrador'))
);

CREATE TABLE Inscripcion (
  Id               INT AUTO_INCREMENT PRIMARY KEY,
  EstudianteId     INT NOT NULL,
  MateriaId        INT NOT NULL,
  FechaInscripcion DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT UQ_Inscripcion_Estudiante_Materia UNIQUE (EstudianteId, MateriaId),
  CONSTRAINT FK_Inscripcion_Estudiante FOREIGN KEY (EstudianteId)
    REFERENCES Estudiante (Id) ON DELETE CASCADE,
  CONSTRAINT FK_Inscripcion_Materia FOREIGN KEY (MateriaId)
    REFERENCES Materia (Id) ON DELETE RESTRICT
);