using CommunityToolkit.Maui.Views;
using DndManager.Models;

namespace DndManager.Compontents;

public partial class CampaignPopup : Popup
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public CampaignPopup()
    {
        InitializeComponent();

        // Bind cancel and create commands
        CancelCommand = new Command(() => Close(null));
        CreateCommand = new Command(() =>
        {
            Close(new Campaign
            {
                Name = Name,
                Description = Description
            });
        });

        BindingContext = this;
    }

    public Command CancelCommand { get; }
    public Command CreateCommand { get; }
}