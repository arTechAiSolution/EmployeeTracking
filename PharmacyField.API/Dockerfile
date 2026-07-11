# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copy solution and project files
COPY PharmacyField.sln ./
COPY PharmacyField.API/PharmacyField.API.csproj PharmacyField.API/
COPY PharmacyField.Core/PharmacyField.Core.csproj PharmacyField.Core/
COPY PharmacyField.Infrastructure/PharmacyField.Infrastructure.csproj PharmacyField.Infrastructure/

# Restore packages
RUN dotnet restore PharmacyField.sln

# Copy the remaining source code
COPY . .

# Publish the API
RUN dotnet publish PharmacyField.API/PharmacyField.API.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "PharmacyField.API.dll"]