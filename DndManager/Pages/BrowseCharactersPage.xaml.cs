using DndManager.ViewModels;

namespace DndManager.Pages;

public partial class BrowseCharactersPage : ContentPage
{
    public BrowseCharactersPage(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        BindingContext = serviceProvider.GetRequiredService<BrowseCharactersViewModel>();
    }

    protected override void OnAppearing()
    {
        ((BrowseCharactersViewModel)BindingContext).ReloadCharacters();
    }
}