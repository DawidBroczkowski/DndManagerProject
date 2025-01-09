using DndManager.Dtos;
using DndManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DndManager.Utility
{
    public static class Extensions
    {

        public static UpdateCharacterDto ToUpdateDto(this Character original, Character updated)
        {
            var dto = new UpdateCharacterDto
            {
                Id = original!.Id,
                Name = original.Name == updated.Name ? null : updated.Name,
                Race = original.Race == updated.Race ? null : updated.Race,
                Class = original.Class == updated.Class ? null : updated.Class,
                Subclass = original.Subclass == updated.Subclass ? null : updated.Subclass,
                Level = original.Level == updated.Level ? (int?)null : updated.Level,
                Background = original.Background == updated.Background ? null : updated.Background,
                Alignment = original.Alignment == updated.Alignment ? null : updated.Alignment,
                Notes = original.Notes == updated.Notes ? null : updated.Notes,
                ExperiencePoints = original.ExperiencePoints == updated.ExperiencePoints ? (int?)null : updated.ExperiencePoints,
                ArmorClass = original.ArmorClass == updated.ArmorClass ? (int?)null : updated.ArmorClass,
                Initiative = original.Initiative == updated.Initiative ? (int?)null : updated.Initiative,
                Speed = original.Speed == updated.Speed ? (int?)null : updated.Speed,
                HitPoints = original.HitPoints == updated.HitPoints ? (int?)null : updated.HitPoints,
                HitPointsMax = original.HitPointsMax == updated.HitPointsMax ? (int?)null : updated.HitPointsMax,
                HitPointsTemporary = original.HitPointsTemporary == updated.HitPointsTemporary ? (int?)null : updated.HitPointsTemporary,
                HitDice = original.HitDice == updated.HitDice ? (int?)null : updated.HitDice,
                HitDiceMax = original.HitDiceMax == updated.HitDiceMax ? (int?)null : updated.HitDiceMax,
                ProficiencyBonus = original.ProficiencyBonus == updated.ProficiencyBonus ? (int?)null : updated.ProficiencyBonus,
                DeathSavesSuccesses = original.DeathSavesSuccesses == updated.DeathSavesSuccesses ? (int?)null : updated.DeathSavesSuccesses,
                DeathSavesFailures = original.DeathSavesFailures == updated.DeathSavesFailures ? (int?)null : updated.DeathSavesFailures,
                PassivePerception = original.PassivePerception == updated.PassivePerception ? (int?)null : updated.PassivePerception,
                PassiveInsight = original.PassiveInsight == updated.PassiveInsight ? (int?)null : updated.PassiveInsight,
                SpellSaveDC = original.SpellSaveDC == updated.SpellSaveDC ? (int?)null : updated.SpellSaveDC,
                AbilityScores = original.AbilityScores.SequenceEqual(updated.AbilityScores) ? null : updated.AbilityScores,
                SavingThrows = original.SavingThrows.SequenceEqual(updated.SavingThrows) ? null : updated.SavingThrows,
                Skills = original.Skills.SequenceEqual(updated.Skills) ? null : updated.Skills,
                Currency = original.Currency.SequenceEqual(updated.Currency) ? null : updated.Currency,
                Resources = original.Resources.SequenceEqual(updated.Resources) ? null : updated.Resources,
                Attacks = original.Attacks.SequenceEqual(updated.Attacks) ? null : updated.Attacks,
                SkillProficiencies = original.SkillProficiencies.SequenceEqual(updated.SkillProficiencies) ? null : updated.SkillProficiencies,
                SkillExpertises = original.SkillExpertises.SequenceEqual(updated.SkillExpertises) ? null : updated.SkillExpertises,
                OtherProficiencies = original.OtherProficiencies.SequenceEqual(updated.OtherProficiencies) ? null : updated.OtherProficiencies,
                OtherExpertises = original.OtherExpertises.SequenceEqual(updated.OtherExpertises) ? null : updated.OtherExpertises,
                Languages = original.Languages.SequenceEqual(updated.Languages) ? null : updated.Languages,
                Equipment = original.Equipment.SequenceEqual(updated.Equipment) ? null : updated.Equipment,
                Conditions = original.Conditions.SequenceEqual(updated.Conditions) ? null : updated.Conditions,
                Vulnerabilities = original.Vulnerabilities.SequenceEqual(updated.Vulnerabilities) ? null : updated.Vulnerabilities,
                DamageResistances = original.DamageResistances.SequenceEqual(updated.DamageResistances) ? null : updated.DamageResistances,
                DamageImmunities = original.DamageImmunities.SequenceEqual(updated.DamageImmunities) ? null : updated.DamageImmunities,
                ConditionImmunities = original.ConditionImmunities.SequenceEqual(updated.ConditionImmunities) ? null : updated.ConditionImmunities,
                Senses = original.Senses.SequenceEqual(updated.Senses) ? null : updated.Senses
            };

            return dto;
        }

        public static void Update(this Character original, UpdateCharacterDto dto)
        {
            original.Name = dto.Name ?? original.Name;
            original.Race = dto.Race ?? original.Race;
            original.Class = dto.Class ?? original.Class;
            original.Subclass = dto.Subclass ?? original.Subclass;
            original.Level = dto.Level ?? original.Level;
            original.Background = dto.Background ?? original.Background;
            original.Alignment = dto.Alignment ?? original.Alignment;
            original.Notes = dto.Notes ?? original.Notes;
            original.ExperiencePoints = dto.ExperiencePoints ?? original.ExperiencePoints;
            original.ArmorClass = dto.ArmorClass ?? original.ArmorClass;
            original.Initiative = dto.Initiative ?? original.Initiative;
            original.Speed = dto.Speed ?? original.Speed;
            original.HitPoints = dto.HitPoints ?? original.HitPoints;
            original.HitPointsMax = dto.HitPointsMax ?? original.HitPointsMax;
            original.HitPointsTemporary = dto.HitPointsTemporary ?? original.HitPointsTemporary;
            original.HitDice = dto.HitDice ?? original.HitDice;
            original.HitDiceMax = dto.HitDiceMax ?? original.HitDiceMax;
            original.ProficiencyBonus = dto.ProficiencyBonus ?? original.ProficiencyBonus;
            original.DeathSavesSuccesses = dto.DeathSavesSuccesses ?? original.DeathSavesSuccesses;
            original.DeathSavesFailures = dto.DeathSavesFailures ?? original.DeathSavesFailures;
            original.PassivePerception = dto.PassivePerception ?? original.PassivePerception;
            original.PassiveInsight = dto.PassiveInsight ?? original.PassiveInsight;
            original.SpellSaveDC = dto.SpellSaveDC ?? original.SpellSaveDC;
            if (dto.AbilityScores != null)
            {
                original.AbilityScores = dto.AbilityScores;
            }

            if (dto.SavingThrows != null)
            {
                original.SavingThrows = dto.SavingThrows;
            }

            if (dto.Skills != null)
            {
                original.Skills = dto.Skills;
            }

            if (dto.Currency != null)
            {
                original.Currency = dto.Currency;
            }

            if (dto.Resources != null)
            {
                original.Resources = dto.Resources;
            }

            if (dto.Attacks != null)
            {
                original.Attacks = dto.Attacks;
            }

            if (dto.SkillProficiencies != null)
            {
                original.SkillProficiencies = dto.SkillProficiencies;
            }

            if (dto.SkillExpertises != null)
            {
                original.SkillExpertises = dto.SkillExpertises;
            }

            if (dto.OtherProficiencies != null)
            {
                original.OtherProficiencies = dto.OtherProficiencies;
            }

            if (dto.OtherExpertises != null)
            {
                original.OtherExpertises = dto.OtherExpertises;
            }

            if (dto.Languages != null)
            {
                original.Languages = dto.Languages;
            }

            if (dto.Equipment != null)
            {
                original.Equipment = dto.Equipment;
            }

            if (dto.Conditions != null)
            {
                original.Conditions = dto.Conditions;
            }

            if (dto.Vulnerabilities != null)
            {
                original.Vulnerabilities = dto.Vulnerabilities;
            }

            if (dto.DamageResistances != null)
            {
                original.DamageResistances = dto.DamageResistances;
            }

            if (dto.DamageImmunities != null)
            {
                original.DamageImmunities = dto.DamageImmunities;
            }

            if (dto.ConditionImmunities != null)
            {
                original.ConditionImmunities = dto.ConditionImmunities;
            }

            if (dto.Senses != null)
            {
                original.Senses = dto.Senses;
            }

            if (dto.ConditionImmunities != null)
            {
                original.ConditionImmunities = dto.ConditionImmunities;
            }
        }
    }
}
