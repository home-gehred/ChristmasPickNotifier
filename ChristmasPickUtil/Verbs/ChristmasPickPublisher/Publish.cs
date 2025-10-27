using ChristmasPickCommon;
using ChristmasPickCommon.Configuration;
using ChristmasPickUtil.Configuration;
using Common;
using Common.ChristmasPickList;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ChristmasPickUtil.Verbs.ChristmasPickPublisher
{
    public class Publish : VerbBase<PublishOptions>
    {

        public Publish(IConfiguration config, ILogger<PublishOptions> logger) 
            : base(config, logger)
        {
        }

        private XMasPickList GetXmasPickList(XMasDay christmasDay, XMasPickListType listType)
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

        public override Task<int> DoVerbAsync(PublishOptions options)
        {
            var xmasDayValid = XMasDay.TryParse(options.Year, out XMasDay xmasDay);
            if (!xmasDayValid)
            {
                _logger.LogError("Could not convert Year: {year} to XMasDay type.", options.Year);
                return Task.FromResult(1);
            }
            if (!XMasPickListType.TryParse(options.Type.ToString(), out XMasPickListType pickListType))
            {
                _logger.LogError("Could not convert list type: {list} to XMasDay list type.", options.Type);
                return Task.FromResult(1);
            }
            
            _logger.LogInformation("Command is publishing picks for {year} for {listtype}", xmasDay, pickListType);
            var pickListToPublish = GetXmasPickList(xmasDay, pickListType);
            var emailAddressProvider = BuildEmailAddressProvider();
            var emailTemplate = GetEmailTemplate();

            var emailCount = 0;
            var totalEmailSent = 0;
            var totalGiftMakerSkipped = 0;
            Person? giftMaker = null;
            try
            {
                string pickMsg = "\tFor the Christmas of {0} {1} will buy a {2} gift for {3}";
                string emailSubject = $"IMPORTANT: Gehred Nation Christmas Pick for {xmasDay.Year}";
                foreach (var xmasPick in pickListToPublish)
                {
                    giftMaker = xmasPick.Subject;
                    if (emailAddressProvider.ShouldBeContacted(giftMaker))
                    {
                        var giftMessage = string.Format(pickMsg,
                                xmasDay.Year,
                                giftMaker.ToString(),
                                pickListType.GiftAmount.ToString("c"),
                                xmasPick.Recipient);

                        var plainTextEmailBody = CreatePlainTextEmailBody(giftMaker, giftMessage, emailTemplate);
                        var htmlEmailBody = CreateHTMLTextEmailBody(giftMaker, giftMessage, emailTemplate);
                        emailCount++; //await EmailGiftMakerPickMessage(giftMaker, emailSubject, htmlEmailBody, plainTextEmailBody);

                        _logger.LogInformation("Sent {emailCount} email(s) to {giftMaker}", emailCount, giftMaker);

                        emailAddressProvider.SetContactStatus(giftMaker, emailCount <= 0);

                        totalEmailSent += emailCount;

                    }
                    else
                    {
                        totalGiftMakerSkipped++;
                        _logger.LogInformation("{giftMaker} skipped per ShouldBeContactedFlag is set.", giftMaker);
                    }

                    if (totalEmailSent >= options.MaxEmailsToSend)
                    {
                        _logger.LogInformation("Publisher has sent {totalEmailSent} email(s) which is greater then or equal to {maxEmailsToSend}. Terminating program.", totalEmailSent, options.MaxEmailsToSend);
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                if (giftMaker != null)
                {
                    _logger.LogError(ex, "Processing emails for {giftMaker}.", giftMaker);
                }
                else
                {
                    _logger.LogError(ex, "Processing emails and person was not defined.");
                }
                _logger.LogError(ex, "Something bad has happened.");
            }
            finally
            {
                emailAddressProvider.Save();
                _logger.LogInformation("Sent {totalEmailSent} emails this run, and skipped {skipped}", totalEmailSent, totalGiftMakerSkipped);
            }

            return Task.FromResult(0);
        }
    }
}
