

using Common.ChristmasPickList;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ChristmasPickUtil.Verbs.ChristmasReport;

public class Report : VerbBase<ReportOptions>
{
    public Report(IConfiguration config, ILogger<ReportOptions> logger) : base(config, logger)
    {
    }

    public override async Task<int> DoVerbAsync(ReportOptions options)
    {
        await Task.Delay(TimeSpan.FromSeconds(3));
        var xmasDay = GetXMasDay(options.Year);
        var pickListType = GetPickListType(options.Type);
        if (xmasDay == null || pickListType == null)
        {
            return -1;
        }
        var pickListToReportOn = GetXmasPickList(xmasDay, pickListType);
        var reportFilePath = Path.Combine(GetReportPath(), $"{xmasDay}_masterReport.txt");

        var asList = new List<XMasPick>(pickListToReportOn.AsEnumerable<XMasPick>());

        var groupedAndOrdered = asList.GroupBy(x => x.Subject.LastName)
                .OrderBy(g => g.Key)
              .SelectMany(grp => grp.OrderBy(x => x.Subject.FirstName));

        using (var writer = new StreamWriter(reportFilePath, append: false))
        {
            foreach (var pick in groupedAndOrdered)
            {
                writer.WriteLine($"{pick.Subject} ===> {pick.PickMessage}\r\n");
            }
        }

        Console.WriteLine($"File was created at {reportFilePath}.");

        return 0;
    }
}
