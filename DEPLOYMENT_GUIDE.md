# Finance Backend - Complete Guide

## 📋 Project Overview

This is a **.NET 10 REST API** for managing financial records with Role-Based Access Control (RBAC).

### Architecture Stack
- **Framework**: .NET 10 Web API
- **Database**: SQLite (Entity Framework Core)
- **Authentication**: JWT Bearer Tokens
- **Hashing**: BCrypt.Net-Next
- **API Documentation**: Swagger/OpenAPI

---

## 🏗️ How This Project Works

### **1. Architecture Layers**

#### Controllers (`Controllers/` folder)
- `AuthController.cs` - Handles user registration & login
- `FinancialRecordsController.cs` - CRUD operations on financial records
- `DashboardController.cs` - Analytics & reporting endpoints
- `UsersController.cs` - User management (Admin only)

#### Services (`Services/` folder)
- `AuthService.cs` - JWT token generation, user authentication
- `FinanceService.cs` - Business logic for financial records
- `UserService.cs` - User management operations

#### Data Layer (`Data/` & `Domain/` folders)
- `AppDbContext.cs` - Entity Framework Core database context
- `User.cs` - User entity with roles
- `FinancialRecord.cs` - Financial transaction entity

#### DTOs (`DTOs/` folder)
- Data Transfer Objects for API request/response payloads

#### Middleware (`Middleware/` folder)
- `ErrorHandlingMiddleware.cs` - Global exception handling

### **2. Authentication Flow**

```
User Login (username/password)
        ↓
AuthService validates credentials (BCrypt)
        ↓
JWT Token generated (claims include user role)
        ↓
Token sent to client
        ↓
Client includes token in Authorization header: "Bearer {token}"
        ↓
Server validates JWT signature & expiration
        ↓
Request proceeds with user context
```

### **3. Role-Based Access Control (RBAC)**

Three roles with increasing permissions:

| Role | Permissions |
|------|------------|
| **Viewer** | View financial records |
| **Analyst** | View + Create records + Access dashboard analytics |
| **Admin** | Full access (Create, Read, Update, Delete, User Management) |

### **4. Default Test Users (Auto-seeded)**
- Admin: `admin` / `admin123`
- Analyst: `analyst` / `analyst123`
- Viewer: `viewer` / `viewer123`

### **5. Key Technologies Used**

- **JWT Authentication**: Secure token-based authentication
- **Entity Framework Core**: ORM for database operations
- **SQLite**: Lightweight local database
- **Swagger**: Interactive API documentation
- **Global Error Handling**: Centralized exception management

---

## 📤 How to Push to GitHub

### **Step 1: Initialize Git Repository (if not already done)**

```bash
cd c:\Users\ar226\Downloads\Finance-backend
git init
```

### **Step 2: Create .gitignore File**

Create a `.gitignore` file in the root to exclude unnecessary files:

```bash
# Create .gitignore
echo bin/ > .gitignore
echo obj/ >> .gitignore
echo .vs/ >> .gitignore
echo .vscode/ >> .gitignore
echo *.db >> .gitignore
echo appsettings.Development.json >> .gitignore
echo /*.db-shm >> .gitignore
echo /*.db-wal >> .gitignore
```

### **Step 3: Add All Files to Git**

```bash
git add .
```

### **Step 4: Create Initial Commit**

```bash
git commit -m "Initial commit: Finance Backend .NET 10 API with RBAC"
```

### **Step 5: Create GitHub Repository**

1. Go to [GitHub.com](https://github.com)
2. Click **"New Repository"**
3. Name it: `Finance-backend` (or your choice)
4. **DO NOT initialize with README** (you already have one)
5. Click **"Create repository"**

### **Step 6: Add Remote and Push**

```bash
# Add the remote repository
git remote add origin https://github.com/YOUR_USERNAME/Finance-backend.git

# Rename branch to main
git branch -M main

# Push to GitHub
git push -u origin main
```

### **Step 7: Verify on GitHub**

Visit `https://github.com/YOUR_USERNAME/Finance-backend` to confirm all files are uploaded.

---

## 🚀 How to Deploy This Project

### **Option 1: Azure App Service (Recommended for Production)**

#### Prerequisites:
- Azure account with active subscription
- Azure CLI installed on your machine

#### Deployment Steps:

```bash
# Step 1: Login to Azure
az login

# Step 2: Create a resource group
az group create --name FinanceBackendRG --location eastus

# Step 3: Create App Service Plan
az appservice plan create --name FinanceBackendPlan --resource-group FinanceBackendRG --sku F1 --is-linux

# Step 4: Create Web App
az webapp create --resource-group FinanceBackendRG --plan FinanceBackendPlan --name financebackend-app --runtime "DOTNETCORE|10.0"

# Step 5: Configure database connection (important!)
# Before deployment, update your appsettings.json to use SQL Database or connection string from Azure

# Step 6: Deploy from local Git
cd Finance-backend
git remote add azure https://your-username@financebackend-app.scm.azurewebsites.net:443/financebackend-app.git
git push azure main

# OR use ZIP deployment
dotnet publish -c Release -o ./publish
cd publish
Compress-Archive -Path * -DestinationPath ../app.zip
az webapp deployment source config-zip --resource-group FinanceBackendRG --name financebackend-app --src app.zip
```

### **Option 2: Docker Containerization (For Any Cloud)**

#### Step 1: Create Dockerfile

```dockerfile
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["FinanceBackend/FinanceBackend.csproj", "FinanceBackend/"]
RUN dotnet restore "FinanceBackend/FinanceBackend.csproj"
COPY . .
WORKDIR "/src/FinanceBackend"
RUN dotnet build "FinanceBackend.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "FinanceBackend.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "FinanceBackend.dll"]
```

#### Step 2: Create Docker Compose File (optional)

```yaml
version: '3.8'
services:
  finance-api:
    build: .
    ports:
      - "5141:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__DefaultConnection=data.db
    volumes:
      - ./data:/app/data
```

#### Step 3: Deploy to Docker Hub / Azure Container Registry

```bash
# Build Docker image
docker build -t financebackend:latest .

# Tag for Docker Hub
docker tag financebackend:latest YOUR_DOCKERHUB_USERNAME/financebackend:latest

# Login and push
docker login
docker push YOUR_DOCKERHUB_USERNAME/financebackend:latest

# OR push to Azure Container Registry
az acr login --name mycontainerregistry
docker tag financebackend:latest mycontainerregistry.azurecr.io/financebackend:latest
docker push mycontainerregistry.azurecr.io/financebackend:latest
```

### **Option 3: IIS on Windows Server**

#### Prerequisites:
- Windows Server with IIS installed
- .NET 10 Hosting Bundle installed on server

#### Steps:

```bash
# Step 1: Publish the application as self-contained
dotnet publish -c Release -r win-x64 --self-contained -o ./publish

# Step 2: Copy publish folder to server (e.g., C:\inetpub\wwwroot\financebackend)
# Can use FTP, RDP, or file share

# Step 3: Create IIS Application
# - Open IIS Manager
# - Create new Application Pool (.NET 10.0)
# - Create new Website pointing to publish folder
# - Set application pool to created pool
# - Configure SSL certificate (HTTPS)

# Step 4: Configure Security
# - Set file permissions appropriately
# - Configure firewall rules
```

### **Option 4: Heroku (Simple, Free Tier Available)**

```bash
# Step 1: Create account at heroku.com and install Heroku CLI

# Step 2: Create Procfile in root directory
echo "web: dotnet FinanceBackend.dll" > Procfile

# Step 3: Login to Heroku
heroku login

# Step 4: Create app
heroku create financebackend-app

# Step 5: Add buildpack
heroku buildpacks:add https://github.com/jincod/dotnetcore-buildpack.git

# Step 6: Deploy
git push heroku main
```

---

## 🔧 Production Checklist

Before deploying to production:

- [ ] **Change JWT Secret Key** in `appsettings.json` to a strong, unique value
- [ ] **Use managed database** (SQL Server/PostgreSQL) instead of SQLite
- [ ] **Enable HTTPS** everywhere (SSL certificate)
- [ ] **Set up proper logging** and monitoring
- [ ] **Remove default credentials** or change them immediately
- [ ] **Enable CORS** if frontend is on different domain
- [ ] **Implement rate limiting** to prevent abuse
- [ ] **Add input validation** and sanitization
- [ ] **Set up CI/CD pipeline** (GitHub Actions, Azure DevOps, etc.)
- [ ] **Configure backups** for database
- [ ] **Use environment variables** for sensitive data (never hardcode)
- [ ] **Set ASPNETCORE_ENVIRONMENT** to `Production`

---

## 📝 Useful Commands

### **Local Development**

```bash
# Restore NuGet packages
dotnet restore

# Build project
dotnet build

# Run project
dotnet run --project FinanceBackend/FinanceBackend.csproj

# Run tests
dotnet test

# Publish for deployment
dotnet publish -c Release -o ./publish
```

### **Git Operations**

```bash
# Check status
git status

# View commits
git log --oneline

# Create new branch
git checkout -b feature/new-feature

# Push branch
git push origin feature/new-feature

# Create Pull Request on GitHub
# (via web interface)
```

---

## 🆘 Troubleshooting

### **Issue: Database migration errors**
```bash
# Update database schema
dotnet ef database update
```

### **Issue: Port already in use**
Edit `launchSettings.json` and change port numbers

### **Issue: JWT token expired/invalid**
- Verify JWT secret key matches on server
- Check token expiration time in `AuthService.cs`
- Ensure system clocks are synchronized

### **Issue: CORS errors**
Add CORS configuration in `Program.cs`:
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
app.UseCors("AllowAll");
```

---

## 📊 API Testing

Use the included `FinanceBackend.http` file (REST Client in VS Code) or:

### **Get JWT Token**
```
POST https://localhost:7292/api/auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "admin123"
}
```

### **Use Token in Requests**
```
GET https://localhost:7292/api/financialrecords
Authorization: Bearer {token_from_login}
```

### **Access Swagger UI**
```
https://localhost:7292/swagger
```

---

## 📚 Additional Resources

- [.NET 10 Documentation](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-10)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [JWT Best Practices](https://tools.ietf.org/html/rfc7519)
- [Azure App Service Documentation](https://docs.microsoft.com/en-us/azure/app-service/)
- [Docker Documentation](https://docs.docker.com/)

---

**Last Updated**: April 2026
**Project**: Finance Backend .NET 10 API
**Version**: 1.0.0
