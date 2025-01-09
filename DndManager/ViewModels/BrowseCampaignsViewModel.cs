using DndManager.Accessor;
using DndManager.DndApplication;
using DndManager.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace DndManager.ViewModels
{
    public class BrowseCampaignsViewModel : INotifyPropertyChanged
    {
        private IServiceProvider _serviceProvider;
        private Accessor<ApplicationContext> _context;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public ObservableCollection<Campaign> Campaigns => _context.Value!.GetMasterUser().Campaigns;
        public ICommand CampaignTappedCommand { get; }

        public BrowseCampaignsViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _context = _serviceProvider.GetRequiredService<Accessor<ApplicationContext>>();
            CampaignTappedCommand = new Command<Campaign>(OnCampaignTapped);
        }

        private async void OnCampaignTapped(Campaign campaign)
        {
            if (campaign == null)
                return;

            // Navigate to campaign page with campaign.Id
            await Shell.Current.GoToAsync($"campaigndetails?id={campaign.Id}");
        }
    }
}
