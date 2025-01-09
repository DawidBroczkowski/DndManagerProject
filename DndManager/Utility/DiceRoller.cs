 using DndManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DndManager.Utility
{
    public class DiceRoller
    {
        public ObservableCollection<DiceRollEntry> DiceRolls { get; set; } = new();
        public int FinalModifier { get; set; } = 0;

        public void AddDiceRoll(DiceRoll diceRoll, string special = "")
        {
            DiceRollEntry entry = new DiceRollEntry
            {
                Roll = diceRoll,
                Special = special
            };

            DiceRolls.Add(entry);
        }

        public int Roll()
        {
            int result = 0;
            foreach (var diceRoll in DiceRolls)
            {
                if (diceRoll.Special == "advantage")
                {
                    result += Math.Max(diceRoll.Roll.Roll(), diceRoll.Roll.Roll());
                }
                else if (diceRoll.Special == "disadvantage")
                {
                    result += Math.Min(diceRoll.Roll.Roll(), diceRoll.Roll.Roll());
                }
                else
                {
                    result += diceRoll.Roll.Roll();
                }   
            }
            return result + FinalModifier;
        }
    }
    public class DiceRollEntry
    {
        public DiceRoll Roll { get; set; } = new();
        public string Special { get; set; } = string.Empty;
    }


}
