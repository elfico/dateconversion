using DateTimeConversion.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;

namespace DateTimeConversion
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();
            string environment = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT")!;

            try
            {
                string appSetting = string.IsNullOrEmpty(environment) ? $"appsettings.json" : $"appsettings.{environment}.json";

                IConfiguration configuration = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile(appSetting, true, true)
                  .Build();

                var servicesProvider = BuildDI(configuration);
                using (servicesProvider as IDisposable)
                {
                    var runner = servicesProvider.GetRequiredService<Core>();

                    await runner.RunTasksAsync();

                    Console.WriteLine("Press ANY key to exit");
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                // NLog: catch any exception and log it.
                logger.Error(ex, "Stopped program because of exception");
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                LogManager.Shutdown();
            }
        }

        private static IServiceProvider BuildDI(IConfiguration configuration)
        {
            //create a serviceProvider
            IServiceCollection services = new ServiceCollection();

            //add services and resolve types

            services.AddSingleton(configuration);

            string connectionString = configuration.GetConnectionString("AccountDb");

            services.AddDbContext<AccountDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            //add the main program to run
            services.AddSingleton<Core>();

            //add logging
            services.AddLogging(loggingBuilder =>
            {
                // configure Logging with NLog
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                loggingBuilder.AddNLog(configuration);
            });
            //generate a provider
            return services.BuildServiceProvider();
        }
    }
}