using DndManager.ViewModels;

namespace DndManager.Pages;

public partial class EncounterPage : ContentPage
{
	public EncounterPage(IServiceProvider serviceProvider)
	{
		InitializeComponent();
		BindingContext = serviceProvider.GetRequiredService<EncounterViewModel>();
    }
}