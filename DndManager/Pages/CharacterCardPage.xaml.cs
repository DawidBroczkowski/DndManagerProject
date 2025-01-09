using DndManager.Models;
using DndManager.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DndManager.Pages;

public partial class CharacterCardPage : ContentPage
{
    public CharacterCardPage(IServiceProvider serviceProvider)
    {
		InitializeComponent();
        BindingContext = serviceProvider.GetRequiredService<CharacterCardViewModel>();
        ((CharacterCardViewModel)BindingContext).SetPage(this);
    }

    private void OnNameTextChanged(object sender, TextChangedEventArgs e)
    {
        ((CharacterCardViewModel)BindingContext).NewResourceName = e.NewTextValue;
    }

    protected override void OnAppearing()
    {   
        base.OnAppearing();
        Task.Run(async () => await ((CharacterCardViewModel)BindingContext).InitializeAsync());
    }
}