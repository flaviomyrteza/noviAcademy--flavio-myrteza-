using System.Text.Json.Serialization;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using NLog.Extensions.Logging;
using NoviCode;
using NoviCode.Persistence.Context;

var builder = WebApplication.CreateBuilder(args);

// Day 7: replace the built-in DI container with Autofac.
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

// Logging via NLog (same nlog.config layout as the Console app).
builder.Logging.ClearProviders();
builder.Logging.AddNLog("nlog.config");

// One AppDbContext per request (scoped) — the EF Core repositories depend on it.
// Connection string lives in appsettings.json (ConnectionStrings:DefaultConnection), not in code.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
	?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(connectionString));

builder.Services.AddScoped<IPlayerRepository, EfPlayerRepository>();
builder.Services.AddScoped<IWalletRepository, EfWalletRepository>();

// Single-instance in-memory cache, used by the caching decorators (Day 7).
// The decorators talk only to the ICache port (from Day 6); the IMemoryCache-backed
// implementation (MemoryCacheStore) lives in Infrastructure, so caching stays a swappable detail.
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ICache, MemoryCacheStore>();

// Accept/emit enums (e.g. Currency) as their string names, not numbers.
builder.Services.AddControllers()
	.AddJsonOptions(options =>
		options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Swagger / OpenAPI — interactive API docs at /swagger.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Autofac: MediatR handlers + behaviour, keyed strategies, factory, services + caching decorators.
builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{
	container.RegisterModule(new ApplicationModule());
	container.RegisterModule(new InfrastructureModule());
});

var app = builder.Build();

// Serve the Swagger JSON and UI in Development.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	app.MapGet("/", () => Results.Redirect("/swagger")); // root → Swagger UI
}

app.MapControllers();

app.Run();
