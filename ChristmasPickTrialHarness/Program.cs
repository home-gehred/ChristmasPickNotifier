using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ChristmasPickNotifier.Notifier;
using ChristmasPickTrialHarness.Configuration;
using ChristmasPickCommon.Configuration;
using ChristmasPickCommon.Factories;
using ChristmasPickTrialHarness.Factories;

namespace ChristmasPickTrialHarness
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            using var host = CreateHostBuilder(args).Build();
            CancellationTokenSource source = new CancellationTokenSource();
            await host.StartAsync(source.Token);
            //await host.RunAsync();
            Console.WriteLine("You got here!");
            await host.StopAsync(source.Token);
            return 0;        
        }
        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
            .ConfigureLogging(logging =>
                {
                    // Add any 3rd party loggers like NLog or Serilog
                    //builder.Logging.ClearProviders();
                    //builder.Logging.AddConsole();
                    //logging.ClearProviders();
                    logging.AddConsole();
                })
            .ConfigureAppConfiguration((hostingContext, configuration) =>
                {
                    configuration.Sources.Clear();

                    IHostEnvironment env = hostingContext.HostingEnvironment;

                    configuration
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                        .Build();

                    //IConfigurationRoot configurationRoot = configuration.Build();

                    /*TransientFaultHandlingOptions options = new();
                    configurationRoot.GetSection(nameof(TransientFaultHandlingOptions))
                                     .Bind(options);

                    Console.WriteLine($"TransientFaultHandlingOptions.Enabled={options.Enabled}");
                    Console.WriteLine($"TransientFaultHandlingOptions.AutoRetryDelay={options.AutoRetryDelay}");*/
                })
                .ConfigureServices((_, services) => {
                    services.AddTransient<IProvideConfiguration, ProvideMicrosoftConfiguration>();
                    services.AddTransient<IPickListServiceFactory, PickListServiceFactory>();
                    services.AddHostedService<CreateChristmasPicks>();
                });
                
        }

        public static async Task<int> CreateChristmasPicks(IServiceProvider services)
        {
            using var serviceScope = services.CreateScope();
            var provider = serviceScope.ServiceProvider;

            var christmasPick = provider.GetRequiredService<ICreateChristmsPick>();

            await christmasPick.CreateChristmasPicksAsync();

            return 0;
        }
    }
}
