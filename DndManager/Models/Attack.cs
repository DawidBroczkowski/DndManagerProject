using DndManager.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DndManager.Models
{
    public record Attack : INotifyPropertyChanged
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

        private int _bonus;
        public int Bonus
        {
            get => _bonus;
            set
            {
                _bonus = value;
                RaisePropertyChanged(nameof(Bonus));
            }
        }

        private DiceRoller _diceRoller = new DiceRoller();
        public DiceRoller DiceRoller
        {
            get => _diceRoller;
            set
            {
                _diceRoller = value;
                RaisePropertyChanged(nameof(DiceRoller));
            }
        }
    }
}