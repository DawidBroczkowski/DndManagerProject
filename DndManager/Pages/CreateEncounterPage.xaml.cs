using DndManager.ViewModels;

namespace DndManager.Pages;

public partial class CreateEncounterPage : ContentPage
{
	public CreateEncounterPage(IServiceProvider serviceProvider)
	{
		InitializeComponent();
        BindingContext = serviceProvider.GetRequiredService<CreateEncounterViewModel>();
    }
}