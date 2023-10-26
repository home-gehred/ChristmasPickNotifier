using System;
using System.Collections.Generic;

namespace ChristmasPickCommon
{
    public class XMasPickListType
    {
        public static XMasPickListType Adult = new XMasPickListType("adult", 5M);
        public static XMasPickListType Kid = new XMasPickListType("kid", 20M); 
        private readonly string listType;
        private readonly decimal giftAmount;
        private XMasPickListType(string listType, decimal giftAmount)
        {
            this.listType = listType;
            this.giftAmount = giftAmount;
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

        public static IEnumerable<string> ValidPickListTypes()
        {
            return new List<string> { 
                Adult.ToString(),
                Kid.ToString()
            };
        }

        public decimal GiftAmount {
            get
            {
                return giftAmount;
            }
        }
    }
}
