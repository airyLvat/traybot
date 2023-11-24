using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using traybot.Models;
using traybot.Handlers;

namespace traybot
{
    public class Program
    {
        private DiscordSocketClient _client;
        private Config _config;
        private MessageRouter mr;

        static void Main(string[] args)
            => new Program()
                .MainAsync()
                .GetAwaiter()
                .GetResult();

        public Program()
        {
            var _config = new ConfigurationBuilder()
                .SetBasePath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config"))
                .AddJsonFile("appsettings.json")
                .Build()
                .Get<Config>();

            mr = new MessageRouter(_config);

            var dsg = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent,
                AlwaysDownloadUsers = true
            };

            _client = new DiscordSocketClient(dsg);

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
            List<IResponse> responses = mr.RouteMessage(message);

            foreach (IResponse response in responses)
            {

                switch (response.ResponseKind)
                {
                    case ResponseKinds.Response.Noop:
                        break;
                    case ResponseKinds.Response.Reaction:
                        await response.ReactionResponse(message);
                        break;
                    case ResponseKinds.Response.Command:
                        await response.Execute(message);
                        _config = new ConfigurationBuilder()
                            .SetBasePath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config"))
                            .AddJsonFile("appsettings.json")
                            .Build()
                            .Get<Config>();
                        break;
                }
            }
        }
    }
}