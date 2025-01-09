using DndManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DndManager.Dtos
{
    public record ConnectToSessionDto
    {
        public Guid CampaignId { get; set; }
        public Guid PlayerId { get; set; }
        public string PlayerName { get; set; } = string.Empty;
        public Character Character { get; set; } = new();
    }
}
