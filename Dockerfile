# Build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copy solution and project files
COPY *.sln ./
COPY src/WebApi.Host/*.csproj src/WebApi.Host/
COPY src/WebApi.Base/*.csproj src/WebApi.Base/
COPY src/WebApi.Services/*.csproj src/WebApi.Services/

# Restore dependencies
RUN dotnet restore

# Copy all source code
COPY src/ src/

# Build the application
RUN dotnet build src/WebApi.Host/WebApi.Host.csproj -c Release --no-restore

# Publish the application
RUN dotnet publish src/WebApi.Host/WebApi.Host.csproj -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app

# Install PostgreSQL client tools (optional, for database access)
USER root
RUN apt-get update && apt-get install -y \
    curl \
    postgresql-client \
    && rm -rf /var/lib/apt/lists/*

# Create app user
RUN addgroup --system --gid 1001 dotnetapp
RUN adduser --system --uid 1001 --gid 1001 dotnetapp
USER dotnetapp

# Copy published files
COPY --from=build --chown=dotnetapp:dotnetapp /app/publish .

# Create logs directory
RUN mkdir -p logs

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80

# Expose port
EXPOSE 80

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
    CMD curl -f http://localhost/health || exit 1

# Set entry point
ENTRYPOINT ["dotnet", "WebApi.Host.dll"]