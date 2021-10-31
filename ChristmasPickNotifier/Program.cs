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

Sadly I lost alot of the instructions for setting up VS Code, but here is one article that walked
me through the basics.
https://docs.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide#start-up-and-configuration
https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs-code?tabs=csharp

func start –-dotnet-isolated-debug --verbose
*/

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ChristmasPickNotifier.Notifier;
using ChristmasPickNotifier.Notifier.ConfigurationProviders;

namespace ChristmasPickNotifier3
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureAppConfiguration(builder => {
                    builder.AddUserSecrets<Program>();
                })
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(services => {
                    services.AddScoped<IProvideConfiguration, DefaultConfigurationProvider>();
                    services.AddScoped<INotifierFactory, NotifierFactory>();
                })
                .Build();

            host.Run();
        }
    }
}