using DndManager.ViewModels;

namespace DndManager.Pages;

public partial class PlayerMainPage : ContentPage
{
	public PlayerMainPage(IServiceProvider serviceProvider)
	{
		InitializeComponent();
		BindingContext = serviceProvider.GetRequiredService<PlayerViewModel>();
    }
}