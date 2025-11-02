using Common;
using Common.ChristmasPickList;
using Common.ChristmasPickList.Rules;

namespace ChristmasPickUtil.Verbs.ChristmasPick.Factories.RuleProviders
{
    public class KidListRuleProvider : IPickListRuleProvider
    {
        private FamilyTree mFamily;
        private XMasArchive mArchive;
        private int mYearsBack;

        public KidListRuleProvider(FamilyTree family, XMasArchive archive, int years)
        {
            mFamily = family;
            mArchive = archive;
            mYearsBack = years;
        }

        public IList<IPickListRule> GetRulesForPickList()
        {
            List<IPickListRule> testRules = new List<IPickListRule>();
            testRules.Add(new SiblingRule(mFamily));
            testRules.Add(new ChristmasPastRule(mArchive, mYearsBack));
            return testRules;
        }
    }
}
