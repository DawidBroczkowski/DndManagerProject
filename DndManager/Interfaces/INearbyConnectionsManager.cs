
using DndManager.Accessor;
using DndManager.Network;
using DndManager.Network.Interfaces;

namespace DndManager.Interfaces
{
    public interface INearbyConnectionsManager
    {
        event Action<string>? OnConnectionEstablished;
        event Action<string>? OnConnectionLost;
        event Action<string, string>? OnDataReceived;
        event Action<string, string>? OnEndpointDiscovered;
        event Action<string>? OnEndpointLost;

        void ConnectToEndpoint(string endpointId, string localEndpointId);
        void DisconnectFromEndpoint(string endpointId);
        IReadOnlyCollection<string> GetConnectedEndpoints();
        IReadOnlyDictionary<string, string> GetDiscoveredEndpoints();
        void SendData(string endpointId, string message);
        void StartAdvertising(string localEndpointName);
        void StartDiscovery();
        void StopAdvertising();
        void StopDiscovery();
        void SetMediator(Accessor<SessionMediator> mediator);
    }
}