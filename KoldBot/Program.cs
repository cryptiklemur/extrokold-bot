using DiscordBot;
using DiscordBot.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace KoldBot
{
    class Program
    {
        private static void Main(string[] args) => new Program().Start().GetAwaiter().GetResult();

        private string configPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\KoldBot\\";

        public async Task Start()
        {
            string configFile = configPath + "global.json";

            Directory.CreateDirectory(configPath);
            Config config = new Config();
            if (!File.Exists(configFile))
            {
                config.Get();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Please edit '{Path.GetFullPath(configFile)}' with your settings.");
                Thread.Sleep(5000);

                return;
            }

            GlobalConfig c = config.Get<GlobalConfig>();

            DiscordBot.Bot bot = new DiscordBot.Bot(c);

            bot.Container.Add(config);

            await bot.Start();
        }
    }
}
