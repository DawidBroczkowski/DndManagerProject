using CommunityToolkit.Maui.Views;
using DndManager.Accessor;
using DndManager.Compontents;
using DndManager.DndApplication;
using DndManager.Dtos;
using DndManager.Interfaces;
using DndManager.Models;
using DndManager.Network;
using DndManager.Network.Interfaces;
using DndManager.Network.Utility;
using DndManager.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DndManager.ViewModels
{
    public class CampaignViewModel : INotifyPropertyChanged
    {
        private readonly IServiceProvider _serviceProvider;
        private Accessor<CampaignService> _campaignService;
        private Accessor<ServerSession> _session;
        private Accessor<INearbyConnectionsManager> _connections;
        private Accessor<SessionMediator> _mediator;
        private Accessor<ApplicationContext> _context;
        private Accessor<EncounterService> _encounterService;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public ObservableCollection<GetConnectToSessionDto> JoinRequests => _session.Value!.JoinRequests;
        public ObservableCollection<PlayerInSession> Players => _session.Value!.Players;
        public ObservableCollection<CharacterInCampaign> Characters => _campaignService.Value!.Campaign!.Characters;
        public Campaign Campaign => _campaignService.Value!.Campaign!;

        public ICommand EndSessionCommand { get; set; }
        public ICommand AcceptJoinRequestCommand { get; set; }
        public ICommand RejectJoinRequestCommand { get; set; }
        public ICommand StartEncounterCommand { get; set; }

        public CampaignViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _session = _serviceProvider.GetRequiredService<Accessor<ServerSession>>();
            _connections = _serviceProvider.GetRequiredService<Accessor<INearbyConnectionsManager>>();
            _mediator = _serviceProvider.GetRequiredService<Accessor<SessionMediator>>();
            _context = _serviceProvider.GetRequiredService<Accessor<ApplicationContext>>();
            _campaignService = _serviceProvider.GetRequiredService<Accessor<CampaignService>>();
            _encounterService = _serviceProvider.GetRequiredService<Accessor<EncounterService>>();

            // These might not be defined, the rest are defined in previous view models
            _session.Value ??= new ServerSession();
            _mediator.Value ??= new SessionMediator(serviceProvider);
            _mediator.Value.SetSession(_session);
            _mediator.Value.SetConnections(_connections);

            EndSessionCommand = new Command(EndSession);
            AcceptJoinRequestCommand = new Command<GetConnectToSessionDto>(AcceptPlayer);
            RejectJoinRequestCommand = new Command<GetConnectToSessionDto>(RejectPlayer);
            StartEncounterCommand = new Command(async () => await StartEncounterAsync());

            Initialize();
        }

        public void Initialize()
        {
            if (_context.Value!.ServerSession is null)
            {
                _connections.Value!.StartAdvertising(_campaignService.Value!.Campaign!.Name);
                _context.Value!.ServerSession = _session.Value;

                // Event to actually handle requests with code
                _session.Value!.OnCharacterUpdate += HandleCharacterUpdate; // When a player updates their character
                _session.Value!.OnFullCharacterUpdate += HandleFullCharacterUpdate; // When a player sends a full character update
                _session.Value!.OnPlayerAccepted += HandlePlayerAccepted; // When a master accepts a player to the game session

                // Events to react to requests
                _session.Value!.OnPlayerJoinRequestAdded += OnPlayerJoinRequestAdded; // When a player requests to join the game session
                _session.Value!.OnPlayerRejected += OnPlayerRejected; // When a master rejects a player from the game session
                _session.Value!.OnPlayerDisconnected += OnPlayerDisconnected; // When a player disconnects or is disconnected from the game session

                // Events to react to nearby connections
                _connections.Value!.OnConnectionEstablished += OnConnectionEstablished; // When a connection is established
                _connections.Value!.OnConnectionLost += OnConnectionLost; // When a connection is lost

                // Debug
                //_connections.Value.OnDataReceived += OnDataReceived; // When data is received

                App.Current!.MainPage!.DisplayAlert("Success", "Started advertising", "OK");
            }
        }

        // Methods used in the page
        public void AcceptPlayer(GetConnectToSessionDto request)
        {
            _session.Value!.AcceptPlayer(request.PlayerId);
            RaisePropertyChanged(nameof(JoinRequests));
            RaisePropertyChanged(nameof(Players));
            RaisePropertyChanged(nameof(Characters));
            RaisePropertyChanged(nameof(Campaign));
        }

        public void RejectPlayer(GetConnectToSessionDto request)
        {
            _session.Value!.RejectPlayer(request.PlayerId);
            RaisePropertyChanged(nameof(JoinRequests));
        }

        public void DisconnectPlayer(Guid playerId)
        {
            _session.Value!.DisconnectPlayer(playerId);
            Players.Remove(Players.First(x => x.Id == playerId));
            RaisePropertyChanged(nameof(Players));
        }

        public void RequestCharacterUpdate(Guid playerId)
        {
            _session.Value!.RequestUpdateCharacter(playerId);
        }

        public async void EndSession()
        {
            _connections.Value!.StopAdvertising();
            _context.Value!.ServerSession = null;
            _campaignService.Value!.Campaign = null;
            await _context.Value!.SaveUserAsync();

            // Unsubscribe from events
            _session.Value!.OnCharacterUpdate -= HandleCharacterUpdate;
            _session.Value.OnFullCharacterUpdate -= HandleFullCharacterUpdate;
            _session.Value.OnPlayerAccepted -= HandlePlayerAccepted;

            _session.Value.OnPlayerJoinRequestAdded -= OnPlayerJoinRequestAdded;
            _session.Value.OnPlayerRejected -= OnPlayerRejected;
            _session.Value.OnPlayerDisconnected -= OnPlayerDisconnected;

            _connections.Value!.OnConnectionEstablished -= OnConnectionEstablished;
            _connections.Value.OnConnectionLost -= OnConnectionLost;

            await Shell.Current.GoToAsync("..");
        }

        // Game session events

        public void OnPlayerJoinRequestAdded(GetConnectToSessionDto dto)
        {
            RaisePropertyChanged(nameof(JoinRequests));
        }

        public void OnPlayerDisconnected(Guid playerId)
        {
            RaisePropertyChanged(nameof(Players));
        }

        public async void HandlePlayerAccepted(Character character)
        {
            if (_campaignService.Value!.GetCharacter(character.Id) is null)
            {
                _campaignService.Value!.AddCharacter(character.Id, character.Name, character);
            }
            else
            {
                bool success = _campaignService.Value!.TryUpdateCharacter(character);
            }
            RaisePropertyChanged(nameof(Players));
            RaisePropertyChanged(nameof(Characters));
            await _context.Value!.SaveUserAsync();
        }

        public void OnPlayerRejected(Guid playerId)
        {
            RaisePropertyChanged(nameof(JoinRequests));
        }

        public async void HandleCharacterUpdate(UpdateCharacterDto dto)
        {
            bool success = _campaignService.Value!.TryUpdateCharacter(dto);
            if (success)
            {
                //
            }
            else
            {
                //
            }
            RaisePropertyChanged(nameof(Characters));
            await _context.Value!.SaveUserAsync();
        }

        public async void HandleFullCharacterUpdate(Character character)
        {
            bool success = _campaignService.Value!.TryUpdateCharacter(character);
            if (success)
            {
                //
            }
            else
            {
                //
            }
            RaisePropertyChanged(nameof(Characters));
            await _context.Value!.SaveUserAsync();
        }

        // Nearby connections events
        public void OnConnectionEstablished(string endpointId)
        {
            //
        }

        public void OnConnectionLost(string endpointId)
        {
            var player = Players.FirstOrDefault(x => x.EndpointId == endpointId);
            if (player is not null)
            {
                Players.Remove(player);
            }
            RaisePropertyChanged(nameof(Players));
        }

        public void OnDataReceived(string endpointId, string message)
        {
            App.Current.MainPage!.DisplayAlert($"Message from {endpointId}", message, "OK");
        }

        public async Task StartEncounterAsync()
        {
            try
            {
                if (_encounterService.Value is null)
                {
                    var encounterPopup = new SelectEncounterPopup(_context.Value!.GetMasterUser().Encounters);
                    var result = await Shell.Current.ShowPopupAsync(encounterPopup);
                    if (result is not Encounter encounter)
                    {
                        return;
                    }

                    // No idea why the encounter saves characters, maybe automapper doesn't deep copy
                    ObservableCollection<CharacterInEncounter> charactersIterate = new();
                    foreach (var character in encounter.Characters)
                    {
                        if (character.IsPlayerCharacter is false)
                        {
                            character.Order = 0;
                            character.Initiative = 0;
                            character.IsCurrentTurn = false;
                            charactersIterate.Add(character);
                        }
                    }
                    encounter.Characters = charactersIterate;

                    var selectCharactersPopup = new SelectCharactersPopup(Characters);
                    var selectedResult = await Shell.Current.ShowPopupAsync(selectCharactersPopup);
                    if (selectedResult is not ObservableCollection<object> characters || characters.Count == 0)
                    {
                        return;
                    }

                    var selectedCharacters = characters.Select(x => x as CharacterInCampaign).ToList();

                    _encounterService.Value = new();
                    var copy = encounter.Copy();
                    _encounterService!.Value!.InitializeEncounter(copy);

                    foreach (var character in selectedCharacters)
                    {
                        _encounterService.Value!.AddCharacter(character!.Character!);
                    }
                }

                await Shell.Current.GoToAsync("EncounterPage");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
