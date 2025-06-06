using DotNetEnv;
using HRManagement.Application.Interfaces;
using HRManagement.Domain.Entities;
using HRManagement.Infrastructure.Context;
using HRManagement.Infrastructure.Repository;
using HRManagement.Infrastructure.Services;
using HRManagement.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Environment variables
    // CONNECTION_STRING, JWT_SECRET (with 512 bits)
    var environment = builder.Environment.EnvironmentName;
    var envPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "HRManagement.Api", ".env");
    Env.Load(envPath);

    // Serilog configs
    // For future: Upload logs into ElasticSearch
    builder.Host.UseSerilog((context, services, configuration) =>
    {
        configuration
            .ReadFrom.Configuration(builder.Configuration)
            .ReadFrom.Services(services);
    });

    // For future: Load appsettings based on the environment
    //   .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true) 
    builder.Configuration
           .SetBasePath(builder.Environment.ContentRootPath)
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .AddEnvironmentVariables();

    // JWT Auth Settings
    var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET")
        ?? throw new InvalidOperationException("JWT_SECRET not configured");

    builder.Configuration["JWT_SECRET"] = jwtSecret;
    var key = Encoding.ASCII.GetBytes(jwtSecret);

    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(opt =>
        {
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],                
                ClockSkew = TimeSpan.Zero
            };
        });

    // Configure DbContext
    var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        if (environment == "Testing")
            options.UseInMemoryDatabase("TestDb");
        else
            options.UseSqlServer(connectionString, sql => sql.EnableRetryOnFailure());
    });

    // Dependecy Injection
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    builder.Services.AddScoped<IEmployeeService, EmployeeService>();
    builder.Services.AddScoped<IDepartmentService, DepartmentService>();
    builder.Services.AddScoped<IAuthService, AuthService>();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new() { Title = "HRManagement API", Version = "v1" });

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Insira o token JWT no campo abaixo usando o esquema: Bearer {seu token}",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        // Swagger
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseSerilogRequestLogging();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.MapGet("/api/ping", () => Results.Text("ok"));

    // Ensure Seed for Testing environment
    if (environment == "Testing")
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        context.Database.EnsureCreated();
        
        if (!context.Departments.Any())
        {
            context.Departments.Add(new Department("Test", null));
            context.SaveChanges();
        }
        if (!context.Employees.Any(e => e.Email == "validemail@example.com"))
        {
            var departmentId = context.Departments.First().Id;
            context.Employees.Add(new Employee
            (
                "Test",
                "validemail@example.com",
                BCrypt.Net.BCrypt.HashPassword("ValidPass123"),
                "Test",
                DateTime.UtcNow,
                true,
                departmentId
            ));
            context.SaveChanges();
        }
    }

    Log.Information("Application started successfully and running in {Environment} environment.", app.Environment.EnvironmentName);

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program { }