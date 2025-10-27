using ChristmasPickUtil.Configuration;
using Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace ChristmasPickUtil.Verbs
{
    public abstract class VerbBase<T>
    {
        protected readonly IConfiguration _config;
        protected readonly ILogger<T> _logger;
        protected readonly IChristmasPickUtilConfiguration _cfgProvider;

        protected VerbBase(IConfiguration config, ILogger<T> logger)
        {
            _config = config;
            _logger = logger;
            var provider = new ProvideMicrosoftConfiguration(_config);
            _cfgProvider = new ChristmasPickUtilConfiguration(provider);
        }

        protected IEmailAddressProvider BuildEmailAddressProvider()
        {
            var familyContacts = _cfgProvider.GetConfiguration(CfgKey.FamilyContacts);
            return new JsonFileEmailAddressProvider(familyContacts);
        }

        protected string GetEmailTemplate()
        {
            var pathToTemplate = _cfgProvider.GetConfiguration(CfgKey.PathToEmailTemplate);
            var emailTemplate = System.IO.File.ReadAllText(pathToTemplate);

            return emailTemplate;
        }

        protected string CreatePlainTextEmailBody(Person giftMaker, string pickMessage, string emailTemplate)
        {
            var plainTextMsg = emailTemplate;
            plainTextMsg = plainTextMsg.Replace("[[newline]]", string.Empty);
            plainTextMsg = plainTextMsg.Replace("{giftMaker}", giftMaker.ToString());
            plainTextMsg = plainTextMsg.Replace("{pickMessage}", pickMessage);
            return plainTextMsg;
        }

        protected string CreateHTMLTextEmailBody(Person giftMaker, string pickMessage, string emailTemplate)
        {
            var htmlMsg = emailTemplate.Replace("[[newline]]", "<p>");
            htmlMsg = htmlMsg.Replace("\n", string.Empty);
            htmlMsg = htmlMsg.Replace("{giftMaker}", giftMaker.ToString());
            htmlMsg = htmlMsg.Replace("{pickMessage}", pickMessage);
            return $"<!DOCTYPE html><html><body>{htmlMsg}</body></html>";
        }


        public abstract Task<int> DoVerbAsync(T options);
    }
}
