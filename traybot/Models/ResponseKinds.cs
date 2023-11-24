using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace traybot.Models
{
    internal class ResponseKinds
    {
        public enum Response
        {
            Noop,
            Reaction,
            Message,
            Moderation,
            Command
        }
    }
}
