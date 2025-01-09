using CommunityToolkit.Maui.Views;
using DndManager.Accessor;
using DndManager.Compontents;
using DndManager.DndApplication;
using DndManager.Models;
using DndManager.Services;
using System.Windows.Input;

namespace DndManager.ViewModels
{
    public class MasterViewModel
    {
        private readonly IServiceProvider _serviceProvider;
        private Accessor<ApplicationContext> _context;
        private Accessor<CampaignService> _campaignService;
        private Page _page;

        public ICommand StartSessionComand { get; set; }
        public ICommand CreateCampaignCommand { get; set; }
        public ICommand BrowseCampaignsCommand { get; set; }
        public ICommand CreateEnemyCommand { get; set; }
        public ICommand CreateEncounterCommand { get; set; }

        public MasterViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _context = _serviceProvider.GetRequiredService<Accessor<ApplicationContext>>();
            _campaignService = _serviceProvider.GetRequiredService<Accessor<CampaignService>>();
            _campaignService.Value ??= new CampaignService();

            StartSessionComand = new Command(StartCampaignSession);
            CreateCampaignCommand = new Command(CreateCampaign);
            BrowseCampaignsCommand = new Command(BrowseCampaignsAsync);
            CreateEnemyCommand = new Command(CreateEnemyAsync);
            CreateEncounterCommand = new Command(CreateEncounterAsync);
        }

        public async Task Initialize()
        {

        }

        public async void StartCampaignSession()
        {
            if(_campaignService.Value!.Campaign is not null)
            {
                await Shell.Current.GoToAsync("CampaignSessionPage");
                return;
            }

            var popup = new SelectCampaignPopup(_serviceProvider);
            var result = await _page.ShowPopupAsync(popup);
            var campaign = result as Campaign;

            if (campaign is null)
            {
                return;
            }

            _campaignService.Value!.InitializeCampaign(campaign!);
            await Shell.Current.GoToAsync("CampaignSessionPage");
        }

        // Create enemy, needs a factory
        public void CreateEnemy()
        {
            // navigate to enemy creation page
        }

        // Create a campaign           
        public async void CreateCampaign()
        {
            var popup = new CampaignPopup();
            var result = await _page.ShowPopupAsync(popup);
            if (result is not null)
            {
                _context.Value!.GetMasterUser().Campaigns.Add((result as Campaign)!);
                await _context.Value.SaveUserAsync();
            }
            else
            {
                App.Current.MainPage.DisplayAlert("Error", "Campaign creation failed", "OK");
            }
        }

        public void SetPage(Page page)
        {
            _page = page;
        }

        public async void BrowseCampaignsAsync()
        {
            await Shell.Current!.GoToAsync("BrowseCampaignsPage");
        }

        public async void CreateEnemyAsync()
        {
            await Shell.Current.GoToAsync("CreateEnemyPage");
        }

        public async void CreateEncounterAsync()
        {
            await Shell.Current.GoToAsync("CreateEncounterPage");
        }
    }
}
