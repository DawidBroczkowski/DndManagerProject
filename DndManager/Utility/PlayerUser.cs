using DndManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DndManager.Utility
{
    public record PlayerUser : ApplicationUser
    {
        public List<Character> Characters { get; set; } = new();
    }
}
