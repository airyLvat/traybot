using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace traybot.Models
{
    internal interface IResponse
    {
        public ResponseKinds.Response ResponseKind { get; set; }
        public async Task ReactionResponse(SocketMessage message) { }
        public async Task Execute(SocketMessage message) { }
    }
}
