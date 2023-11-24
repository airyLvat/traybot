using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace traybot.Models
{
    internal class TriggerKinds
    {
        public enum Trigger
        {
            Noop,
            Command,
            UserSpecific,
            Word
        }
    }
}
