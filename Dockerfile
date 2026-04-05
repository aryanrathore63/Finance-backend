FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build-env
WORKDIR /app

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore FinanceBackend/FinanceBackend.csproj

# Build and publish a release
RUN dotnet publish FinanceBackend/FinanceBackend.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build-env /app/out .

# Create directory for SQLite database
RUN mkdir -p /app/data
ENV ConnectionStrings__DefaultConnection="Data Source=/app/data/finance.db"

EXPOSE 8080
ENTRYPOINT ["dotnet", "FinanceBackend.dll"]
