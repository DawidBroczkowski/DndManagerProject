using DndManager.Pages;

namespace DndManager
{
    public partial class AppShell : Shell
    {
        private readonly IServiceProvider _serviceProvider;

        public AppShell(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;

            // Player
            Routing.RegisterRoute("MainPage/PlayerMainPage", typeof(PlayerMainPage));
            Routing.RegisterRoute("MainPage/PlayerMainPage/CreateCharacterPage", typeof(CreateCharacterPage));
            Routing.RegisterRoute("MainPage/PlayerMainPage/BrowseCharactersPage", typeof(BrowseCharactersPage));
            Routing.RegisterRoute("MainPage/PlayerMainPage/BrowseCharactersPage/CharacterCardPage", typeof(CharacterCardPage));
            Routing.RegisterRoute("MainPage/PlayerMainPage/InSessionCharacterCardPage", typeof(CharacterCardPage));
            // Master
            Routing.RegisterRoute("MainPage/MasterMainPage", typeof(MasterMainPage));
            Routing.RegisterRoute("MainPage/MasterMainPage/CampaignSessionPage", typeof(CampaignSessionPage));
            Routing.RegisterRoute("MainPage/MasterMainPage/BrowseCampaignsPage", typeof(BrowseCampaignsPage));
            Routing.RegisterRoute("MainPage/MasterMainPage/CreateEnemyPage", typeof(CreateEnemyPage));
            Routing.RegisterRoute("MainPage/MasterMainPage/CreateEncounterPage", typeof(CreateEncounterPage));
            Routing.RegisterRoute("MainPage/MasterMainPage/EncounterPage", typeof(EncounterPage));
        }

        public void OnConnectionLost(string endpointId)
        {
            App.Current!.MainPage!.DisplayAlert("Disconnected", "Connection lost", "OK");
        }
    }
}
