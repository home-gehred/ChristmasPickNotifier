using Common;
using Common.ChristmasPickList;
using Common.ChristmasPickList.Rules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace ChristmasPickUtil.Verbs.ViewChristmasPickTemplate
{
    public class ViewTemplate : VerbBase<ViewTemplateOptions>
    {
        public ViewTemplate(IConfiguration config, ILogger<ViewTemplateOptions> logger)
            : base(config, logger)
        {
        }

        public override async Task<int> DoVerbAsync(ViewTemplateOptions options)
        {
            var outputFolder = options.OutputFolder;
            if (string.IsNullOrWhiteSpace(options.OutputFolder))
            {
                var executingAssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidOperationException("Could not determine assembly directory");
                outputFolder = executingAssemblyPath;
            }
            _ = XMasDay.TryParse(3000, out XMasDay xmasDay);

            var plainTextEmailPath = Path.Combine(outputFolder!, $"PlainTextEmailTempalate_{xmasDay.Year}.txt");
            var htmlEmailPath = Path.Combine(outputFolder!, $"HtmlEmailTempalate_{xmasDay.Year}.html");

            Person giftMaker = new Person("Stanley", "GiftMaker", new DateTime(1972, 7, 27), Guid.NewGuid().ToString());
            var emailTemplate = GetEmailTemplate();
            string pickMsg = "\tFor the Christmas of {0} {1} will buy a {2} gift for {3}";
            var giftMessage = string.Format(pickMsg,
                xmasDay.Year,
                "GiftMakerName",
                20.ToString("c"),
                "Recipient");

            var plainTextEmailBody = CreatePlainTextEmailBody(giftMaker, giftMessage, emailTemplate);
            var htmlEmailBody = CreateHTMLTextEmailBody(giftMaker, giftMessage, emailTemplate);

            _logger.LogInformation("Templates are written to {outputFolder}", outputFolder);
            await File.WriteAllTextAsync(plainTextEmailPath, plainTextEmailBody);
            await File.WriteAllTextAsync(htmlEmailPath, htmlEmailBody);
            return 0;
        }
    }
}
