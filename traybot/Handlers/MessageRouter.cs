using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using traybot.Models;
using traybot;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Text.RegularExpressions;

namespace traybot.Handlers
{
    internal class MessageRouter
    {
        public Config _config { get; set; }
        public List<long> UserSpecificList { get; set; }
        public List<string> CommandsList { get; set; }

        public MessageRouter(Config config)
        {
            _config = config;
            UserSpecificList = _config.UserSpecific.Select(e => e.UserID).ToList();
            CommandsList = _config.Command.Select(e => string.Join("", _config.BotProperties.Prefix, e.CommandName)).ToList();
        }

        public List<IResponse> RouteMessage(SocketMessage message)
        {
            TriggerKinds.Trigger noopTrigger = TriggerKinds.Trigger.Noop;
            var retList = new List<IResponse>() { new Noop(noopTrigger) };

            if (CommandsList.Contains(message.Content.Split(" ")[0]))
            {
                retList.Clear();
                Command thisCommand = _config.Command.First(e => string.Join("", _config.BotProperties.Prefix, e.CommandName) == message.Content.Split(" ")[0]);

                if (thisCommand.Enabled && thisCommand.AllowedUsers.ToList().Contains((long)message.Author.Id))
                {
                    retList.Add(thisCommand);
                }

                return retList;
            }

            if (UserSpecificList.Contains((long)message.Author.Id))
            {
                bool valid = false;
                List<UserSpecific> thisUser = _config.UserSpecific.Where(e => e.UserID == (long)message.Author.Id && e.Enabled == true).ToList();
                List<UserSpecific> toDo = new List<UserSpecific>();

                if (thisUser.Count > 0)
                {
                    foreach (var c in thisUser)
                    {
                        Match match = Regex.Match(message.Content, c.Pattern);

                        if (match.Success)
                        {
                            toDo.Add(c);
                            valid = true;
                        }
                    }
                }

                if (valid)
                {
                    retList.Clear();
                    retList.AddRange(toDo);
                }
            }

            return retList;
        }
    }
}
