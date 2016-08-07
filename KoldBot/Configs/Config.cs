using Discord;
using Newtonsoft.Json;
using DiscordBot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoldBot.Configs
{
    public class Config
    {
        private string _directory;
        
        public Config()
        {
            _directory = "./config/";
            Directory.CreateDirectory(_directory);
        }

        public GlobalConfig Get()
        {
            string fileName = _directory + "global.json";
            if (!File.Exists(fileName))
            {
                Set(new GlobalConfig());
            }

            return (GlobalConfig) JsonConvert.DeserializeObject(File.ReadAllText(fileName), typeof(GlobalConfig));
        }

        public ServerConfig Get(IGuild guild)
        {
            string fileName = _directory + "server." + guild.Id + ".json";
            if (!File.Exists(fileName))
            {
                Set(guild, new ServerConfig());
            }

            return (ServerConfig) JsonConvert.DeserializeObject(File.ReadAllText(fileName), typeof(ServerConfig));
        }

        public void Set(GlobalConfig config)
        {
            File.WriteAllText(_directory + "global.json", JsonConvert.SerializeObject(config));
        }

        public void Set(IGuild guild, ServerConfig config)
        {
            File.WriteAllText(_directory + "server." + guild.Id + ".json", JsonConvert.SerializeObject(config));
        }
    }
}
