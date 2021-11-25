using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;

namespace Common.ChristmasPickList
{
    public class XMasPickList : IXmlSerializable, IEnumerable<XMasPick>
    {
        private IList<XMasPick> mPickList = new List<XMasPick>();
        private DateTime mChristmasDate = new DateTime(DateTime.MinValue.Year, 12, 25);

        public XMasPickList()
        {
        }

        public XMasPickList(int year)
        {
            mChristmasDate = new DateTime(year, 12, 25);
        }

        public XMasPickList(DateTime christmasYear)
        {
            mChristmasDate = new DateTime(christmasYear.Year, 12, 25);
        }

        public void Add(XMasPick pick)
        {
            mPickList.Add(pick);
        }

        public DateTime PickListDate
        {
            get
            {
                return mChristmasDate;
            }
        }

        public bool IsPersonPickedAlready(Person subject)
        {
            foreach (XMasPick pick in this.mPickList)
            {
                if (pick.Recipient == subject)
                {
                    return true;
                }
            }
            return false;
        }


        public Person GetRecipientFor(Person subject)
        {
            foreach (XMasPick pick in this.mPickList)
            {
                if (pick.Subject == subject)
                {
                    return pick.Recipient;
                }
            }
            throw new ArgumentException(string.Format("Unable to find subject person {0} in list", subject.ToString()));
        }

        public bool IsPickListForYear(DateTime date)
        {
            return (date == this.mChristmasDate);
        }

        public bool IsPickListEmpty()
        {
            return (this.mPickList.Count <= 0);
        }

        public bool HasSubjectBoughtAPresentForRecipient(Person subject, Person recipient)
        {
            bool hasBeenDone = false;
            foreach (XMasPick pick in this.mPickList)
            {
                XMasPick tmp = new XMasPick(subject, recipient);
                if (tmp == pick)
                {
                    hasBeenDone = true;
                    break;
                }
            }
            return hasBeenDone;
        }

        public override bool Equals(object o)
        {
            if (o.GetType() == typeof(XMasPickList))
                return XMasPickList.AreEquals(this, (XMasPickList)o);

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public XMasPick this[int i]
        {
            get
            {
                return mPickList[i];
            }
            set
            {
                mPickList[i] = value;
            }
        }

        protected static bool AreEquals(XMasPickList a, XMasPickList b)
        {
            bool areEqual = true;
            if (a.mChristmasDate == b.mChristmasDate)
            {
                if (a.mPickList.Count == b.mPickList.Count)
                {
                    for (int i = 0; i < a.mPickList.Count; i++)
                    {
                        if (a.mPickList[i] != b.mPickList[i])
                        {
                            areEqual = false;
                            break;
                        }
                    }
                }
                else
                {
                    areEqual = false;
                }
            }
            else
            {
                areEqual = false;
            }
            return areEqual;
        }

        public static bool operator ==(XMasPickList a, XMasPickList b)
        {
            return XMasPickList.AreEquals(a, b);
        }

        public static bool operator !=(XMasPickList a, XMasPickList b)
        {
            return !(XMasPickList.AreEquals(a, b));
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement();
            this.mChristmasDate = DateTime.Parse(reader.ReadElementString());
            reader.ReadStartElement();
            while (reader.IsStartElement("Pick"))
            {
                XMasPick tmp = new XMasPick();
                tmp.ReadXml(reader);
                this.mPickList.Add(tmp);
            }
            reader.ReadEndElement();
            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("XMasDate", this.mChristmasDate.ToString("MM/dd/yyyy"));
            writer.WriteStartElement("Picks");
            foreach (XMasPick pick in this.mPickList)
            {
                writer.WriteStartElement("Pick");
                pick.WriteXml(writer);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        public IEnumerator<XMasPick> GetEnumerator()
        {
            return mPickList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return mPickList.GetEnumerator();
        }
    }
}
