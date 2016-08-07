using Discord;
using Discord.Commands;
using DiscordBot.Client.Modules;
using System.Threading.Tasks;
using Discord.WebSocket;
using DiscordBot;
using System;
using System.Timers;
using KoldBot.Configs;
using System.IO;
using System.Collections.Generic;
using DiscordBot.Configuration;

namespace KoldBot.Module.Kold
{
    [Module]
    public class KoldModule : AbstractModule
    {
        private DiscordSocketClient _client;
        private MessageBuffer _messageBuffer;
        private Config _config;

        public KoldModule(DiscordSocketClient client, MessageBuffer messageBuffer, Config config)
        {
            _client = client;
            _messageBuffer = messageBuffer;
            _config = config;

            client.UserJoined += OnUserJoin;
        }

        [Command("key")]
        [Description("PMs you a key for Unearned Bounty!")]
        public async Task GiveKey(IMessage message)
        {
            IMessageChannel channel = message.Channel; ;
            if (message.Channel is IGuildChannel)
            {
                channel = await (message.Author as IGuildUser).CreateDMChannelAsync();
                await message.DeleteAsync();
            }

            Configs.GlobalConfig config = _config.Get<Configs.GlobalConfig>();
            if (config.keys.ContainsKey(message.Author.Id))
            {
                await _messageBuffer.AddItem(channel, config.keys[message.Author.Id]);

                return;
            }

            string keyFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\KoldBot\\keys.txt";
            if (!File.Exists(keyFile))
            {
                await _messageBuffer.AddItem(message.Channel, "Bot has ran out of keys. Tell rjdunlap or Aaron to add more.");

                return;
            }

            List<string> keys = new List<string>(File.ReadAllLines(keyFile));
            if (keys.Count == 0)
            {
                await _messageBuffer.AddItem(message.Channel, "Bot has ran out of keys. Tell rjdunlap or Aaron to add more.");
                return;
            }

            string key = keys[0];
            keys.RemoveAt(0);
            File.WriteAllLines(keyFile, keys.ToArray());

            config.keys.Add(message.Author.Id, key);
            _config.Set<Configs.GlobalConfig>(config);

            await _messageBuffer.AddItem(channel, config.keys[message.Author.Id]);
        }

        public async Task OnUserJoin(IGuildUser member)
        {
            Configs.ServerConfig config = _config.Get<Configs.ServerConfig>(member.Guild);
            if (config.welcomeMessage == null || config.welcomeChannel.Equals(null))
            {
                return;
            }

            IMessageChannel channel = (IMessageChannel)await _client.GetChannelAsync(config.welcomeChannel);

            Timer timer = new Timer(500);
            timer.Elapsed += async (object sender, ElapsedEventArgs args) =>
            {
                timer.Stop();

                member = await member.Guild.GetUserAsync(member.Id);

                string message = config.welcomeMessage;
                message = message.Replace("{{user}}", member.Username ?? member.Mention);
                message = message.Replace("{{mention}}", member.Mention);
                message = message.Replace("{{id}}", member.Id.ToString());

                await _messageBuffer.AddItem(channel, message);
            };

            timer.Start();
        }

        [Command("ping")]
        [Description("Replies with pong")]
        public async Task Ping(IMessage message)
        {
            if (!isAllowed(message.Author as IGuildUser, new string[] { "Extrokold Dev", "Bot Commander", "Bot Owner" }))
            {
                return;
            }

            await _messageBuffer.AddItem(message.Channel, "Pong!");
        }

        [Command("welcome-channel")]
        [Description("Sets the welcome channel")]
        public async Task SetWelcomeChannel(IMessage message, [Remainder] string text)
        {
            if (!isAllowed(message.Author as IGuildUser, new string[] { "Extrokold Dev", "Bot Commander", "Bot Owner" }))
            {
                return;
            }

            if (!(message.Channel is IGuildChannel))
            {
                return;
            }

            if (text.Length == 0)
            {
                await _messageBuffer.AddItem(message.Channel, ":thumbsdown::skin-tone-2:");

                return;
            }

            IGuildChannel channel = message.Channel as IGuildChannel;

            Configs.ServerConfig config = _config.Get<Configs.ServerConfig>(channel.Guild);
            config.welcomeChannel = Convert.ToUInt64(text.Replace("<#", "").Replace(">", ""));
            _config.Set<Configs.ServerConfig>(channel.Guild, config);

            await _messageBuffer.AddItem(message.Channel, ":thumbsup::skin-tone-2:");
        }

        [Command("welcome-message")]
        [Description("Sets the welcome channel")]
        public async Task SetWelcomeMessage(IMessage message, [Remainder] string text)
        {
            if (!isAllowed(message.Author as IGuildUser, new string[] { "Extrokold Dev", "Bot Commander", "Bot Owner" }))
            {
                return;
            }

            if (!(message.Channel is IGuildChannel))
            {
                return;
            }

            if (text.Length == 0)
            {
                await _messageBuffer.AddItem(message.Channel, ":thumbsdown::skin-tone-2:");

                return;
            }

            IGuildChannel channel = message.Channel as IGuildChannel;

            Configs.ServerConfig config = _config.Get<Configs.ServerConfig>(channel.Guild);
            config.welcomeMessage = text;
            _config.Set<Configs.ServerConfig>(channel.Guild, config);

            await _messageBuffer.AddItem(message.Channel, ":thumbsup::skin-tone-2:");
        }
    }
}