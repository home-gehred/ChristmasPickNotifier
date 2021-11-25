using System;

namespace ChristmasPickCommon.Exceptions
{
    public class ChristmasPickConfigurationException : Exception
    {
        /*public ChristmasPickConfigurationException()
        {
        }*/

        public ChristmasPickConfigurationException(string cfgKey)
            : base($"The configuration setting {cfgKey} was not found. Please check the configuration of application.")
        {
        }

        /*public EmployeeListNotChristmasPickConfigurationExceptionFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }*/
    }
}
