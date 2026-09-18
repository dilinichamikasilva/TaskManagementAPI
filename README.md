# DemoApi

A REST API for managing projects, tasks, and users, built with **.NET 10** and **Entity Framework Core**. This is a personal learning project used to practice a layered ASP.NET Core architecture, EF Core migrations, JWT authentication, and unit testing — it has been developed and tested locally only, and is not deployed anywhere.

## What it does

- **Projects** — create and manage projects
- **Tasks** — CRUD operations on tasks, each linked to a project and optionally assigned to a user, with a status (`ToDo`, `InProgress`, `Done`)
- **Users** — manage user accounts
- **Auth** — register/login with email + password; all resource endpoints require a valid JWT

## Tech stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 10 (Web API) |
| Database | SQL Server (LocalDB) via Entity Framework Core |
| Auth | JWT Bearer tokens, passwords hashed with BCrypt |
| API docs | Swagger / OpenAPI (Swashbuckle) |
| Testing | xUnit + Moq |

## Architecture

The project follows a layered structure to keep concerns separated:

```
Controllers/   → HTTP endpoints, request/response DTOs only
Services/      → business logic and validation
Repositories/  → data access via EF Core
Models/        → EF Core entities
Dtos/          → request/response contracts (never expose entities directly)
Mapping/       → entity ↔ DTO conversion
Middleware/    → global exception handling
Data/          → DbContext
Migrations/    → EF Core migrations
```

Each resource (Tasks, Projects, Users) follows the same `Repository → Service → Controller` pattern, with an interface for each layer so it stays testable and swappable.

## Getting started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server LocalDB (installed with Visual Studio, or the standalone [SQL Server Express LocalDB](https://learn.microsoft.com/sql/database-engine/configure-windows/sql-server-express-localdb))

### 1. Clone and restore

```bash
git clone https://github.com/dilinichamikasilva/TaskManagementAPI.git
cd TaskManagementAPI
dotnet restore
```

### 2. Configure the JWT signing key

The app requires a `Jwt:Key` to sign tokens. For local development, set it via user-secrets instead of committing it to `appsettings.json`:

```bash
dotnet user-secrets init
dotnet user-secrets set "Jwt:Key" "a-long-random-development-only-secret"
```

### 3. Apply the database migrations

```bash
dotnet tool install --global dotnet-ef   # if not already installed
dotnet ef database update
```

This creates the `DemoApiDb` database in LocalDB with the `Projects`, `Users`, and `Tasks` tables.

### 4. Run the API

```bash
dotnet run
```

Swagger UI is available at `https://localhost:<port>/swagger` when running in Development — use it to explore and try out every endpoint.

## Authentication flow

1. `POST /api/auth/register` with `{ name, email, password }` → returns a JWT
2. `POST /api/auth/login` with `{ email, password }` → returns a JWT
3. Pass the token as `Authorization: Bearer <token>` on every other request

A ready-to-use set of sample requests (register, login, and CRUD calls) is in [`DemoApi.http`](./DemoApi.http) — open it in VS Code or Visual Studio's HTTP client.

## API endpoints

| Method | Endpoint | Auth required | Description |
|---|---|---|---|
| POST | `/api/auth/register` | No | Create an account, returns a JWT |
| POST | `/api/auth/login` | No | Log in, returns a JWT |
| GET | `/api/tasks` | Yes | List all tasks |
| GET | `/api/tasks/{id}` | Yes | Get a task by id |
| POST | `/api/tasks` | Yes | Create a task |
| PUT | `/api/tasks/{id}` | Yes | Update a task |
| DELETE | `/api/tasks/{id}` | Yes | Delete a task |
| GET | `/api/projects` | Yes | List all projects |
| GET | `/api/projects/{id}` | Yes | Get a project by id |
| POST | `/api/projects` | Yes | Create a project |
| PUT | `/api/projects/{id}` | Yes | Update a project |
| DELETE | `/api/projects/{id}` | Yes | Delete a project |
| GET | `/api/users` | Yes | List all users |
| GET | `/api/users/{id}` | Yes | Get a user by id |
| PUT | `/api/users/{id}` | Yes | Update a user |
| DELETE | `/api/users/{id}` | Yes | Delete a user |

## Running the tests

```bash
dotnet test
```

Unit tests cover the service layer (validation rules, password hashing, login/registration logic) with the repository layer mocked out.

## What I learned building this

- Structuring an ASP.NET Core API into Repository/Service/Controller layers with interfaces at each boundary
- Modeling relationships (one-to-many, optional foreign keys) with EF Core and generating/applying migrations
- Implementing JWT-based authentication from scratch: password hashing, token issuing, and protecting endpoints with `[Authorize]`
- Keeping API contracts separate from database entities using DTOs
- Centralizing error handling with custom middleware instead of repeating try/catch in every controller
- Writing unit tests against the service layer using mocked dependencies (Moq)

## Notes / limitations

- Tested locally only — not deployed to any environment.
- Uses SQL Server LocalDB, which is Windows-only; a real deployment would need a different SQL Server/Postgres setup.
- No role-based authorization — any authenticated user can access any endpoint.
