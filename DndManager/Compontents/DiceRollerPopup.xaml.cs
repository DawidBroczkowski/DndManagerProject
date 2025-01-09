using CommunityToolkit.Maui.Views;
using DndManager.Models;
using System.ComponentModel;
using System.Windows.Input;

namespace DndManager.Compontents;

public partial class DiceRollerPopup : Popup, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void RaisePropertyChanged(string propertyName)
    => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    private int _rollResult = 0;
    public int RollResult
    {
        get => _rollResult;
        set
        {
            _rollResult = value;
            RaisePropertyChanged(nameof(RollResult));
        }
    }
	public DiceRoll DiceRoll { get; set; }

	public ICommand RollCommand { get; set; }

    public DiceRollerPopup(DiceRoll roll)
	{
		InitializeComponent();
        DiceRoll = roll;
        RollCommand = new Command(RollDice);
        BindingContext = this;
    }

    private void RollDice()
	{
        RollResult = DiceRoll.Roll();
    }
}