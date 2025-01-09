using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Resource = DndManager.Models.Resource;

namespace DndManager.Models
{
    public record Character : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Guid _id;
        public Guid Id
        {
            get => _id;
            set
            {
                _id = value;
                RaisePropertyChanged(nameof(Id));
            }
        }

        private Guid _concurrencyState = Guid.NewGuid();
        public Guid ConcurrencyState
        {
            get => _concurrencyState;
            set
            {
                _concurrencyState = value;
                RaisePropertyChanged(nameof(ConcurrencyState));
            }
        }

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        private string _race = string.Empty;
        public string Race
        {
            get => _race;
            set
            {
                _race = value;
                RaisePropertyChanged(nameof(Race));
            }
        }

        private string _class = string.Empty;
        public string Class
        {
            get => _class;
            set
            {
                _class = value;
                RaisePropertyChanged(nameof(Class));
            }
        }

        private string _subclass = string.Empty;
        public string Subclass
        {
            get => _subclass;
            set
            {
                _subclass = value;
                RaisePropertyChanged(nameof(Subclass));
            }
        }

        private int _level;
        public int Level
        {
            get => _level;
            set
            {
                _level = value;
                RaisePropertyChanged(nameof(Level));
            }
        }

        private string _background = string.Empty;
        public string Background
        {
            get => _background;
            set
            {
                _background = value;
                RaisePropertyChanged(nameof(Background));
            }
        }

        private string _alignment = string.Empty;
        public string Alignment
        {
            get => _alignment;
            set
            {
                _alignment = value;
                RaisePropertyChanged(nameof(Alignment));
            }
        }

        private string _notes = string.Empty;
        public string Notes
        {
            get => _notes;
            set
            {
                _notes = value;
                RaisePropertyChanged(nameof(Notes));
            }
        }

        private int _experiencePoints;
        public int ExperiencePoints
        {
            get => _experiencePoints;
            set
            {
                _experiencePoints = value;
                RaisePropertyChanged(nameof(ExperiencePoints));
            }
        }

        private int _armorClass;
        public int ArmorClass
        {
            get => _armorClass;
            set
            {
                _armorClass = value;
                RaisePropertyChanged(nameof(ArmorClass));
            }
        }

        private int _initiative;
        public int Initiative
        {
            get => _initiative;
            set
            {
                _initiative = value;
                RaisePropertyChanged(nameof(Initiative));
            }
        }

        private int _speed;
        public int Speed
        {
            get => _speed;
            set
            {
                _speed = value;
                RaisePropertyChanged(nameof(Speed));
            }
        }

        private int _hitPoints;
        public int HitPoints
        {
            get => _hitPoints;
            set
            {
                _hitPoints = value;
                RaisePropertyChanged(nameof(HitPoints));
            }
        }

        private int _hitPointsMax;
        public int HitPointsMax
        {
            get => _hitPointsMax;
            set
            {
                _hitPointsMax = value;
                RaisePropertyChanged(nameof(HitPointsMax));
            }
        }

        private int _hitPointsTemporary;
        public int HitPointsTemporary
        {
            get => _hitPointsTemporary;
            set
            {
                _hitPointsTemporary = value;
                RaisePropertyChanged(nameof(HitPointsTemporary));
            }
        }

        private int _hitDice;
        public int HitDice
        {
            get => _hitDice;
            set
            {
                _hitDice = value;
                RaisePropertyChanged(nameof(HitDice));
            }
        }

        private int _hitDiceMax;
        public int HitDiceMax
        {
            get => _hitDiceMax;
            set
            {
                _hitDiceMax = value;
                RaisePropertyChanged(nameof(HitDiceMax));
            }
        }

        private int _proficiencyBonus;
        public int ProficiencyBonus
        {
            get => _proficiencyBonus;
            set
            {
                _proficiencyBonus = value;
                RaisePropertyChanged(nameof(ProficiencyBonus));
            }
        }

        private int _deathSavesSuccesses;
        public int DeathSavesSuccesses
        {
            get => _deathSavesSuccesses;
            set
            {
                _deathSavesSuccesses = value;
                RaisePropertyChanged(nameof(DeathSavesSuccesses));
            }
        }

        private int _deathSavesFailures;
        public int DeathSavesFailures
        {
            get => _deathSavesFailures;
            set
            {
                _deathSavesFailures = value;
                RaisePropertyChanged(nameof(DeathSavesFailures));
            }
        }

        private int _passivePerception;
        public int PassivePerception
        {
            get => _passivePerception;
            set
            {
                _passivePerception = value;
                RaisePropertyChanged(nameof(PassivePerception));
            }
        }

        private int _passiveInsight;
        public int PassiveInsight
        {
            get => _passiveInsight;
            set
            {
                _passiveInsight = value;
                RaisePropertyChanged(nameof(PassiveInsight));
            }
        }

        private int _spellSaveDC;
        public int SpellSaveDC
        {
            get => _spellSaveDC;
            set
            {
                _spellSaveDC = value;
                RaisePropertyChanged(nameof(SpellSaveDC));
            }
        }

        public ObservableCollection<AbilityScore> AbilityScores { get; set; } = new ObservableCollection<AbilityScore>();
        public ObservableCollection<SavingThrow> SavingThrows { get; set; } = new ObservableCollection<SavingThrow>();
        public ObservableCollection<Skill> Skills { get; set; } = new ObservableCollection<Skill>();
        public ObservableCollection<Currency> Currency { get; set; } = new ObservableCollection<Currency>();
        public ObservableCollection<Resource> Resources { get; set; } = new ObservableCollection<Resource>();
        public ObservableCollection<Attack> Attacks { get; set; } = new ObservableCollection<Attack>();
        public ObservableCollection<Spell> Spells { get; set; } = new ObservableCollection<Spell>();

        public ObservableCollection<string> SkillProficiencies { get; set; } = new();
        public ObservableCollection<string> SkillExpertises { get; set; } = new();
        public ObservableCollection<string> OtherProficiencies { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> OtherExpertises { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> Languages { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<Item> Equipment { get; set; } = new ObservableCollection<Item>();
        public ObservableCollection<string> Conditions { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<Feat> Feats { get; set; } = new ObservableCollection<Feat>();

        public ObservableCollection<string> Vulnerabilities { get; set; } = new();
        public ObservableCollection<string> DamageResistances { get; set; } = new();
        public ObservableCollection<string> DamageImmunities { get; set; } = new();
        public ObservableCollection<string> ConditionImmunities { get; set; } = new();
        public ObservableCollection<string> Senses { get; set; } = new();

        public AbilityScore? GetAbilityScore(string name)
        {
            return AbilityScores.FirstOrDefault(a => a.Name == name);
        }

        public SavingThrow? GetSavingThrow(string name)
        {
            return SavingThrows.FirstOrDefault(s => s.Name == name);
        }

        public int? GetAbilityScoreModifier(string name)
        {
            var ability = GetAbilityScore(name);

            if (ability == null)
                return null;

            int score = ability.Score;

            var result = Math.Floor((score - 10) / 2.0);
            return (int)result;
        }

        public Skill? GetSkill(string name)
        {
            return Skills.FirstOrDefault(s => s.Name == name);
        }

        public Resource? GetResource(string name)
        {
            return Resources.FirstOrDefault(r => r.Name == name);
        }

        public Currency? GetCurrency(string name)
        {
            return Currency.FirstOrDefault(c => c.Name == name);
        }

        static public Character Copy(Character original)
        {
            return App.Mapper.Map<Character>(original);
        }
    }
}