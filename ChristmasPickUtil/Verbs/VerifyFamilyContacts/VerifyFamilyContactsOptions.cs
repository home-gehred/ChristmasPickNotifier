using ChristmasPickUtil.Verbs.Models;
using ChristmasPickUtil.Verbs.TypeConverters;
using CommandLine;
using System.ComponentModel;

namespace ChristmasPickUtil.Verbs.VerifyFamilyContacts
{
    [Verb("VerifyFamilyContact", HelpText = "Uses the archive and confirms each person has an entry in the pick list")]
    public class VerifyFamilyContactsOptions
    {
        [Option('y', "year", Required = false, HelpText = "The year of christmas to verify, defaults to current year.")]
        public int? Year { get; set; } = null;

        [Option('t', "type", Required = false, Default = ListType.Adult, HelpText = "Only two possible values [adult,kid]")]
        [TypeConverter(typeof(ListTypeConverter))]
        public ListType Type { get; set; }
    }
}
