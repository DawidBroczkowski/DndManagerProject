using DndManager.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DndManager.Models
{
    public record Spell : INotifyPropertyChanged
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

        private Resource _resource = new Resource();
        public Resource Resource
        {
            get => _resource;
            set
            {
                _resource = value;
                RaisePropertyChanged(nameof(Resource));
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

        private string _school = string.Empty;
        public string School
        {
            get => _school;
            set
            {
                _school = value;
                RaisePropertyChanged(nameof(School));
            }
        }

        private string _castingTime = string.Empty;
        public string CastingTime
        {
            get => _castingTime;
            set
            {
                _castingTime = value;
                RaisePropertyChanged(nameof(CastingTime));
            }
        }

        private string _range = string.Empty;
        public string Range
        {
            get => _range;
            set
            {
                _range = value;
                RaisePropertyChanged(nameof(Range));
            }
        }

        private string _duration = string.Empty;
        public string Duration
        {
            get => _duration;
            set
            {
                _duration = value;
                RaisePropertyChanged(nameof(Duration));
            }
        }

        private string _components = string.Empty;
        public string Components
        {
            get => _components;
            set
            {
                _components = value;
                RaisePropertyChanged(nameof(Components));
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
        public List<DiceRoll> DiceRolls { get; set; } = new List<DiceRoll>();
    }
}