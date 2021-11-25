using System;
using System.Collections.Generic;
using System.Text;

namespace Common.ChristmasPickList.Rules
{
  public class AlwayPassRule : IPickListRule
  {
    public bool IsPickValidForSubject(Person subject, Person toBuyPresentFor)
    {
      return true;
    }
  }
}
