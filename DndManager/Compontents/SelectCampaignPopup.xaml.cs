using CommunityToolkit.Maui.Views;
using DndManager.Accessor;
using DndManager.DndApplication;
using DndManager.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DndManager.Compontents;

public partial class SelectCampaignPopup : Popup
{
	private Accessor<ApplicationContext> _context;
	public ObservableCollection<Campaign> Campaigns => _context.Value!.GetMasterUser().Campaigns;

    public ICommand CampaignTappedCommand { get; set; }

    public SelectCampaignPopup(IServiceProvider serviceProvider)
	{
		InitializeComponent();
        _context = serviceProvider.GetRequiredService<Accessor<ApplicationContext>>();
        CampaignTappedCommand = new Command<Campaign>(SelectCampaign);
        BindingContext = this;
    }

    public void SelectCampaign(Campaign campaign)
    {
        Close(campaign);
    }
}