using System;
using System.Collections.Generic;
using System.Text;

namespace Common.ChristmasPickList
{
  public interface IPickListRuleProvider
  {
    IList<IPickListRule> GetRulesForPickList();
  }

  public class AlwaysPassRuleProvider : IPickListRuleProvider
  {
    public IList<IPickListRule> GetRulesForPickList()
    {
      List<IPickListRule> rules = new List<IPickListRule>();
      rules.Add(new Rules.AlwayPassRule());
      return rules;
    }
  }
}
