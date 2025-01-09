using DndManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DndManager.Dtos
{
    public record GetConnectToSessionDto
    {
        public Guid PlayerId { get; set; }
        public string PlayerName { get; set; } = string.Empty;
        public Character Character { get; set; } = new();
        public string EndpointId { get; set; } = string.Empty;
    }
}
