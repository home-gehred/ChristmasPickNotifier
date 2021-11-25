using System;
using System.Collections.Generic;
using System.Text;

namespace Common.ChristmasPickList.Rules
{
  public class SpouseRule: IPickListRule
  {
    private FamilyTree mFamily;

    public SpouseRule(FamilyTree family)
    {
      mFamily = family;
    }

    public bool IsPickValidForSubject(Person subject, Person toBuyPresentFor)
    {
      return !(mFamily.AreSpouses(subject, toBuyPresentFor));
    }

  }
}
