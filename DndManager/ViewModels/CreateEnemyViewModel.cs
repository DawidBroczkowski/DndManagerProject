using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Views;
using DndManager.Accessor;
using DndManager.Compontents;
using DndManager.DndApplication;
using DndManager.Dtos;
using DndManager.Models;
using DndManager.Utility;
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
    public class CreateEnemyViewModel : INotifyPropertyChanged
    {
        private IServiceProvider _serviceProvider;
        private Accessor<ApplicationContext> _context;
        private Page _page;

        public CreateDndEnemyDto Enemy { get; set; } = new CreateDndEnemyDto();  

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private bool _isMonster = false;
        public bool IsMonster
        {
            get => _isMonster;
            set
            {
                _isMonster = value;
                RaisePropertyChanged(nameof(IsMonster));
            }
        }

        // <-- General -->
        public ObservableCollection<string> Languages { get; set; } = new();
        public ObservableCollection<Item> Equipment { get; } = new();
        public List<string> SavingThrowProficiencies { get; set; } = new();
        public ObservableCollection<string> SkillProficiencies { get; set; } = new();
        public ObservableCollection<string> SkillExpertises { get; set; } = new();
        public ObservableCollection<string> Vulnerabilities { get; set; } = new();
        public ObservableCollection<string> DamageResistances { get; set; } = new();
        public ObservableCollection<string> DamageImmunities { get; set; } = new();
        public ObservableCollection<string> ConditionImmunities { get; set; } = new();
        public ObservableCollection<string> Senses { get; set; } = new();
        public string NewLanguage { get; set; }
        public Item NewItem { get; set; } = new();
        public string NewSkillProficiency { get; set; }
        public string NewSkillExpertise { get; set; }
        public string NewOtherProficiency { get; set; }
        public string NewOtherExpertise { get; set; }
        public string NewVulnerability { get; set; }
        public string NewDamageResistance { get; set; }
        public string NewDamageImmunity { get; set; }
        public string NewConditionImmunity { get; set; }
        public string NewSense { get; set; }

        public ICommand CreateEnemyCommand { get; }
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

        public ICommand AddVulnerabilityCommand => new Command(() =>
        {
            if (!string.IsNullOrWhiteSpace(NewVulnerability))
            {
                Vulnerabilities.Add(NewVulnerability);
                NewVulnerability = string.Empty;
                RaisePropertyChanged(nameof(NewVulnerability));
            }
        });

        public ICommand RemoveVulnerabilityCommand => new Command<string>(vulnerability =>
        {
            if (Vulnerabilities.Contains(vulnerability))
                Vulnerabilities.Remove(vulnerability);
        });

        public ICommand AddDamageResistanceCommand => new Command(() =>
        {
            if (!string.IsNullOrWhiteSpace(NewDamageResistance))
            {
                DamageResistances.Add(NewDamageResistance);
                NewDamageResistance = string.Empty;
                RaisePropertyChanged(nameof(NewDamageResistance));
            }
        });

        public ICommand RemoveDamageResistanceCommand => new Command<string>(resistance =>
        {
            if (DamageResistances.Contains(resistance))
                DamageResistances.Remove(resistance);
        });

        public ICommand AddDamageImmunityCommand => new Command(() =>
        {
            if (!string.IsNullOrWhiteSpace(NewDamageImmunity))
            {
                DamageImmunities.Add(NewDamageImmunity);
                NewDamageImmunity = string.Empty;
                RaisePropertyChanged(nameof(NewDamageImmunity));
            }
        });

        public ICommand RemoveDamageImmunityCommand => new Command<string>(immunity =>
        {
            if (DamageImmunities.Contains(immunity))
                DamageImmunities.Remove(immunity);
        });

        public ICommand AddConditionImmunityCommand => new Command(() =>
        {
            if (!string.IsNullOrWhiteSpace(NewConditionImmunity))
            {
                ConditionImmunities.Add(NewConditionImmunity);
                NewConditionImmunity = string.Empty;
                RaisePropertyChanged(nameof(NewConditionImmunity));
            }
        });

        public ICommand RemoveConditionImmunityCommand => new Command<string>(immunity =>
        {
            if (ConditionImmunities.Contains(immunity))
                ConditionImmunities.Remove(immunity);
        });

        public ICommand AddSenseCommand => new Command(() =>
        {
            if (!string.IsNullOrWhiteSpace(NewSense))
            {
                Senses.Add(NewSense);
                NewSense = string.Empty;
                RaisePropertyChanged(nameof(NewSense));
            }
        });

        public ICommand RemoveSenseCommand => new Command<string>(sense =>
        {
            if (Senses.Contains(sense))
                Senses.Remove(sense);
        });

        // <-- Resources -->
        public DndManager.Models.Resource NewResource { get; set; } = new DndManager.Models.Resource();
        public ObservableCollection<DndManager.Models.Resource> Resources { get; set; } = new();

        private string _name;
        public string NewResourceName
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged(nameof(NewResourceName));
                UpdateSuggestions(); // Update suggestions as user types
            }
        }

        private ObservableCollection<string> _suggestedResourceNames = new ObservableCollection<string>();
        public ObservableCollection<string> SuggestedResourceNames
        {
            get => _suggestedResourceNames;
            set
            {
                _suggestedResourceNames = value;
                RaisePropertyChanged(nameof(SuggestedResourceNames));
            }
        }

        public bool AreSuggestionsVisible => SuggestedResourceNames.Any();

        public ObservableCollection<string> CommonResourceNames { get; set; } = new ObservableCollection<string>
        {
            "Spell Slots (1st Level)",
            "Spell Slots (2nd Level)",
            "Spell Slots (3rd Level)",
            "Spell Slots (4th Level)",
            "Spell Slots (5th Level)",
            "Spell Slots (6th Level)",
            "Spell Slots (7th Level)",
            "Spell Slots (8th Level)",
            "Spell Slots (9th Level)",
            "Rage",
            "Channel Divinity",
            "Lay on Hands",
            "Ki Points",
            "Superiority Dice",
            "Sorcery Points",
            "Wild Shape",
            "Bardic Inspiration",
            "Oath Charge",
        };

        public ICommand AddResourceCommand => new Command(() =>
        {
            if (!string.IsNullOrEmpty(NewResourceName) && NewResource.Max > 0)
            {
                Resources.Add(new DndManager.Models.Resource
                {
                    Name = NewResourceName,
                    Max = NewResource.Max,
                    Current = NewResource.Max
                });
                NewResource = new DndManager.Models.Resource();
                RaisePropertyChanged(nameof(NewResource));
            }
        });

        public ICommand RemoveResourceCommand => new Command<DndManager.Models.Resource>(resource =>
        {
            if (resource != null)
                Resources.Remove(resource);
        });

        public ICommand EditResourceCommand => new Command<Resource>(resource =>
        {
            // Logic for editing a resource (e.g., showing a popup or inline edit)
        });

        public ICommand SelectSuggestionCommand => new Command<string>(suggestion =>
        {
            NewResourceName = suggestion; // Set selected suggestion
            SuggestedResourceNames.Clear(); // Hide suggestions
        });

        // <-- Attacks -->
        public ObservableCollection<Attack> Attacks { get; set; } = new ObservableCollection<Attack>();
        public Attack NewAttack { get; set; } = new Attack();

        public ICommand AddAttackCommand { get; }
        public ICommand EditAttackCommand { get; }
        public ICommand RemoveAttackCommand { get; }
        public ICommand AddDiceRollCommand { get; }
        public ICommand EditDiceRollCommand { get; }
        public ICommand RemoveDiceRollCommand { get; }

        // <-- Vulnerabilities -->


        public CreateEnemyViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _context = serviceProvider.GetRequiredService<Accessor<ApplicationContext>>();

            // Initialize commands
            AddAttackCommand = new Command(AddAttack);
            EditAttackCommand = new Command<Attack>(EditAttack);
            RemoveAttackCommand = new Command<Attack>(RemoveAttack);

            AddDiceRollCommand = new Command(async () => await AddDiceRoll());
            EditDiceRollCommand = new Command<DiceRollEntry>(EditDiceRoll);
            RemoveDiceRollCommand = new Command<DiceRollEntry>(RemoveDiceRoll);

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

            CreateEnemyCommand = new Command(async () => await CreateEnemy());
        }

        private void UpdateSuggestions()
        {
            if (string.IsNullOrWhiteSpace(NewResourceName))
            {
                SuggestedResourceNames.Clear();
                return;
            }

            var suggestions = CommonResourceNames
                .Where(r => r.StartsWith(NewResourceName, StringComparison.OrdinalIgnoreCase))
                .ToObservableCollection();

            SuggestedResourceNames.Clear();
            foreach (var suggestion in suggestions)
            {
                SuggestedResourceNames.Add(suggestion);
            }
        }

        private void AddAttack()
        {
            if (!string.IsNullOrWhiteSpace(NewAttack.Name))
            {
                Attacks.Add(NewAttack);
                NewAttack = new Attack(); // Reset for next input
                RaisePropertyChanged(nameof(NewAttack));
            }
        }

        private void EditAttack(Attack attack)
        {
            if (attack == null) return;

            // Logic to edit the attack, such as opening a dialog for the user to update its details
        }

        private void RemoveAttack(Attack attack)
        {
            if (attack == null) return;
            Attacks.Remove(attack);
        }

        private async Task AddDiceRoll()
        {
            var popup = new AddDiceRollPopup();
            var result = await _page.ShowPopupAsync(popup);

            if (result is not DiceRoll diceRoll) return;

            NewAttack.DiceRoller.AddDiceRoll(diceRoll);
            RaisePropertyChanged(nameof(NewAttack)); // Notify the UI
        }

        private void EditDiceRoll(DiceRollEntry diceRoll)
        {
            // Logic to edit the dice roll
        }

        private void RemoveDiceRoll(DiceRollEntry diceRoll)
        {
            NewAttack.DiceRoller.DiceRolls.Remove(diceRoll);
            RaisePropertyChanged(nameof(NewAttack)); // Notify the UI
        }

        public void SetPage(Page page)
        {
            _page = page;
        }

        public async Task CreateEnemy()
        {
            Enemy.IsMonster = IsMonster;
            Enemy.SkillProficiencies = SkillProficiencies;
            Enemy.SkillExpertises = SkillExpertises;
            Enemy.Vulnerabilities = Vulnerabilities;
            Enemy.DamageResistances = DamageResistances;
            Enemy.DamageImmunities = DamageImmunities;
            Enemy.ConditionImmunities = ConditionImmunities;
            Enemy.Senses = Senses;
            Enemy.Languages = Languages;
            Enemy.Equipment = Equipment;
            Enemy.Resources = Resources;
            Enemy.Attacks = Attacks;

            var factory = new DndCharacterFactory();
            var enemy = factory.CreateEnemy(Enemy);
            _context.Value!.GetMasterUser().Enemies.Add(enemy);
            await _context.Value.SaveUserAsync();
            await App.Current!.MainPage!.DisplayAlert("Enemy Created", "The enemy has been created successfully", "OK");
            await Shell.Current!.GoToAsync("..");
        }
    }
}

