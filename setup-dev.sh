#!/bin/bash

# Development setup script for .NET 6 Web API Template

echo "🚀 Setting up .NET 6 Web API Template development environment..."

# Check if .NET 6 is installed
if ! command -v dotnet &> /dev/null; then
    echo "❌ .NET 6 SDK is not installed. Please install it from https://dotnet.microsoft.com/download/dotnet/6.0"
    exit 1
fi

# Check .NET version
DOTNET_VERSION=$(dotnet --version)
if [[ ! $DOTNET_VERSION == 6.* ]]; then
    echo "⚠️  Warning: .NET 6 SDK not found. Current version: $DOTNET_VERSION"
    echo "   This template requires .NET 6. You can install it from https://dotnet.microsoft.com/download/dotnet/6.0"
fi

echo "✅ .NET SDK version: $DOTNET_VERSION"

# Restore packages
echo "📦 Restoring NuGet packages..."
dotnet restore

if [ $? -ne 0 ]; then
    echo "❌ Failed to restore packages"
    exit 1
fi

# Build solution
echo "🔨 Building solution..."
dotnet build --configuration Debug

if [ $? -ne 0 ]; then
    echo "❌ Build failed"
    exit 1
fi

echo "✅ Build successful"

# Check if SQL Server LocalDB is available (Windows only)
if [[ "$OSTYPE" == "msys" || "$OSTYPE" == "win32" ]]; then
    if command -v sqllocaldb &> /dev/null; then
        echo "✅ SQL Server LocalDB detected"
    else
        echo "⚠️  SQL Server LocalDB not found. You may need to update the connection string in appsettings.json"
    fi
fi

# Create logs directory
mkdir -p logs
echo "✅ Created logs directory"

echo ""
echo "🎉 Setup complete! You can now:"
echo "   1. Run the API: dotnet run --project src/DotNet6WebApiTemplate.Api"
echo "   2. Run tests: dotnet test"
echo "   3. View API docs: Navigate to https://localhost:5001/swagger after running the API"
echo ""
echo "💡 Tip: Update the connection string in src/DotNet6WebApiTemplate.Api/appsettings.json"
echo "         if you're not using SQL Server LocalDB"