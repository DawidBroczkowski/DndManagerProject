using AutoMapper;
using DndManager.Utility;

namespace DndManager
{
    public partial class App : Application
    {
        public static IMapper Mapper { get; private set; }

        public App(IServiceProvider serviceProvider)
        {
            Current!.UserAppTheme = AppTheme.Light;
            InitializeComponent();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });

            Mapper = config.CreateMapper();

            MainPage = serviceProvider.GetRequiredService<AppShell>();
        }

        protected override async void OnStart()
        {
            await Shell.Current.GoToAsync("//MainPage");
            base.OnStart();
        }
    }
}
