using ChristmasPickUtil.Verbs.ChristmasPickPublisher;
using ChristmasPickUtil.Verbs.ResetChristmasPickPublisher;
using ChristmasPickUtil.Verbs.ViewChristmasPickTemplate;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace ChristmasPickUtil;

class Program
{
    static void Main(string[] args)
    {
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .AddUserSecrets<Program>()
            .Build();
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddConsole(options =>
                {
                    options.FormatterName = ConsoleFormatterNames.Simple;
                })
                .AddSimpleConsole(options =>
                {
                    options.IncludeScopes = true;
                    options.TimestampFormat = "yyyy-MM-ddTHH:mm:ss ";
                })
                .SetMinimumLevel(LogLevel.Information); // Set minimum level
        });

        // Create a logger from the factory
        //var logger = loggerFactory.CreateLogger<Program>();
        var exitCode = CommandLine.Parser.Default.ParseArguments<
           PublishOptions,
           ResetOptions,
           ViewTemplateOptions>(args)
           .MapResult(
               (PublishOptions options) => RunPublish(config, loggerFactory, options),
               (ResetOptions options) => RunReset(config, loggerFactory, options),
               (ViewTemplateOptions options) => RunViewTemplate(config, loggerFactory, options),
               errors => 1
           );

        Console.WriteLine($"Exiting with code: {exitCode}");
    }

    static int RunPublish(IConfiguration config, ILoggerFactory loggerFactory, PublishOptions options)
    {
        var logger = loggerFactory.CreateLogger<PublishOptions>();
        var verb = new Publish(config, logger);
        return verb.DoVerbAsync(options).GetAwaiter().GetResult();
    }

    static int RunReset(IConfiguration config, ILoggerFactory loggerFactory, ResetOptions options)
    {
        var logger = loggerFactory.CreateLogger<ResetOptions>();
        var verb = new Reset(config, logger);
        return verb.DoVerbAsync(options).GetAwaiter().GetResult();
    }

    static int RunViewTemplate(IConfiguration config, ILoggerFactory loggerFactory, ViewTemplateOptions options)
    {
        var logger = loggerFactory.CreateLogger<ViewTemplateOptions>();
        var verb = new ViewTemplate(config, logger);
        return verb.DoVerbAsync(options).GetAwaiter().GetResult();
    }

}
