using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DndManager.Models
{
    public record CharacterInCampaign : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _playerName = string.Empty;
        public string PlayerName
        {
            get => _playerName;
            set
            {
                _playerName = value;
                RaisePropertyChanged(nameof(PlayerName));
            }
        }

        private Guid _playerId;
        public Guid PlayerId
        {
            get => _playerId;
            set
            {
                _playerId = value;
                RaisePropertyChanged(nameof(PlayerId));
            }
        }

        private Character? _character;
        public Character? Character
        {
            get => _character;
            set
            {
                _character = value;
                RaisePropertyChanged(nameof(Character));
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
    }
}