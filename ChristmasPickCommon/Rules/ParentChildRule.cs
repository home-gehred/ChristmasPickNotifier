using System;
using System.Collections.Generic;
using System.Text;

namespace Common.ChristmasPickList.Rules
{
  public class ParentChildRule : IPickListRule
  {
    private FamilyTree mFamily;

    public ParentChildRule(FamilyTree family)
    {
      mFamily = family;
    }

    public bool IsPickValidForSubject(Person subject, Person toBuyPresentFor)
    {
      return !(mFamily.AreParentChild(subject, toBuyPresentFor));
    }

  }
}
