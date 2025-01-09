using DndManager.ViewModels;
using System.Windows.Input;

namespace DndManager.Pages;

public partial class MasterMainPage : ContentPage
{
	public MasterMainPage(IServiceProvider serviceProvider)
	{
		InitializeComponent();
        BindingContext = serviceProvider.GetRequiredService<MasterViewModel>();
        ((MasterViewModel)BindingContext).SetPage(this);
    }

    private async void OnStartSessionClicked(object sender, EventArgs e)
    {
        // modal to select campaign
        // navigate to session page
    }

    private async void OnCreateCampaignClicked(object sender, EventArgs e)
    {
        // go to campaign creation
    }

    private async void OnBrowseCampaignsClicked(object sender, EventArgs e)
    {
        // modal to display campaign list
        // navigate to campaign page
    }

    private async void OnSettingsClicked(object sender, EventArgs e)
    {
        // navigate to settings page
    }
}