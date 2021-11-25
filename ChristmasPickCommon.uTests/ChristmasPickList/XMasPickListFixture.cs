using System;
using Xunit;
using System.IO;
using System.Xml.Serialization;
using Common.ChristmasPickList;

namespace Common.Test.ChristmasPickList
{
  public class XMasPickListFixture : BaseFixture
  {

    protected XMasPickList CreateTestPickList()
    {
      XMasPickList testYear = new XMasPickList(2001);
      Person Dad = new Person("Tom", "Gehred", new DateTime(1936,3,5), "11111111-6666-3333-4444-555555555555");
      Person Mike = new Person("Mike", "Gehred", new DateTime(1954, 3, 18), "11111111-7777-3333-4444-555555555555");
      Person Jim = new Person("Jim", "Gehred", new DateTime(1963, 2, 13), "11111111-8888-3333-4444-555555555555");
      Person TonyI = new Person("Tony", "Ingrassia", new DateTime(1901, 3, 3), "11111111-9999-3333-4444-555555555555");

      testYear.Add(new XMasPick(Dad, Mike));
      testYear.Add(new XMasPick(Mike, Jim));
      testYear.Add(new XMasPick(Jim, TonyI));
      testYear.Add(new XMasPick(TonyI, Dad));

      return testYear;
    }

    [Fact]
    public void TestChristmaPickListSerialization()
    {
      XMasPickList testPickList = CreateTestPickList();

      Stream myStream = new BufferedStream(new MemoryStream(new byte[2048], true), 2048);
      if (myStream != null)
      {
        XmlSerializer xml = new XmlSerializer(typeof(XMasPickList));
        xml.Serialize(myStream, testPickList);
      }

      long length = myStream.Position;
      myStream.Position = 0;
      StreamReader rawData = new StreamReader(myStream);
      string actual = rawData.ReadToEnd();
      actual = actual.Substring(0, (int)length);
      Assert.Equal("<?xml version=\"1.0\"?>\n<XMasPickList>\n  <XMasDate>12/25/2001</XMasDate>\n  <Picks>\n    <Pick>\n      <Subject firstname=\"Tom\" lastname=\"Gehred\" birthday=\"3/5/1936\" id=\"11111111-6666-3333-4444-555555555555\" />\n      <Recipient firstname=\"Mike\" lastname=\"Gehred\" birthday=\"3/18/1954\" id=\"11111111-7777-3333-4444-555555555555\" />\n    </Pick>\n    <Pick>\n      <Subject firstname=\"Mike\" lastname=\"Gehred\" birthday=\"3/18/1954\" id=\"11111111-7777-3333-4444-555555555555\" />\n      <Recipient firstname=\"Jim\" lastname=\"Gehred\" birthday=\"2/13/1963\" id=\"11111111-8888-3333-4444-555555555555\" />\n    </Pick>\n    <Pick>\n      <Subject firstname=\"Jim\" lastname=\"Gehred\" birthday=\"2/13/1963\" id=\"11111111-8888-3333-4444-555555555555\" />\n      <Recipient firstname=\"Tony\" lastname=\"Ingrassia\" birthday=\"3/3/1901\" id=\"11111111-9999-3333-4444-555555555555\" />\n    </Pick>\n    <Pick>\n      <Subject firstname=\"Tony\" lastname=\"Ingrassia\" birthday=\"3/3/1901\" id=\"11111111-9999-3333-4444-555555555555\" />\n      <Recipient firstname=\"Tom\" lastname=\"Gehred\" birthday=\"3/5/1936\" id=\"11111111-6666-3333-4444-555555555555\" />\n    </Pick>\n  </Picks>\n</XMasPickList>", actual);

    }

    [Fact]
    public void TestChristmasPickListDeserialization()
    {
      XMasPickList expectedPick = this.CreateTestPickList();
      XMasPickList actual = null;
      byte[] serializedPickItem = ConvertStringToByteArray("<?xml version=\"1.0\"?>\r\n<XMasPickList>\r\n  <XMasDate>12/25/2001</XMasDate>\r\n  <Picks>\r\n    <Pick>\r\n      <Subject firstname=\"Tom\" lastname=\"Gehred\" birthday=\"3/5/1936\" id=\"11111111-6666-3333-4444-555555555555\" />\r\n      <Recipient firstname=\"Mike\" lastname=\"Gehred\" birthday=\"3/18/1954\" id=\"11111111-7777-3333-4444-555555555555\" />\r\n    </Pick>\r\n    <Pick>\r\n      <Subject firstname=\"Mike\" lastname=\"Gehred\" birthday=\"3/18/1954\" id=\"11111111-7777-3333-4444-555555555555\" />\r\n      <Recipient firstname=\"Jim\" lastname=\"Gehred\" birthday=\"2/13/1963\" id=\"11111111-8888-3333-4444-555555555555\" />\r\n    </Pick>\r\n    <Pick>\r\n      <Subject firstname=\"Jim\" lastname=\"Gehred\" birthday=\"2/13/1963\" id=\"11111111-8888-3333-4444-555555555555\" />\r\n      <Recipient firstname=\"Tony\" lastname=\"Ingrassia\" birthday=\"3/3/1901\" id=\"11111111-9999-3333-4444-555555555555\" />\r\n    </Pick>\r\n    <Pick>\r\n      <Subject firstname=\"Tony\" lastname=\"Ingrassia\" birthday=\"3/3/1901\" id=\"11111111-9999-3333-4444-555555555555\" />\r\n      <Recipient firstname=\"Tom\" lastname=\"Gehred\" birthday=\"3/5/1936\" id=\"11111111-6666-3333-4444-555555555555\" />\r\n    </Pick>\r\n  </Picks>\r\n</XMasPickList>");
      Stream testData = new MemoryStream(serializedPickItem);
      XmlSerializer xml = new XmlSerializer(typeof(XMasPickList));
      actual = (XMasPickList)xml.Deserialize(testData);
      Assert.Equal(expectedPick, actual);
    }

  }
}
