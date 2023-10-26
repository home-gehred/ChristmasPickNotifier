using System;
using System.Collections.Generic;
using System.Linq;
using ChristmasPickCommon;
using CommandLine;

namespace ChristmasPickPublisher.CommandLineOptions
{
    [Verb("publish", HelpText = "Publish all christmas picks")]
    public class ChristmasPickPublisherOptions
    {
        [Option('y', "year", Required = true, HelpText = "The year to generate christmas picks for")]
        public int Year { get; set; }

        [Option('l', "list", Required = true, HelpText = "Only two possible values [adult,kid]")]
        public string ListType { get; set; }
        
        [Option('m', "max", Required = true, HelpText = "Max number of emails to send")]
        public int MaxEmail { get; set; }

        // Look into how to do this using CommandLine options (it may already work)
        public (bool, string[]) IsValid()
        {
            var validationMessages = new List<string>();
            if ((Year < 2000) && (Year > 2100))
            {
                validationMessages.Add($"The christmas year of {Year} is not not between 2000 - 2100.");
            }

            var listTypeValid = XMasPickListType.TryParse(ListType, out XMasPickListType listType);
            if (!listTypeValid)
            {
                validationMessages.Add($"Unable to convert {ListType} to XMasPickListType. Valid values are [{string.Join(',', XMasPickListType.ValidPickListTypes())}].");
            }

            return (validationMessages.Count == 0, validationMessages.ToArray());
        }
    }
}
