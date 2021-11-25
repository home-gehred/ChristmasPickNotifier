using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace Common.ChristmasPickList
{
  /// <summary>
  /// Deserialize Family File
  /// Create Kid List
  /// Match up kid to kid using the following rules
  ///  1. Siblings cannot pick each other
  ///  2. Cannot have the same person from the last five years
  ///  When all kids have a name list is complete.
  ///  Create adult List (21 or greater)
  ///  Match up name to name using following rules
  ///  1. Kids can't have parents (Nemo can't have Beth, Nick can't have Anne or Rick)
  ///  2. Cannot pick spouses
  ///  3. Siblings cannot pick each other
  ///  3. Cannot have the same person from the last five years.
  /// </summary>
  /// <remarks>
  /// The Xml Lay out of history should look like this...
  /// <![CDATA[
  ///   <ChristmasPickList>
  ///     <XMasDate>12/25/2008</XMasDate>
  ///     <XMasPick>
  ///       <Subject>
  ///         <Person />
  ///       </Subject>
  ///       <Recipient>
  ///         <Person />
  ///       </Recipient>
  ///     </XMasPick>
  ///     <XMasPick>
  ///       <Subject>
  ///         <Person />
  ///       </Subject>
  ///       <Recipient>
  ///         <Person />
  ///       </Recipient>
  ///     </XMasPick>
  ///   </ChristmasPickList>
  /// ]]>
  /// </remarks>
  [DebuggerDisplay("IsSet:{mIsSet} [{Subject} buys for {Recipient}]")]
  public class XMasPick : IXmlSerializable
  {
    private Person mSubject;
    private Person mRecipient = null;
    private bool mIsSet;

    public XMasPick()
    {
      mSubject = null;
      mRecipient = null;
      mIsSet = false;
    }

    public XMasPick(Person subject)
    {
      mSubject = subject;
      mIsSet = false;
    }

    public XMasPick(Person subject, Person recipient)
    {
      if (subject == recipient)
        throw new ArgumentException(string.Format("Cannot create an xmas pick with 2 of the same people."));

      mSubject = subject;
      mRecipient = recipient;
      mIsSet = true;
    }

    public Person Subject
    {
      get
      {
        return mSubject;
      }
    }

    public Person Recipient
    {
      get
      {
        return mRecipient;
      }
    }

    public void BuyPresentFor(Person recipient)
    {
      if (mSubject == recipient)
        throw new ArgumentException(string.Format("Recipient cannot equal subject"));
      mRecipient = recipient;
      mIsSet = true;
    }

    public bool IsPickSet
    {
      get
      {
        return mIsSet;
      }
    }

    public override bool Equals(object o)
    {
      if (o.GetType() == typeof(XMasPick))
        return XMasPick.AreEquals(this, (XMasPick)o);

      return false;
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }

    protected static bool AreEquals(XMasPick a, XMasPick b)
    {
      bool areEqual = (a.mSubject == b.mSubject);
      if (areEqual)
      {
        areEqual = (a.mRecipient == b.mRecipient) && (a.mIsSet == b.mIsSet);
      }
      return areEqual;
    }

    public static bool operator ==(XMasPick a, XMasPick b)
    {
      return XMasPick.AreEquals(a, b);
    }

    public static bool operator !=(XMasPick a, XMasPick b)
    {
      return !(XMasPick.AreEquals(a, b));
    }

    public System.Xml.Schema.XmlSchema GetSchema()
    {
      throw new NotImplementedException();
    }

    public void ReadXml(XmlReader reader)
    {
      reader.Read();

      Person tmp = new Person();
      tmp.ReadXml(reader);
      this.mSubject = tmp;
      tmp = new Person();
      tmp.ReadXml(reader);
      this.mRecipient = tmp;
      this.mIsSet = true;
      reader.ReadEndElement();
    }

    public void WriteXml(XmlWriter writer)
    {
      writer.WriteStartElement("Subject");
      this.mSubject.WriteXml(writer);
      writer.WriteEndElement();
      writer.WriteStartElement("Recipient");
      this.mRecipient.WriteXml(writer);
      writer.WriteEndElement();
    }
  }
}
