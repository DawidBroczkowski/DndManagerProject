using DndManager.Dtos;
using DndManager.Models;
using DndManager.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DndManager.Services
{
    public class CampaignService
    {
        public Campaign? Campaign { get; set; }

        public void InitializeCampaign(Campaign campaign)
        {
            Campaign = campaign;
        }

        public void AddCharacter(Guid playerId, string playerName, Character character)
        {
            CharacterInCampaign characterInCampaign = new CharacterInCampaign
            {
                Character = character,
                PlayerId = playerId,
                PlayerName = playerName
            };
            Campaign?.Characters.Add(characterInCampaign);
        }

        public bool TryRemoveCharacter(Guid characterId)
        {
            if (Campaign is null)
            {
                return false;
            }
            return Campaign.Characters.Remove(Campaign.Characters.FirstOrDefault(x => x.Character!.Id == characterId)!);
        }

        public bool TryUpdateCharacter(UpdateCharacterDto updateCharacterDto)
        {
            if (Campaign is null)
            {
                return false;
            }
            var characterInCampaign = Campaign.Characters.FirstOrDefault(x => x.Character!.Id == updateCharacterDto.Id);
            if (characterInCampaign is null)
            {
                return false;
            }

            characterInCampaign.Character!.Update(updateCharacterDto);

            return true;
        }

        public bool TryUpdateCharacter(Character character)
        {
            if (Campaign is null)
            {
                return false;
            }
            var characterInCampaign = Campaign.Characters.FirstOrDefault(x => x.Character!.Id == character.Id);
            if (characterInCampaign is null)
            {
                return false;
            }

            characterInCampaign.Character = character;

            return true;
        }

        public ObservableCollection<CharacterInCampaign> GetCharacters()
        {
            if (Campaign is null)
            {
                return new();
            }
            return Campaign.Characters;
        }

        public CharacterInCampaign? GetCharacter(Guid characterId)
        {
            if (Campaign is null)
            {
                return null;
            }
            return Campaign.Characters.FirstOrDefault(x => x.Character!.Id == characterId);
        }
    }
}
