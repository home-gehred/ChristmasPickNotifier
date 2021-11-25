using System;
using System.Text;
using System.Collections.Generic;
using Xunit;
using Common;
using System.IO;
using System.Xml.Serialization;
using Common.ChristmasPickList;


namespace Common.Test.ChristmasPickList
{
  public class XMasPickFixture : BaseFixture
  {
    [Fact]
    public void TestChristmaPickSerialization()
    {
        Person Bob = new Person("Bob", "Gehred", new DateTime(1972, 7, 27), "21111111-2222-3333-4444-555555555555");
        Person Angie = new Person("Angie", "Gehred", new DateTime(1971, 9, 26), "11111111-2222-3333-4444-555555555555");

      XMasPick testPick = new XMasPick(Bob, Angie);

      Stream myStream = new BufferedStream(new MemoryStream(new byte[1024], true), 1024);
      if (myStream != null)
      {
        XmlSerializer xml = new XmlSerializer(typeof(XMasPick));
        xml.Serialize(myStream, testPick);
      }

      long length = myStream.Position;
      myStream.Position = 0;
      StreamReader rawData = new StreamReader(myStream);
      string actual = rawData.ReadToEnd();
      actual = actual.Substring(0, (int)length);
      Assert.Equal("<?xml version=\"1.0\"?>\n<XMasPick>\n  <Subject firstname=\"Bob\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"21111111-2222-3333-4444-555555555555\" />\n  <Recipient firstname=\"Angie\" lastname=\"Gehred\" birthday=\"9/26/1971\" id=\"11111111-2222-3333-4444-555555555555\" />\n</XMasPick>", actual);

    }

    [Fact]
    public void TestChristmasPickDeserialization()
    {
        Person Bob = new Person("Bob", "Gehred", new DateTime(1972, 7, 27), "21111111-2222-3333-4444-555555555555");
        Person Angie = new Person("Angie", "Gehred", new DateTime(1971, 9, 26), "11111111-2222-3333-4444-555555555555");

      XMasPick expectedPick = new XMasPick(Bob, Angie);
      XMasPick actual = null;
      byte[] serializedPickItem = ConvertStringToByteArray("<?xml version=\"1.0\"?>\n<XMasPick>\n  <Subject firstname=\"Bob\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"21111111-2222-3333-4444-555555555555\" />\n  <Recipient firstname=\"Angie\" lastname=\"Gehred\" birthday=\"9/26/1971\" id=\"11111111-2222-3333-4444-555555555555\" />\n</XMasPick>");
      Stream testData = new MemoryStream(serializedPickItem);
      XmlSerializer xml = new XmlSerializer(typeof(XMasPick));
      actual = (XMasPick)xml.Deserialize(testData);
      Assert.Equal(expectedPick, actual);
    }


  }
}
