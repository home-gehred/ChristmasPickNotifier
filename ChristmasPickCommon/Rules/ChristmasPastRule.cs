using System;
using System.Collections.Generic;
using System.Text;

namespace Common.ChristmasPickList.Rules
{
  public class ChristmasPastRule : IPickListRule
  {
    private XMasArchive mXmasPast = null;
    private int mYears;

    public ChristmasPastRule(XMasArchive xmashistory, int yearsBack)
    {
      mXmasPast = xmashistory;
      mYears = yearsBack;
    }

    public bool IsPickValidForSubject(Person subject, Person toBuyPresentFor)
    {
      DateTime offendingXmas = DateTime.MinValue;
      bool hasHadPersonInPast = mXmasPast.HasSubjectPersonBoughtAPresentForRecipientInLast(mYears, subject, toBuyPresentFor, out offendingXmas);
      return !(hasHadPersonInPast);
    }

  }
}
