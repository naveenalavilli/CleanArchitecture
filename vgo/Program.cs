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

namespace vgo
{
    public class Program
    {
        //private readonly IRideService _rideService;

        //public Program(IRideService rideService)
        //{
        //    _rideService = rideService;
        //}

        public static void Main(string[] args)
        {
            ////            Log.Logger = new LoggerConfiguration()
            ////           .MinimumLevel.Debug()
            //////           .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            ////           .Enrich.FromLogContext()
            ////          // .WriteTo.File("logs/log-.txt",LogEventLevel.Debug, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}", rollingInterval: RollingInterval.Day)
            ////           .CreateLogger();


            var path = Directory.GetCurrentDirectory();
            var environmentName = "Development";// Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            var configuration = new ConfigurationBuilder()
            .SetBasePath(path)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
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

        //public static void DoWork()
        //{
        //    _rideService.ExpireOldRides();
        //    Console.WriteLine("Working thread...");
        //    Thread.Sleep(2 * 60 * 1000);
        //    DoWork();
        //}


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>().UseSerilog();
                });
    }
}
