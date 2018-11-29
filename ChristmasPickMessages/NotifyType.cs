using System;
using Newtonsoft.Json;

namespace ChristmasPickMessages
{
    public class NotifyType
    {
        public static NotifyType Unknown = new NotifyType("unknown");
        public static NotifyType Email = new NotifyType("email");

        [JsonProperty(PropertyName = "type")]
        private readonly string _name;

        public NotifyType()
        {
            _name = NotifyType.Unknown._name;
        }

        private NotifyType(string typeName)
        {
            _name = typeName;
        }

        public static implicit operator string(NotifyType notifyType)
        {
            return notifyType._name;
        }

        public static implicit operator NotifyType(string name)
        {
            if (string.Compare(NotifyType.Email._name, name, ignoreCase: true) == 0)
            {
                return NotifyType.Email;
            }

            return NotifyType.Unknown;
        }
    }
}
