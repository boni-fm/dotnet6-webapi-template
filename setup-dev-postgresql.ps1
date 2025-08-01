# PostgreSQL Modular Web API Development Setup Script
# This script sets up the development environment for the PostgreSQL modular web API

Write-Host "🚀 Setting up PostgreSQL Modular Web API development environment..." -ForegroundColor Green

# Check if .NET 6 is installed
$dotnetVersion = $null
try {
    $dotnetVersion = dotnet --version
    Write-Host "✅ .NET SDK found: $dotnetVersion" -ForegroundColor Green
} catch {
    Write-Host "❌ .NET SDK is not installed. Please install .NET 6 SDK first." -ForegroundColor Red
    Write-Host "Download from: https://dotnet.microsoft.com/download/dotnet/6.0" -ForegroundColor Yellow
    exit 1
}

# Check if Docker is installed
$dockerInstalled = $false
try {
    $dockerVersion = docker --version
    Write-Host "✅ Docker found: $dockerVersion" -ForegroundColor Green
    $dockerInstalled = $true
} catch {
    Write-Host "⚠️  Docker is not installed. PostgreSQL setup will be skipped." -ForegroundColor Yellow
    Write-Host "Install Docker from: https://www.docker.com/" -ForegroundColor Yellow
    $dockerInstalled = $false
}

# Restore NuGet packages
Write-Host "📦 Restoring NuGet packages..." -ForegroundColor Cyan
dotnet restore
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Failed to restore NuGet packages" -ForegroundColor Red
    exit 1
}

# Install EF Core tools globally
Write-Host "🔧 Installing Entity Framework Core tools..." -ForegroundColor Cyan
try {
    dotnet tool install --global dotnet-ef --version 6.0.29 2>$null
} catch {
    Write-Host "EF Core tools already installed" -ForegroundColor Yellow
}

# Set up PostgreSQL with Docker (if Docker is available)
if ($dockerInstalled) {
    Write-Host "🐘 Setting up PostgreSQL container..." -ForegroundColor Cyan
    
    # Stop existing container if running
    try {
        docker stop postgres-dev 2>$null
        docker rm postgres-dev 2>$null
    } catch {
        # Ignore errors if container doesn't exist
    }
    
    # Start PostgreSQL container
    docker run -d `
        --name postgres-dev `
        -e POSTGRES_PASSWORD=postgres `
        -e POSTGRES_DB=WebApiTemplateDb `
        -p 5432:5432 `
        postgres:14-alpine
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✅ PostgreSQL container started successfully" -ForegroundColor Green
        Write-Host "   Connection: Host=localhost;Database=WebApiTemplateDb;Username=postgres;Password=postgres;Port=5432" -ForegroundColor Gray
        
        # Wait for PostgreSQL to be ready
        Write-Host "⏳ Waiting for PostgreSQL to be ready..." -ForegroundColor Yellow
        Start-Sleep -Seconds 10
        
        # Create and apply migrations
        Write-Host "🗃️  Creating database migrations..." -ForegroundColor Cyan
        Push-Location "src/WebApi.Services"
        
        try {
            dotnet ef migrations add InitialCreate 2>$null
        } catch {
            Write-Host "⚠️  Could not create migrations automatically." -ForegroundColor Yellow
            Write-Host "   Please run manually: cd src/WebApi.Services && dotnet ef migrations add InitialCreate" -ForegroundColor Gray
        }
        
        try {
            dotnet ef database update 2>$null
        } catch {
            Write-Host "⚠️  Could not update database automatically." -ForegroundColor Yellow
            Write-Host "   Please run manually: cd src/WebApi.Services && dotnet ef database update" -ForegroundColor Gray
        }
        
        Pop-Location
    } else {
        Write-Host "❌ Failed to start PostgreSQL container" -ForegroundColor Red
    }
}

# Build the solution
Write-Host "🔨 Building the solution..." -ForegroundColor Cyan
dotnet build src/WebApi.Host/WebApi.Host.csproj
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Build failed" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "🎉 Development environment setup complete!" -ForegroundColor Green
Write-Host ""
Write-Host "📋 Next steps:" -ForegroundColor Cyan
Write-Host "   1. Start the API: cd src/WebApi.Host && dotnet run" -ForegroundColor Gray
Write-Host "   2. Open Swagger UI: http://localhost:5000/swagger" -ForegroundColor Gray
Write-Host "   3. Check health: http://localhost:5000/health" -ForegroundColor Gray
Write-Host ""
Write-Host "🐘 PostgreSQL Info:" -ForegroundColor Cyan
if ($dockerInstalled) {
    Write-Host "   Container: postgres-dev (running on port 5432)" -ForegroundColor Gray
    Write-Host "   Database: WebApiTemplateDb" -ForegroundColor Gray
    Write-Host "   Username: postgres" -ForegroundColor Gray
    Write-Host "   Password: postgres" -ForegroundColor Gray
} else {
    Write-Host "   Please install PostgreSQL manually or use Docker" -ForegroundColor Gray
}
Write-Host ""
Write-Host "🛠️  Development commands:" -ForegroundColor Cyan
Write-Host "   dotnet run --project src/WebApi.Host              # Start the API" -ForegroundColor Gray
Write-Host "   dotnet test                                        # Run tests" -ForegroundColor Gray
Write-Host "   docker-compose up                                  # Start with Docker Compose" -ForegroundColor Gray
Write-Host "   cd src/WebApi.Services && dotnet ef migrations add <Name>  # Add migration" -ForegroundColor Gray
Write-Host ""
Write-Host "Happy coding! 🚀" -ForegroundColor Green