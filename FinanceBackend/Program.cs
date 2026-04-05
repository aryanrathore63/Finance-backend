using System.Text;
using FinanceBackend.Data;
using FinanceBackend.Domain;
using FinanceBackend.Services;
using FinanceBackend.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add Scoped Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IFinanceService, FinanceService>();
builder.Services.AddScoped<IUserService, UserService>();

// Add DB Context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("AnalystPlus", policy => policy.RequireRole("Admin", "Analyst"));
    options.AddPolicy("ViewerPlus", policy => policy.RequireRole("Admin", "Analyst", "Viewer"));
});

// Swagger configuration with JWT support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Finance Backend API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    
    if (!db.Users.Any())
    {
        db.Users.AddRange(new List<User>
        {
            new User { Username = "admin", PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"), Role = UserRole.Admin },
            new User { Username = "analyst", PasswordHash = BCrypt.Net.BCrypt.HashPassword("analyst123"), Role = UserRole.Analyst },
            new User { Username = "viewer", PasswordHash = BCrypt.Net.BCrypt.HashPassword("viewer123"), Role = UserRole.Viewer }
        });
        db.SaveChanges();
    }

    if (!db.FinancialRecords.Any())
    {
        db.FinancialRecords.AddRange(new List<FinancialRecord>
        {
            new FinancialRecord { Amount = 5000, Type = TransactionType.Income, Category = "Salary", Date = DateTime.UtcNow.AddDays(-10), Notes = "Monthly Salary" },
            new FinancialRecord { Amount = 1200, Type = TransactionType.Expense, Category = "Rent", Date = DateTime.UtcNow.AddDays(-5), Notes = "Monthly Rent" },
            new FinancialRecord { Amount = 150, Type = TransactionType.Expense, Category = "Food", Date = DateTime.UtcNow.AddDays(-2), Notes = "Groceries" },
            new FinancialRecord { Amount = 200, Type = TransactionType.Income, Category = "Freelance", Date = DateTime.UtcNow.AddDays(-1), Notes = "Project Payment" }
        });
        db.SaveChanges();
    }
}

app.Run();
