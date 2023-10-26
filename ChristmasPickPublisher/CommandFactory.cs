using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChristmasPickPublisher.CommandLineOptions;
using ChristmasPickPublisher.Commands.EmailContacts;
using ChristmasPickPublisher.Commands.PublishChristmasPicks;
using CommandLine;
using CommandLine.Text;
using Microsoft.Extensions.Hosting;

namespace ChristmasPickPublisher
{
    public class CommandFactory : BackgroundService
    {
        private readonly IArgumentProvider _argumentProvider;
        private readonly IEmailContacts _emailContacts;
        private readonly IPublishChristmasPicks _christmasPublisher;
        
        public CommandFactory(
            IArgumentProvider argumentProvider,
            IEmailContacts emailContacts,
            IPublishChristmasPicks christmasPublisher)
        {
            _argumentProvider = argumentProvider;
            _emailContacts = emailContacts;
            _christmasPublisher = christmasPublisher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            CancellationTokenSource cancelTokenSrc = null;
            try
            {
                cancelTokenSrc = new CancellationTokenSource();
                await CreateCommand(cancelTokenSrc.Token);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally {
                cancelTokenSrc.Cancel();
                _argumentProvider.Cancel();
            }
            return;
        }

        private async Task<int> CreateCommand(CancellationToken token)
        {
            var verbs = new Type[] { typeof(ChristmasPickPublisherOptions), typeof(EmailContactsOptions)};
            var args = await _argumentProvider.GetArguments();
            
            var parser = new CommandLine.Parser(with => with.HelpWriter = null);
            var parserResult = parser.ParseArguments(args, verbs);

            // this is wierd with using async parsing commands...
            // need to learn a better way to do this.
            await (await parserResult
                .WithParsedAsync(RunAsync))
                .WithNotParsedAsync(errs => DisplayHelp(parserResult, errs));

            Console.WriteLine("When do you see this?");
            
            return 0;
        }

        private Task DisplayHelp<T>(ParserResult<T> result, IEnumerable<Error> errs)
        {
            HelpText helpText = null;
            if (errs.IsVersion())  //check if error is version request
                helpText = HelpText.AutoBuild(result);
            else
            {
                helpText = HelpText.AutoBuild(result, h =>
                {
                    return HelpText.DefaultParsingErrorsHandler(result, h);
                }, e => e, verbsIndex: true);
            }
            Console.WriteLine(helpText);
            return Task.CompletedTask;
        }

        private Task RunAsync(object options)
        {
            CancellationTokenSource source = new CancellationTokenSource();
            if (options.GetType() == typeof(ChristmasPickPublisherOptions))
            {
                return _christmasPublisher.Publish((ChristmasPickPublisherOptions)options, source.Token);
            }

            if (options.GetType() == typeof(EmailContactsOptions))
            {
                return _emailContacts.EmailAllContectsAsync((EmailContactsOptions)options, source.Token);
            }

            throw new NotSupportedException($"The {options.GetType()} is not supported.");
        }
    }
}
