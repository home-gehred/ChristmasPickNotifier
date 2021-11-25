using System;
using System.Collections.Generic;
using Xunit;
using Common.ChristmasPickList;

namespace Common.Test.ChristmasPickList
{
  public class SequentialNumberGenerator : INumberGenerator
  {
    private int mListSize;
    private int mNext = -1;
    public SequentialNumberGenerator(int sizeOfList)
    {
      mListSize = sizeOfList;
    }

    public int GenerateNumber()
    {
      mNext++;
      if (mNext >= mListSize)
        mNext = 0;
      return mNext;
    }

      public int GenerateNumberBetweenZeroAnd(int max)
      {
          if (mNext < max)
          {
              mNext++;
          }
          else
          {
              return max;
          }
          return mNext;
      }
  }

  public class AlwaysTrueRule : IPickListRule
  {
    public bool IsPickValidForSubject(Person subject, Person toBuyPresentFor)
    {
      return true;
    }
  }
 
  public class TestRuleProvider : IPickListRuleProvider
  {
    public IList<IPickListRule> GetRulesForPickList()
    {
      IList<IPickListRule> myRules = new List<IPickListRule>();
      myRules.Add(new AlwaysTrueRule());
      return myRules;
    }
  }

  public class PickListServiceFixture : BaseFixture
  {
    [Fact]
    public void TestChristmaPickServiceUsingTheSequentialNumberGeneratorWithOneRuleThatIsAlwaysPassingForOddChristmasYear()
    {
      PersonCollection pickList = new PersonCollection();

      Person MaxG = new Person("Max", "Gehred", new DateTime(2001, 9, 30), "31111111-2222-3333-4444-555555555555");
      Person CharlotteG = new Person("Charlotte", "Gehred", new DateTime(2005, 4, 21), "41111111-2222-3333-4444-555555555555");
      Person MadelineG = new Person("Madeline", "Gehred", new DateTime(1988, 4, 15), "14111111-2222-3333-4444-555555555555");
      Person CecilaG = new Person("Cecila", "Gehred", new DateTime(1990, 6, 21), "15111111-2222-3333-4444-555555555555");
      pickList.Add(MaxG);
      pickList.Add(CharlotteG);
      pickList.Add(MadelineG);
      pickList.Add(CecilaG);

      IPickListService testObj = new PickListServiceAdvanced(new SequentialNumberGenerator(pickList.Count), new TestRuleProvider(), pickList);

      XMasPickList actual = testObj.CreateChristmasPick(new DateTime(2007, 12, 25));

      XMasPick expectedFirstPick = new XMasPick(CharlotteG, MaxG);
      Assert.True((expectedFirstPick == actual[0]));

      XMasPick expectedSecondPick = new XMasPick(MaxG, CecilaG);
      Assert.True(expectedSecondPick == actual[1]);

      XMasPick expectedThirdPick = new XMasPick(CecilaG, MadelineG);
      Assert.True(expectedThirdPick == actual[2]);

      XMasPick expectedFourthPick = new XMasPick(MadelineG, CharlotteG);
      Assert.True(expectedFourthPick == actual[3]);

    }

    [Fact]
    public void TestChristmaPickServiceUsingTheSequentialNumberGeneratorWithOneRuleThatIsAlwaysPassingForEvenChristmasYear()
    {
      PersonCollection pickList = new PersonCollection();

      Person MaxG = new Person("Max", "Gehred", new DateTime(2001, 9, 30), "31111111-2222-3333-4444-555555555555");
      Person CharlotteG = new Person("Charlotte", "Gehred", new DateTime(2005, 4, 21), "41111111-2222-3333-4444-555555555555");
      Person MadelineG = new Person("Madeline", "Gehred", new DateTime(1988, 4, 15), "14111111-2222-3333-4444-555555555555");
      Person CecilaG = new Person("Cecila", "Gehred", new DateTime(1990, 6, 21), "15111111-2222-3333-4444-555555555555");
      pickList.Add(MaxG);
      pickList.Add(CharlotteG);
      pickList.Add(MadelineG);
      pickList.Add(CecilaG);

      IPickListService testObj = new PickListServiceAdvanced(new SequentialNumberGenerator(pickList.Count), new TestRuleProvider(), pickList);

      XMasPickList actual = testObj.CreateChristmasPick(new DateTime(2008, 12, 25));

      XMasPick expectedFirstPick = new XMasPick(MadelineG, CecilaG);
      Assert.True((expectedFirstPick == actual[0]));

      XMasPick expectedSecondPick = new XMasPick(CecilaG, MaxG );
      Assert.True(expectedSecondPick == actual[1]);

      XMasPick expectedThirdPick = new XMasPick(MaxG, CharlotteG);
      Assert.True(expectedThirdPick == actual[2]);

      XMasPick expectedFourthPick = new XMasPick(CharlotteG, MadelineG);
      Assert.True(expectedFourthPick == actual[3]);

    }

  }
}
