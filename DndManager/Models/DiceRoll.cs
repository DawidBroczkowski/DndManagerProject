using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DndManager.Models
{
    public class DiceRoll : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        private int _numberOfDice;
        public int NumberOfDice
        {
            get => _numberOfDice;
            set
            {
                _numberOfDice = value;
                RaisePropertyChanged(nameof(NumberOfDice));
            }
        }

        private int _diceSize;
        public int DiceSize
        {
            get => _diceSize;
            set
            {
                _diceSize = value;
                RaisePropertyChanged(nameof(DiceSize));
            }
        }

        private int _modifier;
        public int Modifier
        {
            get => _modifier;
            set
            {
                _modifier = value;
                RaisePropertyChanged(nameof(Modifier));
            }
        }

        public int Roll()
        {
            var total = 0;
            for (var i = 0; i < NumberOfDice; i++)
            {
                total += RandomNumberGenerator.GetInt32(1, DiceSize + 1);
            }
            total += Modifier;
            return total;
        }
    }
}