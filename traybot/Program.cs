using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace traybot
{
    public class Program
    {
        private readonly DiscordSocketClient _client;
        private Reactions _reactionsConfig;

        static void Main(string[] args)
            => new Program()
                .MainAsync()
                .GetAwaiter()
                .GetResult();

        public Program()
        {
            var reactionConfig = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json").Build();

            var section = reactionConfig.GetSection("Reactions");
            _reactionsConfig = section.Get<Reactions>();

            var config = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
            };

            _client = new DiscordSocketClient(config);

            _client.Log += LogAsync;
            _client.Ready += ReadyAsync;
            _client.MessageReceived += MessageReceivedAsync;
        }

        public async Task MainAsync()
        {
            await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("Discord_Bot_Token"));
            await _client.StartAsync();
            await Task.Delay(Timeout.Infinite);
        }

        private Task LogAsync(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private Task ReadyAsync()
        {
            Console.WriteLine($"{_client.CurrentUser} is connected");
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(SocketMessage message)
        {
            if (message.Author.Id == 497792454393593876) // tray
            {
                if (message.Content == "!ping")
                {
                    await message.Channel.SendMessageAsync("pong!!!");
                }

                if (message.Content == "!togglereaction")
                {
                    _reactionsConfig.DoReaction = !(true && _reactionsConfig.DoReaction);
                }

                if (_reactionsConfig.DoReaction)
                {
                    await message.AddReactionAsync((new Emoji("\U0001f495")));
                }
            }
        }
    }

    public class Reactions
    {
        public bool DoReaction { get; set;}
    }
}