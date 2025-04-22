using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Constants = CatherineDesktopApp.Shared.Constants;

namespace CatherineDesktopApp.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("ConfigurationKeyNotFound");

            // Replace the placeholder with the actual AppData path
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var dataDirectory = Path.Combine(appDataPath, Constants.AppDataSubFolderName);
            if (!Directory.Exists(dataDirectory))
            {
                Directory.CreateDirectory(dataDirectory);
            }

            connectionString = connectionString.Replace("|DataDirectory|", $"{dataDirectory}/");

            var connection = new Microsoft.Data.Sqlite.SqliteConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = $@"PRAGMA key = 'qzcZK4jZU^y36D6G0ku#b&38kp';";
            command.ExecuteNonQuery();

            optionsBuilder.UseSqlite(connection);

            return new AppDbContext(optionsBuilder.Options, configuration);
        }
    }
}
