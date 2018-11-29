using System;
using Newtonsoft.Json;

namespace ChristmasPickMessages
{
    public interface IEnvelope
    {
        [JsonProperty(PropertyName = "payloadtype")]
        string PayloadType { get; set; }
        [JsonProperty(PropertyName = "content")]
        string Payload { get; set; }
    }

    public class Envelope : IEnvelope
    {
        public Envelope()
        {

        }

        public Envelope(object content)
        {
            PayloadType = content.GetType().FullName;
            Payload = JsonConvert.SerializeObject(content);
        }

        public string PayloadType { get; set; }
        public string Payload { get; set; }
    }
}
