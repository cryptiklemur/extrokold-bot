using DiscordBot.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace KoldBot.Configs
{
    public class GlobalConfig : IConfig
    {
        public Dictionary<ulong, string> keys { get; set; } = new Dictionary<ulong, string>();
    }
}
