using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChristmasPickCommon.Configuration;
using ChristmasPickMessages;
using ChristmasPickMessages.Messages;
using ChristmasPickNotifier.Notifier.Email;
using ChristmasPickPublisher.CommandLineOptions;
using ChristmasPickPublisher.Configuration;
using Common;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ChristmasPickPublisher.Commands.EmailContacts
{
    public interface IEmailContacts
    {
        Task EmailAllContectsAsync(EmailContactsOptions options, CancellationToken stoppingToken);
    }

    public class EmailContactService: BackgroundService, IEmailContacts
    {
        private readonly ILogger<EmailContactService> _logger;
        private readonly IProvideConfiguration _cfgProvider;
        private EmailContactsOptions _options;

        public EmailContactService(
            ILogger<EmailContactService> logger,
            IProvideConfiguration configurationProvider
        )
        {
            _logger = logger;
            _cfgProvider = configurationProvider;
            _options = null;
        }

        public Task EmailAllContectsAsync(EmailContactsOptions options, CancellationToken stoppingToken)
        {
            _options = options;

            return ExecuteAsync(stoppingToken);
        }

        protected PickAvailableMessage CreateEmailMessage(EmailAddress To, Person giftMaker)
        {
            var pathToTemplate = _options.TemplatePath;
            var emailTemplate = System.IO.File.ReadAllText(pathToTemplate);
            var plainTextMsg = emailTemplate;
            var htmlMsg = plainTextMsg.Replace("[[newline]]", "<p>");
            htmlMsg = htmlMsg.Replace("\n", string.Empty);
            htmlMsg = htmlMsg.Replace("{giftMaker}", giftMaker.ToString());
            plainTextMsg = plainTextMsg.Replace("[[newline]]", string.Empty);
            plainTextMsg = plainTextMsg.Replace("{giftMaker}", giftMaker.ToString());
            var content = new PickAvailableMessage
            {
                HtmlBody = $"<!DOCTYPE html><html><body>{htmlMsg}</body></html> ",
                PlainTextBody = plainTextMsg,
                Name = "C",
                NotificationType = NotifyType.Email,
                Subject = "IMPORTANT: Gehred Nation Christmas Pick for 2021",
                ToAddress = To
            };
            return content;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Here we go!");
            var sendInBlueApiKey = _cfgProvider.GetConfiguration(CfgKey.SendInBlueApiKey);
            var emailer = new SendInBlueNotifyPickIsAvailable(sendInBlueApiKey);
            var familyContacts = _cfgProvider.GetConfiguration(CfgKey.FamilyContacts);
            var emailAddressProvider = new JsonFileEmailAddressProvider(familyContacts);

            foreach(var person in emailAddressProvider.GetAllPeopleWithContact())
            {
                var emailAddresses = emailAddressProvider.GetEmailAddresses(person).ToList();
                if (emailAddressProvider.ShouldBeContacted(person))
                {
                    var anySuccess = false;
                    _logger.LogInformation($"{person.FirstName} {person.LastName} has {emailAddresses.Count} email addresses");
                    foreach(var emailAddress in emailAddresses)
                    {
                        _logger.LogInformation($"{emailAddress.ToString()}");
                        var emailMessage = CreateEmailMessage(emailAddress, person);
                        
                        var testEnvelope = new Envelope(emailMessage);
                        var emailSendStatus = await emailer.Notify(testEnvelope);
                        // For Debug
                        //var emailSendStatus = NotifierResultFactory.CreateFailed("Just testing");
                        //var emailSendStatus = NotifierResultFactory.Success;
                        if (!emailSendStatus.IsSuccess())
                        {
                            _logger.LogError($"{emailAddress.ToString()} Status: Error: {emailSendStatus.Message}");
                        }
                        else
                        {
                            _logger.LogInformation($"{emailAddress.ToString()} Status: Ok");
                            anySuccess = true;
                        }
                        
                        await Task.Delay(TimeSpan.FromSeconds(_options.WaitTimeInSeconds));

                    }
                    if (anySuccess)
                    {
                        emailAddressProvider.SetContactStatus(person, shouldBeContacted: false);
                    }
                }
            }

            return;
        }
    }
}
