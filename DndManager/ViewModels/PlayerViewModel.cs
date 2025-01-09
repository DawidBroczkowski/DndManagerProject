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
using DndManager.Pages;
using DndManager.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DndManager.ViewModels
{
    public class PlayerViewModel : INotifyPropertyChanged
    {
        private readonly IServiceProvider _serviceProvider;
        private Accessor<CharacterService> _characterService;
        private Character _characterUpdateModel;
        private Accessor<ClientSession> _session;
        private Accessor<SessionMediator> _mediator;
        private Accessor<ApplicationContext> _context;
        private Accessor<INearbyConnectionsManager> _connections;

        private Guid _characterId = Guid.Empty;

        private bool _isConnected = false;
        public bool IsConnected
        {
            get => _isConnected;
            set
            {
                _isConnected = value;
                RaisePropertyChanged(nameof(IsConnected));
            }
        }
        private bool _isInSession = false;
        public bool IsInSession
        {
            get => _isInSession;
            set
            {
                _isInSession = value;
                RaisePropertyChanged(nameof(IsInSession));

            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public IReadOnlyDictionary<string, string> DiscoveredEndpoints => _connections.Value!.GetDiscoveredEndpoints();

        public ICommand JoinSessionPressed { get; set; }
        public ICommand CreateCharacter { get; set; }
        public ICommand DisconnectFromSessionCommand { get; set; }
        public ICommand BrowseCharactersCommand { get; set; }
        public ICommand OpenCharacterCardCommand { get; set; }

        public PlayerViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _characterService = _serviceProvider.GetRequiredService<Accessor<CharacterService>>();
            _session = _serviceProvider.GetRequiredService<Accessor<ClientSession>>();
            _mediator = _serviceProvider.GetRequiredService<Accessor<SessionMediator>>();
            _context = _serviceProvider.GetRequiredService<Accessor<ApplicationContext>>();
            _connections = _serviceProvider.GetRequiredService<Accessor<INearbyConnectionsManager>>();

            _characterService.Value ??= new CharacterService();
            _session.Value ??= new ClientSession(serviceProvider);
            _mediator.Value ??= new SessionMediator(serviceProvider);
            _mediator.Value.SetSession(_session);
            _mediator.Value.SetConnections(_connections);

            JoinSessionPressed = new Command(async () => await JoinSession());
            CreateCharacter = new Command(async () => await NavigateToCharacterCreation());
            DisconnectFromSessionCommand = new Command(Disconnect);
            BrowseCharactersCommand = new Command(async () => await Shell.Current.GoToAsync("BrowseCharactersPage"));
            OpenCharacterCardCommand = new Command(async () => await NavigateToCharactedCard());
        }

        public async Task JoinSession()
        {
            Initialize();
            RaisePropertyChanged(nameof(DiscoveredEndpoints));
            var sessionPopup = new SelectSessionPopup(DiscoveredEndpoints);
            var result = await Shell.Current.ShowPopupAsync(sessionPopup);
            if (result is not KeyValuePair<string, string> session)
            {
                Disconnect();
                return;
            }

            ConnectToEndpoint(session.Key);
            _session.Value!.EndpointId = session.Key;

            var characterPopup = new SelectCharacterPopup(_serviceProvider);
            result = await Shell.Current.ShowPopupAsync(characterPopup);
            if (result is not Character selectedCharacter)
            {
                Disconnect();
                return;
            }

            _characterId = selectedCharacter.Id;

            SendJoinGameSessionRequest(selectedCharacter);
        }

        public void Initialize()
        {
            if (_context.Value!.ClientSession is not null)
            {
                return;
            }

            _context.Value!.ClientSession = _session.Value!;
            _context.Value!.ClientSession.OnHandleDisconnectFromGameSession += OnDisconnectedFromSession;
            _context.Value!.ClientSession.OnHandleGameSessionEndedByHost += OnGameSessionEndedByHost;
            _context.Value!.ClientSession.OnHandleGameSessionEndedByClient += OnGameSessionEndedByClient;

            _connections.Value!.OnConnectionLost += OnConnectionLost;
            _connections.Value!.OnConnectionEstablished += OnConnectionEstablished;
            _connections.Value!.OnEndpointDiscovered += OnEndpointDiscovered;
            _connections.Value!.OnEndpointLost += OnEndpointLost;

            _session.Value!.OnHandleJoinGameSessionRequestAccepted += OnJoinRequestAccepted;
            _session.Value!.OnHandleJoinGameSessionRequestRejected += OnJoinRequestRejected;

            StartDiscovery();
        }

        public void Disconnect()
        {
            StopDiscovery();

            _connections.Value!.OnConnectionLost -= OnConnectionLost;
            _connections.Value!.OnConnectionEstablished -= OnConnectionEstablished;
            _connections.Value!.OnEndpointDiscovered -= OnEndpointDiscovered;
            _connections.Value!.OnEndpointLost -= OnEndpointLost;

            _session.Value!.OnHandleJoinGameSessionRequestAccepted -= OnJoinRequestAccepted;
            _session.Value!.OnHandleJoinGameSessionRequestRejected -= OnJoinRequestRejected;

            DisconnectFromEndpoint(_session.Value!.EndpointId);
        }

        public void StartDiscovery()
        {
            _connections.Value!.StartDiscovery();
        }

        public void StopDiscovery()
        {
            _connections.Value!.StopDiscovery();
        }

        public void OnEndpointDiscovered(string endpointId, string endpointName)
        {
            RaisePropertyChanged(nameof(DiscoveredEndpoints));
        }

        public void OnEndpointLost(string endpointId)
        {
            RaisePropertyChanged(nameof(DiscoveredEndpoints));
        }

        public void ConnectToEndpoint(string endpointId)
        {
            _connections.Value!.ConnectToEndpoint(endpointId, _context.Value!.GetUser().Id.ToString());
        }

        public void OnConnectionEstablished(string endpointId)
        {
            IsConnected = true;
            _session.Value!.EndpointId = endpointId;
        }

        public void DisconnectFromEndpoint(string endpointId)
        {
            IsConnected = false;
            IsInSession = false;
            _context.Value!.ClientSession = null;
            _session.Value!.SendDisconnectFromGameSessionRequest();
            _connections.Value!.DisconnectFromEndpoint(endpointId);
        }

        public void OnGameSessionEndedByHost()
        {
            Disconnect();
            IsConnected = false;
            IsInSession = false;
            _context.Value!.ClientSession = null;
            RaisePropertyChanged(nameof(DiscoveredEndpoints));
            App.Current!.MainPage!.DisplayAlert("Session ended", "Game session ended by host", "OK");
        }

        public void OnGameSessionEndedByClient()
        {
            Disconnect();
            IsConnected = false;
            IsInSession = false;
            _context.Value!.ClientSession = null;
            RaisePropertyChanged(nameof(DiscoveredEndpoints));
            App.Current!.MainPage!.DisplayAlert("Session ended", "Game session ended by client", "OK");
        }

        public void OnDisconnectedFromSession()
        {
            Disconnect();
            IsConnected = false;
            IsInSession = false;
            _context.Value!.ClientSession = null;
            RaisePropertyChanged(nameof(DiscoveredEndpoints));
            App.Current!.MainPage!.DisplayAlert("Disconnected", "You have been disconnected from the session", "OK");
        }


        public void OnConnectionLost(string endpointId)
        {
            Disconnect();
            IsConnected = false;
            IsInSession = false;
            _context.Value!.ClientSession = null;
            RaisePropertyChanged(nameof(DiscoveredEndpoints));
            App.Current!.MainPage!.DisplayAlert("Disconnected", "You have been disconnected from the session", "OK");
        }

        public void SendJoinGameSessionRequest(Character character)
        {
            var dto = new GetConnectToSessionDto
            {
                PlayerId = _context.Value!.GetUser().Id,
                PlayerName = _context.Value!.GetUser().UserName,
                Character = character
            };

            _session.Value!.SendJoinGameSessionRequest(dto);
        }

        public void OnJoinRequestAccepted()
        {
            IsInSession = true;
        }

        public void OnJoinRequestRejected()
        {
            App.Current!.MainPage!.DisplayAlert("Join Request Rejected", "You have been rejected from the session", "OK");
        }

        public async Task NavigateToCharacterCreation()
        {
            await Shell.Current.GoToAsync("CreateCharacterPage");
        }

        public async Task NavigateToCharactedCard()
        {
            _characterService.Value = new();
            _characterService.Value!.InitializeCharacter(_context.Value!.GetPlayerUser().Characters.First(x => x.Id == _characterId));
            await Shell.Current.GoToAsync("InSessionCharacterCardPage");
        }
    }
}
