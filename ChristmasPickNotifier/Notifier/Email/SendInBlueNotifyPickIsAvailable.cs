using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChristmasPickMessages;
using ChristmasPickMessages.Messages;
using Newtonsoft.Json;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;

namespace ChristmasPickNotifier.Notifier.Email
{
    public class SendInBlueNotifyPickIsAvailable : INotifier
    {
        private readonly TransactionalEmailsApi _sendInBlueClient;
        private readonly SendSmtpEmailSender _from;
        private readonly SendSmtpEmailReplyTo _replyTo;

        public SendInBlueNotifyPickIsAvailable(string sendInBlueApiKey)
        {
            Configuration.Default.ApiKey.Add("api-key", sendInBlueApiKey);
            _sendInBlueClient = new TransactionalEmailsApi();
            _from = new SendSmtpEmailSender("Santa's Lil' Helper", "santaslilHelper@gehrednation.org");
            _replyTo = new SendSmtpEmailReplyTo("santaslilHelper@gehrednation.org", "Santa's Lil' Helper");

        }

        public Task<NotifierResult> Notify(IEnvelope message)
        {
            var plainTextEmailCfg = JsonConvert.DeserializeObject<PickAvailableMessage>(message.Payload);
            
            var smtpEmailTo = new SendSmtpEmailTo(plainTextEmailCfg.ToAddress, plainTextEmailCfg.Name);
            var To = new List<SendSmtpEmailTo>();
            To.Add(smtpEmailTo);

            var htmlContent = plainTextEmailCfg.HtmlBody;
            var textContent = plainTextEmailCfg.PlainTextBody;
            var subject = plainTextEmailCfg.Subject;
            try
            {
                var sendSmtpEmail = new SendSmtpEmail(
                    _from,
                    To,
                    bcc: null,
                    cc: null,
                    htmlContent,
                    textContent,
                    subject,
                    _replyTo,
                    attachment:null, headers:null, templateId:null, _params:null, messageVersions:null, tags:null);
                CreateSmtpEmail result = _sendInBlueClient.SendTransacEmail(sendSmtpEmail);
                return System.Threading.Tasks.Task.FromResult(NotifierResultFactory.Success);
            }
            catch (Exception e)
            {
                return System.Threading.Tasks.Task.FromResult(NotifierResultFactory.CreateFailed($"SendInBlue Exception: {e.Message} <{e.ToString()}>."));
            }
        }
    }
}
