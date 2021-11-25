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
  ///<?xml version=\"1.0\"?>
  ///<Family>
  ///  <FamilyName>Brew City Gehreds</FamilyName>
  ///  <Parents>
  ///    <Person firstname=\"Angie\" lastname=\"Gehred\" birthday=\"9/26/1971\" />
  ///    <Person firstname=\"Bob\" lastname=\"Gehred\" birthday=\"7/27/1972\" />
  ///  </Parents>
  ///  <Children>
  ///    <Person firstname=\"Max\" lastname=\"Gehred\" birthday=\"9/30/2001\" />
  ///    <Person firstname=\"Charlotte\" lastname=\"Gehred\" birthday=\"4/21/2005\" />
  ///  </Children>
  ///</Family>
  ///]]>
  /// </remarks>
  public class Family : IXmlSerializable, IEnumerable<Person>
  {
    private PersonCollection mParents;
    private PersonCollection mChildren;
    private PersonCollection mAllPeople;
    private string mName;
    public Family()
    {
      mName = "Null Family";
    }

    public Family(string FamilyName, PersonCollection parents, PersonCollection siblings)
    {
      mName = FamilyName;
      mParents = parents;
      mChildren = siblings;
      mAllPeople = new PersonCollection();
      foreach (Person dude in parents)
        mAllPeople.Add(dude);
      foreach (Person dude in mChildren)
        mAllPeople.Add(dude);
    }

    public string Name
    {
      get
      {
        return mName;
      }
    }

    public override bool Equals(object o)
    {
      if (o.GetType() == typeof(Family))
        return Family.AreEquals(this, (Family)o);

      return false;
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }

    protected static bool AreEquals(Family a, Family b)
    {
      bool areEqual = false;

      if (a.mParents == b.mParents)
      {
        if (a.mChildren == b.mChildren)
        {
          areEqual = (string.Compare(a.mName, b.mName, true) == 0);
        }
      }

      return areEqual;
    }

    public static bool operator ==(Family a, Family b)
    {
      return Family.AreEquals(a, b);
    }

    public static bool operator !=(Family a, Family b)
    {
      return !(Family.AreEquals(a, b));
    }

    public bool IsFamilyMember(Person someone)
    {
      bool isFamilyMember = false;
      foreach (Person familyMember in this.mAllPeople)
      {
        isFamilyMember = (someone == familyMember);
        if (isFamilyMember)
          break;
      }
      return isFamilyMember;
    }

    public FamilyRelation DetermineRelationship(Person a, Person b)
    {
      FamilyRelation relationship = FamilyRelation.NoRelation;
      bool isPersonAParent = false;
      bool isPersonBParent = false;
      bool isPersonAChild = false;
      bool isPersonBChild = false;
      if (a == b)
      {
        throw new ArgumentException("Both people are the same person");
      }

      foreach (Person parent in this.mParents)
      {
        if (parent == a)
          isPersonAParent = true;
        if (parent == b)
          isPersonBParent = true;
      }

      foreach (Person child in this.mChildren)
      {
        if (child == a)
          isPersonAChild = true;
        if (child == b)
          isPersonBChild = true;
      }

      if ((isPersonAParent == false) &&
         (isPersonBParent == false) &&
         (isPersonAChild == false) &&
         (isPersonBChild == false))
      {
        throw new ArgumentException("Neither person are in this family");
      }
      else
      {
        if ((isPersonAParent == true) &&
           (isPersonBParent == true) &&
           (isPersonAChild == true) &&
           (isPersonBChild == true))
        {
          throw new ArgumentException("Something doesn't make sense here!");
        }

        // At least one will be true
        if (((isPersonAParent == true) || (isPersonAChild == true)) && ((isPersonBParent == false) && (isPersonBChild == false)))
        {
          relationship = FamilyRelation.NoRelation;
        }

        if (((isPersonBParent == true) || (isPersonBChild == true)) && ((isPersonAParent == false) && (isPersonAChild == false)))
        {
          relationship = FamilyRelation.NoRelation;
        }

        if (((isPersonAParent == true) || (isPersonBParent == true)) && ((isPersonAChild == true) || (isPersonBChild == true)))
        {
          relationship = FamilyRelation.ParentChild;
        }

        if (((isPersonAParent == false) && (isPersonBParent == false)) && ((isPersonAChild == true) && (isPersonBChild == true)))
        {
          relationship = FamilyRelation.Siblings;
        }

        if (((isPersonAParent == true) || (isPersonBParent == true)) && ((isPersonAChild == false) && (isPersonBChild == false)))
        {
          relationship = FamilyRelation.Spouse;
        }

      }
      return relationship;
    }

    public IEnumerator<Person> GetEnumerator()
    {
      return mAllPeople.GetEnumerator();
    }


    IEnumerator IEnumerable.GetEnumerator()
    {
      return mAllPeople.GetEnumerator();
    }

    public System.Xml.Schema.XmlSchema GetSchema()
    {
      throw new NotImplementedException();
    }

    public void ReadXml(System.Xml.XmlReader reader)
    {
      reader.Read();
      this.mName = reader.ReadElementString("FamilyName");
      this.mParents = new PersonCollection();
      mParents.ReadXml(reader);
      this.mChildren = new PersonCollection();
      mChildren.ReadXml(reader);
      reader.ReadEndElement();
      mAllPeople = new PersonCollection();
      foreach (Person parentalUnit in this.mParents)
        this.mAllPeople.Add(parentalUnit);
      foreach (Person child in this.mChildren)
        this.mAllPeople.Add(child);

    }

    public void WriteXml(System.Xml.XmlWriter writer)
    {
      writer.WriteElementString("FamilyName", this.Name);
      writer.WriteStartElement("Parents");
      this.mParents.WriteXml(writer);
      writer.WriteEndElement();
      writer.WriteStartElement("Children");
      this.mChildren.WriteXml(writer);
      writer.WriteEndElement();
    }

  }
}
