# Agent Notes

## Repository Overview

Full-stack reservation system with a .NET 10 backend and two React 19 Vite frontends (web and admin). No CI, Docker, or tests exist yet.

## Backend (`backend/`)

- **Solution**: `ReserveStar.slnx` — build from `backend/` with `dotnet build` or `dotnet build ReserveStar.slnx`.
- **.NET version**: `net10.0`. Uses Central Package Management (`Directory.Packages.props`).
- **Projects** (bottom-up dependency order):
   - `ReserveStar.Helper` — environment variables, constants, API abstractions, extensions.
   - `ReserveStar.Data` — EF Core + PostgreSQL (`Npgsql.EntityFrameworkCore.PostgreSQL`).
   - `ReserveStar.Core` — authorization, Redis cache, repository/UoW, RabbitMQ queue, middlewares, resources.
   - `ReserveStar.Application` — MediatR commands/handlers (only `Auth/Login` so far).
   - `ReserveStar.Utils` — DI registration (`ServiceExtensions`) and middleware pipeline helpers (`ApplicationExtensions`).
- **No executable host project exists yet**. There is no `Program.cs` or ASP.NET API project. `ReserveStar.Utils` is meant to be referenced by a future host.
- **Nullable settings vary by project**: `Helper`, `Core`, `Utils` → `enable`; `Application`, `Data` → `disable`.
- **EF Core migrations are gitignored** (`**/[Mm]igrations` in root `.gitignore`). Generate them with `dotnet ef migrations add <Name>` inside the `Data` project (or a future host).

### Environment & Configuration

- Config is driven by **environment variables** with the prefix `RESERVESTAR_*` (not `RESERVESTAR_*`).
- `ServiceExtensions.AddEnvironmentVariables()` also loads a `.env` file from the **parent directory of `Directory.GetCurrentDirectory()`**. This means:
   - If you run `dotnet` from `backend/`, it looks for `.env` in the repo root.
   - If you run from a subproject folder, it looks in `backend/`.
   - The repo currently has an empty `backend/.env`.
- Key variables (with hardcoded defaults):
   - `RESERVESTAR_DB_INSTANCE` (localhost), `RESERVESTAR_DB_PORT` (5432), `RESERVESTAR_DB_NAME` (rESERVESTAR), `RESERVESTAR_DB_USERNAME` (postgres), `RESERVESTAR_DB_PASSWORD` (postgres)
   - `RESERVESTAR_JWT_TOKEN_KEY` (secret)
   - `RESERVESTAR_REDIS_SERVISE_ENDPOINT`, `RESERVESTAR_REDIS_SERVICE_PORT`, `RESERVESTAR_REDIS_SERVISE_PASSWORD`
   - `RESERVESTAR_QUEUE_PROVIDER` (development), `RESERVESTAR_RABBITMQ_HOST` (localhost), `RESERVESTAR_RABBITMQ_PORT` (5672), `RESERVESTAR_RABBITMQ_USER` (guest), `RESERVESTAR_RABBITMQ_PASSWORD` (guest), `RESERVESTAR_RABBITMQ_VHOST` (/)
   - `DEFAULT_SUPERADMIN_USERNAME` (web), `DEFAULT_SUPERADMIN_PASSWORD` (web), `DEFAULT_SUPERADMIN_EMAIL` (web)

### Running / Verifying

- Build all: `cd backend && dotnet build`
- Build single project: `cd backend/ReserveStar.Core && dotnet build`
- No test projects exist.

## Frontends

Both frontends are **React 19 + TypeScript + Vite** and use **Bun** (`bun.lock`).

### `frontend/web/`

- Consumer-facing app.
- Dev server: `bun run dev` → Vite on default port `5173`.
- Build: `bun run build` (runs `tsc -b && vite build`).
- Lint: `bun run lint`
- **React Compiler is enabled** via `@rolldown/plugin-babel` with `reactCompilerPreset()` in `vite.config.ts`. This impacts dev/build performance.

### `frontend/admin/`

- Admin-facing app. Identical toolchain to `frontend/web/`.
- Dev server: `bun run dev` → Vite on default port `5174`.

### CORS Note

Backend `ServiceExtensions.AddCommonServices()` hardcodes CORS origins to `localhost:5173` and `localhost:5174`. If you change Vite ports, update the backend CORS policy.

## Monorepo Boundaries

- `backend/` — .NET solution. No shared code with frontends except via HTTP API contract.
- `frontend/web/` and `frontend/admin/` — independent Vite apps. `frontend/shared/` is currently empty.
- No root-level package manager or workspace config. Frontends must be managed individually.

## Style / Conventions

- **Backend**: MediatR command/handler pattern (`LoginCommand` → `LoginCommandHandler`). Handlers inherit from `BaseHandler` for resource translation (`T(key, languageId)`). FluentValidation validators are registered assembly-scoped.
- **Frontend**: ESLint config is flat (`eslint.config.js`) using `typescript-eslint`, `eslint-plugin-react-hooks`, and `eslint-plugin-react-refresh`.

## Important Constraints

- Do not commit `.env` files (gitignored).
- Do not commit generated EF Core migrations (gitignored).
- No tests exist — changes cannot be verified by running a test suite.
