/*
To run and debug this project...
Start by using Visual studio Code. (Visual Studio communit edition will not work)

In a terminal window start up 'azurite' this will start up an emulator
To see what is happening with the emulater I install the Azure Storage explorer in download folder

Don't forget that this app uses Azure key vault, but to run locally you can use
dotnet user-secrets list 
which will display the secrets that have been set.

To get more help you can use
dotnet user-secrets --help
*/


using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ChristmasPickCommon.Configuration;
using ChristmasPickPublisher.Configuration;

namespace ChristmasPickPublisher
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
                    logging.AddConsole();
                })
            .ConfigureAppConfiguration((hostingContext, configuration) =>
                {
                    configuration.Sources.Clear();

                    IHostEnvironment env = hostingContext.HostingEnvironment;
                    configuration
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                        .AddUserSecrets<Program>()
                        .Build();
                })
                .ConfigureServices((_, services) => {
                    services.AddTransient<IProvideConfiguration, ProvideMicrosoftConfiguration>();
                    services.AddHostedService<PublishChristmasPicks>();
                });
                
        }
        public static async Task<int> PublishChristmasPicks(IServiceProvider services)
        {
            using var serviceScope = services.CreateScope();
            var provider = serviceScope.ServiceProvider;

            var christmasPick = provider.GetRequiredService<IPublishChristmsPicks>();

            await christmasPick.PublishChristmasPicksAsync();

            return 0;
        }

    }
}
