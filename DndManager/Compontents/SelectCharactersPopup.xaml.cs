using CommunityToolkit.Maui.Views;
using DndManager.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace DndManager.Compontents;

public partial class SelectCharactersPopup : Popup, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void RaisePropertyChanged(string propertyName)
    => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    public ObservableCollection<CharacterInCampaign> Characters { get; set; }
    public ObservableCollection<object> SelectedCharacters { get; set; } = new ObservableCollection<object>();

    public ICommand ProceedCommand { get; set; }
    public ICommand CharacterTappedCommand { get; set; }

    public SelectCharactersPopup(ObservableCollection<CharacterInCampaign> characters)
	{
        Characters = characters;
        ProceedCommand = new Command(Proceed);
        BindingContext = this;
        InitializeComponent();
        RaisePropertyChanged(nameof(Characters));
    }

    public void Proceed()
    {
        Close(SelectedCharacters);
    }

    public void SelectCharacter(CharacterInCampaign character)
    {
        if (SelectedCharacters.Contains(character))
        {
            SelectedCharacters.Remove(character);
        }
        else
        {
            SelectedCharacters.Add(character);
        }
    }
}