using DndManager.Accessor;
using DndManager.Interfaces;
using DndManager.Utility;

namespace DndManager.Network.Interfaces
{
    public interface ISessionMediator
    {
        event Action<string>? OnConnectionEstablished;
        event Action<string>? OnConnectionLost;
        event Action<string, string>? OnDataReceived;
        event Action<string, string>? OnEndpointDiscovered;
        event Action<string>? OnEndpointLost;

        void HandleDataReceived(string endpointId, string data);
        void SendData(string endpointId, Message data);
        void SetConnections(Accessor<INearbyConnectionsManager> connectionsManager);
        void SetSession(Accessor<ClientSession> clientSession);
        void SetSession(Accessor<ServerSession> serverSession);
    }
}