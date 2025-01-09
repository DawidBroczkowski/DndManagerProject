using DndManager.Accessor;
using DndManager.Dtos;
using DndManager.Models;
using DndManager.Network.Interfaces;
using DndManager.Network.Utility;
using DndManager.Services;
using DndManager.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DndManager.Network
{
    public class ServerSession
    {
        public Guid Id { get; set; }
        public DateTime StartedAt { get; set; }
        public ObservableCollection<PlayerInSession> Players { get; set; } = new();
        public ObservableCollection<GetConnectToSessionDto> JoinRequests { get; set; } = new();

        public event Action<GetConnectToSessionDto>? OnPlayerJoinRequestAdded;
        public event Action<Character>? OnPlayerAccepted;
        public event Action<Guid>? OnPlayerRejected;
        public event Action<Guid>? OnPlayerDisconnected;
        public event Action<Character>? OnFullCharacterUpdate;
        public event Action<UpdateCharacterDto>? OnCharacterUpdate;
        public event Action<Guid>? OnCharacterUpdateRequested;

        private Accessor<SessionMediator> _mediator;

        public ServerSession() 
        {
        }

        public void SetMediator(Accessor<SessionMediator> mediator)
        {
            _mediator = mediator;
        }

        // Methods used by the server to manage game session
        public void AcceptPlayer(Guid playerId) // Accept join request (invoked by dm)
        {
            var request = JoinRequests.FirstOrDefault(r => r.PlayerId == playerId);
            if (request != null)
            {
                var player = new PlayerInSession
                {
                    Id = request.PlayerId,
                    StartedAt = DateTime.Now,
                    UserName = request.PlayerName,
                    EndpointId = request.EndpointId,
                    CharacterId = request.Character.Id,
                };

                Players.Add(player);
                JoinRequests.Remove(request);
                _mediator.Value!.SendData(request!.EndpointId, new Message("JoinRequestAccepted"));
                OnPlayerAccepted?.Invoke(request.Character);
            }
        }

        public void RejectPlayer(Guid playerId) // Reject join request (invoked by dm)
        {
            var request = JoinRequests.FirstOrDefault(r => r.PlayerId == playerId);
            if (request != null)
            {
                JoinRequests.Remove(request);
                _mediator.Value!.SendData(request!.EndpointId, new Message("JoinRequestRejected"));
            }
            OnPlayerRejected?.Invoke(playerId);
        }

        public void DisconnectPlayer(Guid playerId) // Disconnect player (invoked by dm)
        {
            var player = Players.FirstOrDefault(p => p.Id == playerId);
            if (player != null)
            {
                Players.Remove(player);
                _mediator.Value!.SendData(player.EndpointId, new Message("SessionEndedByHost"));
            }
            OnPlayerDisconnected?.Invoke(playerId);
        }

        // Methods invoked by the client

        public void HandlePlayerJoinRequest(GetConnectToSessionDto dto) // Receive join request (incoming player call)
        {
            JoinRequests.Add(dto);
            OnPlayerJoinRequestAdded?.Invoke(dto);
        }

        public void HandleDisconnectPlayer(string endpointId) // Disconnect player (incoming player call)
        {
            var player = Players.FirstOrDefault(p => p.EndpointId == endpointId);
            
            _mediator.Value!.SendData(endpointId, new Message("GameSessionEndedByClient"));
            if (player != null)
            {
                Players.Remove(player);
                OnPlayerDisconnected?.Invoke(player!.Id);
            }
        }

        public void HandleUpdateCharacter(UpdateCharacterDto dto) // Update character (incoming player call)
        {
            OnCharacterUpdate?.Invoke(dto);
        }

        public void HandleUpdateFullCharacter(Character character)
        {
            OnFullCharacterUpdate?.Invoke(character);
        }

        // Methods used by the server to send to client
        public void RequestUpdateCharacter(Guid playerId) // Request character update (invoked by dm)
        {
            var player = Players.FirstOrDefault(p => p.Id == playerId);
            if (player != null)
            {
                _mediator.Value!.SendData(player.EndpointId, new Message("RequestUpdateCharacter"));
            }
            OnCharacterUpdateRequested?.Invoke(playerId);
        }
    }
}
