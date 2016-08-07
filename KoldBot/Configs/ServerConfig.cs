using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoldBot.Configs
{
    public class ServerConfig : IConfig
    {
        [JsonProperty("welcomeMessage")]
        public string welcomeMessage { get; set; }

        [JsonProperty("welcomeChannel")]
        public ulong welcomeChannel { get; set; }
    }
}
