using System;
using Microsoft.Extensions.Logging;
using Common;
using Common.ChristmasPickList;

namespace ChristmasPickUtil.Verbs.ChristmasPick.Services
{
    public class PickListServiceWithValidation : IPickListService
    {
        private readonly ILogger logger;
        private readonly IPickListService innerPickListService;
        private readonly XMasPickListValidator validator;
        private readonly PersonCollection personList;
        private readonly XMasArchive archive;
        private readonly IXMasArchivePersister archivePersister;
        public PickListServiceWithValidation(
            IPickListService innerPickListService, 
            PersonCollection personList,
            IXMasArchivePersister archivePersister,
            XMasArchive archive,
            ILogger<PickOptions> logger)
        {
            this.innerPickListService = innerPickListService ?? throw new ArgumentNullException(nameof(innerPickListService));
            this.personList = personList ?? throw new ArgumentNullException(nameof(personList));
            this.archivePersister = archivePersister ?? throw new ArgumentNullException(nameof(archivePersister));
            this.archive = archive ?? throw new ArgumentNullException(nameof(archive));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            validator = new XMasPickListValidator();
        }

        public XMasPickList CreateChristmasPick(DateTime evaluationDate)
        {
            XMasPickList pickList = innerPickListService.CreateChristmasPick(evaluationDate);
            
            var checkList = validator.PickListToValidateWithPeopleList(personList, pickList);
            if (validator.isPickListValid(checkList))
            {
                archive.Add(evaluationDate.Year, pickList);
                archivePersister.SaveArchive(archive);
            }
            else
            {
                logger.LogWarning("The pick list has errors. Nothing was saved.");
            }

            return pickList;
        }
    }
}
