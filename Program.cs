using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RaceDay.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// ---- Database (EF Core, Code-First, SQL Server) ----
builder.Services.AddDbContext<RaceDayDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ---- Server-side session (stores UserId and Role after login) ----
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".RaceDay.Session";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.IdleTimeout = TimeSpan.FromMinutes(60);
});

builder.Services.AddControllers();

// ---- Swagger ----
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "RaceDay API",
        Version = "v1",
        Description =
            "Event management API for South African road running, walking and cycling events. " +
            "Authentication is session based: call POST /api/auth/login first, after which the " +
            "browser sends the session cookie automatically, so every protected endpoint can be " +
            "tested directly from this page. Roles: Organiser and Participant."
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "RaceDay API v1");
    options.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseSession();      // must come before MapControllers so filters can read the session
app.MapControllers();

app.Run();

// Exposed so the unit/integration test project (point 5) can use WebApplicationFactory<Program>
public partial class Program { }
