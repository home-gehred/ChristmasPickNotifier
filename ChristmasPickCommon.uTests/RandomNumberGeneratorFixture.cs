using Xunit;
using Common.ChristmasPickList;
using System.Collections.Generic;
using System;
using Xunit.Abstractions;

namespace Common.Test
{

  public class RandomNumberGeneratorFixture : BaseFixture
  {
    private readonly ITestOutputHelper _testLogger;
    public RandomNumberGeneratorFixture(ITestOutputHelper output) 
    {
        _testLogger = output;
    }

    /// <summary>
    // Frequency (Monobits) Test
    //Test For Frequency Within A Block
    //Runs Test
    //Test For The Longest Run Of Ones In A Block
    //Random Binary Matrix Rank Test
    //Discrete Fourier Transform (Spectral) Test
    //Non-Overlapping (Aperiodic) Template Matching Test
    //Overlapping (Periodic) Template Matching Test
    //Maurer’s Universal Statistical Test
    //Linear Complexity Test
    //Serial Test
    //Approximate Entropy Test
    //Cumulative Sum (Cusum) Test
    //Random Excursions Test
    //Random Excursions Variant Test
    /// </summary>
    /// dotnet test --configuration debug --logger:"console;verbosity=detailed" --filter Common.Test.RandomNumberGeneratorFixture.GivenAMaxNumberOfTenWhenGeenrateRandomNumberThenWillBeLessThanOrEqualToTen
    [Fact]
    public void GivenAMaxNumberOfTenWhenGeenrateRandomNumberThenWillBeLessThanOrEqualToTen()
    {
        // Arrange
        var sut = new RandomNumberGenerator();
        var results = new List<int>();
        var maxNumber = 9;

        // Act
        for (int i = 0; i < 100; i++)
        {
            results.Add(sut.GenerateNumberBetweenZeroAnd(maxNumber));
        }

        // Assert
        // To pass this test count how many times 0 - 9 are generated in 100 times.
        // In my mind each number should show up at least 5-10 % of the time.
        var randomResults = new SortedDictionary<int, int>();
        foreach (var randomNumber in results)
        {
            if (!randomResults.TryAdd(randomNumber,1))
            {
                randomResults[randomNumber]++;
            }
        }
        var totalResults = 0;
        foreach (var key in randomResults.Keys)
        {
            totalResults += randomResults[key];
            var distrubution = ((double)randomResults[key])/((double)results.Count);
            _testLogger.WriteLine($"Key: {key} Value: {randomResults[key]} Percentage: {distrubution}");
            Assert.InRange<double>(distrubution, 0.05, 0.20);

        }
        Assert.Equal(100, totalResults);        
    }
  }

}