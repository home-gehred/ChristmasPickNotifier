using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Common
{
  /// <summary>
  /// 
  /// </summary>
  /// <remarks>
  /// 
  ///<![CDATA[
  ///<?xml version="1.0"?>
  ///<FamilyTree>
  ///  <Family>
  ///    <FamilyName>Brew City Gehreds</FamilyName>
  ///    <Parents>
  ///      <Person firstname="Angie" lastname="Gehred" birthday="9/26/1971" />
  ///      <Person firstname="Bob" lastname="Gehred" birthday="7/27/1972" />
  ///    </Parents>
  ///    <Children>
  ///      <Person firstname="Max" lastname="Gehred" birthday="9/30/2001" />
  ///      <Person firstname="Charlotte" lastname="Gehred" birthday="4/21/2005" />
  ///    </Children>
  ///  </Family>
  ///  <Family>
  ///    <FamilyName>WaWa Tosa Gehreds</FamilyName>
  ///    <Parents>
  ///      <Person firstname="Ann" lastname="Gehred" birthday="9/26/1962" />
  ///      <Person firstname="John" lastname="Gehred" birthday="2/13/1962" />
  ///    </Parents>
  ///    <Children>
  ///      <Person firstname="Madeline" lastname="Gehred" birthday="4/15/1988" />
  ///      <Person firstname="Cecila" lastname="Gehred" birthday="6/21/1990" />
  ///    </Children>
  ///  </Family>
  ///</FamilyTree>
  ///]]>
  /// </remarks>
  public class FamilyTree : IXmlSerializable, IEnumerable<Family>
  {
    private SortedDictionary<string, Family> mFamilyList;

    public FamilyTree()
    {
      mFamilyList = new SortedDictionary<string, Family>();
    }

    public void Add(Family fambly)
    {
      mFamilyList.Add(fambly.Name, fambly);
    }

    public override bool Equals(object o)
    {
      if (o.GetType() == typeof(FamilyTree))
        return FamilyTree.AreEquals(this, (FamilyTree)o);

      return false;
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }

    protected static bool AreEquals(FamilyTree a, FamilyTree b)
    {
      bool areEqual = false;
      if (a.mFamilyList.Keys.Count == b.mFamilyList.Keys.Count)
      {
        areEqual = true;
        foreach (string akey in a.mFamilyList.Keys)
        {
          if (b.mFamilyList.ContainsKey(akey))
          {
            if (a.mFamilyList[akey] != b.mFamilyList[akey])
            {
              areEqual = false;
              break;
            }
          }
          else
          {
            areEqual = false;
            break;
          }
        }
      }
      return areEqual;
    }

    public static bool operator ==(FamilyTree a, FamilyTree b)
    {
      return FamilyTree.AreEquals(a, b);
    }

    public static bool operator !=(FamilyTree a, FamilyTree b)
    {
      return !(FamilyTree.AreEquals(a, b));
    }


    public PersonCollection CreateChristmasKidList(DateTime xMasTime)
    {
      PersonCollection kidList = new PersonCollection();
      foreach (string familyKey in this.mFamilyList.Keys)
      {
        Family current = mFamilyList[familyKey];
        foreach (Person member in current)
        {
          if (member.IsConsideredAChild(xMasTime))
            kidList.Add(member);
        }
      }
      return kidList;
    }

    public PersonCollection CreateChristmasAdultList(DateTime xMasTime)
    {
      PersonCollection adultList = new PersonCollection();
      foreach (string familyKey in this.mFamilyList.Keys)
      {
        Family current = mFamilyList[familyKey];
        foreach (Person member in current)
        {
          if (member.IsConsideredAnAdult(xMasTime))
            adultList.Add(member);
        }
      }
      return adultList;
    }

    public bool AreSiblings(Person personA, Person personB)
    {
      // What family is personA in?
      Family familyA = GetFamilyPersonBelongsTo(personA);
      // What family is personB in?
      Family familyB = GetFamilyPersonBelongsTo(personB);
      bool areSiblings = (familyA == familyB);
      // If they are the same we have to know if
      if (areSiblings)
      {
        FamilyRelation relationShip = familyA.DetermineRelationship(personA, personB);
        // personA is a parent, and personB is a child
        // or personB is a parent and personA is a parent
        areSiblings = relationShip.AreThisRelation(FamilyRelation.Siblings);
      }
      return areSiblings;
    }

    public bool AreParentChild(Person personA, Person personB)
    {
      // What family is personA in?
      Family familyA = GetFamilyPersonBelongsTo(personA);
      // What family is personB in?
      Family familyB = GetFamilyPersonBelongsTo(personB);
      bool areParentChild = (familyA == familyB);
      // If they are the same we have to know if
      if (areParentChild)
      {
        FamilyRelation relationShip = familyA.DetermineRelationship(personA, personB);
        // personA is a parent, and personB is a child
        // or personB is a parent and personA is a parent
        areParentChild = relationShip.AreThisRelation(FamilyRelation.ParentChild);
      }
      return areParentChild;

    }

    public bool AreSpouses(Person personA, Person personB)
    {
      // What family is personA in?
      Family familyA = GetFamilyPersonBelongsTo(personA);
      // What family is personB in?
      Family familyB = GetFamilyPersonBelongsTo(personB);
      bool areSpouses = (familyA == familyB);
      // If they are the same we have to know if
      if (areSpouses)
      {
        FamilyRelation relationShip = familyA.DetermineRelationship(personA, personB);
        // personA is a parent, and personB is a child
        // or personB is a parent and personA is a parent
        areSpouses = relationShip.AreThisRelation(FamilyRelation.Spouse);
      }
      return areSpouses;

    }


    protected Family GetFamilyPersonBelongsTo(Person person)
    {
      Family tmp = null;
      foreach (string familyname in this.mFamilyList.Keys)
      {
        tmp = mFamilyList[familyname];
        if (tmp.IsFamilyMember(person))
          break;
      }
      return tmp;
    }

    public System.Xml.Schema.XmlSchema GetSchema()
    {
      throw new NotImplementedException();
    }

    public void ReadXml(System.Xml.XmlReader reader)
    {
      reader.ReadStartElement();
      while (reader.IsStartElement("Family"))
      {
        Family tmp = new Family();
        tmp.ReadXml(reader);
        this.mFamilyList.Add(tmp.Name, tmp);
      }
      reader.ReadEndElement();

    }

    public void WriteXml(System.Xml.XmlWriter writer)
    {
      foreach (string key in this.mFamilyList.Keys)
      {
        writer.WriteStartElement("Family");
        mFamilyList[key].WriteXml(writer);
        writer.WriteEndElement();
      }
    }

    public IEnumerator<Family> GetEnumerator()
    {
      return mFamilyList.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return mFamilyList.Values.GetEnumerator();
    }
  }

}
