using Newtonsoft.Json;
using System.Collections.Generic;

namespace KoldBot.Configs
{
    public class GlobalConfig : IConfig
    {
        [JsonProperty("keys")]
        public Dictionary<ulong, string> keys { get; set; } = new Dictionary<ulong, string>();
    }
}
