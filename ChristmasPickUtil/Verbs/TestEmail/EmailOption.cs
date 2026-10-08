using CommandLine;

namespace ChristmasPickUtil.Verbs.TestEmail;

[Verb("TestEmail", HelpText = "Sends a test email to specified address")]
public class EmailOption
{
    [Option('a', "address", Required = true, HelpText = "The email address to send test email")]
    public string? Address { get; set; }
}
