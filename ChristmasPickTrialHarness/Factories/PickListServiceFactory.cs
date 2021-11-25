using System;
using Microsoft.Extensions.Logging;
using ChristmasPickCommon;
using Common;
using Common.ChristmasPickList;
using ChristmasPickCommon.Configuration;
using ChristmasPickCommon.Factories;
using ChristmasPickTrialHarness.Configuration;
using ChristmasPickTrialHarness.Factories.RuleProviders;
using ChristmasPickTrialHarness.Services;

namespace ChristmasPickTrialHarness.Factories
{
    public class PickListServiceFactory : IPickListServiceFactory
    {
        private readonly IProvideConfiguration cfgProvider;
        private readonly ILogger<CreateChristmasPicks> logger;

        public PickListServiceFactory(
            IProvideConfiguration cfgProvider,
            ILogger<CreateChristmasPicks> logger)
        {
            this.cfgProvider = cfgProvider ?? throw new ArgumentNullException(nameof(cfgProvider));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private IPickListService CreateKidService(XMasDay xMasDay, XMasPickListType xMasPickListType)
        {
            var kidArchivePath = cfgProvider.GetConfiguration(CfgKey.KidArchivePath);
            var familyArchivePath = cfgProvider.GetConfiguration(CfgKey.FamilyArchivePath);
            IXMasArchivePersister kidPersister = new FileArchivePersister(kidArchivePath);
            IFamilyProvider familyProvider = new FileFamilyProvider(familyArchivePath);
            FamilyTree gehredFamily = familyProvider.GetFamilies();
            XMasArchive kidArchive = kidPersister.LoadArchive();
            // Create kidList
            PersonCollection kidList = gehredFamily.CreateChristmasKidList(xMasDay);

            IPickListRuleProvider kidRules = new KidListRuleProvider(gehredFamily, kidArchive, years:2);
            IPickListService picker = new PickListServiceAdvanced(new RandomNumberGenerator(kidList.Count), kidRules, kidList);
            IPickListService pickerWithValidation = new PickListServiceWithValidation(
                picker,
                kidList,
                kidPersister,
                kidArchive,
                logger);
            return pickerWithValidation;

        }

        private IPickListService CreateAdultService(XMasDay xMasDay, XMasPickListType xMasPickListType)
        {
            var adultArchivePath = cfgProvider.GetConfiguration(CfgKey.AdultArchivePath);
            var familyArchivePath = cfgProvider.GetConfiguration(CfgKey.FamilyArchivePath);
            IXMasArchivePersister adultPersister = new FileArchivePersister(adultArchivePath);
            IFamilyProvider familyProvider = new FileFamilyProvider(familyArchivePath);
            FamilyTree gehredFamily = familyProvider.GetFamilies();
            XMasArchive adultArchive = adultPersister.LoadArchive();
            // Create AdultList
            PersonCollection adultList = gehredFamily.CreateChristmasAdultList(xMasDay);

            IPickListRuleProvider adultRules = new AdultListRuleProvider(gehredFamily, adultArchive, years:5);
            IPickListService picker = new PickListServiceAdvanced(new RandomNumberGenerator(adultList.Count), adultRules, adultList);
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
            IPickListService picker = null;
            if (xMasPickListType == XMasPickListType.Kid)
            {
                picker = CreateKidService(xMasDay, xMasPickListType);
            }
            if (xMasPickListType == XMasPickListType.Adult)
            {
                picker = CreateAdultService(xMasDay, xMasPickListType);
            }

            if (picker == null)
            {
                throw new  NotImplementedException($"The XMasPickListType: {xMasPickListType} does not have a defined service factory.");
            }
            return picker;
        }

    }

}
