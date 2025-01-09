using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DndManager.Models
{
    public record Resource : INotifyPropertyChanged
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

        private int _current;
        public int Current
        {
            get => _current;
            set
            {
                _current = value;
                RaisePropertyChanged(nameof(Current));
            }
        }

        private int _max;
        public int Max
        {
            get => _max;
            set
            {
                _max = value;
                RaisePropertyChanged(nameof(Max));
            }
        }
    }
}