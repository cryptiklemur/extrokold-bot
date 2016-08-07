using DiscordBot;
using DiscordBot.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace KoldBot
{
    class Program
    {
        private static void Main(string[] args) => new Program().Start().GetAwaiter().GetResult();

        private const string configPath = "./config.json";

        public async Task Start()
        {
            if (!File.Exists(configPath))
            {
                File.WriteAllText(configPath, JsonConvert.SerializeObject(new Config(), Formatting.Indented));
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Please edit '{Path.GetFullPath("./config.json")}' with your settings.");
                Thread.Sleep(5000);

                return;
            }

            DiscordBot.Bot bot = new DiscordBot.Bot(JsonConvert.DeserializeObject<Config>(File.ReadAllText(configPath)));

            bot.Container.Add(new KoldBot.Configs.Config());

            await bot.Start();
        }
    }
}
