using ChristmasPickUtil.Verbs.Models;
using ChristmasPickUtil.Verbs.TypeConverters;
using CommandLine;
using System.ComponentModel;

namespace ChristmasPickUtil.Verbs.ChristmasPick
{
    [Verb("Pick", HelpText = "Generates Chirstmas exchange and verifies correctness.")]

    public class PickOptions
    {
        [Option('y', "year", Required = false, HelpText = "The year of christmas to publish, defaults to current year.")]
        public int? Year { get; set; } = null;

        [Option('t', "type", Required = false, Default = ListType.Adult, HelpText = "Only two possible values [adult,kid]")]
        [TypeConverter(typeof(ListTypeConverter))]
        public ListType Type { get; set; }

        [Option('r', "yearsback", Required = false, Default = 5, HelpText = "The number of years back a gift giver can give to the same person again.")]
        public int YearsBack { get; set; }
    }
}
