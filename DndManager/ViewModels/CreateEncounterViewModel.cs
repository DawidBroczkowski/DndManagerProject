using DndManager.Accessor;
using DndManager.DndApplication;
using DndManager.Models;
using DndManager.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Character = DndManager.Models.Character;

namespace DndManager.ViewModels
{
    public class CreateEncounterViewModel : INotifyPropertyChanged
    {
        private IServiceProvider _serviceProvider;
        private Accessor<ApplicationContext> _context;
        private Accessor<EncounterService> _encounterService;

        
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<Character> Enemies => _context.Value!.GetMasterUser().Enemies;
        public ObservableCollection<Character> FilteredEnemies =>
            string.IsNullOrEmpty(EnemyName)
            ? Enemies
            : new ObservableCollection<Character>(Enemies.Where(e => e.Name.Contains(EnemyName, StringComparison.OrdinalIgnoreCase)));


        private Encounter _encounter = new();
        public Encounter Encounter
        {
            get => _encounter;
            set
            {
                _encounter = value;
                RaisePropertyChanged(nameof(Encounter));
            }
        }

        private string _enemyName = string.Empty;
        public string EnemyName
        {
            get => _enemyName;
            set
            {
                _enemyName = value;
                RaisePropertyChanged(nameof(EnemyName));
               RaisePropertyChanged(nameof(FilteredEnemies));
            }
        }


        public ICommand AddEnemyCommand { get; set; }
        public ICommand RemoveEnemyCommand { get; set; }
        public ICommand CreateEncounter { get; set; }

        public CreateEncounterViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _context = serviceProvider.GetRequiredService<Accessor<ApplicationContext>>();
            _encounterService = serviceProvider.GetRequiredService<Accessor<EncounterService>>();

            AddEnemyCommand = new Command<Character>(AddEnemy);
            RemoveEnemyCommand = new Command<CharacterInEncounter>(RemoveEnemy);
            CreateEncounter = new Command(async () => await Create());
        }

        public void AddEnemy(Character enemy)
        {
            CharacterInEncounter character = new();
            character.Character = enemy;
            Encounter.Characters.Add(character);
            RaisePropertyChanged(nameof(Encounter));
        }

        public void RemoveEnemy(CharacterInEncounter enemy)
        {
            Encounter.Characters.Remove(enemy);
            RaisePropertyChanged(nameof(Encounter));
        }

        public async Task Create()
        {
            _context.Value!.GetMasterUser().Encounters.Add(Encounter);
            await _context.Value!.SaveUserAsync();
            await Shell.Current!.GoToAsync("..");
        }
    }
}
