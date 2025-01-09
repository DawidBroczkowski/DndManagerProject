using DndManager.Accessor;
using DndManager.DndApplication;
using DndManager.Models;
using DndManager.Services;
using DndManager.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DndManager.ViewModels
{
    public class BrowseCharactersViewModel : INotifyPropertyChanged
    {
        private readonly IServiceProvider _serviceProvider;
        private Accessor<ApplicationContext> _contex;
        private Accessor<CharacterService> _characerService;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public PlayerUser User => _contex.Value.GetPlayerUser();
        public ObservableCollection<Character> Characters { get; private set; }

        public ICommand CharacterCard { get; set; }

        public BrowseCharactersViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _contex = _serviceProvider.GetRequiredService<Accessor<ApplicationContext>>();
            _characerService = _serviceProvider.GetRequiredService<Accessor<CharacterService>>();
            Characters = new ObservableCollection<Character>();
            CharacterCard = new Command<Character>(async (character) => await NavigateToCharacterCard(character));
        }

        public void ReloadCharacters()
        {
            Characters = new ObservableCollection<Character>(User.Characters ?? new List<Character>());
            RaisePropertyChanged(nameof(Characters));
        }

        public async Task NavigateToCharacterCard(Character character)
        {
            _characerService!.Value = new();
            _characerService!.Value!.InitializeCharacter(character);
            try
            {
                await Shell.Current.GoToAsync("CharacterCardPage");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}

