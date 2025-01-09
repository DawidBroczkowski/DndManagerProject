using DndManager.DndApplication;
using DndManager.Network;
using DndManager.Network.Interfaces;
using Microsoft.Extensions.Logging;
using DndManager.Accessor;
using DndManager.Services;
using DndManager.Pages;
using DndManager.ViewModels;
using DndManager.Utility;
using CommunityToolkit.Maui;

namespace DndManager
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<AppShell>();

            builder.Services.AddSingleton<Accessor<ClientSession>>();
            builder.Services.AddSingleton<Accessor<ServerSession>>();
            builder.Services.AddSingleton<Accessor<SessionMediator>>();
            builder.Services.AddSingleton<Accessor<ApplicationContext>>();
            builder.Services.AddSingleton<Accessor<CampaignService>>();
            builder.Services.AddSingleton<Accessor<CharacterService>>();
            builder.Services.AddSingleton<Accessor<ApplicationUser>>();
            builder.Services.AddSingleton<Accessor<EncounterService>>();


#if ANDROID
            builder.Services.AddSingleton<Interfaces.INearbyConnectionsManager, DndManager.Platforms.Android.NearbyConnectionsManager>();
            builder.Services.AddSingleton<Accessor<Interfaces.INearbyConnectionsManager>>(provider =>
            {
                var instance = provider.GetRequiredService<Interfaces.INearbyConnectionsManager>();
                return new Accessor<Interfaces.INearbyConnectionsManager>(instance);
            });
#endif

            // Pages
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MasterMainPage>();
            builder.Services.AddSingleton<PlayerMainPage>();
            builder.Services.AddSingleton<CampaignSessionPage>();
            builder.Services.AddSingleton<CreateCharacterPage>();
            builder.Services.AddSingleton<BrowseCampaignsPage>();
            builder.Services.AddTransient<CreateEnemyPage>();
            builder.Services.AddSingleton<BrowseCharactersPage>();
            builder.Services.AddSingleton<CharacterCardPage>();
            builder.Services.AddTransient<CreateEncounterPage>();
            builder.Services.AddSingleton<EncounterPage>();

            // ViewModels
            builder.Services.AddSingleton<MasterViewModel>();
            builder.Services.AddSingleton<PlayerViewModel>();
            builder.Services.AddSingleton<CampaignViewModel>();
            builder.Services.AddSingleton<PlayerViewModel>();
            builder.Services.AddSingleton<CreateCharacterViewModel>();
            builder.Services.AddSingleton<BrowseCampaignsViewModel>();
            builder.Services.AddTransient<CreateEnemyViewModel>();
            builder.Services.AddSingleton<BrowseCharactersViewModel>();
            builder.Services.AddSingleton<CharacterCardViewModel>();
            builder.Services.AddTransient<CreateEncounterViewModel>();
            builder.Services.AddSingleton<EncounterViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
