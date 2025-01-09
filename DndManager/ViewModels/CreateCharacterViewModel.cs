using CommunityToolkit.Maui.Core.Extensions;
using DndManager.Accessor;
using DndManager.DndApplication;
using DndManager.Dtos;
using DndManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DndManager.ViewModels
{
    public class CreateCharacterViewModel : INotifyPropertyChanged
    {
        private IServiceProvider _serviceProvider;
        private Accessor<ApplicationContext> _context;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void RaisePropertyChanged(string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public CreateDndCharacterDto Character { get; set; } = new CreateDndCharacterDto();

        public ICommand SubmitCommand { get; set; }

        public ObservableCollection<string> Languages { get; set; } = new();
        public ObservableCollection<Item> Equipment { get; } = new ();
        public List<string> SavingThrowProficiencies { get; set; } = new();
        public ObservableCollection<string> SkillProficiencies { get; set; } = new();
        public ObservableCollection<string> SkillExpertises { get; set; } = new();
        public ObservableCollection<string> OtherProficiencies { get; set; } = new();
        public ObservableCollection<string> OtherExpertises { get; set; } = new();

        public string NewLanguage { get; set; }
        public Item NewItem { get; set; } = new();
        public string NewSkillProficiency { get; set; }
        public string NewSkillExpertise { get; set; }
        public string NewOtherProficiency { get; set; }
        public string NewOtherExpertise { get; set; }

        public Command AddLanguageCommand { get; }
        public Command<string> RemoveLanguageCommand { get; }

        public ICommand AddItemCommand => new Command(() =>
        {
            if (!string.IsNullOrWhiteSpace(NewItem.Name) &&
                !string.IsNullOrWhiteSpace(NewItem.Description) &&
                NewItem.Quantity > 0)
            {
                Equipment.Add(new Item
                {
                    Name = NewItem.Name,
                    Description = NewItem.Description,
                    Quantity = NewItem.Quantity,
                    IsMagical = NewItem.IsMagical,
                    IsEquipped = NewItem.IsEquipped
                });
                NewItem = new Item(); // Reset form
                RaisePropertyChanged(nameof(NewItem));
            }
        });

        public ICommand RemoveItemCommand => new Command<Item>(item =>
        {
            if (Equipment.Contains(item))
            {
                Equipment.Remove(item);
            }
        });

        public ICommand AddSkillProficiencyCommand => new Command(() =>
        {
            if (!string.IsNullOrWhiteSpace(NewSkillProficiency))
            {
                SkillProficiencies.Add(NewSkillProficiency);
                NewSkillProficiency = string.Empty;
                RaisePropertyChanged(nameof(NewSkillProficiency));
            }
        });

        public ICommand RemoveSkillProficiencyCommand => new Command<string>(skill =>
        {
            if (SkillProficiencies.Contains(skill))
                SkillProficiencies.Remove(skill);
        });

        public ICommand AddSkillExpertiseCommand => new Command(() =>
        {
            if (!string.IsNullOrWhiteSpace(NewSkillExpertise))
            {
                SkillExpertises.Add(NewSkillExpertise);
                NewSkillExpertise = string.Empty;
                RaisePropertyChanged(nameof(NewSkillExpertise));
            }
        });

        public ICommand RemoveSkillExpertiseCommand => new Command<string>(skill =>
        {
            if (SkillExpertises.Contains(skill))
                SkillExpertises.Remove(skill);
        });

        public ICommand AddOtherProficiencyCommand => new Command(() =>
        {
            if (!string.IsNullOrWhiteSpace(NewOtherProficiency))
            {
                OtherProficiencies.Add(NewOtherProficiency);
                NewOtherProficiency = string.Empty;
                RaisePropertyChanged(nameof(NewOtherProficiency));
            }
        });

        public ICommand RemoveOtherProficiencyCommand => new Command<string>(proficiency =>
        {
            if (OtherProficiencies.Contains(proficiency))
                OtherProficiencies.Remove(proficiency);
        });

        public ICommand AddOtherExpertiseCommand => new Command(() =>
        {
            if (!string.IsNullOrWhiteSpace(NewOtherExpertise))
            {
                OtherExpertises.Add(NewOtherExpertise);
                NewOtherExpertise = string.Empty;
                RaisePropertyChanged(nameof(NewOtherExpertise));
            }
        });

        public ICommand RemoveOtherExpertiseCommand => new Command<string>(expertise =>
        {
            if (OtherExpertises.Contains(expertise))
                OtherExpertises.Remove(expertise);
        });

        private void UpdateProficiency(string ability, bool isProficient)
        {
            if (isProficient)
            {
                if (!SavingThrowProficiencies.Contains(ability))
                    SavingThrowProficiencies.Add(ability);
            }
            else
            {
                SavingThrowProficiencies.Remove(ability);
            }
        }

        // Properties for Switch Bindings
        private bool _isStrengthProficient;
        public bool IsStrengthProficient
        {
            get => _isStrengthProficient;
            set
            {
                if (_isStrengthProficient != value)
                {
                    _isStrengthProficient = value;
                    UpdateProficiency("Strength", value);
                    RaisePropertyChanged(nameof(IsStrengthProficient));
                }
            }
        }

        private bool _isDexterityProficient;
        public bool IsDexterityProficient
        {
            get => _isDexterityProficient;
            set
            {
                if (_isDexterityProficient != value)
                {
                    _isDexterityProficient = value;
                    UpdateProficiency("Dexterity", value);
                    RaisePropertyChanged(nameof(IsDexterityProficient));
                }
            }
        }

        private bool _isConstitutionProficient;
        public bool IsConstitutionProficient
        {
            get => _isConstitutionProficient;
            set
            {
                if (_isConstitutionProficient != value)
                {
                    _isConstitutionProficient = value;
                    UpdateProficiency("Constitution", value);
                    RaisePropertyChanged(nameof(IsConstitutionProficient));
                }
            }
        }

        private bool _isIntelligenceProficient;
        public bool IsIntelligenceProficient
        {
            get => _isIntelligenceProficient;
            set
            {
                if (_isIntelligenceProficient != value)
                {
                    _isIntelligenceProficient = value;
                    UpdateProficiency("Intelligence", value);
                    RaisePropertyChanged(nameof(IsIntelligenceProficient));
                }
            }
        }

        private bool _isWisdomProficient;
        public bool IsWisdomProficient
        {
            get => _isWisdomProficient;
            set
            {
                if (_isWisdomProficient != value)
                {
                    _isWisdomProficient = value;
                    UpdateProficiency("Wisdom", value);
                    RaisePropertyChanged(nameof(IsWisdomProficient));
                }
            }
        }

        private bool _isCharismaProficient;
        public bool IsCharismaProficient
        {
            get => _isCharismaProficient;
            set
            {
                if (_isCharismaProficient != value)
                {
                    _isCharismaProficient = value;
                    UpdateProficiency("Charisma", value);
                    RaisePropertyChanged(nameof(IsCharismaProficient));
                }
            }
        }

        public CreateCharacterViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _context = _serviceProvider.GetRequiredService<Accessor<ApplicationContext>>();

            SubmitCommand = new Command(OnSubmit);

            AddLanguageCommand = new Command(() =>
            {
                if (!string.IsNullOrWhiteSpace(NewLanguage))
                {
                    Languages.Add(NewLanguage);
                    NewLanguage = string.Empty;
                    RaisePropertyChanged(nameof(NewLanguage));
                }
            });

            RemoveLanguageCommand = new Command<string>((language) =>
            {
                if (Languages.Contains(language))
                    Languages.Remove(language);
            });
        }

        public void Initialize()
        {
            Character = new CreateDndCharacterDto();
            RaisePropertyChanged();
        }

        private async void OnSubmit()
        {
            DndCharacterFactory factory = new();

            Character.SavingThrowProficiencies = SavingThrowProficiencies.ToObservableCollection();
            Character.SkillProficiencies = SkillProficiencies.ToObservableCollection();
            Character.SkillExpertises = SkillExpertises.ToObservableCollection();
            Character.OtherProficiencies = OtherProficiencies.ToObservableCollection();
            Character.OtherExpertises = OtherExpertises.ToObservableCollection();
            Character.Languages = Languages.ToObservableCollection();
            Character.Equipment = Equipment.ToObservableCollection();

            Character character;
            try
            {
                character = factory.CreateCharacter(Character);
            }
            catch (Exception ex)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", ex.Message, "OK");
                return;
            }
            _context!.Value!.GetPlayerUser().Characters.Add(character);
            await _context.Value!.SaveUserAsync();

            await App.Current!.MainPage!.DisplayAlert("Success", "Character created successfully!", "OK");
            await Shell.Current.GoToAsync("//MainPage/PlayerMainPage");
        }
    }
}
