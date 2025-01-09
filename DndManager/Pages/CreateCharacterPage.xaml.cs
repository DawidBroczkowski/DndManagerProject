using DndManager.ViewModels;

namespace DndManager.Pages;

public partial class CreateCharacterPage : ContentPage
{
	public CreateCharacterPage(IServiceProvider serviceProvider)
	{
		InitializeComponent();
        BindingContext = serviceProvider.GetRequiredService<CreateCharacterViewModel>();
    }

	protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as CreateCharacterViewModel)?.Initialize();
    }
}