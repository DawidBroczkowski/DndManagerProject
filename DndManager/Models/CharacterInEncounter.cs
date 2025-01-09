using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DndManager.Models
{
    public record CharacterInEncounter : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Character? Character { get; set; }

        private int _initiative = 0;
        public int Initiative
        {
            get => _initiative;
            set
            {
                _initiative = value;
                RaisePropertyChanged(nameof(Initiative));
            }
        }

        private bool _isPlayerCharacter = false;
        public bool IsPlayerCharacter
        {
            get => _isPlayerCharacter;
            set
            {
                _isPlayerCharacter = value;
                RaisePropertyChanged(nameof(IsPlayerCharacter));
            }
        }

        private bool _isCurrentTurn = false;
        public bool IsCurrentTurn
        {
            get => _isCurrentTurn;
            set
            {
                _isCurrentTurn = value;
                RaisePropertyChanged(nameof(IsCurrentTurn));
            }
        }

        private int _order = 0;
        public int Order
        {
            get => _order;
            set
            {
                _order = value;
                RaisePropertyChanged(nameof(Order));
            }
        }
    }
}
