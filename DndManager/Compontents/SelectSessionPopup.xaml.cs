using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;

namespace DndManager.Compontents;

public partial class SelectSessionPopup : Popup, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void RaisePropertyChanged(string propertyName)
    => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    public IReadOnlyDictionary<string, string> Endpoints { get; set; }

    public ICommand SessionTappedCommand { get; set; }

    public SelectSessionPopup(IReadOnlyDictionary<string, string> endpoints)
	{
        Endpoints = endpoints;
        InitializeComponent();
        SessionTappedCommand = new Command<KeyValuePair<string, string>>(SelectSession);
        BindingContext = this;
        RaisePropertyChanged(nameof(Endpoints));
    }

    public void SelectSession(KeyValuePair<string, string> session)
    {
        Close(session);
    }
}

