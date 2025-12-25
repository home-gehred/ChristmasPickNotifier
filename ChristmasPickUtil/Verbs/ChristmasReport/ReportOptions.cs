using System.ComponentModel;
using ChristmasPickUtil.Verbs.Models;
using ChristmasPickUtil.Verbs.TypeConverters;
using CommandLine;

namespace ChristmasPickUtil.Verbs.ChristmasReport;

[Verb("Report", HelpText = "Generates Chirstmas pick report to bring to Christmas.")]

public class ReportOptions
{
        [Option('y', "year", Required = false, HelpText = "The year of Christmas to publish, defaults to current year.")]
        public int? Year { get; set; } = null;

        [Option('t', "type", Required = false, Default = ListType.Adult, HelpText = "Only two possible values [Adult,Kid]")]
        [TypeConverter(typeof(ListTypeConverter))]
        public ListType Type { get; set; }

}
