# Student Registration

Web application for student registration and subject enrollment, built as a technical assessment.

- **Frontend:** Angular 21 (TypeScript)
- **Backend:** ASP.NET Core 8 Web API (C#), Entity Framework Core
- **Database:** MySQL 8.0

## Features

- Online student registration, JWT login and two roles: **Student** and **Administrator**.
- Student CRUD: registration, viewing and editing your own profile; the administrator lists, deactivates, reactivates and deletes students.
- 10 subjects worth 3 credits each, and 5 professors who teach 2 subjects each.
- Enrollment in exactly **3 subjects (9 credits)** taught by **3 different professors**.
- Each student sees **only the names** of their classmates in the subjects they are enrolled in.
- Admin panel: students, professors and subjects (read-only for the last two).

## Business rules

| Rule | Where it is enforced |
|---|---|
| Exactly 3 subjects, no duplicates | API (400) and UI |
| 3 different professors | API (400) and UI |
| Unique email and ID number | API (409) and MySQL `UNIQUE` constraint |
| Classmates: names only, only for my subjects | `CompaneroDto` and API (403) |
| A student can only manage their own data | JWT + roles (401/403) |
| The administrator cannot deactivate or delete themselves | API (400) |

## Design decisions

- **Credit program:** interpreted as a rule (3 subjects × 3 credits = 9), not as a table.
- **Requirement 8 ("view other students' records"):** interpreted with privacy in mind: a student only sees the names of classmates in the subjects they are enrolled in.
- **Deletion:** the administrator can *deactivate* (soft delete, `Activo = 0`, removes the student's enrollments) or *permanently delete* (with cascade). An inactive student cannot log in. Only the administrator can reactivate.
- **Professors:** fixed data loaded by script; they do not log in.
- **Security:** BCrypt password hashing, signed JWT, role assigned by the server, DTOs that never expose the hash, and validation on both frontend **and** backend.

## Prerequisites

- .NET SDK 8
- Node.js 20 or 22 LTS and npm
- Angular CLI (`npm install -g @angular/cli`)
- MySQL Server 8.0

## Installation and running

### 1. Database

In MySQL (Workbench or CLI), run in this order:

1. `database/01_schema.sql` (creates the `registro_estudiantes` database and tables)
2. `database/02_seed.sql` (professors, subjects, administrator and sample students)

### 2. Backend

The API needs two settings: the connection string to **your** MySQL and a JWT key. Choose **one** option.

**Option A (simplest): edit `appsettings.json`**

Open `backend/RegistroEstudiantes.Api/appsettings.json` and replace:

- `Password=CAMBIAR_PASSWORD` → your local MySQL `root` password (also change `Port` if yours is not 3306).
- `CAMBIAR_ESTA_CLAVE_POR_UNA_DE_32_CARACTERES_O_MAS` → any random text of at least 32 characters.

**Option B: User Secrets (no file edits)**

```bash
cd backend/RegistroEstudiantes.Api
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Port=3306;Database=registro_estudiantes;User=root;Password=YOUR_PASSWORD;"
dotnet user-secrets set "Jwt:Key" "a-long-random-phrase-of-at-least-32-characters"
```

Then run the API:

```bash
cd backend/RegistroEstudiantes.Api
dotnet run
```

The API listens on `http://localhost:5068` (Swagger at `/swagger`).

> Real credentials are never committed to the repository: the committed `appsettings.json` only contains placeholders.

### 3. Frontend

```bash
cd frontend
npm install
ng serve
```

If `npm install` fails with an internal npm error, use `npm install --legacy-peer-deps`.
The app runs on `http://localhost:4200`. If you use another port (`ng serve --port 4201`), the backend already allows 4200 and 4201.

## Test accounts

| Role | Email | Password | Notes |
|---|---|---|---|
| Administrator | `admin@universidad.edu` | `Admin123*` | |
| Student | `laura@universidad.edu` | `Estudiante123*` | Mathematics, Programming, Statistics |
| Student | `pedro@universidad.edu` | `Estudiante123*` | Mathematics, Databases, Networks |
| Student | `sofia@universidad.edu` | `Estudiante123*` | Programming, Statistics, Software Engineering |
| Student | `andres@universidad.edu` | `Estudiante123*` | Enrollment pending |
| Student | `camila@universidad.edu` | `Estudiante123*` | Inactive account (cannot log in) |

These are demo data. Students who register through the UI use their own password (at least 8 characters, with an uppercase letter, a lowercase letter and a number).

## Architecture

```
Angular (port 4200)  ──HTTP/JSON + JWT──>  ASP.NET Core API (5068)  ──EF Core──>  MySQL
```

- **Backend:** `Controllers` (receive and respond), `Services` (business rules), `Entities`, `Dtos`, `Data` (DbContext), `Middleware` (errors → HTTP status codes).
- **Frontend:** `pages` (screens), `core` (HTTP services, token interceptor, guards).
- **Database:** `Profesor`, `Materia`, `Estudiante`, `Inscripcion`, with foreign keys (`RESTRICT` and `CASCADE`) and `UNIQUE` constraints.

## Main endpoints

| Method and route | Description | Access |
|---|---|---|
| `POST /api/auth/registro` | Register a student | Public |
| `POST /api/auth/login` | Log in, returns a JWT | Public |
| `GET/PUT /api/estudiantes/me` | My profile | Authenticated |
| `GET /api/estudiantes` | List students | Admin |
| `PUT /api/estudiantes/{id}/inactivar` and `/reactivar` | Deactivate / reactivate | Admin |
| `DELETE /api/estudiantes/{id}` | Permanently delete | Admin |
| `GET /api/materias` | Subjects with their professor | Authenticated |
| `GET /api/materias/{id}/companeros` | Classmates (names only) | Authenticated |
| `GET /api/materias/resumen` | Subjects with enrolled counts | Admin |
| `GET /api/profesores` | Professors and their subjects | Authenticated |
| `GET/PUT /api/inscripciones/me` | My enrollment (3 subjects) | Student |

## Limitations and future improvements

- Professors cannot be created or edited from the app (fixed data via script).
- There is no Professor role and no password recovery.
- The JWT is stored in `sessionStorage`; a more robust alternative would be an `HttpOnly` cookie.
- There are no automated tests; manual tests are documented in `docs/PRUEBAS.md`.