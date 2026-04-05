# Quick Start Guide - Finance Backend

## Prerequisites
- .NET 10 SDK [Download](https://dotnet.microsoft.com/download/dotnet/10.0)
- Git
- Visual Studio Code or Visual Studio 2024 (optional)

## Getting Started

### 1. Clone Repository
```bash
git clone https://github.com/YOUR_USERNAME/Finance-backend.git
cd Finance-backend
```

### 2. Restore Dependencies
```bash
dotnet restore
```

### 3. Build Project
```bash
dotnet build
```

### 4. Run Application
```bash
dotnet run --project FinanceBackend/FinanceBackend.csproj
```

The API will be available at:
- **HTTP**: http://localhost:5141
- **HTTPS**: https://localhost:7292
- **Swagger UI**: https://localhost:7292/swagger

### 5. Default Credentials for Testing
```
Admin User:
  Username: admin
  Password: admin123

Analyst User:
  Username: analyst
  Password: analyst123

Viewer User:
  Username: viewer
  Password: viewer123
```

---

## Testing with REST Client

### 1. Login and Get Token
```http
POST https://localhost:7292/api/auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "admin123"
}
```

Copy the returned JWT token.

### 2. Get Financial Records
```http
GET https://localhost:7292/api/financialrecords
Authorization: Bearer {your_jwt_token}
```

### 3. Create Financial Record
```http
POST https://localhost:7292/api/financialrecords
Authorization: Bearer {your_jwt_token}
Content-Type: application/json

{
  "description": "Grocery Shopping",
  "amount": 45.50,
  "category": "Food",
  "type": "Expense",
  "date": "2026-04-05"
}
```

### 4. Get Dashboard Summary
```http
GET https://localhost:7292/api/dashboard/summary
Authorization: Bearer {your_jwt_token}
```

---

## Using Swagger UI

1. Navigate to: https://localhost:7292/swagger
2. Click **"Authorize"** button
3. Paste your JWT token with format: `Bearer {token}`
4. Try out endpoints directly in the UI

---

## Running with Docker

### Build Image
```bash
docker build -t financebackend:latest .
```

### Run Container
```bash
docker run -d -p 5141:80 -p 7292:443 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  --name finance-api \
  financebackend:latest
```

### Using Docker Compose
```bash
docker-compose up -d
```

---

## Project Structure Explained

```
Finance-backend/
├── FinanceBackend/              # Main project
│   ├── Controllers/             # API endpoints
│   ├── Services/                # Business logic
│   ├── Domain/                  # Entity models
│   ├── Data/                    # Database context
│   ├── DTOs/                    # Data transfer objects
│   ├── Middleware/              # Custom middleware
│   ├── Program.cs               # Configuration & startup
│   └── appsettings.json         # Configuration
├── DEPLOYMENT_GUIDE.md          # Deployment instructions
├── Dockerfile                   # Docker configuration
├── docker-compose.yml           # Docker Compose setup
├── .github/workflows/           # CI/CD pipelines
└── .gitignore                   # Git ignore rules
```

---

## Key Features

✅ **JWT Authentication** - Secure token-based auth with role claims
✅ **Role-Based Access Control** - Admin, Analyst, Viewer roles
✅ **Financial Records Management** - Create, read, update, delete transactions
✅ **Dashboard Analytics** - Summary statistics and trends
✅ **Global Error Handling** - Centralized exception management
✅ **Swagger Documentation** - Interactive API docs with JWT support
✅ **SQLite Database** - Lightweight local storage with EF Core

---

## Common Commands

```bash
# Restore packages
dotnet restore

# Build project
dotnet build

# Run project in development
dotnet run

# Run tests
dotnet test

# Publish for deployment
dotnet publish -c Release -o ./publish

# Database migration (if using migrations)
dotnet ef database update

# Clean build
dotnet clean
```

---

## Environment Configuration

### Development (appsettings.Development.json)
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug"
    }
  },
  "Jwt": {
    "Key": "A_Very_Long_And_Secure_Secret_Key_For_Finance_Backend_Assignment_2026",
    "Issuer": "FinanceBackend",
    "Audience": "FinanceFrontend"
  }
}
```

### Production
- Change JWT Key to a strong, unique value
- Use Azure SQL Database instead of SQLite
- Enable HTTPS everywhere
- Set ASPNETCORE_ENVIRONMENT=Production

---

## Troubleshooting

### Port Already in Use
Edit `Properties/launchSettings.json` and change port numbers in `applicationUrl`.

### Database Not Found
The SQLite database is auto-created on first run. Check file permissions if issues persist.

### JWT Token Errors
Ensure your JWT secret key in appsettings.json is the same on both server and client.

### CORS Issues
Add CORS middleware in Program.cs for cross-origin requests.

---

## Next Steps

1. Read [DEPLOYMENT_GUIDE.md](DEPLOYMENT_GUIDE.md) for production deployment
2. Review API endpoints in Swagger UI
3. Test all endpoints with provided credentials
4. Customize business logic in Services folder
5. Push changes to GitHub and set up CI/CD

---

## Support Resources

- [.NET 10 Documentation](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-10)
- [Entity Framework Core Docs](https://learn.microsoft.com/en-us/ef/core/)
- [JWT.io - JWT Introduction](https://jwt.io/introduction)
- [ASP.NET Core Security](https://learn.microsoft.com/en-us/aspnet/core/security/)

---

**Happy Coding! 🚀**
