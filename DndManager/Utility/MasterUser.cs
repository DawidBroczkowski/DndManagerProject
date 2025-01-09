using DndManager.Models;
using System.Collections.ObjectModel;

namespace DndManager.Utility
{
    public record MasterUser : ApplicationUser
    {
        public ObservableCollection<Campaign> Campaigns { get; set; } = new();
        public ObservableCollection<Character> Enemies { get; set; } = new();
        public ObservableCollection<Encounter> Encounters { get; set; } = new();
    }
}
