using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Common;
using System.IO;
using System.Xml.Serialization;
using Xunit;

namespace Common.Test
{

  public class AgeFixture
  {
    [Fact]
    public void ShouldThrowExceptionNowIsEqualToMinValue()
    {
      DateTime bday = new DateTime(1972, 7, 27);
      var actual = Assert.Throws<ArgumentException>(() => {
            Age.CalculateAge(DateTime.MinValue, bday);
      });
    }
    
    [Fact]
    public void ShouldThrowExceptionNowIsEqualToMaxValue()
    {
      DateTime bday = new DateTime(1972, 7, 27);
      var actual = Assert.Throws<ArgumentException>(() => {
        Age.CalculateAge(DateTime.MaxValue, bday);
      });
    }

    [Fact]
    public void ShouldThrowExceptionBDayIsEqualToMinValue()
    {
      DateTime bday = new DateTime(1972, 7, 27);
      DateTime now = new DateTime(2008,9,18);
      var actual = Assert.Throws<ArgumentException>(() => {
        Age.CalculateAge(now, DateTime.MinValue);
      });
    }

    [Fact]
    public void ShouldThrowExceptionBDayIsEqualToMaxValue()
    {
      DateTime bday = new DateTime(1972, 7, 27);
      DateTime now = new DateTime(2008, 9, 18);
      var actual = Assert.Throws<ArgumentException>(() => {
        Age.CalculateAge(now, DateTime.MaxValue);
      });
    }

    [Fact]
    public void ShouldThrowExceptionBDayIsMoreRecentThenNow()
    {
      DateTime bday = new DateTime(2008, 9, 18);
      DateTime now = new DateTime(1972, 7, 27);
      var actual = Assert.Throws<ArgumentException>(() => {
        Age.CalculateAge(now, bday);
      });
    }

    [Fact]
    public void BDayAndNowAreEqualYearAndDayShouldEqualZero()
    {
      DateTime bday = new DateTime(2008, 9, 18);
      DateTime now = bday;
      Age actual = Age.CalculateAge(now, bday);
      Assert.Equal(0, actual.Year);
      Assert.Equal(0, actual.Day);
    }

    [Fact]
    public void BDayAndNowAreLessThenOneYear()
    {
      DateTime bday = new DateTime(2008, 9, 18);
      DateTime now = new DateTime(2008, 9, 27); 
      Age actual = Age.CalculateAge(now, bday);
      Assert.Equal(0, actual.Year);
      Assert.Equal(9, actual.Day);
    }

    [Fact]
    public void ShouldReturnCurrentAgeIsLessThenOrEqualToTestAgeBecauseOfYears()
    {
      Age currentAge = new Age(5, 4, 12);
      Age testAge = new Age(6, 4, 12);
      Assert.True((currentAge <= testAge));
    }

    [Fact]
    public void ShouldReturnCurrentAgeIsLessThenOrEqualToTestAgeBecauseOfMonths()
    {
      Age currentAge = new Age(5, 4, 12);
      Age testAge = new Age(5, 8, 12);
      Assert.True((currentAge <= testAge));
    }

    [Fact]
    public void ShouldReturnCurrentAgeIsLessThenOrEqualToTestAgeBecauseOfDays()
    {
      Age currentAge = new Age(5, 4, 12);
      Age testAge = new Age(5, 4, 18);
      Assert.True((currentAge <= testAge));
    }

    [Fact]
    public void ShouldReturnCurrentAgeIsLessThenOrEqualToTestAgeBecauseTheyAreEqual()
    {
      Age currentAge = new Age(5, 4, 21);
      Age testAge = new Age(5, 4, 21);
      Assert.True((currentAge <= testAge));
    }

    [Fact]
    public void ShouldReturnCurrentAgeIsGreaterThenOrEqualToTestAgeBecauseOfYears()
    {
      Age currentAge = new Age(6, 4, 12);
      Age testAge = new Age(5, 4, 12);
      Assert.True((currentAge >= testAge));
    }

    [Fact]
    public void ShouldReturnCurrentAgeIsGreaterThenOrEqualToTestAgeBecauseOfMonths()
    {
      Age currentAge = new Age(5, 8, 12);
      Age testAge = new Age(5, 4, 12);
      Assert.True((currentAge >= testAge));
    }

    [Fact]
    public void ShouldReturnCurrentAgeIsGreaterThenOrEqualToTestAgeBecauseOfDays()
    {
      Age currentAge = new Age(5, 4, 18);
      Age testAge = new Age(5, 4, 12);
      Assert.True((currentAge >= testAge));
    }

    [Fact]
    public void ShouldReturnCurrentAgeIsGreaterThenOrEqualToTestAgeBecauseTheyAreEqual()
    {
      Age currentAge = new Age(5, 4, 21);
      Age testAge = new Age(5, 4, 21);
      Assert.True((currentAge >= testAge));
    }

    [Fact]
    public void ShouldReturnCurrentAgeIsGreaterThenTestAgeBecauseOfYears()
    {
      Age currentAge = new Age(6, 4, 12);
      Age testAge = new Age(5, 4, 12);
      Assert.True((currentAge > testAge));
    }

    [Fact]
    public void ShouldReturnCurrentAgeIsGreaterThenTestAgeBecauseOfMonths()
    {
      Age currentAge = new Age(5, 8, 12);
      Age testAge = new Age(5, 4, 12);
      Assert.True((currentAge > testAge));
    }

    [Fact]
    public void ShouldReturnCurrentAgeIsGreaterThenTestAgeBecauseOfDays()
    {
      Age currentAge = new Age(5, 4, 18);
      Age testAge = new Age(5, 4, 12);
      Assert.True((currentAge > testAge));
    }

    [Fact]
    public void ShouldReturnFalseCurrentAgeIsGreaterThenTestAgeBecauseTheyAreEqual()
    {
      Age currentAge = new Age(5, 4, 21);
      Age testAge = new Age(5, 4, 21);
      Assert.False((currentAge > testAge));
    }

    [Fact]
    public void ShouldReturnCurrentAgeIsLessThenTestAgeBecauseOfYears()
    {
      Age currentAge = new Age(5, 4, 12);
      Age testAge = new Age(6, 4, 12);
      Assert.True((currentAge < testAge));
    }

    [Fact]
    public void ShouldReturnCurrentAgeIsLessThenTestAgeBecauseOfMonths()
    {
      Age currentAge = new Age(5, 4, 12);
      Age testAge = new Age(5, 8, 12);
      Assert.True((currentAge < testAge));
    }

    [Fact]
    public void ShouldReturnCurrentAgeIsLessThenTestAgeBecauseOfDays()
    {
      Age currentAge = new Age(5, 4, 12);
      Age testAge = new Age(5, 4, 18);
      Assert.True((currentAge < testAge));
    }

    [Fact]
    public void ShouldReturnFalseCurrentAgeIsLessThenTestAgeBecauseTheyAreEqual()
    {
      Age currentAge = new Age(5, 4, 21);
      Age testAge = new Age(5, 4, 21);
      Assert.False((currentAge < testAge));
    }
  }
}
