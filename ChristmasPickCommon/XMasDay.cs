using System;
using System.Diagnostics;

namespace Common.ChristmasPickList
{
    [DebuggerDisplay("{xmasDay}")]
    public class XMasDay
    {
        private readonly DateTime xmasDay;
        private readonly static int min = 1972;
        private readonly static int max = 3000;
        public  XMasDay(int year)
        {
            if (!IsValidYear(year))
            {
                throw new ArgumentOutOfRangeException(nameof(year));
            }
            xmasDay = new DateTime(year, month:12, day:25, hour:0, minute:0, second:0, kind:DateTimeKind.Utc);
        }

        public int Year => xmasDay.Year;
        private static bool IsValidYear(int year)
        {
            return (year >= min) && (year <= max); 
        }

        public static bool TryParse(int year, out XMasDay xmasDay)
        {
            if (IsValidYear(year))
            {
                xmasDay = new XMasDay(year);
                return true;
            }
            xmasDay = null;
            return false;
        }

        public static implicit operator DateTime(XMasDay x)
        {
            return x.xmasDay;
        }

        public static int Max => max;
        public static int Min => min;

        public override string ToString()
        {
            return xmasDay.Year.ToString();
        }
    }
}