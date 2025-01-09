using DndManager.DndApplication;
using DndManager.Models;
using DndManager.Accessor;
using DndManager.Utility;
using DndManager.Services;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Maui.Core.Extensions;

namespace DndManager.ViewModels
{
    public class EncounterViewModel : INotifyPropertyChanged
    {
        private IServiceProvider _serviceProvider;
        private Accessor<ApplicationContext> _context;
        private Accessor<EncounterService> _encounterService;

        private MasterUser _user => _context.Value!.GetMasterUser();
        private Encounter _encounter => _encounterService.Value!.Encounter!;

        private bool _initiativeIsRolled = false;
        public bool InitiativeIsRolled
        {
            get => _initiativeIsRolled;
            set
            {
                _initiativeIsRolled = value;
                RaisePropertyChanged(nameof(InitiativeIsRolled));
            }
        }
        public ObservableCollection<CharacterInEncounter> Characters => _encounterService.Value!.Encounter!.Characters.OrderByDescending(x => x.Initiative).ToObservableCollection();

        private string _newCondition = string.Empty;
        public string NewCondition
        {
            get => _newCondition;
            set
            {
                _newCondition = value;
                RaisePropertyChanged(nameof(NewCondition));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public ICommand AddConditionCommand { get; set; }
        public ICommand RemoveConditionCommand { get; set; }
        public ICommand NextTurnCommand { get; set; }
        public ICommand EndEncounterCommand { get; set; }
        public ICommand RollInitiativeCommand { get; set; }

        public EncounterViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _context = serviceProvider.GetRequiredService<Accessor<ApplicationContext>>();
            _encounterService = serviceProvider.GetRequiredService<Accessor<EncounterService>>();

            AddConditionCommand = new Command<CharacterInEncounter>(AddCondition);
            RemoveConditionCommand = new Command<(CharacterInEncounter Character, string Condition)>(parameter => RemoveCondition(parameter.Character, parameter.Condition));
            NextTurnCommand = new Command(NextTurn);
            EndEncounterCommand = new Command(async () => await EndEncounter());
            RollInitiativeCommand = new Command(RollInitiative);
        }

        // It's possible to have advantage on an initiative roll, but for now we'll keep it simple.
        public void RollInitiative()
        {
            _encounterService.Value!.RollInitiative();
            InitiativeIsRolled = true;
            RaisePropertyChanged(nameof(Characters));
        }

        public void NextTurn()
        {
            _encounterService.Value!.NextTurn();
        }

        public void AddCondition(CharacterInEncounter character)
        {
            if (string.IsNullOrEmpty(NewCondition))
                return;

            character.Character!.Conditions.Add(NewCondition);
            NewCondition = string.Empty;
        }

        public void RemoveCondition(CharacterInEncounter character, string condition)
        {
            character.Character!.Conditions.Remove(condition);
        }

        public async Task EndEncounter()
        {
            InitiativeIsRolled = false;
            _encounterService.Value = null;
            await Shell.Current.GoToAsync("..");
        }
    }
}
