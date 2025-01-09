using CommunityToolkit.Maui.Core.Extensions;
using DndManager.Dtos;
using DndManager.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DndManager.Models
{
    public class DndCharacterFactory
    {
        public Character CreateCharacter(CreateDndCharacterDto dto)
        {
            // Validate input
            if (dto.Level < 1 || dto.Level > 20)
            {
                throw new ArgumentException("Level must be between 1 and 20.");
            }

            // Map base ability scores
            var abilityScores = new ObservableCollection<AbilityScore>
            {
                CreateAbilityScore("Strength", dto.Strength),
                CreateAbilityScore("Dexterity", dto.Dexterity),
                CreateAbilityScore("Constitution", dto.Constitution),
                CreateAbilityScore("Intelligence", dto.Intelligence),
                CreateAbilityScore("Wisdom", dto.Wisdom),
                CreateAbilityScore("Charisma", dto.Charisma)
            };

            // Calculate proficiency bonus (based on level)
            int proficiencyBonus = CalculateProficiencyBonus(dto.Level);

            // Calculate base AC (10 + Dexterity modifier)
            int dexterityModifier = GetModifier(dto.Dexterity);
            int armorClass = 10 + dexterityModifier;

            // Initialize resources based on class and level
            var resources = InitializeResources(dto);
            string spellCastingAbility = GetSpellCastingAbilityScore(dto.Class);


            // Create the character object
            var character = new Character
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Race = dto.Race,
                Class = dto.Class,
                Subclass = dto.Subclass,
                Level = dto.Level,
                Background = dto.Background,
                Alignment = dto.Alignment,
                Notes = dto.Notes,
                ExperiencePoints = 0,
                ArmorClass = armorClass,
                Initiative = dexterityModifier,
                Speed = CalculateBaseSpeed(dto.Class),
                HitPoints = CalculateHitPoints(dto.Class, dto.Level, dto.Constitution),
                HitPointsMax = CalculateHitPoints(dto.Class, dto.Level, dto.Constitution),
                HitPointsTemporary = 0,
                HitDice = dto.Level,
                HitDiceMax = dto.Level,
                ProficiencyBonus = proficiencyBonus,
                DeathSavesSuccesses = 0,
                DeathSavesFailures = 0,
                PassivePerception = 10 + GetModifier(dto.Wisdom),
                PassiveInsight = 10 + GetModifier(dto.Wisdom),
                SpellSaveDC = 8 + proficiencyBonus + GetSpellCastingAbilityScoreModifier(dto),
                AbilityScores = abilityScores,
                SavingThrows = InitializeSavingThrows(dto.SavingThrowProficiencies, abilityScores),
                Skills = InitializeSkills(dto.SkillProficiencies, abilityScores, proficiencyBonus),
                Currency = new ObservableCollection<Currency>(),
                Resources = resources,
                Attacks = new ObservableCollection<Attack>(),
                SkillProficiencies = dto.SkillProficiencies,
                SkillExpertises = dto.SkillExpertises,
                OtherProficiencies = dto.OtherProficiencies,
                OtherExpertises = dto.OtherExpertises,
                Languages = dto.Languages,
                Equipment = dto.Equipment
            };

            return character;
        }

        public Character CreateEnemy(CreateDndEnemyDto dto)
        {
            // Map base ability scores
            var abilityScores = new ObservableCollection<AbilityScore>
            {
                CreateAbilityScore("Strength", dto.Strength),
                CreateAbilityScore("Dexterity", dto.Dexterity),
                CreateAbilityScore("Constitution", dto.Constitution),
                CreateAbilityScore("Intelligence", dto.Intelligence),
                CreateAbilityScore("Wisdom", dto.Wisdom),
                CreateAbilityScore("Charisma", dto.Charisma)
            };

            int proficiencyBonus = CalculateProficiencyBonus(dto.Level);

            // Calculate base AC and other properties if not provided
            int armorClass = dto.ArmorClass > 0 ? dto.ArmorClass : 10 + GetModifier(dto.Dexterity);
            int initiative = dto.Initiative != 0 ? dto.Initiative : GetModifier(dto.Dexterity);
            int speed = dto.Speed != 0 ? dto.Speed : 30;

            ObservableCollection<DndManager.Models.Resource> resources;

            if(dto.Resources.Any() is true)
            {
                resources = dto.Resources;
            }
            else
            {
                // If the enemy is not a monster, calculate values like a player character
                resources = dto.IsMonster || string.IsNullOrEmpty(dto.Class)
                    ? new ObservableCollection<Resource>()
                    : InitializeResources(dto);
            }

            // Calculate hit points if not provided
            int hitPointsMax = dto.HitPointsMax ??
                (dto.IsMonster ? 0 : CalculateHitPoints(dto.Class, dto.Level, dto.Constitution));

            // Create the enemy object
            var enemy = new Character
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Race = dto.Race,
                Class = dto.Class,
                Subclass = dto.Subclass,
                Level = dto.Level,
                Background = dto.Background,
                Alignment = dto.Alignment,
                Notes = dto.Notes,
                ExperiencePoints = 0,
                ArmorClass = armorClass,
                Initiative = initiative,
                Speed = speed,
                HitPoints = hitPointsMax,
                HitPointsMax = hitPointsMax,
                HitPointsTemporary = 0,
                HitDice = dto.Level,
                HitDiceMax = dto.Level,
                ProficiencyBonus = proficiencyBonus,
                DeathSavesSuccesses = 0,
                DeathSavesFailures = 0,
                PassivePerception = 10 + GetModifier(dto.Wisdom),
                PassiveInsight = 10 + GetModifier(dto.Wisdom),
                SpellSaveDC = dto.IsMonster ? 0 : 8 + proficiencyBonus + GetModifier(dto.Intelligence),
                AbilityScores = abilityScores,
                SavingThrows = InitializeSavingThrows(dto.SavingThrowProficiencies, abilityScores),
                Skills = InitializeSkills(dto.SkillProficiencies, abilityScores, proficiencyBonus),
                Currency = new ObservableCollection<Currency>(), // Defaults, monsters usually don't have currency
                Resources = resources,
                Attacks = dto.Attacks, // Predefined attacks for the enemy
                OtherProficiencies = new ObservableCollection<string>(),
                Languages = dto.Languages,
                Equipment = dto.Equipment,
                Vulnerabilities = dto.Vulnerabilities,
                DamageResistances = dto.DamageResistances,
                DamageImmunities = dto.DamageImmunities,
                ConditionImmunities = dto.ConditionImmunities,
                Senses = dto.Senses
            };

            return enemy;
        }

        private static AbilityScore CreateAbilityScore(string name, int score)
        {
            return new AbilityScore
            {
                Name = name,
                Score = score,
                Modifier = GetModifier(score)
            };
        }

        private static int GetModifier(int score)
        {
            return (int)Math.Floor((score - 10) / 2.0);
        }

        private static int CalculateProficiencyBonus(int level)
        {
            return (level - 1) / 4 + 2;
        }

        private static int CalculateBaseSpeed(string characterClass)
        {
            // Default speed, can be adjusted based on class or race later
            return 30;
        }

        private static int CalculateHitPoints(string characterClass, int level, int constitution)
        {
            int conModifier = GetModifier(constitution);
            int hitDie = characterClass switch
            {
                "Barbarian" => 12,
                "Fighter" => 10,
                "Paladin" => 10,
                "Ranger" => 10,
                "Cleric" => 8,
                "Druid" => 8,
                "Rogue" => 8,
                "Warlock" => 8,
                "Wizard" => 6,
                "Sorcerer" => 6,
                _ => 8 // Default hit die
            };

            int baseHitPoints = hitDie + conModifier; // First level
            int additionalHitPoints = (int)Math.Floor((hitDie / 2.0) + 1) + conModifier; // Average roll for subsequent levels

            return baseHitPoints + (level - 1) * additionalHitPoints;
        }

        private static ObservableCollection<Resource> InitializeResources(CreateDndCharacterDto dto)
        {
            var resources = new ObservableCollection<Resource>();

            switch (dto.Class)
            {
                case "Barbarian":
                    resources.Add(new Resource { Name = "Rage", Current = CalculateBarbarianRages(dto.Level), Max = CalculateBarbarianRages(dto.Level) });
                    break;

                case "Bard":
                    AddSpellSlots(resources, dto.Level, "Bard");
                    int inspiration = GetModifier(dto.Charisma) >= 1 ? GetModifier(dto.Charisma) : 1;
                    resources.Add(new Resource { Name = "Bardic Inspiration", Current = inspiration, Max = inspiration });
                    break;

                case "Cleric":
                    AddSpellSlots(resources, dto.Level, "Cleric");
                    resources.Add(new Resource { Name = "Channel Divinity", Current = CalculateChannelDivinityUses(dto.Level), Max = CalculateChannelDivinityUses(dto.Level) });
                    break;

                case "Druid":
                    AddSpellSlots(resources, dto.Level, "Druid");
                    resources.Add(new Resource { Name = "Wild Shape", Current = 2, Max = 2 });
                    break;

                case "Fighter":
                    if (dto.Level >= 2) // Starting Indomitable feature at Level 2
                    {
                        resources.Add(new Resource { Name = "Second Wind", Current = 1, Max = 1 });
                    }
                    break;

                case "Paladin":
                    AddSpellSlots(resources, dto.Level, "Paladin");
                    resources.Add(new Resource { Name = "Lay on Hands", Current = dto.Level * 5, Max = dto.Level * 5 });
                    resources.Add(new Resource { Name = "Oath Charges", Current = CalculateOathCharges(dto.Level), Max = CalculateOathCharges(dto.Level) });
                    break;

                case "Monk":
                    resources.Add(new Resource { Name = "Ki Points", Current = dto.Level, Max = dto.Level });
                    break;

                case "Ranger":
                    AddSpellSlots(resources, dto.Level, "Ranger");
                    break;

                case "Rogue":
                    if (dto.Level >= 3)
                    {
                        resources.Add(new Resource { Name = "Cunning Action", Current = 1, Max = 1 });
                    }
                    break;

                case "Sorcerer":
                    AddSpellSlots(resources, dto.Level, "Sorcerer");
                    resources.Add(new Resource { Name = "Sorcery Points", Current = dto.Level, Max = dto.Level });
                    break;

                case "Warlock":
                    AddWarlockSpellSlots(resources, dto.Level);
                    break;

                case "Wizard":
                    AddSpellSlots(resources, dto.Level, "Wizard");
                    break;

                default:
                    throw new ArgumentException("Unknown class: " + dto.Class);
            }

            return resources;
        }
        private static ObservableCollection<Resource> InitializeResources(CreateDndEnemyDto dto)
        {
            var resources = new ObservableCollection<Resource>();

            switch (dto.Class)
            {
                case "Barbarian":
                    resources.Add(new Resource { Name = "Rage", Current = CalculateBarbarianRages(dto.Level), Max = CalculateBarbarianRages(dto.Level) });
                    break;

                case "Bard":
                    AddSpellSlots(resources, dto.Level, "Bard");
                    int inspiration = GetModifier(dto.Charisma) >= 1 ? GetModifier(dto.Charisma) : 1;
                    resources.Add(new Resource { Name = "Bardic Inspiration", Current = inspiration, Max = inspiration });
                    break;

                case "Cleric":
                    AddSpellSlots(resources, dto.Level, "Cleric");
                    resources.Add(new Resource { Name = "Channel Divinity", Current = CalculateChannelDivinityUses(dto.Level), Max = CalculateChannelDivinityUses(dto.Level) });
                    break;

                case "Druid":
                    AddSpellSlots(resources, dto.Level, "Druid");
                    resources.Add(new Resource { Name = "Wild Shape", Current = 2, Max = 2 });
                    break;

                case "Fighter":
                    if (dto.Level >= 2) // Starting Indomitable feature at Level 2
                    {
                        resources.Add(new Resource { Name = "Second Wind", Current = 1, Max = 1 });
                    }
                    break;

                case "Paladin":
                    AddSpellSlots(resources, dto.Level, "Paladin");
                    resources.Add(new Resource { Name = "Lay on Hands", Current = dto.Level * 5, Max = dto.Level * 5 });
                    resources.Add(new Resource { Name = "Oath Charges", Current = CalculateOathCharges(dto.Level), Max = CalculateOathCharges(dto.Level) });
                    break;

                case "Monk":
                    resources.Add(new Resource { Name = "Ki Points", Current = dto.Level, Max = dto.Level });
                    break;

                case "Ranger":
                    AddSpellSlots(resources, dto.Level, "Ranger");
                    break;

                case "Rogue":
                    if (dto.Level >= 3)
                    {
                        resources.Add(new Resource { Name = "Cunning Action", Current = 1, Max = 1 });
                    }
                    break;

                case "Sorcerer":
                    AddSpellSlots(resources, dto.Level, "Sorcerer");
                    resources.Add(new Resource { Name = "Sorcery Points", Current = dto.Level, Max = dto.Level });
                    break;

                case "Warlock":
                    AddWarlockSpellSlots(resources, dto.Level);
                    break;

                case "Wizard":
                    AddSpellSlots(resources, dto.Level, "Wizard");
                    break;

                default:
                    throw new ArgumentException("Unknown class: " + dto.Class);
            }

            return resources;
        }

        private static int CalculateOathCharges(int level)
        {
            return level < 5 ? 1 : 2; // Example logic; adjust per subclass/SRD details
        }

        private static void AddSpellSlots(ObservableCollection<Resource> resources, int level, string className)
        {
            // Assign spell slots based on caster type
            var fullCasterSlots = new[] { new[] { 2 }, new[] { 3 }, new[] { 4, 2 }, new[] { 4, 3 }, new[] { 4, 3, 2 }, new[] { 4, 3, 3 }, new[] { 4, 3, 3, 1 }, new[] { 4, 3, 3, 2 }, new[] { 4, 3, 3, 3, 1 }, new[] { 4, 3, 3, 3, 2 }, new[] { 4, 3, 3, 3, 2, 1 }, new[] { 4, 3, 3, 3, 2, 1 }, new[] { 4, 3, 3, 3, 2, 1, 1 }, new[] { 4, 3, 3, 3, 2, 1, 1 }, new[] { 4, 3, 3, 3, 2, 1, 1, 1 }, new[] { 4, 3, 3, 3, 2, 1, 1, 1 }, new[] { 4, 3, 3, 3, 2, 1, 1, 1, 1 }, new[] { 4, 3, 3, 3, 3, 1, 1, 1, 1 }, new[] { 4, 3, 3, 3, 3, 2, 1, 1, 1 }, new[] { 4, 3, 3, 3, 3, 2, 2, 1, 1 } };
            var halfCasterSlots = new[] { new[] { 0 }, new[] { 2 }, new[] { 3 }, new[] { 3 }, new[] { 4, 2 }, new[] { 4, 2 }, new[] { 4, 3 }, new[] { 4, 3 }, new[] { 4, 3, 2 }, new[] { 4, 3, 2 }, new[] { 4, 3, 3 }, new[] { 4, 3, 3 }, new[] { 4, 3, 3, 1 }, new[] { 4, 3, 3, 1 }, new[] { 4, 3, 3, 2 }, new[] { 4, 3, 3, 2 }, new[] { 4, 3, 3, 3, 1 }, new[] { 4, 3, 3, 3, 1 }, new[] { 4, 3, 3, 3, 2 }, new[] { 4, 3, 3, 3, 2 } };

            int[][] spellSlots;

            if (level > 20)
            {
                level = 20; // Cap at level 20
            }

            if (className == "Paladin" || className == "Ranger")
            {
                spellSlots = halfCasterSlots;
                level = (int)Math.Ceiling(level / 2.0); // Half caster level
            }
            else if (className == "Bard" || className == "Cleric" || className == "Druid" || className == "Sorcerer" || className == "Wizard")
            {
                spellSlots = fullCasterSlots;
            }
            else
            {
                return; // Non-casters don't have spell slots
            }

            foreach (var (slotCount, index) in spellSlots[level - 1].Select((slots, i) => (slots, i + 1)))
            {
                resources.Add(new Resource { Name = $"Spell Slots ({index} Level)", Current = slotCount, Max = slotCount });
            }
        }

        private static void AddWarlockSpellSlots(ObservableCollection<Resource> resources, int level)
        {
            // Example for Warlock-specific pact slots (SRD compliant)
            int slots = level >= 2 ? 2 : 1;
            int slotLevel = level <= 10 ? (level + 1) / 2 : 5;
            resources.Add(new Resource { Name = "Pact Magic Slots", Current = slots, Max = slots });
            resources.Add(new Resource { Name = "Pact Magic Slot Level", Current = slotLevel, Max = slotLevel });
        }

        private static int CalculateBarbarianRages(int level)
        {
            if (level < 3) return 2;
            if (level < 6) return 3;
            if (level < 12) return 4;
            return level < 17 ? 5 : int.MaxValue; // Unlimited rages at level 20
        }

        private static int CalculateChannelDivinityUses(int level)
        {
            return level < 6 ? 1 : 2;
        }

        private static ObservableCollection<SavingThrow> InitializeSavingThrows(ObservableCollection<string> proficiencies, ObservableCollection<AbilityScore> abilityScores)
        {
            return abilityScores.Select(ascore => new SavingThrow
            {
                Name = ascore.Name,
                Proficient = proficiencies.Contains(ascore.Name),
                Modifier = ascore.Modifier + (proficiencies.Contains(ascore.Name) ? CalculateProficiencyBonus(1) : 0) // Adjust level for proficiency bonus
            }).ToObservableCollection();
        }
        private static ObservableCollection<Skill> InitializeSkills(ObservableCollection<string> proficiencies, ObservableCollection<AbilityScore> abilityScores, int proficiencyBonus)
        {
            var skills = new ObservableCollection<Skill>();

            // Map skills to corresponding ability scores (example for simplicity)
            var skillMapping = new Dictionary<string, string>
            {
                { "Athletics", "Strength" },
                { "Acrobatics", "Dexterity" },
                { "Sleight of Hand", "Dexterity" },
                { "Stealth", "Dexterity" },
                { "Arcana", "Intelligence" },
                { "History", "Intelligence" },
                { "Investigation", "Intelligence" },
                { "Nature", "Intelligence" },
                { "Religion", "Intelligence" },
                { "Animal Handling", "Wisdom" },
                { "Insight", "Wisdom" },
                { "Medicine", "Wisdom" },
                { "Perception", "Wisdom" },
                { "Survival", "Wisdom" },
                { "Deception", "Charisma" },
                { "Intimidation", "Charisma" },
                { "Performance", "Charisma" },
                { "Persuasion", "Charisma" }
            };

            foreach (var skillName in skillMapping.Keys)
            {
                var abilityScore = abilityScores.First(a => a.Name == skillMapping[skillName]);
                skills.Add(new Skill
                {
                    Name = skillName,
                    Proficiency = proficiencies.Contains(skillName),
                    Modifier = abilityScore.Modifier + (proficiencies.Contains(skillName) ? proficiencyBonus : 0)
                });
            }

            return skills;
        }

        private static string GetSpellCastingAbilityScore(string className)
        {
            return className switch
            {
                "Bard" => "Charisma",
                "Cleric" => "Wisdom",
                "Druid" => "Wisdom",
                "Paladin" => "Charisma",
                "Ranger" => "Wisdom",
                "Sorcerer" => "Charisma",
                "Warlock" => "Charisma",
                "Wizard" => "Intelligence",
                _ => throw new ArgumentException("Unknown class: " + className)
            };
        }

        private static int GetSpellCastingAbilityScoreModifier(CreateDndCharacterDto dto)
        {
            var score = dto.Class switch
            {
                "Bard" => dto.Charisma,
                "Cleric" => dto.Wisdom,
                "Druid" => dto.Wisdom,
                "Paladin" => dto.Charisma,
                "Ranger" => dto.Wisdom,
                "Sorcerer" => dto.Charisma,
                "Warlock" => dto.Charisma,
                "Wizard" => dto.Intelligence,
                _ => throw new ArgumentException("Unknown class: " + dto.Class)
            };

            return GetModifier(score);
        }
    }

}
