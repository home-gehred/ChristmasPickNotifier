using System;
using Newtonsoft.Json;

namespace ChristmasPickMessages.Messages
{
    public class PickAvailableMessage 
    {
        [JsonProperty(PropertyName = "notifier")]
        public NotifyType NotificationType { get; set; }
        [JsonProperty(PropertyName = "toemailaddress")]
        public string ToAddress { get; set; }
        [JsonProperty(PropertyName = "htmlbody")]
        public string HtmlBody { get; set; }
        [JsonProperty(PropertyName = "plaintextbody")]
        public string PlainTextBody { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "subject")]
        public string Subject { get; set; }
    }
}
