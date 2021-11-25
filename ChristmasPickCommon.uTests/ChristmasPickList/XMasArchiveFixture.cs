using System;
using Xunit;
using System.IO;
using System.Xml.Serialization;
using Common.ChristmasPickList;

namespace Common.Test.ChristmasPickList
{
  public class XMasArchiveFixture : BaseFixture
  {
    protected XMasPickList CreateTestPickList(int year, string[] firstName)
    {
      XMasPickList testYear = new XMasPickList(year);

      for (int i = 0; i < firstName.GetLength(0); i++)
      {
        int index = i + 1;
        if (index >= firstName.GetLength(0))
          index = 0;

        Person subject = new Person(firstName[i], "Gehred", new DateTime(1972, 7, 27), string.Format("{0}5111111-2222-3333-4444-555555555555", i));
        Person recipient = new Person(firstName[index], "Gehred", new DateTime(1972, 7, 27), string.Format("5{0}111111-2222-3333-4444-555555555555", index));
        testYear.Add(new XMasPick(subject, recipient));
      }

      return testYear;
    }

    protected XMasArchive CreateXmasPast()
    {

      string[] firstName1 = new string[] { "Ann", "Clare", "Beth", "Meg" };
      string[] firstName2 = new string[] { "Clare", "Ann", "Beth", "Meg" };
      string[] firstName3 = new string[] { "Clare", "Beth", "Ann", "Meg" };
      string[] firstName4 = new string[] { "Clare", "Beth", "Meg", "Ann" };

      XMasArchive tmp = new XMasArchive();
      tmp.Add(1971, this.CreateTestPickList(1971, firstName1));
      tmp.Add(1972, this.CreateTestPickList(1972, firstName2));
      tmp.Add(1973, this.CreateTestPickList(1973, firstName3));
      tmp.Add(1974, this.CreateTestPickList(1974, firstName4));
      return tmp;
    }

    [Fact]
    public void TestChristmasArchiveSerialization()
    {
      XMasArchive testArchive = CreateXmasPast();

      Stream myStream = new BufferedStream(new MemoryStream(new byte[8192], true), 8192);
      if (myStream != null)
      {
        XmlSerializer xml = new XmlSerializer(typeof(XMasArchive));
        xml.Serialize(myStream, testArchive);
      }

      long length = myStream.Position;
      myStream.Position = 0;
      StreamReader rawData = new StreamReader(myStream);
      string actual = rawData.ReadToEnd();
      actual = actual.Substring(0, (int)length);
      Assert.Equal("<?xml version=\"1.0\"?>\n<XMasArchive>\n  <ChristmasPast>\n    <XMasDate>12/25/1971</XMasDate>\n    <Picks>\n      <Pick>\n        <Subject firstname=\"Ann\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"05111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Clare\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"51111111-2222-3333-4444-555555555555\" />\n      </Pick>\n      <Pick>\n        <Subject firstname=\"Clare\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"15111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Beth\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"52111111-2222-3333-4444-555555555555\" />\n      </Pick>\n      <Pick>\n        <Subject firstname=\"Beth\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"25111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Meg\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"53111111-2222-3333-4444-555555555555\" />\n      </Pick>\n      <Pick>\n        <Subject firstname=\"Meg\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"35111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Ann\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"50111111-2222-3333-4444-555555555555\" />\n      </Pick>\n    </Picks>\n  </ChristmasPast>\n  <ChristmasPast>\n    <XMasDate>12/25/1972</XMasDate>\n    <Picks>\n      <Pick>\n        <Subject firstname=\"Clare\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"05111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Ann\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"51111111-2222-3333-4444-555555555555\" />\n      </Pick>\n      <Pick>\n        <Subject firstname=\"Ann\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"15111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Beth\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"52111111-2222-3333-4444-555555555555\" />\n      </Pick>\n      <Pick>\n        <Subject firstname=\"Beth\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"25111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Meg\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"53111111-2222-3333-4444-555555555555\" />\n      </Pick>\n      <Pick>\n        <Subject firstname=\"Meg\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"35111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Clare\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"50111111-2222-3333-4444-555555555555\" />\n      </Pick>\n    </Picks>\n  </ChristmasPast>\n  <ChristmasPast>\n    <XMasDate>12/25/1973</XMasDate>\n    <Picks>\n      <Pick>\n        <Subject firstname=\"Clare\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"05111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Beth\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"51111111-2222-3333-4444-555555555555\" />\n      </Pick>\n      <Pick>\n        <Subject firstname=\"Beth\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"15111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Ann\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"52111111-2222-3333-4444-555555555555\" />\n      </Pick>\n      <Pick>\n        <Subject firstname=\"Ann\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"25111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Meg\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"53111111-2222-3333-4444-555555555555\" />\n      </Pick>\n      <Pick>\n        <Subject firstname=\"Meg\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"35111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Clare\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"50111111-2222-3333-4444-555555555555\" />\n      </Pick>\n    </Picks>\n  </ChristmasPast>\n  <ChristmasPast>\n    <XMasDate>12/25/1974</XMasDate>\n    <Picks>\n      <Pick>\n        <Subject firstname=\"Clare\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"05111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Beth\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"51111111-2222-3333-4444-555555555555\" />\n      </Pick>\n      <Pick>\n        <Subject firstname=\"Beth\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"15111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Meg\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"52111111-2222-3333-4444-555555555555\" />\n      </Pick>\n      <Pick>\n        <Subject firstname=\"Meg\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"25111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Ann\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"53111111-2222-3333-4444-555555555555\" />\n      </Pick>\n      <Pick>\n        <Subject firstname=\"Ann\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"35111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Clare\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"50111111-2222-3333-4444-555555555555\" />\n      </Pick>\n    </Picks>\n  </ChristmasPast>\n</XMasArchive>", actual);

    }

    [Fact]
    public void TestChristmasArchiveDeserialization()
    {
      XMasArchive expectedPick = this.CreateXmasPast();
      XMasArchive actual = null;
      byte[] serializedPickItem = ConvertStringToByteArray("<?xml version=\"1.0\"?>\n<XMasArchive>\n  <ChristmasPast>\n    <XMasDate>12/25/1971</XMasDate>\n    <Picks>\n      <Pick>\n        <Subject firstname=\"Ann\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"05111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Clare\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"51111111-2222-3333-4444-555555555555\" />\n      </Pick>\n      <Pick>\n        <Subject firstname=\"Clare\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"15111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Beth\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"52111111-2222-3333-4444-555555555555\" />\n      </Pick>\n      <Pick>\n        <Subject firstname=\"Beth\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"25111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Meg\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"53111111-2222-3333-4444-555555555555\" />\n      </Pick>\n      <Pick>\n        <Subject firstname=\"Meg\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"35111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Ann\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"50111111-2222-3333-4444-555555555555\" />\n      </Pick>\n    </Picks>\n  </ChristmasPast>\n  <ChristmasPast>\n    <XMasDate>12/25/1972</XMasDate>\n    <Picks>\n      <Pick>\n        <Subject firstname=\"Clare\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"05111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Ann\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"51111111-2222-3333-4444-555555555555\" />\n      </Pick>\n      <Pick>\n        <Subject firstname=\"Ann\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"15111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Beth\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"52111111-2222-3333-4444-555555555555\" />\n      </Pick>\n      <Pick>\n        <Subject firstname=\"Beth\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"25111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Meg\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"53111111-2222-3333-4444-555555555555\" />\n      </Pick>\n      <Pick>\n        <Subject firstname=\"Meg\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"35111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Clare\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"50111111-2222-3333-4444-555555555555\" />\n      </Pick>\n    </Picks>\n  </ChristmasPast>\n  <ChristmasPast>\n    <XMasDate>12/25/1973</XMasDate>\n    <Picks>\n      <Pick>\n        <Subject firstname=\"Clare\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"05111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Beth\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"51111111-2222-3333-4444-555555555555\" />\n      </Pick>\n      <Pick>\n        <Subject firstname=\"Beth\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"15111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Ann\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"52111111-2222-3333-4444-555555555555\" />\n      </Pick>\n      <Pick>\n        <Subject firstname=\"Ann\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"25111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Meg\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"53111111-2222-3333-4444-555555555555\" />\n      </Pick>\n      <Pick>\n        <Subject firstname=\"Meg\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"35111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Clare\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"50111111-2222-3333-4444-555555555555\" />\n      </Pick>\n    </Picks>\n  </ChristmasPast>\n  <ChristmasPast>\n    <XMasDate>12/25/1974</XMasDate>\n    <Picks>\n      <Pick>\n        <Subject firstname=\"Clare\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"05111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Beth\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"51111111-2222-3333-4444-555555555555\" />\n      </Pick>\n      <Pick>\n        <Subject firstname=\"Beth\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"15111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Meg\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"52111111-2222-3333-4444-555555555555\" />\n      </Pick>\n      <Pick>\n        <Subject firstname=\"Meg\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"25111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Ann\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"53111111-2222-3333-4444-555555555555\" />\n      </Pick>\n      <Pick>\n        <Subject firstname=\"Ann\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"35111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Clare\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"50111111-2222-3333-4444-555555555555\" />\n      </Pick>\n    </Picks>\n  </ChristmasPast>\n</XMasArchive>");
      Stream testData = new MemoryStream(serializedPickItem);
      XmlSerializer xml = new XmlSerializer(typeof(XMasArchive));
      actual = (XMasArchive)xml.Deserialize(testData);
      Assert.Equal(expectedPick, actual);
    }

    [Fact]
    public void ShouldReturnTheMostRecentYearThatAnnHadPickedClare()
    {
      XMasArchive testObj = this.CreateXmasPast();
      Person subject = new Person("Ann", "Gehred", new DateTime(1972, 7, 27), "05111111-2222-3333-4444-555555555555");
      Person recipient = new Person("Clare", "Gehred", new DateTime(1972, 7, 27), "51111111-2222-3333-4444-555555555555");
      DateTime xmasDate = DateTime.MaxValue;
      bool actual = testObj.HasSubjectPersonBoughtAPresentForRecipientInLast(60, subject, recipient, out xmasDate);
      Assert.Equal(new DateTime(1974, 12, 25), xmasDate);
      Assert.True(actual);
    }

    [Fact]
    public void ShouldThrowExceptionAddIsForAYearThatDoesNotMatchPickList()
    {
      XMasArchive testObj = this.CreateXmasPast();
      string[] firstName1 = new string[] { "Mike", "Paul", "John", "Jim" };
      XMasPickList Xmas95 = this.CreateTestPickList(1995, firstName1);
      Assert.Throws<ArgumentException>(() => {
        testObj.Add(1990, Xmas95);
      });
    }

    [Fact]
    public void ShouldOverwriteArchivedYearWithYearPassedInOnAdd()
    {
      XMasArchive testObj = this.CreateXmasPast();
      string[] firstName1 = new string[] { "Mike", "Paul", "John", "Jim" };
      XMasPickList Xmas74 = this.CreateTestPickList(1974, firstName1);
      testObj.Add(1974, Xmas74);

      Stream myStream = new BufferedStream(new MemoryStream(new byte[8192], true), 8192);
      if (myStream != null)
      {
        XmlSerializer xml = new XmlSerializer(typeof(XMasArchive));
        xml.Serialize(myStream, testObj);
      }

      long length = myStream.Position;
      myStream.Position = 0;
      StreamReader rawData = new StreamReader(myStream);
      string actual = rawData.ReadToEnd();
      actual = actual.Substring(0, (int)length);
      Assert.Equal("<?xml version=\"1.0\"?>\n<XMasArchive>\n  <ChristmasPast>\n    <XMasDate>12/25/1971</XMasDate>\n    <Picks>\n      <Pick>\n        <Subject firstname=\"Ann\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"05111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Clare\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"51111111-2222-3333-4444-555555555555\" />\n      </Pick>\n      <Pick>\n        <Subject firstname=\"Clare\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"15111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Beth\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"52111111-2222-3333-4444-555555555555\" />\n      </Pick>\n      <Pick>\n        <Subject firstname=\"Beth\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"25111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Meg\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"53111111-2222-3333-4444-555555555555\" />\n      </Pick>\n      <Pick>\n        <Subject firstname=\"Meg\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"35111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Ann\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"50111111-2222-3333-4444-555555555555\" />\n      </Pick>\n    </Picks>\n  </ChristmasPast>\n  <ChristmasPast>\n    <XMasDate>12/25/1972</XMasDate>\n    <Picks>\n      <Pick>\n        <Subject firstname=\"Clare\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"05111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Ann\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"51111111-2222-3333-4444-555555555555\" />\n      </Pick>\n      <Pick>\n        <Subject firstname=\"Ann\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"15111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Beth\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"52111111-2222-3333-4444-555555555555\" />\n      </Pick>\n      <Pick>\n        <Subject firstname=\"Beth\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"25111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Meg\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"53111111-2222-3333-4444-555555555555\" />\n      </Pick>\n      <Pick>\n        <Subject firstname=\"Meg\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"35111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Clare\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"50111111-2222-3333-4444-555555555555\" />\n      </Pick>\n    </Picks>\n  </ChristmasPast>\n  <ChristmasPast>\n    <XMasDate>12/25/1973</XMasDate>\n    <Picks>\n      <Pick>\n        <Subject firstname=\"Clare\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"05111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Beth\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"51111111-2222-3333-4444-555555555555\" />\n      </Pick>\n      <Pick>\n        <Subject firstname=\"Beth\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"15111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Ann\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"52111111-2222-3333-4444-555555555555\" />\n      </Pick>\n      <Pick>\n        <Subject firstname=\"Ann\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"25111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Meg\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"53111111-2222-3333-4444-555555555555\" />\n      </Pick>\n      <Pick>\n        <Subject firstname=\"Meg\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"35111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Clare\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"50111111-2222-3333-4444-555555555555\" />\n      </Pick>\n    </Picks>\n  </ChristmasPast>\n  <ChristmasPast>\n    <XMasDate>12/25/1974</XMasDate>\n    <Picks>\n      <Pick>\n        <Subject firstname=\"Mike\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"05111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Paul\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"51111111-2222-3333-4444-555555555555\" />\n      </Pick>\n      <Pick>\n        <Subject firstname=\"Paul\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"15111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"John\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"52111111-2222-3333-4444-555555555555\" />\n      </Pick>\n      <Pick>\n        <Subject firstname=\"John\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"25111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Jim\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"53111111-2222-3333-4444-555555555555\" />\n      </Pick>\n      <Pick>\n        <Subject firstname=\"Jim\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"35111111-2222-3333-4444-555555555555\" />\n        <Recipient firstname=\"Mike\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"50111111-2222-3333-4444-555555555555\" />\n      </Pick>\n    </Picks>\n  </ChristmasPast>\n</XMasArchive>", actual);
    }
    
  }
}
