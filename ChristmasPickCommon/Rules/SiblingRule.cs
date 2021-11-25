using System;
using System.Collections.Generic;
using System.Text;

namespace Common.ChristmasPickList.Rules
{
  public class SiblingRule : IPickListRule
  {
    private FamilyTree mFamily;

    public SiblingRule(FamilyTree family)
    {
      mFamily = family;
    }

    public bool IsPickValidForSubject(Person subject, Person toBuyPresentFor)
    {
      return !(mFamily.AreSiblings(subject, toBuyPresentFor));
    }
  }
}
