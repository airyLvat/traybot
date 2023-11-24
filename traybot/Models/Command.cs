using Discord.WebSocket;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using traybot.Models;
using System.Net.NetworkInformation;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace traybot.Models
{
    internal class Command : IResponse
    {
        public string CommandName { get; set; }
        public ResponseKinds.Response ResponseKind { get; set; }
        public long[] AllowedUsers { get; set; }
        public bool Enabled { get; set; }


        public async Task Execute(SocketMessage message)
        {
            switch (CommandName)
            {
                case "ping":
                    await Ping(message);
                    break;
                case "toggleuser":
                    await ToggleUser(message);
                    break;
            }
        }

        public async Task Ping(SocketMessage message)
        {
            await message.Channel.SendMessageAsync("pong!");
        }

        public async Task ToggleUser(SocketMessage message)
        {
            string userText = message.Content.Split(' ', 2)[1];
            long userID;

            try
            {
               userID = long.Parse(Regex.Replace(userText, @"\D", ""));
            }
            catch
            {
                await message.Channel.SendMessageAsync($"Argument was not a valid user: {userText}");
                return;
            }

            string configPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config");
            var config = new ConfigurationBuilder()
                .SetBasePath(configPath)
                .AddJsonFile("appsettings.json")
                .Build()
                .Get<Config>();

            List<UserSpecific> target = config.UserSpecific.Where(e => e.UserID == userID).ToList();

            if (target.Count > 0)
            {
                foreach (var c in target)
                {
                    bool curr = config.UserSpecific.First(e => e.ID == c.ID).Enabled;
                    config.UserSpecific.First(e => e.ID == c.ID).Enabled = !curr;
                }

                var jsonWriteOptions = new JsonSerializerOptions()
                {
                    WriteIndented = true
                };
                jsonWriteOptions.Converters.Add(new JsonStringEnumConverter());

                var newJson = JsonSerializer.Serialize(config, jsonWriteOptions);

                var appSettingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", "appsettings.json");
                File.WriteAllText(appSettingsPath, newJson);
            }
            else
            {
                await message.Channel.SendMessageAsync($"User `{userID}` has no specific interactions");
            }
        }
    }
   }
