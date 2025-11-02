using System;
using System.Collections.Generic;
using Common.ChristmasPickList;
using Common.ChristmasPickList.Rules;
using Common;

namespace ChristmasPickUtil.Verbs.ChristmasPick.Factories.RuleProviders
{
    public class AdultListRuleProvider : IPickListRuleProvider
    {
        private FamilyTree mFamily;
        private XMasArchive mArchive;
        private int mYearsBack;

        public AdultListRuleProvider(FamilyTree family, XMasArchive archive, int years)
        {
            mFamily = family;
            mArchive = archive;
            mYearsBack = years;
        }

        public IList<IPickListRule> GetRulesForPickList()
        {
            List<IPickListRule> testRules = new List<IPickListRule>();
            testRules.Add(new SiblingRule(mFamily));
            testRules.Add(new ParentChildRule(mFamily));
            testRules.Add(new SpouseRule(mFamily));
            testRules.Add(new ChristmasPastRule(mArchive, mYearsBack));
            return testRules;
        }
    }

}
