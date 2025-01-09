using DndManager.Network.Interfaces;

namespace DndManager
{
    public interface INearbyConnectionsService
    {
        void SendData(string endpointId, string message);
        void StartAdvertising(string localEndpointName);
        void StartDiscovery();
        void SetMediator(ISessionMediator sessionMediator);
    }
}