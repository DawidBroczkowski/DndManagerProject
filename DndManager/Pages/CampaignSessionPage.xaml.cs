using DndManager.ViewModels;

namespace DndManager.Pages;

public partial class CampaignSessionPage : ContentPage
{
	public CampaignSessionPage(IServiceProvider serviceProvider)
	{
		InitializeComponent();
        BindingContext = serviceProvider.GetRequiredService<CampaignViewModel>();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ((CampaignViewModel)BindingContext).Initialize();
    }
}