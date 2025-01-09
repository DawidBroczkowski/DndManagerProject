using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DndManager.Models
{
    public record Skill : INotifyPropertyChanged
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

        private bool _proficiency;
        public bool Proficiency
        {
            get => _proficiency;
            set
            {
                _proficiency = value;
                RaisePropertyChanged(nameof(Proficiency));
            }
        }

        private bool _expertise;
        public bool Expertise
        {
            get => _expertise;
            set
            {
                _expertise = value;
                RaisePropertyChanged(nameof(Expertise));
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
    }
}