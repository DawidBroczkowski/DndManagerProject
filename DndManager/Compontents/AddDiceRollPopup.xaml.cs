using CommunityToolkit.Maui.Views;
using DndManager.Models;
using System.Windows.Input;

namespace DndManager.Compontents;

public partial class AddDiceRollPopup : Popup
{
	public DiceRoll DiceRoll { get; set; } = new DiceRoll();

	public ICommand AddDiceRollCommand { get; set; }

    public AddDiceRollPopup(DiceRoll? diceRoll = null)
	{
		InitializeComponent();

        if (diceRoll is not null)
        {
            DiceRoll = diceRoll;
        }

        AddDiceRollCommand = new Command(AddDiceRoll);
        BindingContext = this;
	}

    private void AddDiceRoll()
    {
        if (DiceRoll.NumberOfDice == 0 || DiceRoll.DiceSize == 0)
        {
            return;
        }

        Close(DiceRoll);
    }


}