using DndManager.Accessor;
using DndManager.DndApplication;
using DndManager.Pages;
using System.Windows.Input;

namespace DndManager
{
    public partial class MainPage : ContentPage
    {
        private IServiceProvider _serviceProvider;
        private Accessor<ApplicationContext> _context;


        public MainPage(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _context = _serviceProvider.GetRequiredService<Accessor<ApplicationContext>>();
            if (_context.Value is null) 
            {
                _context.Value = new ApplicationContext(serviceProvider);
            }
        }

        private async void OnMasterClicked(object sender, EventArgs e)
        {
            if (_context.Value!.MasterUserExists())
            {
                await _context.Value!.LoadUserAsync("master");
            }
            else
            {
                await _context.Value!.CreateMasterUserAsync();

                // modal to get username

                _context.Value!.GetMasterUser().UserName = "Master";
            }

            await Shell.Current.GoToAsync("MasterMainPage");
        }

        private async void OnPlayerClicked(object sender, EventArgs e)
        {
            if (_context.Value!.PlayerUserExists())
            {
                await _context.Value!.LoadUserAsync("player");
            }
            else
            {
                await _context.Value!.CreatePlayerUserAsync();

                // modal to get username

                _context.Value!.GetPlayerUser().UserName = "Player";
            }

            await Shell.Current.GoToAsync("PlayerMainPage");
        }
    }
}
