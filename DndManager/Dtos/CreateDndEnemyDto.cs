 using DndManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DndManager.Dtos
{
    public record CreateDndEnemyDto
    {
        public bool IsMonster { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Level { get; set; }
        public string Class { get; set; } = string.Empty;
        public string Subclass { get; set; } = string.Empty;
        public string Race { get; set; } = string.Empty;
        public string Background { get; set; } = string.Empty;
        public string Alignment { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public int? HitPointsMax { get; set; }

        // Ability Scores
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }

        public ObservableCollection<string> SavingThrowProficiencies { get; set; } = new();
        public ObservableCollection<string> SkillProficiencies { get; set; } = new();
        public ObservableCollection<string> SkillExpertises { get; set; } = new();
        public ObservableCollection<string> Vulnerabilities { get; set; } = new();
        public ObservableCollection<string> DamageResistances { get; set; } = new();
        public ObservableCollection<string> DamageImmunities { get; set; } = new();
        public ObservableCollection<string> ConditionImmunities { get; set; } = new();
        public ObservableCollection<string> Senses { get; set; } = new();

        public ObservableCollection<string> Languages { get; set; } = new();
        public ObservableCollection<Item> Equipment { get; set; } = new();

        // Enemy exclusive properties in creation
        public int ArmorClass { get; set; }
        public int Initiative { get; set; }
        public int Speed { get; set; }
        public ObservableCollection<Attack> Attacks { get; set; } = new ObservableCollection<Attack>();
        public ObservableCollection<DndManager.Models.Resource> Resources { get; set; } = new();
    }
}
