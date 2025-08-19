[![中文说明](https://img.shields.io/badge/文档-中文-blue?style=flat-square)](./README.zh-CN.md)

# HelloCity Backend Service

A modern backend service based on .NET 8, providing user management and checklist functionality with layered architecture, JWT authentication, and PostgreSQL database support.

## Quick Start

### Requirements

- **.NET SDK**: 8.0 or higher
- **PostgreSQL**: 16 or higher
- **Docker Desktop**: 4.43.1 or higher
- **Operating System**: Windows, macOS, Linux

### 1. Clone Project

```bash
git clone https://github.com/HelloCity-AI/HelloCityBackendService.git
cd HelloCityBackendService
```

### 2. Start Database

```bash
docker compose up
```

> API service is commented out in compose.yaml, only database will start. For development/debugging, recommend using `dotnet run`.

### 3. Configure Database Connection

**Important: Never commit real passwords to repository!**

```bash
# Copy configuration template
cp HelloCity.Api/appsettings.Development.json.example HelloCity.Api/appsettings.Development.json
```

Then edit `appsettings.Development.json` with your database information. For development environment, you can refer to the postgres service configuration in `compose.yaml`.

> `appsettings.Development.json` is in .gitignore and will never be committed to repository

### 4. Initialize Database

Connect to PostgreSQL using database client and execute:

```sql
CREATE TABLE IF NOT EXISTS test (
  "Id" SERIAL PRIMARY KEY,
  "Email" VARCHAR(255) NOT NULL,
  "Password" VARCHAR(255) NOT NULL
);

INSERT INTO test ("Email", "Password") VALUES
  ('test@example.com', 'testpass'),
  ('demo@example.com', 'demopass');
```

### 5. Database Migration

```bash
# Add initial migration
dotnet ef migrations add InitialCreate --project HelloCity.Models

# Update database
dotnet ef database update --project HelloCity.Api
```

### 6. Run Project

```bash
# Install dependencies
dotnet restore

# Start API service
cd HelloCity.Api
dotnet run
```

Visit http://localhost:5000/swagger to view API documentation

## Running Tests

### Unit Tests

```bash
# Run all tests
dotnet test

# Generate coverage report
dotnet test --collect:"XPlat Code Coverage"
```

### Generate HTML Coverage Report

```bash
# 1. Install report generator tool (first time only)
dotnet tool install --global dotnet-reportgenerator-globaltool

# 2. Generate report
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coverage-report" -reporttypes:Html

# 3. View report
open coverage-report/index.html  # macOS
start coverage-report/index.html # Windows
```

## API Endpoints

### User Management

- **GET** `/api/user-profile/{id}` - Get user information
- **POST** `/api/user-profile` - Create user
- **PUT** `/api/user-profile/{id}` - Update user information
- **POST** `/api/user-profile/{userId}/checklist-item` - Create checklist item for user
- **GET** `/api/user-profile/{userId}/checklist-item` - Get user's checklist

### Test Endpoints

- **GET** `/api/TestUser` - Test database connection (requires JWT authentication)

Visit `/swagger` page for detailed API documentation.

## Project Structure

```
HelloCity/
├── HelloCity.Api/           # API Layer - Controllers, configurations and DTOs
├── HelloCity.Services/      # Business logic layer
├── HelloCity.Repository/    # Data access layer
├── HelloCity.IRepository/   # Repository interfaces
├── HelloCity.IServices/     # Service interfaces
├── HelloCity.Models/        # Entity models and database context
├── HelloCity.Tests/         # Test projects
└── compose.yaml            # Docker configuration
```

### Design Patterns

- **Layered Architecture**: API → Services → Repository → Database
- **Dependency Injection**: All services managed through DI container
- **Repository Pattern**: Data access abstraction
- **DTO Pattern**: Data transfer objects handled in API layer

## Tech Stack

### Core Framework

- **.NET 8** - Runtime framework
- **ASP.NET Core** - Web API framework
- **Entity Framework Core** - ORM
- **PostgreSQL** - Database

### Development Tools

- **AutoMapper** - Object mapping
- **FluentValidation** - Data validation
- **Serilog** - Structured logging
- **Swagger** - API documentation

### Authentication & Authorization

- **JWT Bearer** - Token authentication
- **Auth0** - Identity service

### Testing Framework

- **xUnit** - Unit testing framework
- **Moq** - Mocking framework
- **FluentAssertions** - Fluent assertion library
- **Coverlet** - Code coverage
- **ReportGenerator** - Coverage reports

### Deployment Tools

- **Docker** - Containerization
- **Docker Compose** - Service orchestration

## Development Configuration

### View Logs

Log files are saved in `Logs/` directory with daily rolling retention of 7 days.

### CORS Configuration

Default allows frontend access from `http://localhost:3000`.

## Development Standards

### Branch Naming

```bash
git checkout -b SCRUM-123-feature-name
```

### Code Commits

- Follow [Conventional Commits](https://www.conventionalcommits.org/) specification
- Run tests before each commit to ensure they pass

### Configuration Files

- Commit `appsettings.json` and `appsettings.Development.json.example`
- Never commit `appsettings.Development.json` (contains real passwords)

## Deployment Guide

### Docker Deployment

```bash
# Uncomment API service configuration in compose.yaml
docker compose up
# Access http://localhost:5050
```

### Production Environment Variables

- `ConnectionStrings__DefaultConnection` - Database connection string
- `JWT__Authority` - JWT validation address
- `JWT__Audience` - JWT audience

---

Last updated: 2025-08-16
