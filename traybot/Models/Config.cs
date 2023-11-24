using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace traybot.Models
{
    internal class Config
    {
        public BotProperties BotProperties { get; set; }
        public UserSpecific[] UserSpecific { get; set; }
        public Command[] Command { get; set; }
    }
}
