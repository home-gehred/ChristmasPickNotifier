using System;
using System.Collections.Generic;

namespace Common.ChristmasPickList
{

    public interface IPickListService
    {
        XMasPickList CreateChristmasPick(DateTime evaluationDate);
    }

    public interface IPickListRule
    {
        bool IsPickValidForSubject(Person subject, Person toBuyPresentFor);
    }


    public class PickListServiceAdvanced : Common.ChristmasPickList.IPickListService
    {
        private readonly IPickListRuleProvider ruleProvider = null;
        private readonly PersonCollection familyList = null;
        private readonly INumberGenerator indexGenerator = null;
        private IList<IPickListRule> mRules = null;

        public PickListServiceAdvanced(INumberGenerator indexGenerator, IPickListRuleProvider rules, PersonCollection pickList)
        {
            ruleProvider = rules;
            familyList = pickList;
            this.indexGenerator = indexGenerator;
        }

        public XMasPickList CreateChristmasPick(DateTime evaluationDate)
        {
            int MaxAttemptsBeforeGivingUp = 75;
            PersonCollection alreadyPicked = new PersonCollection();
            XMasPickList thisYearPickList = new XMasPickList(evaluationDate);
            SortPickList(this.familyList, evaluationDate);

            for (int attempts = 1; attempts <= MaxAttemptsBeforeGivingUp; attempts++)
            {
                foreach (var subject in familyList)
                {
                    PersonCollection availableToBePicked = CreatePossiblePickList(subject, alreadyPicked);

                    if (availableToBePicked.Count == 0)
                    {
                        // Reset pick list creation info and try again.
                        alreadyPicked = new PersonCollection();
                        thisYearPickList = new XMasPickList(evaluationDate);
                        // throw new NotImplementedException("I have no idea how to handle this yet.");
                        Console.WriteLine("{0} had no available people to pick, attempt: {1} Reset lists and try again.", subject, attempts);
                        break;
                    }

                    if (availableToBePicked.Count > 1)
                    {
                        SortPickList(availableToBePicked, evaluationDate);
                        int tmpIndex = indexGenerator.GenerateNumberBetweenZeroAnd((availableToBePicked.Count-1));
                        Person toBuyPresentFor = availableToBePicked.GetAt(tmpIndex);
                        alreadyPicked.Add(toBuyPresentFor);
                        thisYearPickList.Add(new XMasPick(subject, toBuyPresentFor));

                    }
                    else
                    {
                        Person toBuyPresentFor = availableToBePicked.GetAt(0);
                        alreadyPicked.Add(toBuyPresentFor);
                        thisYearPickList.Add(new XMasPick(subject, toBuyPresentFor));
                    }
                }

                // Only verify if picklist has items
                if (thisYearPickList.IsPickListEmpty() == false)
                {
                    Console.WriteLine("Verifing attempt: {0} ...", attempts);

                    try
                    {
                        // Throws exceptions if a person is not recieving a present.
                        foreach (Person person in familyList)
                        {
                            Person recipient = thisYearPickList.GetRecipientFor(person);
                        }
                        Console.WriteLine("Successfully created pick list in {0} attempts.", attempts);
                        break;
                    }
                    catch (Exception err)
                    {
                        Console.WriteLine("Attempt {0} failed because {1}", attempts, err.ToString());
                    }
                }
            }

            return thisYearPickList;
        }

        private PersonCollection CreatePossiblePickList(Person subject, PersonCollection alreadyPicked)
        {
            PersonCollection possiblePeople = new PersonCollection();
            foreach (var person in familyList)
            {
                if (person != subject)
                {
                    if (!alreadyPicked.IsContained((person)))
                    {
                        if (Evaluate(subject, person))
                        {
                            possiblePeople.Add(person);
                        }
                    }
                }
            }
            return possiblePeople;
        }


        protected bool Evaluate(Person subject, Person toBuyFor)
        {
            bool isValid = true;

            if (mRules == null)
            {
                mRules = ruleProvider.GetRulesForPickList();
            }

            foreach (IPickListRule rule in mRules)
            {
                isValid = rule.IsPickValidForSubject(subject, toBuyFor);
                if (isValid == false)
                    break;
            }

            return isValid;
        }

        protected void SortPickList(PersonCollection picklist, DateTime evaluationDate)
        {
            if (IsOddYear(evaluationDate))
            {
                picklist.Sort(new PersonComparerByAgeYoungestToOldest());
            }
            else
            {
                picklist.Sort(new PersonComparerByAgeOldestToYoungest());
            }
        }

        protected bool IsOddYear(DateTime evaluationDate)
        {
            int remainder = 0;
            System.Math.DivRem(evaluationDate.Year, 2, out remainder);
            return (1 == remainder);
        }

    }
}
