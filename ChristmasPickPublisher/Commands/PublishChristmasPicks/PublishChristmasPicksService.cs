using System;
using System.Threading;
using System.Threading.Tasks;
using ChristmasPickCommon;
using ChristmasPickCommon.Configuration;
using ChristmasPickMessages;
using ChristmasPickMessages.Messages;
using ChristmasPickNotifier.Notifier;
using ChristmasPickNotifier.Notifier.Email;
using ChristmasPickPublisher.CommandLineOptions;
using ChristmasPickPublisher.Configuration;
using Common;
using Common.ChristmasPickList;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ChristmasPickPublisher.Commands.PublishChristmasPicks
{
    public interface IPublishChristmasPicks
    {
        Task Publish(ChristmasPickPublisherOptions options, CancellationToken stoppingToken);
    }

    public class PublishChristmasPicksService : BackgroundService, IPublishChristmasPicks
    {
        private readonly ILogger<PublishChristmasPicksService> _logger;
        private readonly IProvideConfiguration _cfgProvider;
        private ChristmasPickPublisherOptions _options;
        private XMasDay _xmasDay;
        private XMasPickListType _pickListType;
        private INotifier _notifier;
        private IEmailAddressProvider _emailAddressProvider;
        private XMasPickList _pickListToPublish;
        private string _emailTemplate;

        public PublishChristmasPicksService(
            ILogger<PublishChristmasPicksService> logger,
            IProvideConfiguration configurationProvider
        )
        {
            _logger = logger;
            _cfgProvider = configurationProvider;
            _options = null;
        }

        private bool GuardAreOptionsValid(ChristmasPickPublisherOptions options)
        {
            var (isValid, reasons) = options.IsValid();
            if (!isValid)
            {
                _logger.LogError("In valid Christmas pick options.");
                foreach(var reason in reasons)
                {
                    _logger.LogError(reason);
                }
            }
            return isValid;
        }

        private INotifier BuildNotifier()
        {
            var sendInBlueApiKey = _cfgProvider.GetConfiguration(CfgKey.SendInBlueApiKey);
            return new SendInBlueNotifyPickIsAvailable(sendInBlueApiKey);
        }

        private IEmailAddressProvider BuildEmailAddressProvider()
        {
            var familyContacts = _cfgProvider.GetConfiguration(CfgKey.FamilyContacts);
            return new JsonFileEmailAddressProvider(familyContacts);
        }

        private XMasPickList GetXmasPickList(XMasDay christmasDay, XMasPickListType listType)
        {
            IXMasArchivePersister persister = null;
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

        private string GetEmailTemplate()
        {
            var pathToTemplate = _cfgProvider.GetConfiguration(CfgKey.PathToEmailTemplate);
            var emailTemplate = System.IO.File.ReadAllText(pathToTemplate);

            return emailTemplate;
        }

        public Task Publish(ChristmasPickPublisherOptions options, CancellationToken stoppingToken)
        {
            var isValid = GuardAreOptionsValid(options);
            if (!isValid)
            {
                return Task.CompletedTask;
            }
            this._options = options;

            var xmasDayValid = XMasDay.TryParse(options.Year, out _xmasDay);
            if (!xmasDayValid)
            {
                _logger.LogError($"Could not convert Year: {options.Year} to XMasDay type.");
            }
            XMasPickListType.TryParse(options.ListType, out _pickListType);

            _emailAddressProvider = BuildEmailAddressProvider();
            _notifier = BuildNotifier();
            _pickListToPublish = GetXmasPickList(_xmasDay, _pickListType);
            _emailTemplate = GetEmailTemplate();
           return ExecuteAsync(stoppingToken);
        }

        public string CreatePlainTextEmailBody(Person giftMaker, string pickMessage)
        {
            var plainTextMsg = _emailTemplate;
            plainTextMsg = plainTextMsg.Replace("[[newline]]", string.Empty);
            plainTextMsg = plainTextMsg.Replace("{giftMaker}", giftMaker.ToString());
            plainTextMsg = plainTextMsg.Replace("{pickMessage}", pickMessage);
            return plainTextMsg;
        }

        public string CreateHTMLTextEmailBody(Person giftMaker, string pickMessage)
        {
            var htmlMsg = _emailTemplate.Replace("[[newline]]", "<p>");
            htmlMsg = htmlMsg.Replace("\n", string.Empty);
            htmlMsg = htmlMsg.Replace("{giftMaker}", giftMaker.ToString());
            htmlMsg = htmlMsg.Replace("{pickMessage}", pickMessage);
            return $"<!DOCTYPE html><html><body>{htmlMsg}</body></html>";
        }

        public async Task<int> EmailGiftMakerPickMessage(Person giftMaker, string subjectLine, string htmlBody, string plainTextBody)
        {
            // Using the Person object lookup email.
            var successfulEmailSentCount = 0;
            var emailAddresses = _emailAddressProvider.GetEmailAddresses(giftMaker);
            foreach(var emailAddress in emailAddresses)
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
                var emailSendStatus = await _notifier.Notify(testEnvelope);
                //var emailSendStatus = NotifierResultFactory.Success;

                if (!emailSendStatus.IsSuccess())
                {
                    _logger.LogError($"{emailAddress} Status: Error: {emailSendStatus.Message}");
                }
                else
                {
                    _logger.LogInformation($"{emailAddress} Status: Ok");
                    successfulEmailSentCount++;
                }
                await Task.Delay(TimeSpan.FromSeconds(10));
            }

            return successfulEmailSentCount;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var emailCount = 0;
            var totalEmailSent = 0;
            Person giftMaker = null;
            try
            {
                string pickMsg = "\tFor the Christmas of {0} {1} will buy a {2} gift for {3}";
                string emailSubject = $"IMPORTANT: Gehred Nation Christmas Pick for {_xmasDay.Year.ToString()}";
                foreach(var xmasPick in _pickListToPublish)
                {
                    giftMaker = xmasPick.Subject;
                    if (_emailAddressProvider.ShouldBeContacted(giftMaker))
                    {
                        var giftMessage = string.Format(pickMsg, 
                                _xmasDay.Year,
                                giftMaker.ToString(),
                                _pickListType.GiftAmount.ToString("c"),
                                xmasPick.Recipient);
                            
                        var plainTextEmailBody = CreatePlainTextEmailBody(giftMaker, giftMessage);
                        var htmlEmailBody = CreateHTMLTextEmailBody(giftMaker, giftMessage);
                        emailCount += await EmailGiftMakerPickMessage(giftMaker, emailSubject, htmlEmailBody, plainTextEmailBody);

                        _logger.LogInformation($"Sent {emailCount}(s) to {giftMaker}");

                        _emailAddressProvider.SetContactStatus(giftMaker, emailCount <= 0);

                        totalEmailSent += emailCount;

                    }

                    if (totalEmailSent >= _options.MaxEmail) {
                        _logger.LogInformation($"Publisher has sent {totalEmailSent} (s) which is greater then or equal to {_options.MaxEmail}. Terminating program.");
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                if (giftMaker != null)
                {
                    _logger.LogError(ex, $"Processing emails for {giftMaker}.");
                }
                else
                {
                    _logger.LogError(ex, $"Processing emails and person was not defined.");
                }
                _logger.LogError(ex, "Something bad has happened.");
            }
            finally
            {
                _emailAddressProvider.Save();
                _logger.LogInformation($"Sent {totalEmailSent} emails this run");
            }


        }
    }
}
