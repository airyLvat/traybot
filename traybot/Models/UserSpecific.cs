using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using traybot.Models;

namespace traybot.Models
{
    internal class UserSpecific : IResponse
    {
        public Guid ID { get; set; }
        public long UserID { get; set; }
        public string Pattern { get; set; }
        public ResponseKinds.Response ResponseKind { get; set; }
        public string? ReactionEmoji { get; set; }
        public string? Message { get; set; }
        public string? Command { get; set; }
        public bool Enabled { get; set; }

        public async Task ReactionResponse(SocketMessage message)
        {
            await message.AddReactionAsync(new Emoji(ReactionEmoji));
        }
    }
}
