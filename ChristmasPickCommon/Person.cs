using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Common
{
  public class Person : IXmlSerializable, IComparable<Person>
  {
    private string mFirstName;
    private string mLastName;
    private DateTime mBirthDay;
    private Guid mId;
    //private string mNamespace = "http://schemaprovider.gehred.org/Person.xsd";

    public Person()
    {
      mBirthDay = DateTime.MinValue;
    }

    public Person(string firstName, string lastName, DateTime birthday, string guid)
    {
      mFirstName = firstName;
      mLastName = lastName;
      mBirthDay = birthday;
      mId = Guid.ParseExact(guid, "D");
    }

    public string FirstName
    {
        get
        {
            return mFirstName;
        }
    }
    public string LastName
    {
        get
        {
            return mLastName;
        }
    }

        public DateTime BirthDay
        {
            get
            {
                return mBirthDay;
            }
        }

    public bool IsConsideredAnAdult(DateTime dateToEvaluate)
    {
      Age personsAge = Age.CalculateAge(dateToEvaluate, mBirthDay);
      Age twentyOneYearOld = new Age(21, 0, 0);
      return (personsAge > twentyOneYearOld);
    }

    public bool IsConsideredAChild(DateTime dateToEvaluate)
    {
      Age personsAge = Age.CalculateAge(dateToEvaluate, mBirthDay);
      Age twentyOneYearOld = new Age(21, 0, 0);
      return (personsAge <= twentyOneYearOld);
    }

    public Age YearsOld(DateTime now)
    {
      return Age.CalculateAge(now, mBirthDay);
    }

    public override string ToString()
    {
      return string.Format("{0} {1}", this.mFirstName, this.mLastName);
    }

    public override bool Equals(object o)
    {
      if (o.GetType() == typeof(Person))
       return Person.AreEquals(this, (Person) o);

      return false;
    }

    public override int GetHashCode()
    {
      return mId.GetHashCode();
    }

    protected static bool AreEquals(Person a, Person b)
    {
            bool areEqual = false;
            if ( (a == null) || (b == null)) {
              return false;
            }
            if (a.mId == b.mId)
            {
                areEqual = true;
            }            //if (string.Compare(a.mFirstName, b.mFirstName) == 0)
            //{
            //  if (string.Compare(a.mLastName, b.mLastName) == 0)
            //  {
            //    if (a.mBirthDay == b.mBirthDay)
            //    {
            //      areEqual = true;
            //    }
            //  }
            //}

            return areEqual;
    }

    public static bool operator==(Person a, Person b)
    {
      if (object.ReferenceEquals(a, null) &&
          object.ReferenceEquals(b, null))
            {
                return true;
            }
      else
            {
                if (object.ReferenceEquals(a, null) ||
                object.ReferenceEquals(b, null))
                {
                    return false;
                }
            }
      return Person.AreEquals(a, b);
    }

    public static bool operator !=(Person a, Person b)
    {
      return !Person.AreEquals(a, b);
    }

    public int CompareTo(Person other)
    {
      // If this < other then return less then zero
      // If this == other then return zero
      // If this > other then return greater then zero
      if (this.mBirthDay <= other.mBirthDay)
      {
        if (this.mBirthDay == other.mBirthDay)
        {
          int compareLastName = string.Compare(this.mLastName, other.mLastName);
          if (compareLastName == 0)
          {
            return string.Compare(this.mFirstName, other.mFirstName);
          }
          return compareLastName;
        }
        else
        {
          return 1;
        }
      }
      else
      {
        // Oldest people go first right away.
        return -1;
      }
    }

    /// <summary>
    /// Per the documentation I read at http://msdn.microsoft.com/en-us/magazine/cc300797.aspx
    /// </summary>
    /// <returns></returns>
    public System.Xml.Schema.XmlSchema GetSchema()
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Per the documentation I read at http://msdn.microsoft.com/en-us/magazine/cc300797.aspx
    /// </summary>
    /// <returns></returns>
    public void ReadXml(System.Xml.XmlReader reader)
    {
      for (int currentAttribute = 0; currentAttribute < reader.AttributeCount; currentAttribute++)
      {
        reader.MoveToAttribute(currentAttribute);
        if (string.Compare(reader.Name, "firstname", true) == 0)
        {
          mFirstName = reader.Value;
        }

        if (string.Compare(reader.Name, "lastname", true) == 0)
        {
          mLastName = reader.Value;
        }

        if (string.Compare(reader.Name, "birthday", true) == 0)
        {
          mBirthDay = DateTime.Parse(reader.Value);
        }

        if (string.Compare(reader.Name, "id", true) == 0)
        {
          mId = Guid.ParseExact(reader.Value, "D");
        }
      }
      reader.Read();
    }

    /// <summary>
    /// Per the documentation I read at http://msdn.microsoft.com/en-us/magazine/cc300797.aspx
    /// </summary>
    /// <returns></returns>
    public void WriteXml(System.Xml.XmlWriter writer)
    { 
      //writer.WriteStartElement("Person", mNamespace);
      writer.WriteAttributeString("firstname", this.mFirstName);
      writer.WriteAttributeString("lastname", this.mLastName);
      writer.WriteAttributeString("birthday", this.mBirthDay.ToString("M/d/yyyy"));
      writer.WriteAttributeString("id", this.mId.ToString("D"));
      //writer.WriteEndElement();
    }
  }
}
