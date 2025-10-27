using ChristmasPickUtil.Verbs.Models;
using ChristmasPickUtil.Verbs.TypeConverters;
using CommandLine;
using System.ComponentModel;

namespace ChristmasPickUtil.Verbs.ChristmasPickPublisher
{

    [Verb("Publish", HelpText = "Sends email to contact list participants")]
    public class PublishOptions
    {
        [Option('y', "year", Required = true, HelpText = "The year of christmas to publish")]
        public int Year { get; set; }

        [Option('t', "type", Required = false, Default = ListType.Adult, HelpText = "Only two possible values [adult,kid]")]
        [TypeConverter(typeof(ListTypeConverter))]
        public ListType Type { get; set; }

        [Option('m', "maxEmailsToSend", Required = false, Default = 500, HelpText = "Max allowed emails to run in a session.")]
        public int MaxEmailsToSend { get; set; }
    }
}
