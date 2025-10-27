using CommandLine;

namespace ChristmasPickUtil.Verbs.ViewChristmasPickTemplate
{
    [Verb("ViewTemplate", HelpText = "Creates the email contents only, does not publish.")]
    public class ViewTemplateOptions
    {
        [Option('o', "output", Required = false, HelpText = "The path where template files will get copied, defaults to folder of exe.")]
        public string? OutputFolder { get; set; }
    }
}
