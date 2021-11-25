using System;
using System.Collections.Generic;

namespace ChristmasPickCommon
{
    public class XMasPickListType
    {
        public static XMasPickListType Adult = new XMasPickListType("adult");
        public static XMasPickListType Kid = new XMasPickListType("kid"); 
        private readonly string listType;
        private XMasPickListType(string listType)
        {
            this.listType = listType;
        }

        public override string ToString()
        {
            return listType;
        }
        public static bool TryParse(string picklisttype, out XMasPickListType listType)
        {
            var picklistTypes = new Dictionary<string, XMasPickListType>
            {
                {XMasPickListType.Adult.listType.ToLower(), XMasPickListType.Adult},
                {XMasPickListType.Kid.listType.ToLower(), XMasPickListType.Kid}
            };

            if (string.IsNullOrEmpty(picklisttype) == false) {
                if (picklistTypes.ContainsKey(picklisttype.ToLower()))
                {
                    listType = picklistTypes.GetValueOrDefault(picklisttype.ToLower());
                    return true;
                }
            }
            listType = null;
            return false;
        }
    }
}
