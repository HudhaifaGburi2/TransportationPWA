using Microsoft.OpenApi.Models;
using Serilog;
using TransportationAttendance.API.Infrastructure;
using TransportationAttendance.API.Middleware;
using TransportationAttendance.API.Services;
using TransportationAttendance.Application;
using TransportationAttendance.Application.Interfaces;
using TransportationAttendance.Infrastructure;
using TransportationAttendance.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger configuration
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Transportation Attendance API",
        Version = "v1",
        Description = "API for Transportation Attendance Management System (TUMS)"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

// CORS configuration
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Add Application layer services
builder.Services.AddApplication();

// Add Infrastructure layer services (DbContexts, Repositories, JWT, etc.)
builder.Services.AddInfrastructure(builder.Configuration);

// Configure JWT Authentication (from guide)
builder.Services.ConfigureJwtAuthentication(builder.Configuration);

// Add Authorization with policies
builder.Services.AddAuthorization(AuthorizationPolicies.ConfigurePolicies);

// Register Current User service
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUser, CurrentUser>();

var app = builder.Build();

// Seed database (skip in Testing environment)
if (!app.Environment.IsEnvironment("Testing"))
{
    await DataSeeder.SeedAsync(app.Services);
}

// Configure the HTTP request pipeline
app.UseExceptionHandling();
app.UseRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Transportation Attendance API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseCors("DefaultPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Health check endpoint
app.MapGet("/health", () => Results.Ok(new { Status = "Healthy", Timestamp = DateTime.UtcNow }))
    .WithTags("Health");

try
{
    Log.Information("Starting Transportation Attendance API");
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
