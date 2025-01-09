using CommunityToolkit.Maui.Views;
using DndManager.Models;
using DndManager.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace DndManager.Compontents;

public partial class SelectEncounterPopup : Popup, INotifyPropertyChanged
{
	public ObservableCollection<Encounter> Encounters { get; set; }
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void RaisePropertyChanged(string propertyName)
    => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public ICommand EncounterTappedCommand { get; set; }
    public SelectEncounterPopup(ObservableCollection<Encounter> encounters)
	{
		Encounters = encounters;
        BindingContext = this;
        EncounterTappedCommand = new Command<Encounter>(SelectEncounter);
        InitializeComponent();
        RaisePropertyChanged(nameof(Encounters));
    }

    public void SelectEncounter(Encounter encounter)
    {
        Close(encounter);
    }
}