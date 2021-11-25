using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
  public class Age
  {
    private int mYear;
    private int mMonth;
    private int mDay;

    public Age(int year, int months, int day)
    {
      mYear = year;
      mMonth = months;
      mDay = day;
    }

    protected static bool IsValidDateTime(DateTime value)
    {
      return !((value == DateTime.MinValue) || (value == DateTime.MaxValue));
    }

    static public Age CalculateAge(DateTime now, DateTime bday)
    {
      if (Age.IsValidDateTime(now) == false)
      {
        throw new ArgumentException("Date time of minvalue or maxvalue is not allowed", "now");
      }

      if (Age.IsValidDateTime(bday) == false)
      {
        throw new ArgumentException("Date time of minvalue or maxvalue is not allowed", "bday");
      }

      if (now < bday)
      {
        throw new ArgumentException("Date now must be greater then bday", "now and bday");
      }


      /*TimeSpan age = now - bday;
      int day = 0;
      int year = System.Math.DivRem(age.Days, 365, out day);
      return new Age(year, day);*/

      int months = 0;
      int days = 0;

      int years = now.Year - bday.Year;
      bday = bday.AddYears(years);
      if (bday > now)
      {
        bday = bday.AddYears(-1);
        years -= 1;
      }

      do
      {
        bday = bday.AddMonths(1);
        months += 1;
      } while (bday <= now);
      months -= 1;
      bday = bday.AddMonths(-1);

      do
      {
        bday = bday.AddDays(1);
        days += 1;
      } while (bday <= now);
      days -= 1;
      return new Age(years, months, days);
    }

    /*
    Public Sub DateSpace(ByVal x As Date, ByVal y As Date, ByRef years As Integer, ByRef months As Integer, ByRef days As Integer)
        x = x.Date
        y = y.Date

        years = y.Year - x.Year
        x = x.AddYears(years)
        If x > y Then
            x = x.AddYears(-1)
            years -= 1
        End If

        months = 0
        Do
            x = x.AddMonths(1)
            months += 1
        Loop While x <= y

        months -= 1
        x = x.AddMonths(-1)

        days = 0
        Do
            x = x.AddDays(1)
            days += 1
        Loop While x <= y

        days -= 1
    End Sub
     * */


    public int Year
    {
      get
      {
        return mYear;
      }
    }

    public int Day
    {
      get
      {
        return mDay;
      }
    }

    public int Month
    {
      get
      {
        return mMonth;
      }
    }

    public static bool operator <=(Age valA, Age valB)
    {
      bool retVal = false;
      if (valA.mYear == valB.mYear)
      {
        if (valA.mMonth == valB.mMonth)
        {
          retVal = (valA.mDay <= valB.mDay);
        }
        else
        {
          retVal = (valA.mMonth <= valB.mMonth);
        }
      }
      else
      {
        retVal = (valA.mYear <= valB.mYear);
      }
      return retVal;
    }

    public static bool operator >=(Age valA, Age valB)
    {
      bool retVal = false;
      if (valA.mYear == valB.mYear)
      {
        if (valA.mMonth == valB.mMonth)
        {
          retVal = (valA.mDay >= valB.mDay);
        }
        else
        {
          retVal = (valA.mMonth >= valB.mMonth);
        }
      }
      else
      {
        retVal = (valA.mYear >= valB.mYear);
      }
      return retVal;
    }

    public static bool operator >(Age valA, Age valB)
    {
      bool retVal = false;
      if (valA.mYear == valB.mYear)
      {
        if (valA.mMonth == valB.mMonth)
        {
          retVal = (valA.mDay > valB.mDay);
        }
        else
        {
          retVal = (valA.mMonth > valB.mMonth);
        }
      }
      else
      {
        retVal = (valA.mYear > valB.mYear);
      }
      return retVal;
    }

    public static bool operator <(Age valA, Age valB)
    {
      bool retVal = false;
      if (valA.mYear == valB.mYear)
      {
        if (valA.mMonth == valB.mMonth)
        {
          retVal = (valA.mDay < valB.mDay);
        }
        else
        {
          retVal = (valA.mMonth < valB.mMonth);
        }
      }
      else
      {
        retVal = (valA.mYear < valB.mYear);
      }
      return retVal;
    }

  }
}
