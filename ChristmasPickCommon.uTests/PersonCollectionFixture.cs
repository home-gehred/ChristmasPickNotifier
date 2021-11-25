using System;
using System.IO;
using System.Xml.Serialization;
using Xunit;

namespace Common.Test
{

  public class PersonCollectionFixture : BaseFixture
  {
    [Fact]
    public void TestPersonCollectionSerialization()
    {
      PersonCollection test = new PersonCollection(new Person("Bob", "Gehred", new DateTime(1972, 7, 27), "21111111-2222-3333-4444-555555555555"), new Person("Angie", "Gehred", new DateTime(1971, 9, 26), "11111111-2222-3333-4444-555555555555"));
      Stream myStream = new BufferedStream(new MemoryStream(new byte[1024], true), 1024);
      if (myStream != null)
      {
        XmlSerializer xml = new XmlSerializer(typeof(PersonCollection));
        xml.Serialize(myStream, test);
      }

      long length = myStream.Position;
      myStream.Position = 0;
      StreamReader xmlData = new StreamReader(myStream);
      string actual = xmlData.ReadToEnd();
      actual = actual.Substring(0, (int)length);
      Assert.Equal("<?xml version=\"1.0\"?>\n<PersonCollection>\n  <Person firstname=\"Bob\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"21111111-2222-3333-4444-555555555555\" />\n  <Person firstname=\"Angie\" lastname=\"Gehred\" birthday=\"9/26/1971\" id=\"11111111-2222-3333-4444-555555555555\" />\n</PersonCollection>", actual);

    }

    [Fact]
    public void TestPersonCollectionDeserialization()
    {
      PersonCollection expected = new PersonCollection(new Person("Bob", "Gehred", new DateTime(1972, 7, 27), "21111111-2222-3333-4444-555555555555"), new Person("Angie", "Gehred", new DateTime(1971, 9, 26), "11111111-2222-3333-4444-555555555555"));
      PersonCollection actual = null;
      byte[] serializedData = ConvertStringToByteArray("<?xml version=\"1.0\"?>\n<PersonCollection>\n  <Person firstname=\"Bob\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"21111111-2222-3333-4444-555555555555\" />\n  <Person firstname=\"Angie\" lastname=\"Gehred\" birthday=\"9/26/1971\" id=\"11111111-2222-3333-4444-555555555555\" />\n</PersonCollection>");
      Stream testData = new MemoryStream(serializedData);
      XmlSerializer xml = new XmlSerializer(typeof(PersonCollection));
      actual = (PersonCollection)xml.Deserialize(testData);
      Assert.Equal(expected, actual);    
    }

    [Fact]
    public void TestPersonCollectionAreEqual()
    {
      PersonCollection listA = new PersonCollection(new Person("Bob", "Gehred", new DateTime(1972, 7, 27), "21111111-2222-3333-4444-555555555555"), new Person("Angie", "Gehred", new DateTime(1971, 9, 26), "11111111-2222-3333-4444-555555555555"));
      PersonCollection listB = new PersonCollection(new Person("Bob", "Gehred", new DateTime(1972, 7, 27), "21111111-2222-3333-4444-555555555555"), new Person("Angie", "Gehred", new DateTime(1971, 9, 26), "11111111-2222-3333-4444-555555555555"));
      bool actual = (listA == listB);
      Assert.True(actual);

    }

    [Fact]
    public void TestPersonCollectionOfDifferentOrderWithSamePeopleAreEqual()
    {
      PersonCollection listA = new PersonCollection(new Person("Bob", "Gehred", new DateTime(1972, 7, 27), "21111111-2222-3333-4444-555555555555"), new Person("Angie", "Gehred", new DateTime(1971, 9, 26), "11111111-2222-3333-4444-555555555555"));
      PersonCollection listB = new PersonCollection(new Person("Angie", "Gehred", new DateTime(1971, 9, 26), "11111111-2222-3333-4444-555555555555"), new Person("Bob", "Gehred", new DateTime(1972, 7, 27), "21111111-2222-3333-4444-555555555555"));
      bool actual = (listA != listB);
      Assert.False(actual);

    }

    [Fact]
    public void TestPersonCollectionShouldBeEqualEvenThoughNamesAreInDifferentOrder()
    {
      DateTime sameDate = new DateTime(1972, 7, 27);
      PersonCollection listA = new PersonCollection(new Person("Bob", "Gehred", sameDate, "21111111-2222-3333-4444-555555555555"), new Person("Angie", "Gehred", sameDate, "11111111-2222-3333-4444-555555555555"));
      PersonCollection listB = new PersonCollection(new Person("Angie", "Gehred", sameDate, "11111111-2222-3333-4444-555555555555"), new Person("Bob", "Gehred", sameDate, "21111111-2222-3333-4444-555555555555"));
      bool actual = (listA == listB);
      Assert.True(actual);

    }

    [Fact]
    public void SortPersonCollectionByOldestToYoungest()
    {
      PersonCollection testList = new PersonCollection();

      Person MaxG = new Person("Max", "Gehred", new DateTime(2001, 9, 30), "31111111-2222-3333-4444-555555555555");
      Person CharlotteG = new Person("Charlotte", "Gehred", new DateTime(2005, 4, 21), "41111111-2222-3333-4444-555555555555");
      Person MadelineG = new Person("Madeline", "Gehred", new DateTime(1988, 4, 15), "14111111-2222-3333-4444-555555555555");
      Person CecilaG = new Person("Cecila", "Gehred", new DateTime(1990, 6, 21), "15111111-2222-3333-4444-555555555555");
      testList.Add(MaxG);
      testList.Add(CharlotteG);
      testList.Add(MadelineG);
      testList.Add(CecilaG);

      testList.Sort(new PersonComparerByAgeOldestToYoungest());

      Assert.True((testList.GetAt(0) == MadelineG));
      Assert.True((testList.GetAt(1) == CecilaG));
      Assert.True((testList.GetAt(2) == MaxG));
      Assert.True((testList.GetAt(3) == CharlotteG));
    }

    [Fact]
    public void SortPersonCollectionByYoungestToOldest()
    {
      PersonCollection testList = new PersonCollection();

      Person MaxG = new Person("Max", "Gehred", new DateTime(2001, 9, 30), "31111111-2222-3333-4444-555555555555");
      Person CharlotteG = new Person("Charlotte", "Gehred", new DateTime(2005, 4, 21), "41111111-2222-3333-4444-555555555555");
      Person MadelineG = new Person("Madeline", "Gehred", new DateTime(1988, 4, 15), "14111111-2222-3333-4444-555555555555");
      Person CecilaG = new Person("Cecila", "Gehred", new DateTime(1990, 6, 21), "15111111-2222-3333-4444-555555555555");
      testList.Add(MaxG);
      testList.Add(CharlotteG);
      testList.Add(MadelineG);
      testList.Add(CecilaG);

      testList.Sort(new PersonComparerByAgeYoungestToOldest());

      Assert.True((testList.GetAt(3) == MadelineG));
      Assert.True((testList.GetAt(2) == CecilaG));
      Assert.True((testList.GetAt(1) == MaxG));
      Assert.True((testList.GetAt(0) == CharlotteG));
    }

  }
}
