using DndManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DndManager.Dtos
{
    public record UpdateCharacterDto
    {
        public Guid CampaignId { get; set; }
        public Guid PlayerId { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.Now;

        // Character
        public Guid? Id { get; set; }
        public Guid? ConcurrencyState { get; set; }
        public string? Name { get; set; }
        public string? Race { get; set; }
        public string? Class { get; set; }
        public string? Subclass { get; set; }
        public int? Level { get; set; }
        public string? Background { get; set; }
        public string? Alignment { get; set; }
        public string? Notes { get; set; }
        public int? ExperiencePoints { get; set; }
        public int? ArmorClass { get; set; }
        public int? Initiative { get; set; }
        public int? Speed { get; set; }
        public int? HitPoints { get; set; }
        public int? HitPointsMax { get; set; }
        public int? HitPointsTemporary { get; set; }
        public int? HitDice { get; set; }
        public int? HitDiceMax { get; set; }
        public int? ProficiencyBonus { get; set; }
        public int? DeathSavesSuccesses { get; set; }
        public int? DeathSavesFailures { get; set; }
        public int? PassivePerception { get; set; }
        public int? PassiveInsight { get; set; }
        public int? SpellSaveDC { get; set; }

        public ObservableCollection<AbilityScore>? AbilityScores { get; set; }
        public ObservableCollection<SavingThrow>? SavingThrows { get; set; }
        public ObservableCollection<Skill>? Skills { get; set; }
        public ObservableCollection<Currency>? Currency { get; set; }
        public ObservableCollection<DndManager.Models.Resource>? Resources { get; set; }
        public ObservableCollection<Attack>? Attacks { get; set; }
        public ObservableCollection<Spell>? Spell { get; set; }
        public ObservableCollection<string>? SkillProficiencies { get; set; }
        public ObservableCollection<string>? SkillExpertises { get; set; }
        public ObservableCollection<string>? OtherExpertises { get; set; }
        public ObservableCollection<string>? OtherProficiencies { get; set; }
        public ObservableCollection<string>? Languages { get; set; }
        public ObservableCollection<Item>? Equipment { get; set; }
        public ObservableCollection<Feat>? Feats { get; set; }
        public ObservableCollection<string>? Conditions { get; set; }
        public ObservableCollection<string>? Vulnerabilities { get; set; }
        public ObservableCollection<string>? DamageResistances { get; set; }
        public ObservableCollection<string>? DamageImmunities { get; set; }
        public ObservableCollection<string>? ConditionImmunities { get; set; }
        public ObservableCollection<string>? Senses { get; set; }
    }
}
