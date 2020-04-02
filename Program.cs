using System.Diagnostics;
using System.IO;
using Domain.Persistence.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace Supermarket.API
{
    public class Program
    {
        public static void Main(string[] args)
        {

            Log.Logger = new LoggerConfiguration()
             .MinimumLevel.Information()
             .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
             .Enrich.FromLogContext()
             .WriteTo.MSSqlServer(connectionString: "Data Source=ERIC-PC;Initial Catalog=Supermarket;Integrated Security=SSPI;trusted_connection=true",
                    tableName: "Log")
             .CreateLogger();

            //var configuration = new ConfigurationBuilder()
            //                .AddJsonFile("appsettings.json")
            //                .Build();

            //Log.Logger = new LoggerConfiguration()
            //    .ReadFrom.Configuration(configuration)
            //    .CreateLogger();


            var host = CreateHostBuilder(args).Build();
            
            using (var scope = host.Services.CreateScope())
            using(var context = scope.ServiceProvider.GetService<AppDbContext>())
            {
                context.Database.EnsureCreated();
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((context, logging) =>
                {
                    logging.ClearProviders();
                    // logging.AddConsole(options => options.IncludeScopes = true);
                    logging.AddDebug();
                    logging.AddConsole();
                })
                //.ConfigureAppConfiguration((builderContext, config) =>
                //{
                //    config.SetBasePath(builderContext.HostingEnvironment.ContentRootPath)
                //        .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: false)
                //        .AddEnvironmentVariables();
                //})
                .UseSerilog()   
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
