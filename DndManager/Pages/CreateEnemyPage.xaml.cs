using DndManager.ViewModels;

namespace DndManager.Pages;

public partial class CreateEnemyPage : ContentPage
{
	public CreateEnemyPage(IServiceProvider serviceProvider)
	{
		InitializeComponent();
		BindingContext = serviceProvider.GetRequiredService<CreateEnemyViewModel>();
        ((CreateEnemyViewModel)BindingContext).SetPage(this);
    }

    private void OnNameTextChanged(object sender, TextChangedEventArgs e)
    {
        // Additional logic to validate or pre-process text can be placed here
        ((CreateEnemyViewModel)BindingContext).NewResourceName = e.NewTextValue;
    }
}