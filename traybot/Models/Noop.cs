using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace traybot.Models
{
    internal class Noop : IResponse
    {
        public TriggerKinds.Trigger TriggerKind { get; set; }
        public ResponseKinds.Response ResponseKind { get; set; }

        public Noop(TriggerKinds.Trigger trigger)
        {
            TriggerKind = trigger;
            ResponseKind = ResponseKinds.Response.Noop;
        }
    }
}
