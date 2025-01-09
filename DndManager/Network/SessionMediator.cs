using DndManager.Accessor;
using DndManager.Dtos;
using DndManager.Interfaces;
using DndManager.Models;
using DndManager.Network.Interfaces;
using DndManager.Utility;
using System.Runtime.CompilerServices;

namespace DndManager.Network
{
    public class SessionMediator
    {
        private readonly IServiceProvider _serviceProvider;
        private Accessor<ServerSession>? _serverSession;
        private Accessor<ClientSession>? _clientSession;
        private Accessor<INearbyConnectionsManager>? _connectionsManager;

        public SessionMediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public event Action<string, string>? OnEndpointDiscovered
        {
            add => _connectionsManager!.Value!.OnEndpointDiscovered += value;
            remove => _connectionsManager!.Value!.OnEndpointDiscovered -= value;
        }

        public event Action<string>? OnEndpointLost
        {
            add => _connectionsManager!.Value!.OnEndpointLost += value;
            remove => _connectionsManager!.Value!.OnEndpointLost -= value;
        }

        public event Action<string>? OnConnectionEstablished
        {
            add => _connectionsManager!.Value!.OnConnectionEstablished += value;
            remove => _connectionsManager!.Value!.OnConnectionEstablished -= value;
        }

        public event Action<string>? OnConnectionLost
        {
            add => _connectionsManager!.Value!.OnConnectionLost += value;
            remove => _connectionsManager!.Value!.OnConnectionLost -= value;
        }

        public event Action<string, string>? OnDataReceived
        {
            add => _connectionsManager!.Value!.OnDataReceived += value;
            remove => _connectionsManager!.Value!.OnDataReceived -= value;
        }

        public void SetSession(Accessor<ServerSession> serverSession)
        {
            _serverSession = serverSession;
            _serverSession.Value!.SetMediator(_serviceProvider.GetRequiredService<Accessor<SessionMediator>>());
        }

        public void SetSession(Accessor<ClientSession> clientSession)
        {
            _clientSession = clientSession;
            _clientSession.Value!.SetMediator(_serviceProvider.GetRequiredService<Accessor<SessionMediator>>());
        }

        public void SetConnections(Accessor<INearbyConnectionsManager> connectionsManager)
        {
            _connectionsManager = connectionsManager;
            _connectionsManager.Value!.SetMediator(_serviceProvider.GetRequiredService<Accessor<SessionMediator>>());
        }

        public void HandleDataReceived(string endpointId, string data)
        {
            Message message = Message.FromJson(data);
            switch (message.MessageType)
            {
                // Player requests to dm
                case "JoinGameSessionRequest":
                    var dto = message.GetPayload<GetConnectToSessionDto>();
                    dto.EndpointId = endpointId;
                    _serverSession!.Value!.HandlePlayerJoinRequest(dto);
                    break;
                case "DisconnectFromGameSessionRequest":
                    _serverSession!.Value!.HandleDisconnectPlayer(endpointId);
                    break;
                case "UpdateCharacter":
                    var characterDto = message.GetPayload<UpdateCharacterDto>();
                    _serverSession!.Value!.HandleUpdateCharacter(characterDto);
                    break;
                case "UpdateFullCharacter":
                    var character = message.GetPayload<Character>();
                    _serverSession!.Value!.HandleUpdateFullCharacter(character);
                    break;
                // Dm requests to player
                case "JoinRequestAccepted":
                    _clientSession!.Value!.HandleJoinGameSessionRequestAccepted();
                    break;
                case "JoinRequestRejected":
                    _clientSession!.Value!.HandleJoinGameSessionRequestRejected();
                    break;
                case "GameSessionEndedByHost":
                    _clientSession!.Value!.HandleGameSessionEndedByHost();
                    break;
                case "GameSessionEndedByClient":
                    _clientSession!.Value!.HandleGameSessionEndedByClient();
                    break;
                case "RequestUpdateCharacter":
                    var characterId = message.GetPayload<Guid>();
                    _clientSession!.Value!.HandleRequestUpdateCharacter(characterId);
                    break;
            }
        }

        public void SendData(string endpointId, Message data)
        {
            _connectionsManager!.Value!.SendData(endpointId, data.ToJson());
        }
    }
}
