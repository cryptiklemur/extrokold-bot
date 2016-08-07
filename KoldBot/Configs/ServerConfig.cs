using DiscordBot.Configuration;
using Newtonsoft.Json;

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
