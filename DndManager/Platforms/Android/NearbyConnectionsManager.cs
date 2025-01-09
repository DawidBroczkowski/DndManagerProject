using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.Gms.Nearby;
using Android.Gms.Nearby.Connection;
using Android.Gms.Tasks;
using DndManager.Network.Interfaces;
using System.Text;
using DndManager.Interfaces;
using DndManager.Accessor;
using DndManager.Network;

namespace DndManager.Platforms.Android
{
    public class NearbyConnectionsManager : INearbyConnectionsManager
    {
        private readonly IConnectionsClient _connectionsClient;
        private readonly string _serviceId = "com.example.dndgame";
        private Accessor<SessionMediator> _sessionMediator;

        public event Action<string, string>? OnEndpointDiscovered;
        public event Action<string>? OnEndpointLost;
        public event Action<string>? OnConnectionEstablished;
        public event Action<string>? OnConnectionLost;
        public event Action<string, string>? OnDataReceived;

        private readonly Dictionary<string, string> _discoveredEndpoints = new();
        private readonly HashSet<string> _connectedEndpoints = new();

        public NearbyConnectionsManager()
        {
            _connectionsClient = NearbyClass.GetConnectionsClient(Platform.AppContext);
        }

        public void SetMediator(Accessor<SessionMediator> mediator)
        {
            _sessionMediator = mediator;
        }

        public void StartAdvertising(string localEndpointName)
        {
            _connectionsClient.StartAdvertising(
                localEndpointName,
                _serviceId,
                new ConnectionLifecycleCallbackImpl(this),
                new AdvertisingOptions.Builder().SetStrategy(Strategy.P2pStar).Build()
            ).AddOnSuccessListener(new OnSuccessListener(() =>
            {
                Console.WriteLine("Advertising started successfully.");
            })).AddOnFailureListener(new OnFailureListener(exception =>
            {
                Console.WriteLine($"Advertising failed: {exception.Message}");
            }));
        }

        public void StartDiscovery()
        {
            _connectionsClient.StartDiscovery(
                _serviceId,
                new EndpointDiscoveryCallbackImpl(this),
                new DiscoveryOptions.Builder().SetStrategy(Strategy.P2pStar).Build()
            );
        }

        public void StopDiscovery()
        {
            _connectionsClient.StopDiscovery();
        }

        public void StopAdvertising()
        {
            _connectionsClient.StopAdvertising();
        }

        public void SendData(string endpointId, string message)
        {
            var payload = Payload.FromBytes(Encoding.UTF8.GetBytes(message));
            _connectionsClient.SendPayload(endpointId, payload);
        }

        public IReadOnlyDictionary<string, string> GetDiscoveredEndpoints()
        {
            return _discoveredEndpoints;
        }

        public IReadOnlyCollection<string> GetConnectedEndpoints()
        {
            return _connectedEndpoints;
        }

        public void ConnectToEndpoint(string endpointId, string localEndpointName)
        {
            if (_discoveredEndpoints.ContainsKey(endpointId))
            {
                _connectionsClient.RequestConnection(localEndpointName, endpointId, new ConnectionLifecycleCallbackImpl(this));
            }
        }

        public void DisconnectFromEndpoint(string endpointId)
        {
            if (_connectedEndpoints.Contains(endpointId))
            {
                _connectionsClient.DisconnectFromEndpoint(endpointId);
                _connectedEndpoints.Remove(endpointId);
                OnConnectionLost?.Invoke(endpointId);
            }
        }

        private class ConnectionLifecycleCallbackImpl : ConnectionLifecycleCallback
        {
            private readonly NearbyConnectionsManager _manager;

            public ConnectionLifecycleCallbackImpl(NearbyConnectionsManager manager)
            {
                _manager = manager;
            }

            public override void OnConnectionInitiated(string endpointId, ConnectionInfo info)
            {
                _manager._connectionsClient.AcceptConnection(endpointId, new PayloadCallbackImpl(_manager));
            }

            public override void OnConnectionResult(string endpointId, ConnectionResolution resolution)
            {
                if (resolution.Status.IsSuccess)
                {
                    _manager._connectedEndpoints.Add(endpointId);
                    _manager.OnConnectionEstablished?.Invoke(endpointId);
                }
            }

            public override void OnDisconnected(string endpointId)
            {
                if (_manager._connectedEndpoints.Remove(endpointId))
                {
                    _manager.OnConnectionLost?.Invoke(endpointId);
                }
            }
        }

        private class EndpointDiscoveryCallbackImpl : EndpointDiscoveryCallback
        {
            private readonly NearbyConnectionsManager _manager;

            public EndpointDiscoveryCallbackImpl(NearbyConnectionsManager manager)
            {
                _manager = manager;
            }

            public override void OnEndpointFound(string endpointId, DiscoveredEndpointInfo info)
            {
                _manager._discoveredEndpoints[endpointId] = info.EndpointName;
                _manager.OnEndpointDiscovered?.Invoke(endpointId, info.EndpointName);
            }

            public override void OnEndpointLost(string endpointId)
            {
                if (_manager._discoveredEndpoints.Remove(endpointId))
                {
                    _manager.OnEndpointLost?.Invoke(endpointId);
                }
            }
        }

        private class PayloadCallbackImpl : PayloadCallback
        {
            private readonly NearbyConnectionsManager _manager;

            public PayloadCallbackImpl(NearbyConnectionsManager manager)
            {
                _manager = manager;
            }

            public override void OnPayloadReceived(string endpointId, Payload payload)
            {
                var message = Encoding.UTF8.GetString(payload.AsBytes()!);
                _manager._sessionMediator.Value?.HandleDataReceived(endpointId, message);
                _manager.OnDataReceived?.Invoke(endpointId, message);
            }

            public override void OnPayloadTransferUpdate(string endpointId, PayloadTransferUpdate update)
            {
                // Optional: handle payload transfer updates, e.g., tracking progress
            }
        }

        public class OnSuccessListener : Java.Lang.Object, IOnSuccessListener
        {
            private readonly Action _onSuccess;

            public OnSuccessListener(Action onSuccess)
            {
                _onSuccess = onSuccess;
            }

            public void OnSuccess(Java.Lang.Object result)
            {
                _onSuccess();
            }
        }

        public class OnFailureListener : Java.Lang.Object, IOnFailureListener
        {
            private readonly Action<Exception> _onFailure;

            public OnFailureListener(Action<Exception> onFailure)
            {
                _onFailure = onFailure;
            }

            public void OnFailure(Java.Lang.Exception e)
            {
                _onFailure(e);
            }
        }
    }
}
