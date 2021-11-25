using System;
using CommandLine;

namespace ChristmasPickPublisher.CommandLineOptions
{
    public class ChristmasPickPublisherOptions
    {
        [Option('y', "year", Required = true, HelpText = "The year to generate christmas picks for")]
        public int Year { get; set; }
        [Option('l', "type", Required = true, HelpText = "Only two possible values [adult,kid]")]
        public string ListType { get; set; }
    }
}
