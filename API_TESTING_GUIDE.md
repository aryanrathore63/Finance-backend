# Finance Backend - Complete API Testing Guide

## 📋 Overview

This document covers ALL API endpoints with:
- Request/Response examples
- Required authentication
- Role-based access control
- Status codes
- Error responses

---

## 🔐 Authentication Flow

### **1. Login Endpoint**

**Endpoint:** `POST /api/auth/login`

**Request:**
```json
{
  "username": "admin",
  "password": "admin123"
}
```

**Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "admin",
  "role": "Admin"
}
```

**Response (401 Unauthorized):**
```json
{
  "message": "Invalid username or password"
}
```

**Test Command:**
```bash
curl -X POST http://localhost:5141/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin123"}'
```

---

### **2. Register Endpoint**

**Endpoint:** `POST /api/auth/register`

**Request:**
```json
{
  "username": "newuser",
  "password": "securepass123"
}
```

**Response (200 OK):**
```json
{
  "message": "User registered successfully"
}
```

**Response (400 Bad Request):**
```json
{
  "message": "Username already exists"
}
```

**Test Command:**
```bash
curl -X POST http://localhost:5141/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"username":"testuser","password":"pass123"}'
```

---

## 💰 Financial Records Endpoints

### **3. Get All Financial Records**

**Endpoint:** `GET /api/financialrecords`

**Required Role:** Analyst+ (Analyst, Admin)

**Query Parameters (Optional):**
- `category` - Filter by category
- `type` - Filter by type (Income/Expense)
- `startDate` - Filter by start date (yyyy-MM-dd)
- `endDate` - Filter by end date (yyyy-MM-dd)

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "amount": 2500.00,
    "type": "Income",
    "category": "Salary",
    "date": "2026-04-01T00:00:00",
    "notes": "Monthly salary",
    "createdAt": "2026-04-01T10:30:00"
  },
  {
    "id": 2,
    "amount": 45.50,
    "type": "Expense",
    "category": "Food",
    "date": "2026-04-02T00:00:00",
    "notes": "Grocery shopping",
    "createdAt": "2026-04-02T14:20:00"
  }
]
```

**Response (401 Unauthorized):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7235#section-3.1",
  "title": "Unauthorized",
  "status": 401
}
```

**Test Command:**
```bash
# Replace TOKEN with actual JWT token from login
curl -X GET "http://localhost:5141/api/financialrecords" \
  -H "Authorization: Bearer TOKEN"

# With filters
curl -X GET "http://localhost:5141/api/financialrecords?category=Food&type=Expense" \
  -H "Authorization: Bearer TOKEN"
```

---

### **4. Get Single Financial Record**

**Endpoint:** `GET /api/financialrecords/{id}`

**Required Role:** Analyst+ (Analyst, Admin)

**Response (200 OK):**
```json
{
  "id": 1,
  "amount": 2500.00,
  "type": "Income",
  "category": "Salary",
  "date": "2026-04-01T00:00:00",
  "notes": "Monthly salary",
  "createdAt": "2026-04-01T10:30:00"
}
```

**Response (404 Not Found):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not Found",
  "status": 404
}
```

**Test Command:**
```bash
curl -X GET "http://localhost:5141/api/financialrecords/1" \
  -H "Authorization: Bearer TOKEN"
```

---

### **5. Create Financial Record**

**Endpoint:** `POST /api/financialrecords`

**Required Role:** Admin

**Request:**
```json
{
  "amount": 150.00,
  "type": "Expense",
  "category": "Transportation",
  "date": "2026-04-05T00:00:00",
  "notes": "Taxi to airport"
}
```

**Response (201 Created):**
```json
{
  "id": 3,
  "amount": 150.00,
  "type": "Expense",
  "category": "Transportation",
  "date": "2026-04-05T00:00:00",
  "notes": "Taxi to airport",
  "createdAt": "2026-04-05T15:45:00"
}
```

**Response (403 Forbidden):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.3",
  "title": "Forbidden",
  "status": 403,
  "detail": "The user does not have [AdminOnly] permission to access this endpoint."
}
```

**Test Command:**
```bash
curl -X POST "http://localhost:5141/api/financialrecords" \
  -H "Authorization: Bearer ADMIN_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "amount": 150.00,
    "type": "Expense",
    "category": "Transportation",
    "date": "2026-04-05T00:00:00",
    "notes": "Taxi to airport"
  }'
```

---

### **6. Update Financial Record**

**Endpoint:** `PUT /api/financialrecords/{id}`

**Required Role:** Admin

**Request:**
```json
{
  "amount": 160.00,
  "type": "Expense",
  "category": "Transportation",
  "date": "2026-04-05T00:00:00",
  "notes": "Uber to airport (updated)"
}
```

**Response (204 No Content):**
```
(Empty body)
```

**Response (404 Not Found):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not Found",
  "status": 404
}
```

**Test Command:**
```bash
curl -X PUT "http://localhost:5141/api/financialrecords/3" \
  -H "Authorization: Bearer ADMIN_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "amount": 160.00,
    "type": "Expense",
    "category": "Transportation",
    "date": "2026-04-05T00:00:00",
    "notes": "Uber to airport (updated)"
  }'
```

---

### **7. Delete Financial Record**

**Endpoint:** `DELETE /api/financialrecords/{id}`

**Required Role:** Admin

**Response (204 No Content):**
```
(Empty body)
```

**Response (404 Not Found):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not Found",
  "status": 404
}
```

**Test Command:**
```bash
curl -X DELETE "http://localhost:5141/api/financialrecords/3" \
  -H "Authorization: Bearer ADMIN_TOKEN"
```

---

## 📊 Dashboard Endpoints

### **8. Get Dashboard Summary**

**Endpoint:** `GET /api/dashboard/summary`

**Required Role:** Viewer+ (Everyone)

**Response (200 OK):**
```json
{
  "totalIncome": 2500.00,
  "totalExpense": 45.50,
  "netBalance": 2454.50,
  "categoryBreakdown": {
    "Salary": 2500.00,
    "Food": -45.50
  },
  "transactionCount": 2
}
```

**Response (401 Unauthorized):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7235#section-3.1",
  "title": "Unauthorized",
  "status": 401
}
```

**Test Command:**
```bash
curl -X GET "http://localhost:5141/api/dashboard/summary" \
  -H "Authorization: Bearer TOKEN"
```

---

### **9. Get Dashboard Insights**

**Endpoint:** `GET /api/dashboard/insights`

**Required Role:** Analyst+ (Analyst, Admin)

**Response (200 OK):**
```json
{
  "monthlyTrends": [
    {
      "month": "April 2026",
      "income": 2500.00,
      "expense": 45.50,
      "netChange": 2454.50
    }
  ],
  "recentTransactions": [
    {
      "id": 2,
      "amount": 45.50,
      "type": "Expense",
      "category": "Food",
      "date": "2026-04-02T00:00:00",
      "notes": "Grocery shopping",
      "createdAt": "2026-04-02T14:20:00"
    },
    {
      "id": 1,
      "amount": 2500.00,
      "type": "Income",
      "category": "Salary",
      "date": "2026-04-01T00:00:00",
      "notes": "Monthly salary",
      "createdAt": "2026-04-01T10:30:00"
    }
  ],
  "highestExpenseCategory": "Food",
  "averageExpensePerTransaction": 45.50
}
```

**Response (403 Forbidden):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.3",
  "title": "Forbidden",
  "status": 403,
  "detail": "The user does not have [AnalystPlus] permission to access this endpoint."
}
```

**Test Command:**
```bash
curl -X GET "http://localhost:5141/api/dashboard/insights" \
  -H "Authorization: Bearer ANALYST_TOKEN"
```

---

## 👥 User Management Endpoints

### **10. Get All Users**

**Endpoint:** `GET /api/users`

**Required Role:** Admin

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "username": "admin",
    "role": "Admin",
    "isActive": true,
    "createdAt": "2026-04-01T08:00:00"
  },
  {
    "id": 2,
    "username": "analyst",
    "role": "Analyst",
    "isActive": true,
    "createdAt": "2026-04-01T08:00:00"
  },
  {
    "id": 3,
    "username": "viewer",
    "role": "Viewer",
    "isActive": true,
    "createdAt": "2026-04-01T08:00:00"
  }
]
```

**Response (403 Forbidden):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.3",
  "title": "Forbidden",
  "status": 403
}
```

**Test Command:**
```bash
curl -X GET "http://localhost:5141/api/users" \
  -H "Authorization: Bearer ADMIN_TOKEN"
```

---

### **11. Update User Role**

**Endpoint:** `PATCH /api/users/{id}/role`

**Required Role:** Admin

**Request:**
```json
"Analyst"
```

**Response (204 No Content):**
```
(Empty body)
```

**Response (404 Not Found):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not Found",
  "status": 404
}
```

**Test Command:**
```bash
curl -X PATCH "http://localhost:5141/api/users/3/role" \
  -H "Authorization: Bearer ADMIN_TOKEN" \
  -H "Content-Type: application/json" \
  -d '"Admin"'
```

---

### **12. Update User Status**

**Endpoint:** `PATCH /api/users/{id}/status`

**Required Role:** Admin

**Request:**
```json
false
```

**Response (204 No Content):**
```
(Empty body)
```

**Test Command:**
```bash
curl -X PATCH "http://localhost:5141/api/users/3/status" \
  -H "Authorization: Bearer ADMIN_TOKEN" \
  -H "Content-Type: application/json" \
  -d 'false'
```

---

### **13. Delete User**

**Endpoint:** `DELETE /api/users/{id}`

**Required Role:** Admin

**Response (204 No Content):**
```
(Empty body)
```

**Response (404 Not Found):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not Found",
  "status": 404
}
```

**Test Command:**
```bash
curl -X DELETE "http://localhost:5141/api/users/3" \
  -H "Authorization: Bearer ADMIN_TOKEN"
```

---

## 🔑 Role-Based Access Control Matrix

| Endpoint | Viewer | Analyst | Admin |
|----------|--------|---------|-------|
| **POST /auth/login** | ✅ | ✅ | ✅ |
| **POST /auth/register** | ✅ | ✅ | ✅ |
| **GET /financialrecords** | ❌ | ✅ | ✅ |
| **GET /financialrecords/{id}** | ❌ | ✅ | ✅ |
| **POST /financialrecords** | ❌ | ❌ | ✅ |
| **PUT /financialrecords/{id}** | ❌ | ❌ | ✅ |
| **DELETE /financialrecords/{id}** | ❌ | ❌ | ✅ |
| **GET /dashboard/summary** | ✅ | ✅ | ✅ |
| **GET /dashboard/insights** | ❌ | ✅ | ✅ |
| **GET /users** | ❌ | ❌ | ✅ |
| **PATCH /users/{id}/role** | ❌ | ❌ | ✅ |
| **PATCH /users/{id}/status** | ❌ | ❌ | ✅ |
| **DELETE /users/{id}** | ❌ | ❌ | ✅ |

---

## 📱 HTTP Status Codes Reference

| Code | Meaning | When Used |
|------|---------|-----------|
| **200** | OK | Request successful (GET, POST with response) |
| **201** | Created | Resource created (POST) |
| **204** | No Content | Request successful (PUT, DELETE, PATCH) |
| **400** | Bad Request | Invalid input (e.g., username exists) |
| **401** | Unauthorized | Missing or invalid token |
| **403** | Forbidden | Insufficient permissions |
| **404** | Not Found | Resource doesn't exist |
| **500** | Internal Server Error | Server error |

---

## 🧪 Complete Test Sequence

### **Step 1: Start Application**
```bash
dotnet run --project FinanceBackend/FinanceBackend.csproj
```

### **Step 2: Login as Admin**
```bash
ADMIN_TOKEN=$(curl -X POST http://localhost:5141/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin123"}' | jq -r '.token')

echo "Admin Token: $ADMIN_TOKEN"
```

### **Step 3: Create Financial Record**
```bash
curl -X POST "http://localhost:5141/api/financialrecords" \
  -H "Authorization: Bearer $ADMIN_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "amount": 5000,
    "type": "Income",
    "category": "Bonus",
    "date": "2026-04-05",
    "notes": "Performance bonus"
  }'
```

### **Step 4: View All Records**
```bash
curl -X GET "http://localhost:5141/api/financialrecords" \
  -H "Authorization: Bearer $ADMIN_TOKEN"
```

### **Step 5: Get Dashboard Summary**
```bash
curl -X GET "http://localhost:5141/api/dashboard/summary" \
  -H "Authorization: Bearer $ADMIN_TOKEN"
```

### **Step 6: List All Users**
```bash
curl -X GET "http://localhost:5141/api/users" \
  -H "Authorization: Bearer $ADMIN_TOKEN"
```

---

## ⚠️ Common Response Errors

### **401 - No Token**
```json
{
  "type": "https://tools.ietf.org/html/rfc7235#section-3.1",
  "title": "Unauthorized",
  "status": 401
}
```
**Fix:** Add `Authorization: Bearer TOKEN` header

### **403 - Insufficient Permissions**
```json
{
  "title": "Forbidden",
  "detail": "The user does not have [AdminOnly] permission to access this endpoint.",
  "status": 403
}
```
**Fix:** Use correct role (Admin for this endpoint)

### **404 - Resource Not Found**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not Found",
  "status": 404
}
```
**Fix:** Check if resource ID exists

---

## 🎯 Testing with Swagger UI

1. Navigate to: `http://localhost:5141/swagger`
2. Click **"Authorize"** button (top right)
3. Paste token with format: `Bearer {your_token}`
4. Try out endpoints directly in the UI

---

