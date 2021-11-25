using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class ExchangeCheckSum
    {
        private int presentsIn;
        private int presentsOut;

        public ExchangeCheckSum()
        {
            presentsIn = 0;
            presentsOut = 0;
        }

        public void updatePresentsIn()
        {
            presentsIn++;
        }
        public void updatePresentsOut()
        {
            presentsOut++;
        }
        public bool isValid()
        {
            return ((presentsIn == 1) && (presentsOut == 1));
        }
        public string DiagnosticMessage()
        {
            if (isValid())
            {
                return "correct";
            }
            else
            {
                if ((presentsIn == 0) && (presentsOut == 0))
                {
                    return "not buying or recieving a gift";
                }
                else
                {
                    return string.Format("buying {0} present(s) and is recieving {1} present(s)", presentsOut, presentsIn);
                }
            }
        }
    }
}
