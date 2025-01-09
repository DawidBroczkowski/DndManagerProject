using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DndManager.Models
{
    public class Enemy
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ArmorClass { get; set; }
        public int HitPoints { get; set; }
        public int HitPointsMax { get; set; }
        public int HitPointsTemporary { get; set; }
        public int Initiative { get; set; }
        public int Speed { get; set; }
        public List<AbilityScore> Abilities { get; set; } = new();
        public List<SavingThrow> SavingThrows { get; set; } = new();
        public List<Resource> Resources { get; set; } = new();
        public List<Attack> Attacks { get; set; } = new();
        public List<Spell> Spells { get; set; } = new();
        public List<Skill> Skills { get; set; } = new();
        public List<Condition> Conditions { get; set; } = new();
    }
}
