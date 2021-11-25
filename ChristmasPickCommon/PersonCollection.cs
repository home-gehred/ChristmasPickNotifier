using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Common
{
  public class PersonComparerByAgeYoungestToOldest : IComparer<Person>
  {
    int IComparer<Person>.Compare(Person x, Person y)
    {
      return x.CompareTo(y);      
    }
  }

  public class PersonComparerByAgeOldestToYoungest : IComparer<Person>
  {
    int IComparer<Person>.Compare(Person x, Person y)
    {
      return (-1 * x.CompareTo(y));
    }
  }

  [Serializable]
  public class PersonCollection : IXmlSerializable, IEnumerable<Person>
  {
    private List<Person> mList;

    public PersonCollection(Person father, Person mother)
    {
      mList = new List<Person>(2);
      mList.Add(father);
      mList.Add(mother);
    }

    public PersonCollection()
    {
      mList = new List<Person>();
    }

    public bool IsContained(Person person)
    {
        return mList.Any(inListPerson => inListPerson == person);
    }


    public void Add(Person item)
    {
      mList.Add(item);
    }

    public override bool Equals(object o)
    {
      if (o.GetType() == typeof(PersonCollection))
        return PersonCollection.AreEquals(this, (PersonCollection)o);

      return false;
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }

    public void Sort(IComparer<Person> comparer)
    {
      this.mList.Sort(comparer);
    }

    protected static bool AreEquals(PersonCollection a, PersonCollection b)
    {
      bool areEqual = false;
      
      if (a.mList.Count == b.mList.Count)
      {
        areEqual = true;
        a.mList.Sort(new PersonComparerByAgeYoungestToOldest());
        b.mList.Sort(new PersonComparerByAgeYoungestToOldest());
        for (int i = 0; i < a.mList.Count; i++)
        {
          if (a.mList[i] != b.mList[i])
          {
            areEqual = false;
            break;
          }
        }
      }
      return areEqual;
    }

    public int Count
    {
      get
      {
        return this.mList.Count;
      }
    }


    public static bool operator ==(PersonCollection a, PersonCollection b)
    {
      return PersonCollection.AreEquals(a, b);
    }

    public static bool operator !=(PersonCollection a, PersonCollection b)
    {
      return !PersonCollection.AreEquals(a, b);
    }

    public Person GetAt(int index)
    {
      if ((index < 0) || (index >= mList.Count))
        throw new ArgumentException(string.Format("{0} is not within array bounds 0 - {1}", index, mList.Count));
      return mList[index];
    }

    public IEnumerator<Person> GetEnumerator()
    {
      return mList.GetEnumerator();
    }


    IEnumerator IEnumerable.GetEnumerator()
    {
      return mList.GetEnumerator();
    }


    public System.Xml.Schema.XmlSchema GetSchema()
    {
      throw new NotImplementedException();
    }

    public void ReadXml(System.Xml.XmlReader reader)
    {
      if (reader.IsEmptyElement == false)
      {
        reader.Read();
        // Need to iterate through the collection
        // For each xml tag call person deserializer
        while (reader.IsStartElement("Person"))
        {
          Person tmp = new Person();
          tmp.ReadXml(reader);
          this.mList.Add(tmp);
        }
      }

      reader.ReadEndElement();
    }

    public void WriteXml(System.Xml.XmlWriter writer)
    {
      foreach (Person individual in this.mList)
      {
        writer.WriteStartElement("Person");
        individual.WriteXml(writer);
        writer.WriteEndElement();
      }
    }

  }
}
