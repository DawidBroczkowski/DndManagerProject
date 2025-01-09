using DndManager.Models;
using System.Collections.ObjectModel;

namespace DndManager.Services
{
    public class EncounterService
    {
        public Encounter? Encounter { get; set; }
        private int _currentTurn = 0;

        public void InitializeEncounter(Encounter encounter)
        {
            Encounter = encounter;
            _currentTurn = 0;
        }

        public void AddCharacter(Character character)
        {
            CharacterInEncounter characterInCampaign = new CharacterInEncounter
            {
                Character = character,
                IsPlayerCharacter = true
            };

            Encounter?.Characters.Add(characterInCampaign);
        }

        public bool TryRemoveCharacter(Guid characterId)
        {
            if (Encounter is null)
            {
                return false;
            }
            return Encounter.Characters.Remove(Encounter.Characters.FirstOrDefault(x => x.Character!.Id == characterId)!);
        }

        public void RollInitiative()
        {
            if (Encounter is null)
            {
                return;
            }

            foreach (var character in Encounter.Characters)
            {
                DiceRoll roll = new DiceRoll
                {
                    DiceSize = 20,
                    Modifier = character.Character!.Initiative,
                    NumberOfDice = 1
                };
                character.Initiative = roll.Roll();
            }

            var orderedCharacters = Encounter.Characters.OrderByDescending(x => x.Initiative).ToList();
            Encounter.Characters = new ObservableCollection<CharacterInEncounter>(orderedCharacters);
            Encounter.Characters[0].IsCurrentTurn = true;
        }

        public void NextTurn()
        {
            if (Encounter is null)
            {
                return;
            }

            Encounter.Characters[_currentTurn].IsCurrentTurn = false;
            _currentTurn = (_currentTurn + 1) % Encounter.Characters.Count;
            Encounter.Characters[_currentTurn].IsCurrentTurn = true;
        }
    }
}
