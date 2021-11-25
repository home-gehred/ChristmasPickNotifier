using Common.ChristmasPickList;

namespace ChristmasPickCommon.Factories
{
    public interface IPickListServiceFactory
    {
        IPickListService CreateService(XMasDay xMasDay, XMasPickListType xMasPickListType);
    }
}
