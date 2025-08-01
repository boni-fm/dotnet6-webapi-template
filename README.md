# PostgreSQL Modular Web API Template

A .NET 6 Web API template with PostgreSQL backend following a modular architecture pattern.

![.NET 6](https://img.shields.io/badge/.NET-6.0-blue)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-14+-blue)
![License](https://img.shields.io/badge/license-MIT-green)
![Build Status](https://github.com/boni-fm/dotnet6-webapi-template/workflows/CI%2FCD%20Pipeline/badge.svg)

## Architecture Overview

This template implements a clean, modular architecture with three main projects:

### 1. WebApi.Host (Minimal Host Project)
- **Purpose**: Acts as the main entry point and startup configuration
- **Contents**: Only `Program.cs` and configuration files
- **Dependencies**: References WebApi.Base and WebApi.Services
- **Characteristics**: Minimal, clean startup with dependency injection

### 2. WebApi.Base (Shared Foundation)
- **Purpose**: Provides common functionality and base classes
- **Contents**:
  - `BaseApiController` with standardized response patterns
  - Common interfaces (`IRepository`, `IUnitOfWork`, etc.)
  - Standard API response models
  - JWT authentication setup
  - CORS configuration
- **Benefits**: Reusable across multiple service modules

### 3. WebApi.Services (Consolidated Business Logic)
- **Purpose**: Contains all business logic, data access, and endpoints
- **Contents**:
  - Entity Framework DbContext with PostgreSQL
  - Repository pattern implementation
  - Business services with AutoMapper
  - Controllers inheriting from BaseApiController
  - Entity models and configurations
  - DTOs and mapping profiles

## Key Features

✅ **PostgreSQL Integration** with Npgsql Entity Framework provider  
✅ **Repository Pattern** with Unit of Work implementation  
✅ **AutoMapper** for object-to-object mapping  
✅ **Base Controller** with standardized API responses  
✅ **JWT Authentication** setup and configuration  
✅ **Health Checks** for PostgreSQL database  
✅ **Docker Support** with PostgreSQL container  
✅ **Modular Design** for easy extension  
✅ **CORS Configuration** for cross-origin requests  
✅ **Structured Logging** with Serilog  

## Getting Started

### Prerequisites
- .NET 6 SDK
- PostgreSQL 14+ (or Docker)
- (Optional) pgAdmin or similar PostgreSQL client

### Quick Start with Docker

1. **Clone and navigate to the project**:
   ```bash
   git clone <repository-url>
   cd dotnet6-webapi-template
   ```

2. **Start with Docker Compose**:
   ```bash
   docker-compose up --build
   ```
   
   This will:
   - Build the API container
   - Start PostgreSQL container
   - Apply database migrations (if configured)
   - Start the API on http://localhost:5000

3. **Access the API**:
   - Swagger UI: http://localhost:5000/swagger
   - Health Check: http://localhost:5000/health
   - Sample API: http://localhost:5000/api/products

### Local Development Setup

1. **Install PostgreSQL** or use Docker:
   ```bash
   docker run --name postgres-dev -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=WebApiTemplateDb -p 5432:5432 -d postgres:14-alpine
   ```

2. **Update connection string** in `src/WebApi.Host/appsettings.Development.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Database=WebApiTemplateDb_Dev;Username=postgres;Password=postgres;Port=5432"
     }
   }
   ```

3. **Install EF Core tools** (if not already installed):
   ```bash
   dotnet tool install --global dotnet-ef
   ```

4. **Create and apply migrations**:
   ```bash
   cd src/WebApi.Services
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

5. **Run the application**:
   ```bash
   cd src/WebApi.Host
   dotnet run
   ```

## Project Structure

```
src/
├── WebApi.Host/                    # 🎯 Main entry point (minimal)
│   ├── Program.cs                  # Startup configuration
│   ├── appsettings.json           # Production settings
│   ├── appsettings.Development.json # Development settings
│   └── WebApi.Host.csproj         # Project file
├── WebApi.Base/                    # 🛠 Shared foundation
│   ├── Controllers/
│   │   └── BaseApiController.cs   # Base controller with common functionality
│   ├── Models/
│   │   └── ApiResponse.cs         # Standardized API response models
│   ├── Interfaces/
│   │   └── IRepository.cs         # Common interfaces
│   ├── Extensions/
│   │   └── ServiceCollectionExtensions.cs # Base service registration
│   └── WebApi.Base.csproj         # Project file
└── WebApi.Services/                # 🏗 Business logic & data access
    ├── Controllers/
    │   └── ProductsController.cs   # Sample CRUD controller
    ├── Data/
    │   ├── ApplicationDbContext.cs # EF Core DbContext
    │   ├── Entities/              # Domain entities
    │   └── Configurations/        # EF Core configurations
    ├── Services/                  # Business logic services
    ├── Repositories/              # Data access layer
    ├── DTOs/                      # Data transfer objects
    ├── Profiles/                  # AutoMapper profiles
    ├── Extensions/
    │   └── ServiceCollectionExtensions.cs # Service registration
    └── WebApi.Services.csproj     # Project file
```

## Sample API Endpoints

The template includes a complete Product CRUD API as an example:

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/products` | Get all products |
| GET | `/api/products/{id}` | Get product by ID |
| POST | `/api/products` | Create new product |
| PUT | `/api/products/{id}` | Update existing product |
| DELETE | `/api/products/{id}` | Delete product |
| GET | `/api/products/category/{category}` | Get products by category |
| GET | `/api/products/active` | Get active products only |

### Example Response Format

All API responses follow a standardized format:

```json
{
  "success": true,
  "message": "Products retrieved successfully",
  "data": [
    {
      "id": 1,
      "name": "Sample Product 1",
      "description": "This is a sample product for demonstration",
      "price": 29.99,
      "quantity": 100,
      "category": "Electronics",
      "isActive": true,
      "createdAt": "2024-01-01T00:00:00Z",
      "updatedAt": "2024-01-01T00:00:00Z"
    }
  ]
}
```

## Adding New Modules

The modular design makes it easy to add new features:

### 1. Add New Entity and Repository
```csharp
// Add to WebApi.Services/Data/Entities/
public class Customer : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

// Add to WebApi.Services/Repositories/
public interface ICustomerRepository : IRepository<Customer> { }
public class CustomerRepository : Repository<Customer>, ICustomerRepository { }
```

### 2. Add Service and DTOs
```csharp
// Add DTOs in WebApi.Services/DTOs/
public class CustomerDto { /* properties */ }

// Add service in WebApi.Services/Services/
public interface ICustomerService { /* methods */ }
public class CustomerService : ICustomerService { /* implementation */ }
```

### 3. Add Controller
```csharp
// Add to WebApi.Services/Controllers/
public class CustomersController : BaseApiController
{
    // Inherits standardized response methods
    // Implement CRUD operations
}
```

### 4. Register Services
```csharp
// Update WebApi.Services/Extensions/ServiceCollectionExtensions.cs
services.AddScoped<ICustomerRepository, CustomerRepository>();
services.AddScoped<ICustomerService, CustomerService>();
```

## Configuration

### Environment Variables

| Variable | Description | Default |
|----------|-------------|---------|
| `ConnectionStrings__DefaultConnection` | PostgreSQL connection string | See appsettings.json |
| `JWT__Key` | JWT signing key | (Change in production!) |
| `JWT__Issuer` | JWT issuer | WebApiTemplate |
| `JWT__Audience` | JWT audience | WebApiTemplate |

### Health Checks

Health checks are available at `/health` and include:
- Database connectivity check
- Application status

## Development Guidelines

### Database Migrations
```bash
# Add migration
dotnet ef migrations add <MigrationName> --project src/WebApi.Services

# Update database
dotnet ef database update --project src/WebApi.Services

# Remove last migration
dotnet ef migrations remove --project src/WebApi.Services
```

### Testing
```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test tests/WebApiTemplate.Tests.Unit
```

### Code Style
- Follow standard C# conventions
- Use async/await for all async operations
- Implement proper error handling
- Add XML documentation for public APIs

## Production Deployment

### Docker Production Build
```dockerfile
# Build optimized image
docker build -t webapi-template:latest .

# Run with production settings
docker run -d \
  --name webapi-prod \
  -p 80:80 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e ConnectionStrings__DefaultConnection="Your-Prod-Connection-String" \
  webapi-template:latest
```

### Security Considerations
- 🔐 Change JWT key in production
- 🔐 Use secure connection strings
- 🔐 Enable HTTPS
- 🔐 Configure proper CORS policies
- 🔐 Set up authentication/authorization as needed

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Support

For support and questions:

- 📧 Email: support@example.com
- 🐛 Issues: [GitHub Issues](https://github.com/boni-fm/dotnet6-webapi-template/issues)
- 📖 Documentation: [Wiki](https://github.com/boni-fm/dotnet6-webapi-template/wiki)

---

Made with ❤️ for the .NET community