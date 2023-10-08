using System;
using CommandLine;

namespace ChristmasPickPublisher.CommandLineOptions
{
    public class ChristmasPickPublisherOptions
    {
        [Option('y', "year", Required = true, HelpText = "The year to generate christmas picks for")]
        public int Year { get; set; }
        [Option('l', "list", Required = true, HelpText = "Only two possible values [adult,kid]")]
        public string ListType { get; set; }
        [Option('m', "max", Required = true, HelpText = "Max number of emails to send")]
        public int MaxEmail { get; set; }
    }
}
