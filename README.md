# FurniSpace Backend

ASP.NET Core Minimal API on .NET 8, organized as Clean Architecture with vertical slices in the application layer.

## Prerequisites

- .NET SDK 8
- Docker (optional, for PostgreSQL)

## Run locally

```powershell
docker compose up -d postgres
dotnet restore
dotnet run --project src/FurniSpace.Api
```

The development profile exposes the API URL shown by `dotnet run`. Check:

- `GET /health`
- `GET /furnispace/api/v1/health/database`
- `GET /furnispace/api/v1/`

Authentication endpoints under `/furnispace/api/v1/auth`:

- `POST /register`
- `POST /login`
- `POST /refresh`
- `POST /logout` (Bearer token required)
- `GET /me` (Bearer token required)

The development JWT signing key is stored in .NET User Secrets and is not committed
to the repository.

The local development configuration connects to `furnispace_db` on PostgreSQL at
`localhost:5432` using the `postgres` role. If your PostgreSQL authentication requires
a password, keep it outside source control:

```powershell
dotnet user-secrets init --project src/FurniSpace.Api
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=furnispace_db;Username=postgres;Password=YOUR_PASSWORD" --project src/FurniSpace.Api
```

## Database migrations

```powershell
dotnet ef migrations add MigrationName --project src/FurniSpace.Infrastructure --startup-project src/FurniSpace.Api --context AppDbContext --output-dir Persistence/Migrations
dotnet ef database update --project src/FurniSpace.Infrastructure --startup-project src/FurniSpace.Api --context AppDbContext
```

## Postman

Collection, local environment, dan panduan pengujian tersedia di
`docs/postman/`. Import kedua file JSON ke Postman lalu pilih environment
`FurniSpace Local`.

## Validate

```powershell
dotnet build --no-restore
dotnet test --no-build
```

## Projects

- `FurniSpace.Api`: HTTP endpoints, middleware, and exception handling.
- `FurniSpace.Application`: use cases, feature DTOs/services, and abstractions.
- `FurniSpace.Domain`: entities and business constants.
- `FurniSpace.Infrastructure`: persistence and external service implementations.

Secrets must be supplied with environment variables or .NET user secrets; do not commit production credentials.
