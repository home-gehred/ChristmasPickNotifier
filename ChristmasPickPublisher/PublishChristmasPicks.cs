using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CommandLine;
using Common;
using Common.ChristmasPickList;
using ChristmasPickCommon;
using ChristmasPickCommon.Configuration;
using ChristmasPickNotifier.Notifier;
using ChristmasPickMessages;
using ChristmasPickMessages.Messages;
using ChristmasPickPublisher.Configuration;
using ChristmasPickPublisher.CommandLineOptions;
using ChristmasPickNotifier.Notifier.Email;

namespace ChristmasPickPublisher
{
    public interface IPublishChristmsPicks
    {
        Task<int> PublishChristmasPicksAsync();
    }
    public class PublishChristmasPicks : BackgroundService, IPublishChristmsPicks
    {
        private readonly ILogger<PublishChristmasPicks> logger;
        private readonly IProvideConfiguration cfgProvider;

        private string emailTemplate;
        public PublishChristmasPicks(
            IProvideConfiguration cfgProvider,
            ILogger<PublishChristmasPicks> logger)
        {
            this.cfgProvider = cfgProvider ?? throw new ArgumentNullException(nameof(cfgProvider));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            emailTemplate = string.Empty;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
           return PublishChristmasPicksAsync();
        }

        protected PickAvailableMessage CreateEmailMessage(EmailAddress To, Person giftMaker, string pickMessage)
        {
            var pathToTemplate = cfgProvider.GetConfiguration(CfgKey.PathToEmailTemplate);
            if (string.IsNullOrEmpty(emailTemplate)) 
            {
                emailTemplate = System.IO.File.ReadAllText(pathToTemplate);
            }
            var plainTextMsg = emailTemplate;
            var htmlMsg = plainTextMsg.Replace("[[newline]]", "<p>");
            htmlMsg = htmlMsg.Replace("\n", string.Empty);
            htmlMsg = htmlMsg.Replace("{giftMaker}", giftMaker.ToString());
            htmlMsg = htmlMsg.Replace("{pickMessage}", pickMessage);
            plainTextMsg = plainTextMsg.Replace("[[newline]]", string.Empty);
            plainTextMsg = plainTextMsg.Replace("{giftMaker}", giftMaker.ToString());
            plainTextMsg = plainTextMsg.Replace("{pickMessage}", pickMessage);
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

        public void RunOptions(ChristmasPickPublisherOptions opts)
        {
            //handle options
            var listTypeValid = XMasPickListType.TryParse(opts.ListType, out XMasPickListType listType);
            var xmasDayValid = XMasDay.TryParse(opts.Year, out XMasDay xmasDay);
            if ((xmasDayValid && listTypeValid) == false)
            {
                throw new ApplicationException(
                    $"Either the year or list type argument is not valid. Please check command line arguments and try again.");
            } 
            var sendGridApiKey = cfgProvider.GetConfiguration(CfgKey.SendGridApiKey);
            var emailer = new SendGridNotifyPickIsAvalable(sendGridApiKey);
            var familyContacts = cfgProvider.GetConfiguration(CfgKey.FamilyContacts);
            var archivePath = string.Empty;
            decimal giftAmount = 0;
            if (listType == XMasPickListType.Kid)
            {
                archivePath = cfgProvider.GetConfiguration(CfgKey.KidArchivePath);
                giftAmount = 20.00M;
            }
            if (listType == XMasPickListType.Adult)
            {
                archivePath = cfgProvider.GetConfiguration(CfgKey.AdultArchivePath);
                giftAmount = 5.00M;
            }
            IXMasArchivePersister persister = new FileArchivePersister(archivePath);
            XMasArchive archive = persister.LoadArchive();
            
            XMasPickList pickList = archive.GetPickListForYear(xmasDay);
            JsonFileEmailAddressProvider emailAddressProvider = new JsonFileEmailAddressProvider(familyContacts);
            var emailCount = 0;
            Person person = null;
            try
            {
                string pickMsg = "\tFor the Christmas of {0} {1} will buy a {2} gift for {3}";
                foreach(var xmasPick in pickList)
                {
                    person = xmasPick.Subject;
                    if (emailAddressProvider.ShouldBeContacted(person))
                    {
                        // Using the Person object lookup email.
                        var emailAddresses = emailAddressProvider.GetEmailAddresses(person);

                        logger.LogInformation($"Attempting to email {person} with address(es), ");
                        var oneGoodEmailSent = false;
                        foreach (var emailAddress in emailAddresses)
                        {
                            emailCount++;
                            logger.LogInformation($"{emailAddress}");
                            var giftMessage = string.Format(pickMsg, xmasDay.Year, person.ToString(), giftAmount.ToString("c"), xmasPick.Recipient);
                            var content = CreateEmailMessage(emailAddress, person, giftMessage);
                            var testEnvelope = new Envelope(content);
                            var emailSendStatus = emailer.Notify(testEnvelope).GetAwaiter().GetResult();
                            // For Debug
                            //var emailSendStatus = NotifierResultFactory.CreateFailed("Just testing");
                            //var emailSendStatus = NotifierResultFactory.Success;
                            if (!emailSendStatus.IsSuccess())
                            {
                                logger.LogError($"{emailAddress} Status: Error: {emailSendStatus.Message}");
                            }
                            else
                            {
                                logger.LogInformation($"{emailAddress} Status: Ok");
                                oneGoodEmailSent = true;
                            }
                            Task.Delay(TimeSpan.FromSeconds(5)).GetAwaiter().GetResult();
                        }
                        emailAddressProvider.SetContactStatus(person, !oneGoodEmailSent);
                    }
                }
            }
            catch (Exception ex)
            {
                if (person != null)
                {
                    logger.LogError(ex, $"Processing emails for {person}.");
                }
                else
                {
                    logger.LogError(ex, $"Processing emails and person was not defined.");
                }
            }
            finally
            {
                emailAddressProvider.Save();
                logger.LogInformation($"Sent {emailCount} emails this run");
            }
        }

        public Task<int> PublishChristmasPicksAsync()
        {
            Parser.Default.ParseArguments<ChristmasPickPublisherOptions>(Environment.GetCommandLineArgs())
                .WithParsed<ChristmasPickPublisherOptions>(RunOptions);

            return Task.FromResult(0);
        }
    }
}
