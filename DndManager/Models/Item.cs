using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DndManager.Models
{
    public record Item : INotifyPropertyChanged
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

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                RaisePropertyChanged(nameof(Description));
            }
        }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                RaisePropertyChanged(nameof(Quantity));
            }
        }

        private bool _isMagical = false;
        public bool IsMagical
        {
            get => _isMagical;
            set
            {
                _isMagical = value;
                RaisePropertyChanged(nameof(IsMagical));
            }
        }

        private bool _isEquipped = false;
        public bool IsEquipped
        {
            get => _isEquipped;
            set
            {
                _isEquipped = value;
                RaisePropertyChanged(nameof(IsEquipped));
            }
        }
    }
}