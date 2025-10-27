using ChristmasPickCommon;
using Common.ChristmasPickList;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ChristmasPickUtil.Verbs.ResetChristmasPickPublisher
{
    public class Reset : VerbBase<ResetOptions>
    {
        public Reset(IConfiguration config, ILogger<ResetOptions> logger) : base(config, logger)
        {
        }

        public override Task<int> DoVerbAsync(ResetOptions options)
        {
            var emailAddressProvider = this.BuildEmailAddressProvider();
            var shouldBeContectedFlag = true;
            foreach(var person in emailAddressProvider.GetAllPeopleWithContact())
            {
                _logger.LogInformation("Resetting {person} contact flag to {shouldbecontacted}", person, shouldBeContectedFlag);
                emailAddressProvider.SetContactStatus(person, true);
            }

            emailAddressProvider.Save();

            return Task.FromResult(1);
        }
    }
}
