using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CommandLine;
using ChristmasPickCommon;
using Common.ChristmasPickList;
using ChristmasPickNotifier.Notifier;
using ChristmasPickTrialHarness.CommandLineOptions;
using ChristmasPickTrialHarness.Configuration;
using ChristmasPickCommon.Configuration;
using ChristmasPickCommon.Factories;

namespace ChristmasPickTrialHarness
{
    public interface ICreateChristmsPick
    {
        Task<int> CreateChristmasPicksAsync();
    }

    public class CreateChristmasPicks : BackgroundService, ICreateChristmsPick
    {
        private readonly ILogger logger;
        //private readonly IHostApplicationLifetime _appLifetime;

        private readonly IProvideConfiguration cfgProvider;
        private readonly IPickListServiceFactory pickListServiceFactory;
        public CreateChristmasPicks(
            IProvideConfiguration cfgProvider,
            IPickListServiceFactory pickListServiceFactory,
            ILogger<CreateChristmasPicks> logger
        )
        {
            this.cfgProvider = cfgProvider ?? throw new ArgumentNullException(nameof(cfgProvider));
            this.pickListServiceFactory = pickListServiceFactory ?? throw new ArgumentNullException(nameof(pickListServiceFactory));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
           return CreateChristmasPicksAsync();
        }

         public Task<int> CreateChristmasPicksAsync()
        {
            IPickListService pickListService = null;
            
            logger.LogDebug(string.Join(" ", Environment.GetCommandLineArgs()));
            Parser.Default.ParseArguments<ChristmasPickOptions>(Environment.GetCommandLineArgs())
                .WithParsed<ChristmasPickOptions>(o =>
                {
                    if (XMasPickListType.TryParse(o.ListType, out XMasPickListType listType))
                    {
                        if (XMasDay.TryParse(o.Year, out XMasDay xmasDay))
                        {
                            logger.LogInformation($"Generating Christmas picks for {o.Year}");
                            pickListService =  pickListServiceFactory.CreateService(xmasDay, listType);                             
                            var pickList = pickListService.CreateChristmasPick(xmasDay);
                        }
                        else
                        {
                            logger.LogInformation($"The program cannot generate Christmas picks for {o.Year}, Please check command line arguments.");
                        }
                    }
                    else
                    {
                        logger.LogInformation($"The listType parameter of {o.ListType} is not valid it must be kid or adult. Check command line parameters.");
                    }
                });

            return Task.FromResult(0);
        }
    }
}
