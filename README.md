# Finance Data Processing and Access Control Backend

This is a .NET 10 REST API for managing financial records with strict Role-Based Access Control (RBAC) and dashboard analytics.

## Features
- **JWT Authentication**: Secure login and registration.
- **Role-Based Access Control**:
  - **Viewer**: Can view records.
  - **Analyst**: Can view and create records, and access dashboard analytics.
  - **Admin**: Full access, including updating/deleting records and user management.
- **Financial Records Management**: CRUD operations for income/expense transactions.
- **Dashboard Analytics**: Summary of totals, category breakdowns, and monthly trends.
- **Global Error Handling**: Centralized exception management.
- **Swagger Documentation**: Interactive API documentation with JWT support.

## Tech Stack
- .NET 10 Web API
- Entity Framework Core (SQLite)
- JWT Bearer Authentication
- BCrypt.Net-Next (Password hashing)
- Swashbuckle (Swagger)

## Getting Started

### Prerequisites
- .NET 10 SDK

### Running the Application
1. Navigate to the project directory:
   ```bash
   cd FinanceBackend
   ```
2. Run the application:
   ```bash
   dotnet run
   ```
3. Open the Swagger UI in your browser:
   `https://localhost:7193/swagger` (Port may vary, check console output).

### Default Users (Seeded)
The application automatically seeds initial users for testing:
- **Admin**: `admin` / `admin123`
- **Analyst**: `analyst` / `analyst123`
- **Viewer**: `viewer` / `viewer123`

## API Endpoints

### Auth
- `POST /api/auth/register`: Register a new user.
- `POST /api/auth/login`: Login and receive a JWT token.

### Financial Records
- `GET /api/financialrecords`: Get filtered records (Viewer+).
- `POST /api/financialrecords`: Create a record (Analyst+).
- `PUT /api/financialrecords/{id}`: Update a record (Admin only).
- `DELETE /api/financialrecords/{id}`: Delete a record (Admin only).

### Dashboard
- `GET /api/dashboard/summary`: Get totals and category breakdown (Analyst+).
- `GET /api/dashboard/insights`: Get trends and recent activity (Analyst+).

### Users (Admin Only)
- `GET /api/users`: List all users.
- `PUT /api/users/{id}/role`: Update a user's role.
