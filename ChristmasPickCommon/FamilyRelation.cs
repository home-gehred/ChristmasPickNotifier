using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
  public class FamilyRelation
  {
    public static FamilyRelation NoRelation = new FamilyRelation(0, "Not related");
    public static FamilyRelation Siblings = new FamilyRelation(1, "Siblings");
    public static FamilyRelation ParentChild = new FamilyRelation(2, "Parent Child");
    public static FamilyRelation Spouse = new FamilyRelation(3, "Spouse");
    
    private int mCode = 0;
    private string mName = "";

    private FamilyRelation(int code, string name)
    {
      mCode = code;
      mName = name;
    }

    public bool AreThisRelation(FamilyRelation relationship)
    {
      return (mCode == relationship.mCode);
    }
  }
}
