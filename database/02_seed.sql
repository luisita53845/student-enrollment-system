USE registro_estudiantes;

INSERT INTO Profesor (Id, Documento, Nombre, Correo, Telefono) VALUES
(1, '80000001', 'Luisa Parra',  'luisa.parra@universidad.edu',  '3001110001'),
(2, '80000002', 'Ana Rodríguez',   'ana.rodriguez@universidad.edu',   '3001110002'),
(3, '80000003', 'Luis Herrera',    'luis.herrera@universidad.edu',    '3001110003'),
(4, '80000004', 'María Torres',    'maria.torres@universidad.edu',    '3001110004'),
(5, '80000005', 'Jorge Castillo',  'jorge.castillo@universidad.edu',  '3001110005');

INSERT INTO Materia (Id, Nombre, Creditos, ProfesorId) VALUES
(1,  'Matemáticas',              3, 1),
(2,  'Física',                   3, 1),
(3,  'Programación',             3, 2),
(4,  'Bases de Datos',           3, 2),
(5,  'Estadística',              3, 3),
(6,  'Álgebra Lineal',           3, 3),
(7,  'Redes de Computadores',    3, 4),
(8,  'Sistemas Operativos',      3, 4),
(9,  'Ingeniería de Software',   3, 5),
(10, 'Inteligencia Artificial',  3, 5);

INSERT INTO Estudiante (Id, Documento, Nombre, Correo, Telefono, PasswordHash, Rol, Activo) VALUES
(1, '10000001', 'Admin Sistema',  'admin@universidad.edu',   NULL,         '$2a$11$dFE/qo0CYb0zgCLe./f9VeqY8on6RriqYFrQNiP4T1Vmhnc2gNfhy', 'Administrador', TRUE),
(2, '10000002', 'Laura Pérez',    'laura@universidad.edu',   '3101000002', '$2a$11$4lSe3I6trn3bMXJbg6aLzOgr92iTtgkYVaoQ2h7nPkBw5J/FxGTDG', 'Estudiante',    TRUE),
(3, '10000003', 'Pedro Gómez',    'pedro@universidad.edu',   '3101000003', '$2a$11$4lSe3I6trn3bMXJbg6aLzOgr92iTtgkYVaoQ2h7nPkBw5J/FxGTDG', 'Estudiante',    TRUE),
(4, '10000004', 'Sofía Ramírez',  'sofia@universidad.edu',   '3101000004', '$2a$11$4lSe3I6trn3bMXJbg6aLzOgr92iTtgkYVaoQ2h7nPkBw5J/FxGTDG', 'Estudiante',    TRUE),
(5, '10000005', 'Andrés Díaz',    'andres@universidad.edu',  '3101000005', '$2a$11$4lSe3I6trn3bMXJbg6aLzOgr92iTtgkYVaoQ2h7nPkBw5J/FxGTDG', 'Estudiante',    TRUE),
(6, '10000006', 'Camila Vargas',  'camila@universidad.edu',  '3101000006', '$2a$11$4lSe3I6trn3bMXJbg6aLzOgr92iTtgkYVaoQ2h7nPkBw5J/FxGTDG', 'Estudiante',    FALSE);

INSERT INTO Inscripcion (EstudianteId, MateriaId) VALUES
(2, 1), (2, 3), (2, 5),
(3, 1), (3, 4), (3, 7),
(4, 3), (4, 5), (4, 9);