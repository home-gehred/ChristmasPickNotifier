using System;
using System.Collections.Generic;
using System.Linq;


namespace Common.ChristmasPickList
{

    public interface INumberGenerator
    {
        int GenerateNumberBetweenZeroAnd(int max);
    }

    public class RandomNumberGenerator : INumberGenerator
    {
        private Random mRandomNum = new Random();

        public int GenerateNumberBetweenZeroAnd(int max)
        {
            return mRandomNum.Next(0, max);
        }
    }
}
