using CommunityToolkit.Maui.Views;
using DndManager.Accessor;
using DndManager.DndApplication;
using DndManager.Models;
using System.Windows.Input;

namespace DndManager.Compontents;

public partial class SelectCharacterPopup : Popup
{
	private Accessor<ApplicationContext> _context;
	public List<Character> Characters => _context.Value!.GetPlayerUser().Characters;

    public ICommand CharacterTappedCommand { get; set; }

    public SelectCharacterPopup(IServiceProvider serviceProvider)
	{
		InitializeComponent();
        _context = serviceProvider.GetRequiredService<Accessor<ApplicationContext>>();
        CharacterTappedCommand = new Command<Character>(SelectCharacter);
        BindingContext = this;
    }

    public void SelectCharacter(Character character)
    {
        Close(character);
    }
}