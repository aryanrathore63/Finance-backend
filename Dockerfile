# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files
COPY ["FinanceBackend/FinanceBackend.csproj", "FinanceBackend/"]
RUN dotnet restore "FinanceBackend/FinanceBackend.csproj"

# Copy source code
COPY . .

# Build project
WORKDIR "/src/FinanceBackend"
RUN dotnet build "FinanceBackend.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "FinanceBackend.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=publish /app/publish .

# Expose ports
EXPOSE 80
EXPOSE 443

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD dotnet FinanceBackend.dll --health || exit 1

# Set environment
ENV ASPNETCORE_ENVIRONMENT=Production

# Run application
ENTRYPOINT ["dotnet", "FinanceBackend.dll"]
