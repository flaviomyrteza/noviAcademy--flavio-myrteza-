using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace NoviCode.Persistence.Context;

// Lets `dotnet ef migrations add` / `database update` build the context at design time
// without running the application. Reads the API's appsettings.json — per
// MigrationCommands.txt, NoviCode.Infrastructure is its own startup project for migrations,
// so the working directory is Infrastructure's, not Api's, and the path has to reach across.
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
	public AppDbContext CreateDbContext(string[] args)
	{
		var apiProjectPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "NoviCode.Api");
		var basePath = Directory.Exists(apiProjectPath) ? apiProjectPath : Directory.GetCurrentDirectory();

		var configuration = new ConfigurationBuilder()
			.SetBasePath(basePath)
			.AddJsonFile("appsettings.json", optional: false)
			.AddJsonFile("appsettings.Development.json", optional: true)
			.Build();

		var connectionString = configuration.GetConnectionString("DefaultConnection")
			?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

		var options = new DbContextOptionsBuilder<AppDbContext>()
			.UseSqlServer(connectionString)
			.Options;

		return new AppDbContext(options);
	}
}
