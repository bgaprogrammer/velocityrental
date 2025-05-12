# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

# Copy solution and project files
COPY CarRentalApi.sln .
COPY CarRentalApi.Api/*.csproj ./CarRentalApi.Api/
COPY CarRentalApi.Core/*.csproj ./CarRentalApi.Core/
COPY CarRentalApi.Infrastructure/*.csproj ./CarRentalApi.Infrastructure/

# Restore dependencies
RUN dotnet restore

# Copy source code
COPY CarRentalApi.Api/. ./CarRentalApi.Api/
COPY CarRentalApi.Core/. ./CarRentalApi.Core/
COPY CarRentalApi.Infrastructure/. ./CarRentalApi.Infrastructure/

# Build the application
RUN dotnet publish ./CarRentalApi.Api/CarRentalApi.Api.csproj -c Release --no-restore -o /app

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app ./

# Env configuration
ENV ASPNETCORE_URLS=http://+:$PORT
ENV ASPNETCORE_ENVIRONMENT=Production

# Health check configuration for Render (optional)
HEALTHCHECK CMD curl --fail http://localhost:$PORT/healthz || exit 1

ENTRYPOINT ["dotnet", "CarRentalApi.Api.dll"]
