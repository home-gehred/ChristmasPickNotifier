
using Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ChristmasPickUtil.Verbs.TestEmail;

public class TestEmailAddressProvider(Person person, EmailAddress emailAddress) : IEmailAddressProvider
{
    private readonly Person _person = person;
    private readonly EmailAddress _emailAddress = emailAddress;

    public IEnumerable<Person> GetAllPeopleWithContact()
    {
        return [_person];
    }

    public IEnumerable<EmailAddress> GetEmailAddresses(Person x)
    {
        return [_emailAddress];
    }

    public void Save()
    {
        // no op
    }

    public void SetContactStatus(Person x, bool shouldBeContacted)
    {
        // no op
    }

    public bool ShouldBeContacted(Person person)
    {
        return true;
    }

}

public class Email : VerbBase<EmailOption>
{
    private readonly Person _testPerson;
    public Email(IConfiguration config, ILogger<EmailOption> logger) : base(config, logger)
    {
        _testPerson = new Person("Stanley", "Emailer", new DateTime(1972, 7, 27), Guid.NewGuid().ToString());
    }

    public override async Task<int> DoVerbAsync(EmailOption options)
    {
        var emailAddress = new EmailAddress(options.Address);
        var emailAddressProvider = new TestEmailAddressProvider(_testPerson, emailAddress);
        int Year = 1990;
        string pickMsg = "\tFor the Christmas of {0} {1} will buy a {2} gift for {3}";
        string emailSubject = $"IMPORTANT: Gehred Nation Christmas Pick for {Year}";
        var emailTemplate = GetEmailTemplate();
        var notifier = BuildNotifier();

        var giftMessage = string.Format(pickMsg,
            Year,
            _testPerson.ToString(),
            1000.00.ToString("c"),
            _testPerson);

        var plainTextEmailBody = CreatePlainTextEmailBody(_testPerson, giftMessage, emailTemplate);
        var htmlEmailBody = CreateHTMLTextEmailBody(_testPerson, giftMessage, emailTemplate);
        var emailCount = await EmailGiftMakerPickMessage(
            emailAddressProvider,
            notifier,
            _testPerson,
            emailSubject,
            htmlEmailBody, plainTextEmailBody);

        _logger.LogInformation("Sent {emailCount} email(s) to {giftMaker}", emailCount, _testPerson);
        return 0;
    }
}