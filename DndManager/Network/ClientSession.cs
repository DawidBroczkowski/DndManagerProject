using DndManager.Accessor;
using DndManager.DndApplication;
using DndManager.Dtos;
using DndManager.Models;
using DndManager.Network.Interfaces;
using DndManager.Services;
using DndManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DndManager.Network
{
    public class ClientSession
    {
        //public readonly CharacterService CharacterService = new();
        public string EndpointId { get; set; } = string.Empty;
        public DateTime StartedAt { get; set; }
        public Accessor<ApplicationContext> Accessor { get; set; }
        public PlayerUser Player => Accessor!.Value!.GetPlayerUser();

        public Action<UpdateCharacterDto>? OnSendUpdateCharacter;
        public Action<Character>? OnHandleRequestUpdateCharacter;
        public Action? OnHandleJoinGameSessionRequestAccepted;
        public Action? OnHandleJoinGameSessionRequestRejected;
        public Action? OnHandleDisconnectFromGameSession;
        public Action? OnHandleGameSessionEndedByHost;
        public Action? OnHandleGameSessionEndedByClient;


        private Accessor<SessionMediator> _mediator;

        public ClientSession(IServiceProvider serviceProvider)
        {
            Accessor = serviceProvider.GetRequiredService<Accessor<ApplicationContext>>();
        }

        public void SetMediator(Accessor<SessionMediator> mediator)
        {
            _mediator = mediator;
        }

        // Methods used by the client

        public void SendJoinGameSessionRequest(GetConnectToSessionDto dto)
        {
            var message = new Message
            {
                MessageType = "JoinGameSessionRequest"
            };

            message.SetPayload(dto);
            _mediator.Value!.SendData(EndpointId, message);
        }

        public void SendDisconnectFromGameSessionRequest()
        {
            var message = new Message
            {
                MessageType = "DisconnectFromGameSessionRequest"
            };

            _mediator.Value!.SendData(EndpointId, message);
        }

        public void SendUpdateCharacter(UpdateCharacterDto dto)
        {
            var message = new Message
            {
                MessageType = "UpdateCharacter"
            };

            message.SetPayload(dto);

            _mediator.Value!.SendData(EndpointId, message);
            OnSendUpdateCharacter?.Invoke(dto);
        }

        public void SendUpdateFullCharacter(Character character)
        {
            var message = new Message
            {
                MessageType = "UpdateFullCharacter"
            };

            message.SetPayload(character);
            _mediator.Value!.SendData(EndpointId, message);
        }

        // Methods invoked by the server

        public void HandleRequestUpdateCharacter(Guid characterId)
        {
            var message = new Message
            {
                MessageType = "UpdateFullCharacter"
            };

            var character = Player.Characters.First(c => c.Id == characterId);
            message.SetPayload(character);
            _mediator.Value!.SendData(EndpointId, message);
            OnHandleRequestUpdateCharacter?.Invoke(character);
        }

        public void HandleJoinGameSessionRequestAccepted()
        {
            OnHandleJoinGameSessionRequestAccepted?.Invoke();
        }

        public void HandleJoinGameSessionRequestRejected()
        {
            OnHandleJoinGameSessionRequestRejected?.Invoke();
        }

        public void HandleDisconnectFromGameSession()
        {
            OnHandleDisconnectFromGameSession?.Invoke();
        }

        public void HandleGameSessionEndedByHost()
        {
            OnHandleGameSessionEndedByHost?.Invoke();
        }

        public void HandleGameSessionEndedByClient()
        {
            OnHandleGameSessionEndedByClient?.Invoke();
        }
    }
}
