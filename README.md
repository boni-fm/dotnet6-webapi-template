# .NET 6 Web API Template

A comprehensive and production-ready .NET 6 Web API template featuring clean architecture, best practices, and modern development tools.

![.NET 6](https://img.shields.io/badge/.NET-6.0-blue)
![License](https://img.shields.io/badge/license-MIT-green)
![Build Status](https://github.com/boni-fm/dotnet6-webapi-template/workflows/CI%2FCD%20Pipeline/badge.svg)

## Features

### 🏗️ Architecture
- **Clean Architecture** with clear separation of concerns
- **Domain-Driven Design** principles
- **SOLID** principles implementation
- **Repository Pattern** for data access
- **Service Layer** for business logic

### 🚀 Technologies & Frameworks
- **.NET 6** - Latest LTS version
- **Entity Framework Core 6** - ORM with SQL Server support
- **AutoMapper** - Object-to-object mapping
- **Serilog** - Structured logging
- **Swagger/OpenAPI** - API documentation
- **JWT Authentication** - Secure API endpoints
- **Health Checks** - Application monitoring

### 🔧 Development Tools
- **Docker** support with multi-stage builds
- **GitHub Actions** CI/CD pipeline
- **EditorConfig** for consistent code style
- **Unit & Integration Tests** with xUnit
- **Code Coverage** reporting

### 📊 Built-in Features
- **CORS** configuration
- **Exception handling** middleware
- **Logging** with multiple sinks
- **Health checks** endpoint
- **Environment-specific** configurations
- **Database migrations** support

## Project Structure

```
src/
├── DotNet6WebApiTemplate.Api/          # Web API layer
│   ├── Controllers/                    # API controllers
│   ├── Extensions/                     # Service registrations
│   └── Program.cs                      # Application entry point
├── DotNet6WebApiTemplate.Application/  # Application layer
│   ├── DTOs/                          # Data transfer objects
│   ├── Interfaces/                    # Application interfaces
│   ├── Mappings/                      # AutoMapper profiles
│   └── Services/                      # Business logic services
├── DotNet6WebApiTemplate.Domain/       # Domain layer
│   ├── Entities/                      # Domain entities
│   ├── Interfaces/                    # Domain interfaces
│   └── Common/                        # Base classes
└── DotNet6WebApiTemplate.Infrastructure/ # Infrastructure layer
    ├── Data/                          # DbContext and configurations
    └── Repositories/                  # Data access implementations

tests/
├── DotNet6WebApiTemplate.Tests.Unit/          # Unit tests
└── DotNet6WebApiTemplate.Tests.Integration/   # Integration tests
```

## Getting Started

### Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server) or [SQL Server LocalDB](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb)
- [Docker](https://www.docker.com/) (optional)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/boni-fm/dotnet6-webapi-template.git
   cd dotnet6-webapi-template
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Update connection string**
   
   Edit `src/DotNet6WebApiTemplate.Api/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Your SQL Server connection string here"
     }
   }
   ```

4. **Run database migrations**
   ```bash
   dotnet ef database update --project src/DotNet6WebApiTemplate.Infrastructure --startup-project src/DotNet6WebApiTemplate.Api
   ```

5. **Run the application**
   ```bash
   dotnet run --project src/DotNet6WebApiTemplate.Api
   ```

6. **Access Swagger UI**
   
   Navigate to `https://localhost:5001/swagger` (or `http://localhost:5000/swagger`)

### Using Docker

1. **Using Docker Compose (Recommended)**
   ```bash
   docker-compose up -d
   ```
   
   This will start both the API and SQL Server containers.

2. **Build and run manually**
   ```bash
   docker build -t dotnet6-webapi-template .
   docker run -p 5000:80 dotnet6-webapi-template
   ```

## API Endpoints

The template includes a sample **Products** resource with full CRUD operations:

| Method | Endpoint           | Description          |
|--------|--------------------|----------------------|
| GET    | `/api/products`    | Get all products     |
| GET    | `/api/products/{id}` | Get product by ID  |
| POST   | `/api/products`    | Create new product   |
| PUT    | `/api/products/{id}` | Update product     |
| DELETE | `/api/products/{id}` | Delete product     |

### Health Check

- **GET** `/health` - Application health status

## Configuration

### Environment Variables

| Variable | Description | Default |
|----------|-------------|---------|
| `ASPNETCORE_ENVIRONMENT` | Environment name | `Development` |
| `ConnectionStrings__DefaultConnection` | Database connection string | LocalDB connection |

### JWT Configuration

Configure JWT settings in `appsettings.json`:

```json
{
  "JWT": {
    "Key": "your-secret-key",
    "Issuer": "your-issuer",
    "Audience": "your-audience",
    "ExpiryInDays": 7
  }
}
```

## Testing

### Run Unit Tests
```bash
dotnet test tests/DotNet6WebApiTemplate.Tests.Unit/
```

### Run Integration Tests
```bash
dotnet test tests/DotNet6WebApiTemplate.Tests.Integration/
```

### Run All Tests
```bash
dotnet test
```

### Generate Code Coverage
```bash
dotnet test --collect:"XPlat Code Coverage"
```

## Database Migrations

### Add a new migration
```bash
dotnet ef migrations add MigrationName --project src/DotNet6WebApiTemplate.Infrastructure --startup-project src/DotNet6WebApiTemplate.Api
```

### Update database
```bash
dotnet ef database update --project src/DotNet6WebApiTemplate.Infrastructure --startup-project src/DotNet6WebApiTemplate.Api
```

### Remove last migration
```bash
dotnet ef migrations remove --project src/DotNet6WebApiTemplate.Infrastructure --startup-project src/DotNet6WebApiTemplate.Api
```

## Deployment

### Docker Deployment

1. **Build the image**
   ```bash
   docker build -t your-app-name .
   ```

2. **Run the container**
   ```bash
   docker run -p 8080:80 \
     -e ConnectionStrings__DefaultConnection="your-connection-string" \
     your-app-name
   ```

### CI/CD Pipeline

The template includes a GitHub Actions workflow that:

- Runs tests on multiple .NET versions
- Builds and pushes Docker images
- Performs security scans
- Generates code coverage reports

## Development Guidelines

### Adding New Features

1. **Domain Entity**: Add to `src/DotNet6WebApiTemplate.Domain/Entities/`
2. **Repository Interface**: Add to `src/DotNet6WebApiTemplate.Domain/Interfaces/`
3. **Repository Implementation**: Add to `src/DotNet6WebApiTemplate.Infrastructure/Repositories/`
4. **DTOs**: Add to `src/DotNet6WebApiTemplate.Application/DTOs/`
5. **Service Interface**: Add to `src/DotNet6WebApiTemplate.Application/Interfaces/`
6. **Service Implementation**: Add to `src/DotNet6WebApiTemplate.Application/Services/`
7. **Controller**: Add to `src/DotNet6WebApiTemplate.Api/Controllers/`
8. **Tests**: Add unit and integration tests

### Code Style

- Follow the `.editorconfig` settings
- Use meaningful names for classes, methods, and variables
- Write XML documentation for public APIs
- Keep methods small and focused
- Follow SOLID principles

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Support

For support and questions:

- 📧 Email: support@example.com
- 🐛 Issues: [GitHub Issues](https://github.com/boni-fm/dotnet6-webapi-template/issues)
- 📖 Documentation: [Wiki](https://github.com/boni-fm/dotnet6-webapi-template/wiki)

## Acknowledgments

- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) by Robert C. Martin
- [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/) documentation
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) documentation

---

Made with ❤️ for the .NET community