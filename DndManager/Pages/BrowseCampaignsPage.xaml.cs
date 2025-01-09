using DndManager.ViewModels;

namespace DndManager.Pages;

public partial class BrowseCampaignsPage : ContentPage
{
	public BrowseCampaignsPage(IServiceProvider serviceProvider)
	{
		InitializeComponent();
        BindingContext = serviceProvider.GetRequiredService<BrowseCampaignsViewModel>();
    }
}