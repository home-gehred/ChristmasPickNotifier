using ChristmasPickUtil.Verbs.Models;
using ChristmasPickUtil.Verbs.TypeConverters;
using CommandLine;
using System.ComponentModel;

namespace ChristmasPickUtil.Verbs.ResetChristmasPickPublisher
{
    [Verb("Reset", HelpText = "Resets the 'should be contacted flag' to true.")]
    public class ResetOptions
    {
    }
}
