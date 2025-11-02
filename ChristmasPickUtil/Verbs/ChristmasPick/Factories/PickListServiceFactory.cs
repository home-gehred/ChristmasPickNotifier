using ChristmasPickCommon;
using ChristmasPickCommon.Configuration;
using ChristmasPickCommon.Factories;
using ChristmasPickUtil.Configuration;
using ChristmasPickUtil.Verbs.ChristmasPick.Factories.RuleProviders;
using ChristmasPickUtil.Verbs.ChristmasPick.Services;
using Common;
using Common.ChristmasPickList;
using Microsoft.Extensions.Logging;

namespace ChristmasPickUtil.Verbs.ChristmasPick.Factories
{
    public class PickListServiceFactory : IPickListServiceFactory
    {
        private readonly IProvideConfiguration cfgProvider;
        private readonly ILogger<PickOptions> logger;
        private readonly int yearsBack;

        public PickListServiceFactory(
            IProvideConfiguration cfgProvider,
            ILogger<PickOptions> logger,
            int? yearsBack = null
            )
        {
            this.cfgProvider = cfgProvider ?? throw new ArgumentNullException(nameof(cfgProvider));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.yearsBack = yearsBack ?? 5;
        }

        private IPickListService CreateKidService(XMasDay xMasDay)
        {
            var kidArchivePath = cfgProvider.GetConfiguration(CfgKey.KidArchivePath);
            var familyPath = cfgProvider.GetConfiguration(CfgKey.FamilyPath);
            IXMasArchivePersister kidPersister = new FileArchivePersister(kidArchivePath);
            IFamilyProvider familyProvider = new FileFamilyProvider(familyPath);
            FamilyTree gehredFamily = familyProvider.GetFamilies();
            XMasArchive kidArchive = kidPersister.LoadArchive();
            // Create kidList
            PersonCollection kidList = gehredFamily.CreateChristmasKidList(xMasDay);

            IPickListRuleProvider kidRules = new KidListRuleProvider(gehredFamily, kidArchive, yearsBack);
            IPickListService picker = new PickListServiceAdvanced(new RandomNumberGenerator(), kidRules, kidList);
            IPickListService pickerWithValidation = new PickListServiceWithValidation(
                picker,
                kidList,
                kidPersister,
                kidArchive,
                logger);
            return pickerWithValidation;

        }

        private IPickListService CreateAdultService(XMasDay xMasDay)
        {
            var adultArchivePath = cfgProvider.GetConfiguration(CfgKey.AdultArchivePath);
            var familyArchivePath = cfgProvider.GetConfiguration(CfgKey.FamilyPath);
            IXMasArchivePersister adultPersister = new FileArchivePersister(adultArchivePath);
            IFamilyProvider familyProvider = new FileFamilyProvider(familyArchivePath);
            FamilyTree gehredFamily = familyProvider.GetFamilies();
            XMasArchive adultArchive = adultPersister.LoadArchive();
            // Create AdultList
            PersonCollection adultList = gehredFamily.CreateChristmasAdultList(xMasDay);

            IPickListRuleProvider adultRules = new AdultListRuleProvider(gehredFamily, adultArchive, yearsBack);
            IPickListService picker = new PickListServiceAdvanced(new RandomNumberGenerator(), adultRules, adultList);
            IPickListService pickerWithValidation = new PickListServiceWithValidation(
                picker,
                adultList,
                adultPersister,
                adultArchive,
                logger);
            return pickerWithValidation;

        }

        public IPickListService CreateService(XMasDay xMasDay, XMasPickListType xMasPickListType)
        {
            IPickListService? picker = null;
            if (xMasPickListType == XMasPickListType.Kid)
            {
                picker = CreateKidService(xMasDay);
            }
            if (xMasPickListType == XMasPickListType.Adult)
            {
                picker = CreateAdultService(xMasDay);
            }

            if (picker == null)
            {
                throw new  NotImplementedException($"The XMasPickListType: {xMasPickListType} does not have a defined service factory.");
            }
            return picker;
        }

    }

}
