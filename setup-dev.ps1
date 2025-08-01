# Development setup script for .NET 6 Web API Template

Write-Host "🚀 Setting up .NET 6 Web API Template development environment..." -ForegroundColor Green

# Check if .NET 6 is installed
try {
    $dotnetVersion = dotnet --version
    Write-Host "✅ .NET SDK version: $dotnetVersion" -ForegroundColor Green
    
    if (-not $dotnetVersion.StartsWith("6.")) {
        Write-Host "⚠️  Warning: .NET 6 SDK not found. Current version: $dotnetVersion" -ForegroundColor Yellow
        Write-Host "   This template requires .NET 6. You can install it from https://dotnet.microsoft.com/download/dotnet/6.0" -ForegroundColor Yellow
    }
}
catch {
    Write-Host "❌ .NET 6 SDK is not installed. Please install it from https://dotnet.microsoft.com/download/dotnet/6.0" -ForegroundColor Red
    exit 1
}

# Restore packages
Write-Host "📦 Restoring NuGet packages..." -ForegroundColor Blue
dotnet restore

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Failed to restore packages" -ForegroundColor Red
    exit 1
}

# Build solution
Write-Host "🔨 Building solution..." -ForegroundColor Blue
dotnet build --configuration Debug

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Build failed" -ForegroundColor Red
    exit 1
}

Write-Host "✅ Build successful" -ForegroundColor Green

# Check if SQL Server LocalDB is available
try {
    sqllocaldb info | Out-Null
    Write-Host "✅ SQL Server LocalDB detected" -ForegroundColor Green
}
catch {
    Write-Host "⚠️  SQL Server LocalDB not found. You may need to update the connection string in appsettings.json" -ForegroundColor Yellow
}

# Create logs directory
if (-not (Test-Path "logs")) {
    New-Item -ItemType Directory -Path "logs" | Out-Null
    Write-Host "✅ Created logs directory" -ForegroundColor Green
}

Write-Host ""
Write-Host "🎉 Setup complete! You can now:" -ForegroundColor Green
Write-Host "   1. Run the API: dotnet run --project src/DotNet6WebApiTemplate.Api" -ForegroundColor White
Write-Host "   2. Run tests: dotnet test" -ForegroundColor White
Write-Host "   3. View API docs: Navigate to https://localhost:5001/swagger after running the API" -ForegroundColor White
Write-Host ""
Write-Host "💡 Tip: Update the connection string in src/DotNet6WebApiTemplate.Api/appsettings.json" -ForegroundColor Cyan
Write-Host "         if you're not using SQL Server LocalDB" -ForegroundColor Cyan