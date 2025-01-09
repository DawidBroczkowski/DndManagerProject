using DndManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DndManager.Network.Utility
{
    public class PlayerInSession
    {
        public Guid Id { get; set; }
        public DateTime StartedAt { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string EndpointId { get; set; } = string.Empty;
        public Guid CharacterId { get; set; }
    }
}
