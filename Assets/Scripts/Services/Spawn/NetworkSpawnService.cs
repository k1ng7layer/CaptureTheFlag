using System;
using System.Collections.Generic;
using Mirror;
using Services.MessageDispatcher;
using Services.Network.Handlers;
using Settings;
using Views;
using Object = UnityEngine.Object;

namespace Services.Spawn
{
    public class NetworkSpawnService : IDisposable
    {
        private readonly INetworkMessageDispatcher _networkMessageDispatcher;
        private readonly PrefabBase _prefabBase;
        private readonly Queue<EColor> _colors = new();

        public NetworkSpawnService(
            INetworkMessageDispatcher networkMessageDispatcher, 
            PrefabBase prefabBase
        )
        {
            _networkMessageDispatcher = networkMessageDispatcher;
            _prefabBase = prefabBase;
        }

        public event Action<IEntityView> PlayerSpawned; 

        public void Init()
        {
            _networkMessageDispatcher.Register<SpawnPlayerMessage>(SpawnPlayer);

            foreach (var color in (EColor[])Enum.GetValues(typeof(EColor)))
            {
                _colors.Enqueue(color);
            }
        }

        private void SpawnPlayer(NetworkConnectionToClient conn, SpawnPlayerMessage msg)
        {
            var player = Object.Instantiate(_prefabBase.PlayerPrefab);
            var view = player.GetComponent<PlayerView>();
            var color = _colors.Dequeue();
            
            NetworkServer.AddPlayerForConnection(conn, player);
            
            view.OnSeverColorChange(color);
            
            PlayerSpawned?.Invoke(view);
        }

        public void Dispose()
        {
            _networkMessageDispatcher.Unregister<SpawnPlayerMessage>();
        }
    }
}