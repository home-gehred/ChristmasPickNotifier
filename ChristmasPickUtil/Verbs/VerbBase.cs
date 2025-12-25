using ChristmasPickCommon;
using ChristmasPickMessages;
using ChristmasPickMessages.Messages;
using ChristmasPickNotifier.Notifier;
using ChristmasPickNotifier.Notifier.Email;
using ChristmasPickUtil.Configuration;
using ChristmasPickUtil.Verbs.Models;
using Common;
using Common.ChristmasPickList;
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

        protected INotifier BuildNotifier()
        {
            var sendInBlueApiKey = _cfgProvider.GetConfiguration(CfgKey.SendInBlueApiKey);
            return new SendInBlueNotifyPickIsAvailable(sendInBlueApiKey);
        }

        protected string GetEmailTemplate()
        {
            var pathToTemplate = _cfgProvider.GetConfiguration(CfgKey.PathToEmailTemplate);
            var emailTemplate = System.IO.File.ReadAllText(pathToTemplate);

            return emailTemplate;
        }

        protected string GetReportPath()
        {
            var pathToReportFolder = _cfgProvider.GetConfiguration(CfgKey.ReportPathKey);
            if (string.IsNullOrWhiteSpace(pathToReportFolder))
                throw new InvalidOperationException($"Check configuration for {CfgKey.ReportPathKey} it appears to be missing or not set.");
            return pathToReportFolder;
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

        protected async Task<int> EmailGiftMakerPickMessage(IEmailAddressProvider emailAddressProvider, INotifier notifier, Person giftMaker, string subjectLine, string htmlBody, string plainTextBody)
        {
            // Using the Person object lookup email.
            var successfulEmailSentCount = 0;
            var emailAddresses = emailAddressProvider.GetEmailAddresses(giftMaker);
            foreach (var emailAddress in emailAddresses)
            {
                var content = new PickAvailableMessage
                {
                    HtmlBody = htmlBody,
                    PlainTextBody = plainTextBody,
                    Name = "C",
                    NotificationType = NotifyType.Email,
                    Subject = subjectLine,
                    ToAddress = emailAddress
                };
                var testEnvelope = new Envelope(content);
                var emailSendStatus = await notifier.Notify(testEnvelope);
                //var emailSendStatus = NotifierResultFactory.Success;

                if (!emailSendStatus.IsSuccess())
                {
                    _logger.LogError("{emailAddress} Status: Error: {statusMessage}", emailAddress, emailSendStatus.Message);
                }
                else
                {
                    _logger.LogInformation("{emailAddress} Status: Ok", emailAddress);
                    successfulEmailSentCount++;
                }
                await Task.Delay(TimeSpan.FromSeconds(10));
            }

            return successfulEmailSentCount;
        }

        protected XMasPickList GetXmasPickList(XMasDay christmasDay, XMasPickListType listType)
        {
            FileArchivePersister? persister = null;
            if (listType == XMasPickListType.Kid)
            {
                var kidCfgPath = _cfgProvider.GetConfiguration(CfgKey.KidArchivePath);
                persister = new FileArchivePersister(kidCfgPath);
            }
            if (listType == XMasPickListType.Adult)
            {
                var adultCfgPath = _cfgProvider.GetConfiguration(CfgKey.AdultArchivePath);
                persister = new FileArchivePersister(adultCfgPath);
            }
            if (persister == null) throw new NotImplementedException($"The XMasPickListType of {listType} is not defined.");

            XMasArchive archive = persister.LoadArchive();
            return archive.GetPickListForYear(christmasDay);
        }

        protected XMasDay? GetXMasDay(int? year)
        {
            if (year == null)
            {
                _logger.LogError("Could not conver <null> year to XMasDay type.");
                return null;
            }
            var xmasDayValid = XMasDay.TryParse(year.Value, out XMasDay xmasDay);
            if (!xmasDayValid)
            {
                _logger.LogError("Could not convert Year: {year} to XMasDay type.", year);
                return null;
            }
            return xmasDay;
        }

        protected XMasPickListType? GetPickListType(ListType listType)
        {
            if (!XMasPickListType.TryParse(listType.ToString(), out XMasPickListType pickListType))
            {
                _logger.LogError("Could not convert list type: {list} to XMasDay list type.", listType);
                return null;
            }
            return pickListType;
        }
        public abstract Task<int> DoVerbAsync(T options);
    }
}
