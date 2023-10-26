using CommandLine;

namespace ChristmasPickPublisher.CommandLineOptions
{
    [Verb("emailcontacts", HelpText = "Email all contacts an email template")]
    public class EmailContactsOptions
    {
        [Option('r', "retry", Default = 3, HelpText = "Number of times to try before considering a failure")]
        public int MaxRetry { get; set; }

        [Option('w', "waitInSeconds", Default = 1, HelpText = "The number of seconds to wait between email attempts")]
        public int WaitTimeInSeconds { get; set;}

        [Option('t', "template", Required = true, HelpText = "The path to the email template")]
        public string TemplatePath { get; set; }
    }
}
