using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PharmacyField.Infrastructure.Data;
using System.Text;
using System;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pharmacy Field API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
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
            new string[] { }
        }
    });
});

// Database Context
// Read configured connection string or build from individual env vars (Railway)
var cfgConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
string connectionString;

// If configuration contains CI-style placeholders, build from env vars
bool hasPlaceholders = !string.IsNullOrEmpty(cfgConnectionString) && (cfgConnectionString.Contains("${{") || cfgConnectionString.Contains("${"));
if (!string.IsNullOrEmpty(cfgConnectionString) && !hasPlaceholders)
{
    connectionString = cfgConnectionString;
}
else
{
    var host = Environment.GetEnvironmentVariable("MYSQLHOST") ?? Environment.GetEnvironmentVariable("MySQL.MYSQLHOST") ?? builder.Configuration["MYSQLHOST"];
    var port = Environment.GetEnvironmentVariable("MYSQLPORT") ?? Environment.GetEnvironmentVariable("MySQL.MYSQLPORT") ?? builder.Configuration["MYSQLPORT"];
    var database = Environment.GetEnvironmentVariable("MYSQLDATABASE") ?? Environment.GetEnvironmentVariable("MySQL.MYSQLDATABASE") ?? builder.Configuration["MYSQLDATABASE"];
    var user = Environment.GetEnvironmentVariable("MYSQLUSER") ?? Environment.GetEnvironmentVariable("MySQL.MYSQLUSER") ?? builder.Configuration["MYSQLUSER"];
    var password = Environment.GetEnvironmentVariable("MYSQLPASSWORD") ?? Environment.GetEnvironmentVariable("MySQL.MYSQLPASSWORD") ?? builder.Configuration["MYSQLPASSWORD"];

    port = string.IsNullOrEmpty(port) ? "3306" : port;

    connectionString = $"Server={host};Port={port};Database={database};User={user};Password={password};SslMode=Required;";
}

// Use explicit server version to avoid AutoDetect opening a DB connection at startup
var serverVersion = new MySqlServerVersion(new Version(8, 0, 33));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, serverVersion));

// JWT Authentication
var key = Encoding.ASCII.GetBytes(builder.Configuration["JWT:Secret"] ?? throw new InvalidOperationException());
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });

// CORS - IMPORTANT FIX
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Auto-apply EF Core migrations and seed default admin user on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await DbInitializer.SeedAsync(db);
}

// Configure pipeline

// Enable Swagger in both Development and Production
app.UseSwagger();
app.UseSwaggerUI();

// Comment this temporarily for Railway
// app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

// Configure pipeline
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//// CORS must be between UseRouting and UseAuthorization
//app.UseCors("AllowAll");

//app.UseAuthentication();
//app.UseAuthorization();
//app.MapControllers();

//app.Run();