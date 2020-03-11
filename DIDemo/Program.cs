using DIDemoLib;
using DIDemoLib.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sentry;
using Serilog;
using Serilog.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DIDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
                Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Sentry(o => {
                    o.InitializeSdk = true;
                    o.MaxQueueItems = 10;
                    o.MinimumBreadcrumbLevel = Serilog.Events.LogEventLevel.Debug;
                    o.Dsn = new Dsn("<sentryDSN>");
                 })
                .WriteTo.Console()
                .WriteTo.File("app.log", fileSizeLimitBytes: 20000000, rollOnFileSizeLimit : true, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddSingleton<ILoggerFactory>(sc =>
                {
                    var factory = new SerilogLoggerFactory(null, true);
                    return factory;
                })
                .AddLogging()
                .AddDIDemoLib()
                .BuildServiceProvider();
            
            var logger = serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();
            logger.LogDebug("Starting application");

            var client = serviceProvider.GetService<IApiClient>();
            client.OnException += OnException;
            client.InvokExceptionHandler();
            client.FooMethod();
            logger.LogDebug("The End");

            //Flush all the logs in the queue to Serilog
            Log.CloseAndFlush();

            //FLush All logs in the queue to sentry
            await SentrySdk.FlushAsync(TimeSpan.FromMinutes(1));
        }

        private static void OnException(object sender, ExceptionEventArgs e)
        {
            SentrySdk.CaptureException(e?.Exception);
        }
    }
}
