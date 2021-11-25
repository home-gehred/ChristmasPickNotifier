using System;
using Xunit;
using System.IO;
using System.Xml.Serialization;

namespace Common.Test
{

  public class PersonFixture : BaseFixture
  {
    public PersonFixture()
    {
    }

    [Fact]
    public void ReturnTheNumberOfYears()
    {
      DateTime bDay = new DateTime(1972, 7, 27);
      Person Bob = new Person("Bob", "Gehred", bDay, "21111111-2222-3333-4444-555555555555");
      DateTime testTime = new DateTime(2008, 7, 27);
      Age myAge = Bob.YearsOld(testTime);
      int expected = 36;
      Assert.Equal(expected, myAge.Year);
      Assert.Equal(0, myAge.Month);
      Assert.Equal(0, myAge.Day);
    }

    [Fact]
    public void TestPersonSerialization()
    {
        Person Bob = new Person("Bob", "Gehred", new DateTime(1972, 7, 27), "21111111-2222-3333-4444-555555555555");
      Stream myStream = new BufferedStream(new MemoryStream(new byte[1024], true), 1024);
      if (myStream != null)
      {
        XmlSerializer xml = new XmlSerializer(typeof(Person));
        xml.Serialize(myStream, Bob);
      }

      long length = myStream.Position;
      myStream.Position = 0;
      StreamReader junk = new StreamReader(myStream);
      string actual = junk.ReadToEnd();
      actual = actual.Substring(0, (int)length);
      Assert.Equal("<?xml version=\"1.0\"?>\n<Person firstname=\"Bob\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"21111111-2222-3333-4444-555555555555\" />", actual);

    }

    [Fact]
    public void TestPersonDeserialization()
    {

      Person angie = new Person("Angie", "Gehred", new DateTime(1971, 9, 26), "11111111-2222-3333-4444-555555555555");
      Person actual = null;
      byte[] serializedPerson = ConvertStringToByteArray("<?xml version=\"1.0\"?>\n<Person firstname=\"Angie\" lastname=\"Gehred\" birthday=\"9/26/1971\" id=\"11111111-2222-3333-4444-555555555555\" />");
      Stream testData = new MemoryStream(serializedPerson);
      XmlSerializer xml = new XmlSerializer(typeof(Person));
      actual = (Person)xml.Deserialize(testData);
      Assert.Equal(angie, actual);    
    }

    [Fact]
    public void ShouldBeEquals()
    {
        Person personA = new Person("Bill", "Close", new DateTime(1990, 1, 1), "91111111-2222-3333-4444-555555555555");
        Person personB = new Person("Bill", "Close", new DateTime(1990, 1, 1), "91111111-2222-3333-4444-555555555555");
      Assert.True(personA == personB);
    }

    [Fact]
    public void ShouldNotBeEquals()
    {
        Person personA = new Person("Bill", "Close", new DateTime(1990, 1, 1), "91111111-2222-3333-4444-555555555555");
        Person personB = new Person("Jed", "Close", new DateTime(1990, 1, 1), "81111111-2222-3333-4444-555555555555");
      Assert.False(personA == personB);
    }

    [Fact]
    public void ComparingTwoPeopleShouldReturnNegativeOne()
    {
        Person personA = new Person("Nate", "Close", new DateTime(1990, 1, 1), "21111111-2222-3333-4444-555555555555");
        Person personB = new Person("Nate", "Albertson", new DateTime(1990, 1, 1), "31111111-2222-3333-4444-555555555555");
      Assert.Equal(1, personA.CompareTo(personB));
    }

    [Fact]
    public void PersonIsNotConsideredAChildBecauseAgeIsGreaterThenTwentyOneYearsOld()
    {
        Person personA = new Person("Rita", "Botkin", new DateTime(1932, 12, 23), "61111111-2222-3333-4444-555555555555");
      Assert.False(personA.IsConsideredAChild(DateTime.Now));
    }

    [Fact]
    public void PersonIsConsideredAChildBecauseAgeIsExactlyEqualToTwentyOneYearsOld()
    {
        Person personA = new Person("Rita", "Botkin", new DateTime(1932, 12, 23), "61111111-2222-3333-4444-555555555555");
      Assert.True(personA.IsConsideredAChild(new DateTime(1953, 12, 23)));
    }

    [Fact]
    public void PersonIsConsideredAChildBecauseAgeIsLessThenOneYearOld()
    {
        Person personA = new Person("Rita", "Botkin", new DateTime(1932, 12, 23), "61111111-2222-3333-4444-555555555555");
      Assert.True(personA.IsConsideredAChild(new DateTime(1932, 12, 25)));
    }

  }
}
