using DndManager.Accessor;
using DndManager.DndApplication;
using DndManager.Dtos;
using DndManager.Models;
using DndManager.Network;
using DndManager.Compontents;
using DndManager.Services;
using DndManager.Utility;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Character = DndManager.Models.Character;
using Resource = DndManager.Models.Resource;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Maui.Core.Extensions;

namespace DndManager.ViewModels
{
    public class CharacterCardViewModel : INotifyPropertyChanged
    {
        private readonly IServiceProvider _serviceProvider;
        private Accessor<CharacterService> _characterService;
        private Accessor<ClientSession> _session;
        private Accessor<SessionMediator> _mediator;
        private Accessor<ApplicationContext> _context;
        private CharacterService _copyCharacterService;
        private Page _page;

        private Character _characterCopy;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public List<string> SavingThrowProficiencies { get; set; } = new();

        public ObservableCollection<Item> Equipment => _characterCopy.Equipment;
        public ObservableCollection<Feat> Feats => _characterCopy.Feats;
        public ObservableCollection<string> Languages => _characterCopy.Languages;
        public ObservableCollection<string> SkillProficiencies => _characterCopy.SkillProficiencies;
        public ObservableCollection<string> SkillExpertises => _characterCopy.SkillExpertises;
        public ObservableCollection<string> OtherProficiencies => _characterCopy.OtherProficiencies;
        public ObservableCollection<string> OtherExpertises => _characterCopy.OtherExpertises;

        public AbilityScore Strength => _characterCopy.GetAbilityScore("Strength")! ?? new();
        public AbilityScore Dexterity => _characterCopy.GetAbilityScore("Dexterity")! ?? new();
        public AbilityScore Constitution => _characterCopy.GetAbilityScore("Constitution")! ?? new();
        public AbilityScore Intelligence => _characterCopy.GetAbilityScore("Intelligence")! ?? new();
        public AbilityScore Wisdom => _characterCopy.GetAbilityScore("Wisdom")! ?? new();
        public AbilityScore Charisma => _characterCopy.GetAbilityScore("Charisma")! ?? new();

        public SavingThrow StrengthSavingThrow => _characterCopy.GetSavingThrow("Strength")! ?? new();
        public SavingThrow DexteritySavingThrow => _characterCopy.GetSavingThrow("Dexterity")! ?? new();
        public SavingThrow ConstitutionSavingThrow => _characterCopy.GetSavingThrow("Constitution")! ?? new();
        public SavingThrow IntelligenceSavingThrow => _characterCopy.GetSavingThrow("Intelligence")! ?? new();
        public SavingThrow WisdomSavingThrow => _characterCopy.GetSavingThrow("Wisdom")! ?? new();
        public SavingThrow CharismaSavingThrow => _characterCopy.GetSavingThrow("Charisma")! ?? new();

        public int StrengthScore
        {
            get => Strength.Score;
            set
            {
                if (Strength.Score != value)
                {
                    Strength.Score = value;
                    Strength.Modifier = _characterCopy.GetAbilityScoreModifier("Strength")!.Value;
                    RaisePropertyChanged(nameof(StrengthScore));
                    RaisePropertyChanged(nameof(StrengthModifier));
                    RaisePropertyChanged(nameof(StrengthSavingThrowModifier));
                }
            }
        }

        public int DexterityScore
        {
            get => Dexterity.Score;
            set
            {
                if (Dexterity.Score != value)
                {
                    Dexterity.Score = value;
                    Dexterity.Modifier = _characterCopy.GetAbilityScoreModifier("Dexterity")!.Value;
                    RaisePropertyChanged(nameof(DexterityScore));
                    RaisePropertyChanged(nameof(DexterityModifier));
                    RaisePropertyChanged(nameof(DexteritySavingThrowModifier));
                }
            }
        }

        public int ConstitutionScore
        {
            get => Constitution.Score;
            set
            {
                if (Constitution.Score != value)
                {
                    Constitution.Score = value;
                    Constitution.Modifier = _characterCopy.GetAbilityScoreModifier("Constitution")!.Value;
                    RaisePropertyChanged(nameof(ConstitutionScore));
                    RaisePropertyChanged(nameof(ConstitutionModifier));
                    RaisePropertyChanged(nameof(ConstitutionSavingThrowModifier));
                }
            }
        }

        public int IntelligenceScore
        {
            get => Intelligence.Score;
            set
            {
                if (Intelligence.Score != value)
                {
                    Intelligence.Score = value;
                    Intelligence.Modifier = _characterCopy.GetAbilityScoreModifier("Intelligence")!.Value;
                    RaisePropertyChanged(nameof(IntelligenceScore));
                    RaisePropertyChanged(nameof(IntelligenceModifier));
                    RaisePropertyChanged(nameof(IntelligenceSavingThrowModifier));
                }
            }
        }

        public int WisdomScore
        {
            get => Wisdom.Score;
            set
            {
                if (Wisdom.Score != value)
                {
                    Wisdom.Score = value;
                    Wisdom.Modifier = _characterCopy.GetAbilityScoreModifier("Wisdom")!.Value;
                    RaisePropertyChanged(nameof(WisdomScore));
                    RaisePropertyChanged(nameof(WisdomModifier));
                    RaisePropertyChanged(nameof(WisdomSavingThrowModifier));
                }
            }
        }

        public int CharismaScore
        {
            get => Charisma.Score;
            set
            {
                if (Charisma.Score != value)
                {
                    Charisma.Score = value;
                    Charisma.Modifier = _characterCopy.GetAbilityScoreModifier("Charisma")!.Value;
                    RaisePropertyChanged(nameof(CharismaScore));
                    RaisePropertyChanged(nameof(CharismaModifier));
                    RaisePropertyChanged(nameof(CharismaSavingThrowModifier));
                }
            }
        }

        public int StrengthModifier => _characterCopy.GetAbilityScoreModifier("Strength") ?? 0;
        public int DexterityModifier => _characterCopy.GetAbilityScoreModifier("Dexterity") ?? 0;
        public int ConstitutionModifier => _characterCopy.GetAbilityScoreModifier("Constitution") ?? 0;
        public int IntelligenceModifier => _characterCopy.GetAbilityScoreModifier("Intelligence") ?? 0;
        public int WisdomModifier => _characterCopy.GetAbilityScoreModifier("Wisdom") ?? 0;
        public int CharismaModifier => _characterCopy.GetAbilityScoreModifier("Charisma") ?? 0;

        public bool StrengthSavingThrowProficient
        {
            get => _characterCopy.GetSavingThrow("Strength") != null ? _characterCopy.GetSavingThrow("Strength")!.Proficient : false;
            set
            {
                if (_characterCopy.GetSavingThrow("Strength")!.Proficient != value)
                {
                    _characterCopy.GetSavingThrow("Strength")!.Proficient = value;
                    RaisePropertyChanged(nameof(StrengthSavingThrowProficient));
                    RaisePropertyChanged(nameof(StrengthSavingThrowModifier));
                }
            }
        }

        public bool DexteritySavingThrowProficient
        {
            get => _characterCopy.GetSavingThrow("Dexterity") != null ? _characterCopy.GetSavingThrow("Dexterity")!.Proficient : false;
            set
            {
                if (_characterCopy.GetSavingThrow("Dexterity")!.Proficient != value)
                {
                    _characterCopy.GetSavingThrow("Dexterity")!.Proficient = value;
                    RaisePropertyChanged(nameof(DexteritySavingThrowProficient));
                    RaisePropertyChanged(nameof(DexteritySavingThrowModifier));
                }
            }
        }

        public bool ConstitutionSavingThrowProficient
        {
            get => _characterCopy.GetSavingThrow("Constitution") != null ? _characterCopy.GetSavingThrow("Constitution")!.Proficient : false;
            set
            {
                if (_characterCopy.GetSavingThrow("Constitution")!.Proficient != value)
                {
                    _characterCopy.GetSavingThrow("Constitution")!.Proficient = value;
                    RaisePropertyChanged(nameof(ConstitutionSavingThrowProficient));
                    RaisePropertyChanged(nameof(ConstitutionSavingThrowModifier));
                }
            }
        }

        public bool IntelligenceSavingThrowProficient
        {
            get => _characterCopy.GetSavingThrow("Intelligence") != null ? _characterCopy.GetSavingThrow("Intelligence")!.Proficient : false;
            set
            {
                if (_characterCopy.GetSavingThrow("Intelligence")!.Proficient != value)
                {
                    _characterCopy.GetSavingThrow("Intelligence")!.Proficient = value;
                    RaisePropertyChanged(nameof(IntelligenceSavingThrowProficient));
                    RaisePropertyChanged(nameof(IntelligenceSavingThrowModifier));
                }
            }
        }

        public bool WisdomSavingThrowProficient
        {
            get => _characterCopy.GetSavingThrow("Wisdom") != null ? _characterCopy.GetSavingThrow("Wisdom")!.Proficient : false;
            set
            {
                if (_characterCopy.GetSavingThrow("Wisdom")!.Proficient != value)
                {
                    _characterCopy.GetSavingThrow("Wisdom")!.Proficient = value;
                    RaisePropertyChanged(nameof(WisdomSavingThrowProficient));
                    RaisePropertyChanged(nameof(WisdomSavingThrowModifier));
                }
            }
        }

        public bool CharismaSavingThrowProficient
        {
            get => _characterCopy.GetSavingThrow("Charisma") != null ? _characterCopy.GetSavingThrow("Charisma")!.Proficient : false;
            set
            {
                if (_characterCopy.GetSavingThrow("Charisma")!.Proficient != value)
                {
                    _characterCopy.GetSavingThrow("Charisma")!.Proficient = value;
                    RaisePropertyChanged(nameof(CharismaSavingThrowProficient));
                    RaisePropertyChanged(nameof(CharismaSavingThrowModifier));
                }
            }
        }


        public int StrengthSavingThrowModifier => getSavingThrowModifier("Strength");
        public int DexteritySavingThrowModifier => getSavingThrowModifier("Dexterity");
        public int ConstitutionSavingThrowModifier => getSavingThrowModifier("Constitution");
        public int IntelligenceSavingThrowModifier => getSavingThrowModifier("Intelligence");
        public int WisdomSavingThrowModifier => getSavingThrowModifier("Wisdom");
        public int CharismaSavingThrowModifier => getSavingThrowModifier("Charisma");

        public ObservableCollection<SavingThrow> SavingThrows => _characterCopy.SavingThrows;
        public ObservableCollection<Skill> Skills => _characterCopy.Skills;
        public ObservableCollection<Currency> Currency => _characterCopy.Currency;

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

        // New properties for adding new items, proficiencies, etc. to the character
        public Item NewItem { get; set; } = new();
        public Feat NewFeat { get; set; } = new();
        public string NewSkillProficiency { get; set; }
        public string NewSkillExpertise { get; set; }
        public string NewOtherProficiency { get; set; }
        public string NewOtherExpertise { get; set; }
        public string NewLanguage { get; set; }

        
        public Attack NewAttack { get; set; } = new Attack();
        public Spell NewSpell { get; set; } = new Spell();
        public ObservableCollection<Attack> Attacks => _characterCopy.Attacks;
        public ObservableCollection<Spell> Spells => _characterCopy.Spells;
        public ICommand DiceRollCommand { get; }
        public ICommand AddAttackCommand { get; }
        public ICommand EditAttackCommand { get; }
        public ICommand RemoveAttackCommand { get; }
        public ICommand AddDiceRollCommand { get; }
        public ICommand EditDiceRollCommand { get; }
        public ICommand RemoveDiceRollCommand { get; }
        public ICommand AddSpellDiceRollCommand { get; }
        public ICommand EditSpellDiceRollCommand { get; }
        public ICommand RemoveSpellDiceRollCommand { get; }
        public ICommand AddSpellCommand { get; }
        public ICommand EditSpellCommand { get; }
        public ICommand RemoveSpellCommand { get; }
        public ICommand IncreaseResourceQuantityCommand => new Command<DndManager.Models.Resource>(resource =>
        {
            if (_characterCopy.Resources.Contains(resource))
            {
                resource.Current++;
                RaisePropertyChanged(nameof(Equipment));
            }
        });
        public ICommand DecreaseResourceQuantityCommand => new Command<DndManager.Models.Resource>(resource =>
        {
            if (_characterCopy.Resources.Contains(resource))
            {
                resource.Current--;
                RaisePropertyChanged(nameof(Equipment));
            }
        });

        public ICommand SaveCharacterCommand { get; set; }

        public Character CharacterCopy
        {
            get => _characterCopy;
            set
            {
                if (_characterCopy != value)
                {
                    _characterCopy = value;
                    OnPropertyChanged(nameof(CharacterCopy));
                }
            }
        }
        public ICommand AddFeatCommand => new Command(() =>
        {
            
            if (!string.IsNullOrWhiteSpace(NewFeat.Name) &&
                !string.IsNullOrWhiteSpace(NewFeat.Description))
            {
                Feat newFeat = new Feat
                {
                    Name = NewFeat.Name,
                    Description = NewFeat.Description
                };
                Feats.Add(newFeat);
                NewFeat = new Feat();
                RaisePropertyChanged(nameof(Feats));
                RaisePropertyChanged(nameof(NewFeat));
            }
        });

        public ICommand RemoveFeatCommand => new Command<Feat>(feat =>
        {
            if (_characterCopy.Feats.Contains(feat))
            {
                Feats.Remove(feat);
                RaisePropertyChanged(nameof(Feats));
            }
        });

        public ICommand IncreaseItemQuantityCommand => new Command<Item>(item =>
        {
            if (_characterCopy.Equipment.Contains(item))
            {
                item.Quantity++;
                RaisePropertyChanged(nameof(Equipment));
            }
        });

        public ICommand DecreaseItemQuantityCommand => new Command<Item>(item =>
        {
            if (_characterCopy.Equipment.Contains(item))
            {
                if (item.Quantity >= 1)
                {
                    item.Quantity--;
                }
                RaisePropertyChanged(nameof(Equipment));
            }
        });

        public ICommand AddItemCommand => new Command(() =>
        {

            if (!string.IsNullOrWhiteSpace(NewItem.Name) &&
                !string.IsNullOrWhiteSpace(NewItem.Description) &&
                NewItem.Quantity > 0)
            {
                Item newItem = new Item
                {
                    Name = NewItem.Name,
                    Description = NewItem.Description,
                    Quantity = NewItem.Quantity,
                    IsMagical = NewItem.IsMagical,
                    IsEquipped = NewItem.IsEquipped
                };
                Equipment.Add(newItem);
                NewItem = new Item();
                RaisePropertyChanged(nameof(Equipment));
                RaisePropertyChanged(nameof(NewItem));
            }
        });

        public ICommand RemoveItemCommand => new Command<Item>(item =>
        {
            if (_characterCopy.Equipment.Contains(item))
            {
                Equipment.Remove(item);
                RaisePropertyChanged(nameof(Equipment));
            }
        });
        public ICommand AddLanguageCommand => new Command(() =>
        {
            if (!string.IsNullOrWhiteSpace(NewLanguage))
            {
                Languages.Add(NewLanguage);
                NewLanguage = string.Empty;
                RaisePropertyChanged(nameof(NewLanguage));
                RaisePropertyChanged(nameof(Languages));
            }
        });

        public ICommand RemoveLanguageCommand => new Command<string>(skill =>
        {
            if (Languages.Contains(skill))
            {
                Languages.Remove(skill);
                RaisePropertyChanged(nameof(Languages));
            }
        });
        public ICommand AddSkillProficiencyCommand => new Command(() =>
        {
            if (!string.IsNullOrWhiteSpace(NewSkillProficiency))
            {
                SkillProficiencies.Add(NewSkillProficiency);
                NewSkillProficiency = string.Empty;
                RaisePropertyChanged(nameof(NewSkillProficiency));
                RaisePropertyChanged(nameof(SkillProficiencies));
            }
        });

        public ICommand RemoveSkillProficiencyCommand => new Command<string>(skill =>
        {
            if (SkillProficiencies.Contains(skill))
            {
                SkillProficiencies.Remove(skill);
                RaisePropertyChanged(nameof(SkillProficiencies));
            }
        });

        public ICommand AddSkillExpertiseCommand => new Command(() =>
        {
            if (!string.IsNullOrWhiteSpace(NewSkillExpertise))
            {
                SkillExpertises.Add(NewSkillExpertise);
                NewSkillExpertise = string.Empty;
                RaisePropertyChanged(nameof(NewSkillExpertise));
                RaisePropertyChanged(nameof(SkillExpertises));
            }
        });

        public ICommand RemoveSkillExpertiseCommand => new Command<string>(skill =>
        {
            if (SkillExpertises.Contains(skill)&& _characterCopy.SkillExpertises.Contains(skill))
            {
                SkillExpertises.Remove(NewSkillExpertise);
                SkillExpertises.Remove(skill);
                RaisePropertyChanged(nameof(SkillExpertises));
            }
        });

        public ICommand AddOtherProficiencyCommand => new Command(() =>
        {
            if (!string.IsNullOrWhiteSpace(NewOtherProficiency))
            {
                OtherProficiencies.Add(NewOtherProficiency);
                NewOtherProficiency = string.Empty;
                RaisePropertyChanged(nameof(NewOtherProficiency));
                RaisePropertyChanged(nameof(OtherProficiencies));
            }
        });

        public ICommand RemoveOtherProficiencyCommand => new Command<string>(proficiency =>
        {
            if (OtherProficiencies.Contains(proficiency) && _characterCopy.OtherProficiencies.Contains(proficiency))
            {
                OtherProficiencies.Remove(proficiency);
                RaisePropertyChanged(nameof(OtherProficiencies));
            }
        });
        public ICommand AddOtherExpertiseCommand => new Command(() =>
        {
            if (!string.IsNullOrWhiteSpace(NewOtherExpertise))
            {
                OtherExpertises.Add(NewOtherExpertise);
                NewOtherExpertise = string.Empty;
                RaisePropertyChanged(nameof(NewOtherExpertise));
                RaisePropertyChanged(nameof(OtherProficiencies));
            }
        });

        public ICommand RemoveOtherExpertiseCommand => new Command<string>(expertise =>
        {
            if (OtherExpertises.Contains(expertise) && _characterCopy.OtherExpertises.Contains(expertise))
            {
                OtherExpertises.Remove(expertise);
                RaisePropertyChanged(nameof(NewOtherExpertise));
            }
        });

        public CharacterCardViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _characterService = _serviceProvider.GetRequiredService<Accessor<CharacterService>>();
            _session = _serviceProvider.GetRequiredService<Accessor<ClientSession>>();
            _mediator = _serviceProvider.GetRequiredService<Accessor<SessionMediator>>();
            _context = _serviceProvider.GetRequiredService<Accessor<ApplicationContext>>();

            AddAttackCommand = new Command(AddAttack);
            EditAttackCommand = new Command<Attack>(EditAttack);
            RemoveAttackCommand = new Command<Attack>(RemoveAttack);

            AddDiceRollCommand = new Command(async () => await AddDiceRoll());
            EditDiceRollCommand = new Command<DiceRollEntry>(EditDiceRoll);
            RemoveDiceRollCommand = new Command<DiceRollEntry>(RemoveDiceRoll);

            AddSpellCommand = new Command(AddSpell);
            EditSpellCommand = new Command<Spell>(EditSpell);
            RemoveSpellCommand = new Command<Spell>(RemoveSpell);

            AddSpellDiceRollCommand = new Command(async () => await AddSpellDiceRoll());
            EditSpellDiceRollCommand = new Command<DiceRollEntry>(EditSpellDiceRoll);
            RemoveSpellDiceRollCommand = new Command<DiceRollEntry>(RemoveSpellDiceRoll);

            AddSpellCommand = new Command(AddSpell);

            SaveCharacterCommand = new Command(async () => await SaveCharacter());
            DiceRollCommand = new Command<DiceRollEntry>(RollDice);

            _copyCharacterService = new();
            CharacterCopy = new();
            _copyCharacterService.InitializeCharacter(_characterCopy!);
        }

        public async Task InitializeAsync()
        {
            _copyCharacterService = new();
            CharacterCopy = Character.Copy(_characterService.Value!.GetCharacter()!);
            _copyCharacterService.InitializeCharacter(_characterCopy!);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task SaveCharacter()
        {
            try
            {
                var character = _characterService.Value!.GetCharacter();

                var updateCharacterDto = character!.ToUpdateDto(_characterCopy);

                if (updateCharacterDto is null)
                {
                    return;
                }

                bool success = _characterService.Value.TryUpdateCharacter(updateCharacterDto);
                if (success)
                {
                    await _context.Value!.SaveUserAsync();
                    await App.Current!.MainPage!.DisplayAlert("Success", "Character updated successfully", "OK");
                }

                if (_context.Value!.ClientSession is not null &&
                    string.IsNullOrEmpty(_context.Value!.ClientSession!.EndpointId) is false)
                {
                    _context.Value!.ClientSession!.SendUpdateCharacter(updateCharacterDto);
                }

                OnPropertyChanged(nameof(StrengthSavingThrowModifier));
                OnPropertyChanged(nameof(DexteritySavingThrowModifier));
                OnPropertyChanged(nameof(ConstitutionSavingThrowModifier));
                OnPropertyChanged(nameof(IntelligenceSavingThrowModifier));
                OnPropertyChanged(nameof(WisdomSavingThrowModifier));
                OnPropertyChanged(nameof(CharismaSavingThrowModifier));

                OnPropertyChanged(nameof(StrengthModifier));
                OnPropertyChanged(nameof(DexterityModifier));
                OnPropertyChanged(nameof(ConstitutionModifier));
                OnPropertyChanged(nameof(IntelligenceModifier));
                OnPropertyChanged(nameof(WisdomModifier));
                OnPropertyChanged(nameof(CharismaModifier));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void SetPage(Page page)
        {
            _page = page;
        }
        private void AddAttack()
        {
            if (!string.IsNullOrWhiteSpace(NewAttack.Name))
            {
                Attacks.Add(NewAttack);
                NewAttack = new Attack();
                RaisePropertyChanged(nameof(NewAttack));
                RaisePropertyChanged(nameof(Attacks));
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
            if (Attacks.Contains(attack))
            {
                Attacks.Remove(attack);
                RaisePropertyChanged(nameof(Attacks));
            }
        }

        private async Task AddDiceRoll()
        {
            var popup = new AddDiceRollPopup();
            var result = await _page.ShowPopupAsync(popup);

            if (result is not DiceRoll diceRoll) return;

            NewAttack.DiceRoller.AddDiceRoll(diceRoll);
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



        private void AddSpell()
        {
            if (!string.IsNullOrWhiteSpace(NewSpell.Name))
            {
                Spells.Add(NewSpell);
                NewSpell = new Spell();
                RaisePropertyChanged(nameof(NewSpell));
                RaisePropertyChanged(nameof(Spells));
            }
        }
        private void EditSpell(Spell spell)
        {
            //if (attack == null) return;

            // Logic to edit the attack, such as opening a dialog for the user to update its details
        }
        private void RemoveSpell(Spell spell)
        {
            if (spell == null) return;
            if (Spells.Contains(spell))
            {
                Spells.Remove(spell);
                RaisePropertyChanged(nameof(Spells));
            }
        }


        private async Task AddSpellDiceRoll()
        {
            var popup = new AddDiceRollPopup();
            var result = await _page.ShowPopupAsync(popup);

            if (result is not DiceRoll diceRoll) return;

            NewSpell.DiceRoller.AddDiceRoll(diceRoll);
        }

        private void EditSpellDiceRoll(DiceRollEntry diceRoll)
        {
            // Logic to edit the dice roll
        }

        private void RemoveSpellDiceRoll(DiceRollEntry diceRoll)
        {
            NewSpell.DiceRoller.DiceRolls.Remove(diceRoll);
            RaisePropertyChanged(nameof(NewSpell)); // Notify the UI
        }

        // <-- Resources -->
        public DndManager.Models.Resource NewResource { get; set; } = new DndManager.Models.Resource();
        public ObservableCollection<DndManager.Models.Resource> Resources => _characterCopy.Resources;

        private string _name;
        public string NewResourceName
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged(nameof(NewResourceName));
                Task.Run(async () => await UpdateSuggestions()); // Update suggestions as user types
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
        private async Task UpdateSuggestions()
        {
            if (string.IsNullOrWhiteSpace(NewResourceName))
            {
                SuggestedResourceNames = new ObservableCollection<string>();
                return;
            }

            var suggestions = await Task.Run(() =>
            {
                var lowerCaseNewResourceName = NewResourceName.ToLower();
                return CommonResourceNames
                    .Where(name => name.ToLower().Contains(lowerCaseNewResourceName))
                    .ToList();
            });

            SuggestedResourceNames = new ObservableCollection<string>(suggestions);
            RaisePropertyChanged(nameof(SuggestedResourceNames));
        }

        private int getSavingThrowModifier(string ability)
        {
            var savingThrow = _characterCopy.GetSavingThrow(ability);
            if (savingThrow is null)
            {
                return 0;
            }

            return savingThrow.Proficient
                ? savingThrow.Modifier + _characterCopy.ProficiencyBonus
                : savingThrow.Modifier;
        }

        public void RollDice(DiceRollEntry diceRoll)
        {
            var popup = new DiceRollerPopup(diceRoll.Roll);
            _page.ShowPopup(popup);
        }
    }
}
