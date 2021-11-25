using System;


namespace Common.ChristmasPickList
{

    public interface INumberGenerator
    {
        int GenerateNumber();
        int GenerateNumberBetweenZeroAnd(int max);
    }

    public class RandomNumberGenerator : INumberGenerator
    {
        private int mMax = 0;
        private Random mRandomNum = new Random(10);


        public RandomNumberGenerator(int maxNumber)
        {
            mMax = maxNumber;
        }

        public int GenerateNumber()
        {
            return mRandomNum.Next(-10, mMax);
        }

        public int GenerateNumberBetweenZeroAnd(int max)
        {
            return mRandomNum.Next(0, max);
        }
    }
}
