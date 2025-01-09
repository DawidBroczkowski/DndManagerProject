using DndManager.Accessor;
using DndManager.Interfaces;
using DndManager.Network;
using DndManager.Network.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DndManager.Platforms
{
    public class NearbyConnectionsManager : INearbyConnectionsManager
    {
        public event Action<string>? OnConnectionEstablished;
        public event Action<string>? OnConnectionLost;
        public event Action<string, string>? OnDataReceived;
        public event Action<string, string>? OnEndpointDiscovered;
        public event Action<string>? OnEndpointLost;

        public void ConnectToEndpoint(string endpointId)
        {
            throw new NotImplementedException();
        }

        public void ConnectToEndpoint(string endpointId, string localEndpointId)
        {
            throw new NotImplementedException();
        }

        public void DisconnectFromEndpoint(string endpointId)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<string> GetConnectedEndpoints()
        {
            throw new NotImplementedException();
        }

        public IReadOnlyDictionary<string, string> GetDiscoveredEndpoints()
        {
            throw new NotImplementedException();
        }

        public void SendData(string endpointId, string message)
        {
            throw new NotImplementedException();
        }

        public void SetMediator(ISessionMediator mediator)
        {
            throw new NotImplementedException();
        }

        public void SetMediator(Accessor<SessionMediator> mediator)
        {
            throw new NotImplementedException();
        }

        public void StartAdvertising(string localEndpointName)
        {
            throw new NotImplementedException();
        }

        public void StartDiscovery()
        {
            throw new NotImplementedException();
        }

        public void StopAdvertising()
        {
            throw new NotImplementedException();
        }

        public void StopDiscovery()
        {
            throw new NotImplementedException();
        }
    }
}
