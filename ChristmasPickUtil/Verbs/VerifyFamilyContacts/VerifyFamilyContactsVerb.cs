using ChristmasPickUtil.Verbs.VerifyFamilyContacts;
using Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ChristmasPickUtil.Verbs.VerifyEmailContacts
{
    public class VerifyFamilyContactsVerb : VerbBase<VerifyFamilyContactsOptions>
    {

        public VerifyFamilyContactsVerb(IConfiguration config, ILogger<VerifyFamilyContactsOptions> logger) : base(config, logger)
        {
        }

        public override Task<int> DoVerbAsync(VerifyFamilyContactsOptions options)
        {
            var xmasDay = GetXMasDay(options.Year);
            var pickListType = GetPickListType(options.Type);
            var pickListToPublish = GetXmasPickList(xmasDay!, pickListType!);
            var emailAddressProvider = BuildEmailAddressProvider();
            var familyContacts = emailAddressProvider.GetAllPeopleWithContact();
            _logger.LogInformation("There are {number} people with contact information.", familyContacts.Count());
            Person? giftMaker = null;
            foreach (var xmasPick in pickListToPublish)
            {
                giftMaker = xmasPick.Subject;
                var giftMakerEmailAddresses = emailAddressProvider.GetEmailAddresses(giftMaker);
                _logger.LogInformation("{person} has [{emails}]", giftMaker.ToString(), string.Join(',', giftMakerEmailAddresses));
            }
            return Task.FromResult(0);
        }
    }
}
