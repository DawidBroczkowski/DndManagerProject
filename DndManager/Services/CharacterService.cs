using DndManager.Dtos;
using DndManager.Models;
using DndManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DndManager.Services
{
    public class CharacterService
    {
        private Guid? _currentCampaingId;
        private Character? _character;

        public int StrengthModifier => _character?.AbilityScores.FirstOrDefault(x => x.Name == "Strength")?.Modifier ?? 0;
        public int DexterityModifier => _character?.AbilityScores.FirstOrDefault(x => x.Name == "Dexterity")?.Modifier ?? 0;
        public int ConstitutionModifier => _character?.AbilityScores.FirstOrDefault(x => x.Name == "Constitution")?.Modifier ?? 0;
        public int IntelligenceModifier => _character?.AbilityScores.FirstOrDefault(x => x.Name == "Intelligence")?.Modifier ?? 0;
        public int WisdomModifier => _character?.AbilityScores.FirstOrDefault(x => x.Name == "Wisdom")?.Modifier ?? 0;
        public int CharismaModifier => _character?.AbilityScores.FirstOrDefault(x => x.Name == "Charisma")?.Modifier ?? 0;

        public int StrengthSavingThrow => _character?.SavingThrows.FirstOrDefault(x => x.Name == "Strength")?.Modifier ?? 0;
        public int DexteritySavingThrow => _character?.SavingThrows.FirstOrDefault(x => x.Name == "Dexterity")?.Modifier ?? 0;
        public int ConstitutionSavingThrow => _character?.SavingThrows.FirstOrDefault(x => x.Name == "Constitution")?.Modifier ?? 0;
        public int IntelligenceSavingThrow => _character?.SavingThrows.FirstOrDefault(x => x.Name == "Intelligence")?.Modifier ?? 0;
        public int WisdomSavingThrow => _character?.SavingThrows.FirstOrDefault(x => x.Name == "Wisdom")?.Modifier ?? 0;
        public int CharismaSavingThrow => _character?.SavingThrows.FirstOrDefault(x => x.Name == "Charisma")?.Modifier ?? 0;

        public CharacterService()
        {
            _currentCampaingId = null;
            _character = null;
        }

        public void InitializeCharacter(Character character)
        {
            _character = character;
        }

        public Character? GetCharacter()
        {
            return _character;
        }

        public void SetCampaignId(Guid campaignId)
        {
            _currentCampaingId = campaignId;
        }

        public bool TryUpdateCharacter(UpdateCharacterDto dto)
        {
            if (_character is null)
            {
                return false;
            }

            _character.Update(dto);

            return true;
        }
    }
}
