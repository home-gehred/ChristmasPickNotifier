using System;
using System.Threading.Tasks;
using ChristmasPickMessages;
using ChristmasPickMessages.Messages;
using ChristmasPickNotifier.Notifier;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ChristmasPickNotifier.Notifier.Email
{
    public class SendGridNotifyPickIsAvalable : INotifier
    {
        private readonly string _sendGridApiKey;
        private readonly SendGridClient _client;
        private readonly EmailAddress _from;
        public SendGridNotifyPickIsAvalable(string sendGridApiKey)
        {
            _sendGridApiKey = sendGridApiKey ?? throw new ArgumentNullException(nameof(sendGridApiKey));
            _client = new SendGridClient(_sendGridApiKey);
            _from = new EmailAddress("santaslilHelper@gehrednation.org", "Santa's Lil' Helper");
        }

        public async Task<NotifierResult> Notify(IEnvelope message)
        {
            var plainTextEmailCfg = JsonConvert.DeserializeObject<PickAvailableMessage>(message.Payload);
            var subject = plainTextEmailCfg.Subject;
            var to = new EmailAddress(plainTextEmailCfg.ToAddress, plainTextEmailCfg.Name);
            var htmlContent = plainTextEmailCfg.HtmlBody;
            var plainTextContent = plainTextEmailCfg.PlainTextBody;            
            var msg = MailHelper.CreateSingleEmail(_from, to, subject, plainTextContent, htmlContent);
            var response = await _client.SendEmailAsync(msg);
            if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                return NotifierResultFactory.Success;
            }
            var responsePayload = await response.Body.ReadAsStringAsync();
            return NotifierResultFactory.CreateFailed($"SendGrid response status code: {response.StatusCode} <{responsePayload}>.");
        }
    }

}
