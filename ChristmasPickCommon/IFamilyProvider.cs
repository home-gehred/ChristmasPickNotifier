using System;
using System.IO;
using System.Xml.Serialization;

namespace Common
{
  public interface IFamilyProvider
  {
    FamilyTree GetFamilies();
  }

  public class FileFamilyProvider : IFamilyProvider
  {
    private string mPath;

    public FileFamilyProvider(string path)
    {
      mPath = path;
    }

    public FamilyTree GetFamilies()
    {
      FileStream testData = new FileStream(mPath, FileMode.Open, FileAccess.Read);
      XmlSerializer xml = new XmlSerializer(typeof(FamilyTree));
      return (FamilyTree)xml.Deserialize(testData);
    }
  }

  public class HardcodedFamilyProvider : IFamilyProvider
  {

    public HardcodedFamilyProvider()
    {
    }

    public FamilyTree GetFamilies()
    {
      FamilyTree christmasPickList = new FamilyTree();

      PersonCollection milwaukeeGehredParents = new PersonCollection(new Person("Bob", "Gehred", new DateTime(1972, 7, 27), "21111111-2222-3333-4444-555555555555"), new Person("Angie", "Gehred", new DateTime(1971, 9, 26), "11111111-2222-3333-4444-555555555555"));
      PersonCollection milwaukeeGehredKids = new PersonCollection();
      milwaukeeGehredKids.Add(new Person("Maxwell", "Gehred", new DateTime(2001, 9, 30), "31111111-2222-3333-4444-555555555555"));
      milwaukeeGehredKids.Add(new Person("Charlotte", "Gehred", new DateTime(2005, 4, 21), "41111111-2222-3333-4444-555555555555"));
      Family milwaukeeGehreds = new Family("milwaukeeGehreds", milwaukeeGehredParents, milwaukeeGehredKids);

      PersonCollection tosaGehredParents = new PersonCollection(new Person("John", "Gehred", new DateTime(1961, 2, 16), "13111111-2222-3333-4444-555555555555"), new Person("Ann", "Gehred", new DateTime(1961, 5, 17), "12111111-2222-3333-4444-555555555555"));
      PersonCollection tosaGehredKids = new PersonCollection();
      tosaGehredKids.Add(new Person("Madeline", "Gehred", new DateTime(1994, 4, 12), "14111111-2222-3333-4444-555555555555"));
      tosaGehredKids.Add(new Person("Cecila", "Gehred", new DateTime(1997, 10, 12), "15111111-2222-3333-4444-555555555555"));
      Family tosaGehreds = new Family("tosaGehreds", tosaGehredParents, tosaGehredKids);

      christmasPickList.Add(milwaukeeGehreds);
      christmasPickList.Add(tosaGehreds);

      return christmasPickList;
    }

  }


}
