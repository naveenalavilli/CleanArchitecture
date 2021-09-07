using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using web;
using web.contracts;

namespace CleanArch
{
    public class Program
    {
        

        public static void Main(string[] args)
        {
        

            var path = Directory.GetCurrentDirectory();
            var environmentName = "Development";// Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            var configuration = new ConfigurationBuilder()
            .SetBasePath(path)           
            .AddJsonFile($"appsettings.{environmentName}.json", optional: false, reloadOnChange: true)
            .Build();

            Log.Logger = new LoggerConfiguration()
              .Enrich.FromLogContext()
              .MinimumLevel.Debug()
              .MinimumLevel.Information()
              .ReadFrom.Configuration(configuration)
              .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                CreateHostBuilder(args).UseSerilog().Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

      


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>().UseSerilog();
                });
    }
}
