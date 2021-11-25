using System;
using System.Collections.Generic;
using Common;

namespace Common.ChristmasPickList
{
    public class XMasPickListValidator
    {
        public XMasPickListValidator()
        {
        }

        public IDictionary<Person, ExchangeCheckSum> PickListToValidateWithPeopleList(PersonCollection personList, XMasPickList pickList)
        {
            IDictionary<Person, ExchangeCheckSum> checkList = new Dictionary<Person, ExchangeCheckSum>();

            foreach (Person person in personList)
            {
                checkList.Add(person, new ExchangeCheckSum());
            }

            foreach (XMasPick pick in pickList)
            {
                if (checkList.ContainsKey(pick.Recipient))
                {
                    checkList[pick.Recipient].updatePresentsIn();
                }
                else
                {
                    throw new Exception(string.Format("The recipient {0} is not found in adult list", pick.Recipient));
                }

                if (checkList.ContainsKey(pick.Subject))
                {
                    checkList[pick.Subject].updatePresentsOut();
                }
                else
                {
                    throw new Exception(string.Format("The subject {0} is not found in adult list", pick.Subject));
                }
            }

            return checkList;
        }

        public bool isPickListValid(IDictionary<Person, ExchangeCheckSum> checkList)
        {
            bool isValid = true;

            foreach (KeyValuePair<Person, ExchangeCheckSum> checkListItem in checkList)
            {
                if (!checkListItem.Value.isValid())
                {
                    isValid = false;
                    break;
                }
            }
            return isValid;
        }
    }
}
