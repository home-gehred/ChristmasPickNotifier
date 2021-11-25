using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace Common.ChristmasPickList
{
  public class XMasArchive : IXmlSerializable
  {
    private IDictionary<DateTime, XMasPickList> mArchive = new Dictionary<DateTime, XMasPickList>();

    public XMasArchive()
    {
    }

    public void Add(int year, XMasPickList pickList)
    {
      DateTime key = new DateTime(year, 12, 25);
      if (!pickList.IsPickListForYear(key))
      {
        throw new ArgumentException(string.Format("Year of {0} does not match pickList date", year), "year/pickList");
      }
      else
      {
        if (mArchive.ContainsKey(key))
        {
          mArchive.Remove(key);
        }
        mArchive.Add(key, pickList);
      }
    }

    public XMasPickList GetPickListForYear(DateTime christmasYear)
    {
      DateTime key = new DateTime(christmasYear.Year, 12, 25);
      if (mArchive.ContainsKey(key))
      {
        return mArchive[key];
      }
      throw new ArgumentException(string.Format("The archive does not contain a pick list for {0} year.", key.ToShortDateString()));
    }

    public bool HasSubjectPersonBoughtAPresentForRecipientInLast(int yearsBack, Person subject, Person recipient, out DateTime mostRecentYear)
    {
      bool hasBeenDone = false;
      mostRecentYear = DateTime.MinValue;
      int currentYear = DateTime.Now.Year;
      for (int year = 0; year < yearsBack; year++)
      {

        DateTime xmasPast = new DateTime((currentYear-year), 12, 25);
        if (this.mArchive.ContainsKey(xmasPast))
        {
          XMasPickList tmp = mArchive[xmasPast];
          hasBeenDone = tmp.HasSubjectBoughtAPresentForRecipient(subject, recipient);
          if (hasBeenDone)
          {
            mostRecentYear = xmasPast;
            break;
          }
        }
      }
      return hasBeenDone;
    }

    public override bool Equals(object o)
    {
      if (o.GetType() == typeof(XMasArchive))
        return XMasArchive.AreEquals(this, (XMasArchive)o);

      return false;
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }

    protected static bool AreEquals(XMasArchive a, XMasArchive b)
    {
      bool areEqual = true;
      if (a.mArchive.Keys.Count != b.mArchive.Keys.Count)
      {
        areEqual = false;
      }
      else
      {
        foreach (DateTime xmasDate in a.mArchive.Keys)
        {
          if (b.mArchive.ContainsKey(xmasDate) == false)
          {
            areEqual = false;
            break;
          }
          else
          {
            if (a.mArchive[xmasDate] != b.mArchive[xmasDate])
            {
              areEqual = false;
              break;
            }
          }
        }
      }
      return areEqual;
    }

    public static bool operator ==(XMasArchive a, XMasArchive b)
    {
      return XMasArchive.AreEquals(a, b);
    }

    public static bool operator !=(XMasArchive a, XMasArchive b)
    {
      return !(XMasArchive.AreEquals(a, b));
    }



    public System.Xml.Schema.XmlSchema GetSchema()
    {
      throw new NotImplementedException();
    }

    public void ReadXml(XmlReader reader)
    {
      while (reader.IsStartElement())
      {
        XMasPickList tmp = new XMasPickList();
        tmp.ReadXml(reader);
        mArchive.Add(tmp.PickListDate, tmp);
      }
      reader.ReadEndElement();
    }

    public void WriteXml(XmlWriter writer)
    {
      foreach (DateTime key in this.mArchive.Keys)
      {
        writer.WriteStartElement("ChristmasPast");
        mArchive[key].WriteXml(writer);
        writer.WriteEndElement();
      }
    }
  }
}
