#!/bin/bash

# PostgreSQL Modular Web API Development Setup Script
# This script sets up the development environment for the PostgreSQL modular web API

echo "🚀 Setting up PostgreSQL Modular Web API development environment..."

# Check if .NET 6 is installed
if ! command -v dotnet &> /dev/null; then
    echo "❌ .NET SDK is not installed. Please install .NET 6 SDK first."
    echo "Download from: https://dotnet.microsoft.com/download/dotnet/6.0"
    exit 1
fi

echo "✅ .NET SDK found: $(dotnet --version)"

# Check if Docker is installed
if ! command -v docker &> /dev/null; then
    echo "⚠️  Docker is not installed. PostgreSQL setup will be skipped."
    echo "Install Docker from: https://www.docker.com/"
    SKIP_DOCKER=true
else
    echo "✅ Docker found: $(docker --version)"
    SKIP_DOCKER=false
fi

# Restore NuGet packages
echo "📦 Restoring NuGet packages..."
dotnet restore
if [ $? -ne 0 ]; then
    echo "❌ Failed to restore NuGet packages"
    exit 1
fi

# Install EF Core tools globally
echo "🔧 Installing Entity Framework Core tools..."
dotnet tool install --global dotnet-ef --version 6.0.29 2>/dev/null || echo "EF Core tools already installed"

# Set up PostgreSQL with Docker (if Docker is available)
if [ "$SKIP_DOCKER" = false ]; then
    echo "🐘 Setting up PostgreSQL container..."
    
    # Stop existing container if running
    docker stop postgres-dev 2>/dev/null || true
    docker rm postgres-dev 2>/dev/null || true
    
    # Start PostgreSQL container
    docker run -d \
        --name postgres-dev \
        -e POSTGRES_PASSWORD=postgres \
        -e POSTGRES_DB=WebApiTemplateDb \
        -p 5432:5432 \
        postgres:14-alpine
    
    if [ $? -eq 0 ]; then
        echo "✅ PostgreSQL container started successfully"
        echo "   Connection: Host=localhost;Database=WebApiTemplateDb;Username=postgres;Password=postgres;Port=5432"
        
        # Wait for PostgreSQL to be ready
        echo "⏳ Waiting for PostgreSQL to be ready..."
        sleep 10
        
        # Create and apply migrations
        echo "🗃️  Creating database migrations..."
        cd src/WebApi.Services
        
        # Note: This might fail due to .NET 6 runtime requirement
        # Users should run this manually if it fails
        dotnet ef migrations add InitialCreate 2>/dev/null || {
            echo "⚠️  Could not create migrations automatically."
            echo "   Please run manually: cd src/WebApi.Services && dotnet ef migrations add InitialCreate"
        }
        
        dotnet ef database update 2>/dev/null || {
            echo "⚠️  Could not update database automatically."
            echo "   Please run manually: cd src/WebApi.Services && dotnet ef database update"
        }
        
        cd ../..
    else
        echo "❌ Failed to start PostgreSQL container"
    fi
fi

# Build the solution
echo "🔨 Building the solution..."
dotnet build src/WebApi.Host/WebApi.Host.csproj
if [ $? -ne 0 ]; then
    echo "❌ Build failed"
    exit 1
fi

echo ""
echo "🎉 Development environment setup complete!"
echo ""
echo "📋 Next steps:"
echo "   1. Start the API: cd src/WebApi.Host && dotnet run"
echo "   2. Open Swagger UI: http://localhost:5000/swagger"
echo "   3. Check health: http://localhost:5000/health"
echo ""
echo "🐘 PostgreSQL Info:"
if [ "$SKIP_DOCKER" = false ]; then
    echo "   Container: postgres-dev (running on port 5432)"
    echo "   Database: WebApiTemplateDb"
    echo "   Username: postgres"
    echo "   Password: postgres"
else
    echo "   Please install PostgreSQL manually or use Docker"
fi
echo ""
echo "🛠️  Development commands:"
echo "   dotnet run --project src/WebApi.Host              # Start the API"
echo "   dotnet test                                        # Run tests"
echo "   docker-compose up                                  # Start with Docker Compose"
echo "   cd src/WebApi.Services && dotnet ef migrations add <Name>  # Add migration"
echo ""
echo "Happy coding! 🚀"