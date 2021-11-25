using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace xUnitTestSecrets.SecretProviders
{
    public class JsonFileProvider : ISecretProvider
    {
        private readonly string _cfgFile;
        private readonly ISecretProvider _innerProvider;
        private JObject _cfgInfo;
        public JsonFileProvider(string pathToCfgFile)
        {
            _cfgFile = string.IsNullOrEmpty(pathToCfgFile) ?
                             throw new ArgumentNullException(nameof(pathToCfgFile)) :
                             pathToCfgFile;
            _innerProvider = null;
        }

        public JsonFileProvider(ISecretProvider innerProvider, string pathToCfgFile)
        {
            _cfgFile = string.IsNullOrEmpty(pathToCfgFile) ?
                             throw new ArgumentNullException(nameof(pathToCfgFile)) :
                             pathToCfgFile;
            _innerProvider = innerProvider ?? throw new ArgumentNullException(nameof(innerProvider));
        }

        protected string GetCfgValue(string key)
        {
            if (_cfgInfo == null)
            {
                var cfg = File.ReadAllText(_cfgFile);
                _cfgInfo = JsonConvert.DeserializeObject<JObject>(cfg);
            }
            if (_cfgInfo.TryGetValue(key, out JToken cfgValue))
            {
                if (cfgValue.Type == JTokenType.String)
                {
                    return cfgValue.Value<string>();
                }
            }
            return string.Empty;
        }

        public string GetSecret(string keyValue)
        {
            string tmpValue = String.Empty;
            if (_innerProvider != null)
            {
                tmpValue = _innerProvider.GetSecret(keyValue);
            }

            if (string.IsNullOrWhiteSpace(tmpValue))
            {
                tmpValue = GetCfgValue(keyValue);
            }

            return tmpValue;
        }
    }
}
