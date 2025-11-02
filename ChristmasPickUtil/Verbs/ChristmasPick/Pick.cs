using ChristmasPickCommon;
using ChristmasPickUtil.Configuration;
using ChristmasPickUtil.Verbs.ChristmasPick.Factories;
using ChristmasPickUtil.Verbs.ChristmasPick.Services;
using Common.ChristmasPickList;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace ChristmasPickUtil.Verbs.ChristmasPick
{
    public class Pick : VerbBase<PickOptions>
    {
        public Pick(IConfiguration config, ILogger<PickOptions> logger) : base(config, logger)
        {
        }

        public override Task<int> DoVerbAsync(PickOptions options)
        {
            var christmasYear = options.Year ?? DateTime.UtcNow.Year;
            var isYearValid = XMasDay.TryParse(christmasYear, out var currentXmasDay);
            var isPickListTypeValid = XMasPickListType.TryParse(options.Type.ToString(), out var listType);
            if (isYearValid == false || isPickListTypeValid == false)
            {
                _logger.LogError("Either the Chirstmas year of {christmasYear} or list type of {listType} is not valid.", christmasYear, options.Type);
                return Task.FromResult(-1);
            }
            _logger.LogInformation("Generating {listType} Christmas picks for {currentXmasDay}...", listType.ToString(), currentXmasDay);
            var configProvider = new ProvideMicrosoftConfiguration(_config);
            var pickListServiceFactory = new PickListServiceFactory(configProvider, _logger, options.YearsBack);
            var xmasPickService = pickListServiceFactory.CreateService(currentXmasDay, listType);

            _ = xmasPickService.CreateChristmasPick(currentXmasDay);

            return Task.FromResult(0);
        }
    }
}
