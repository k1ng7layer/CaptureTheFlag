using System;
using System.Collections.Generic;
using Mirror;
using Services.MessageDispatcher;
using Services.Network.Handlers;
using Settings;
using UnityEngine;
using Views;
using Zenject;
using Object = UnityEngine.Object;

namespace Services.Spawn
{
    public class NetworkSpawnService : IInitializable, IDisposable
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
        
        public void Initialize()
        {
            _networkMessageDispatcher.Register<SpawnPlayerMessage>(ServerSpawnPlayer);
            
            NetworkClient.RegisterPrefab(_prefabBase.PlayerPrefab, OnClientPlayerSpawn, OnPlayerDeSpawn);

            foreach (var color in (EColor[])Enum.GetValues(typeof(EColor)))
            {
                _colors.Enqueue(color);
            }
        }
        
        public void Dispose()
        {
            _networkMessageDispatcher.Unregister<SpawnPlayerMessage>();
        }
        
        private void ServerSpawnPlayer(NetworkConnectionToClient conn, SpawnPlayerMessage msg)
        {
            var player = Object.Instantiate(_prefabBase.PlayerPrefab);
            var view = player.GetComponent<PlayerView>();
            var color = _colors.Dequeue();
            
            NetworkServer.AddPlayerForConnection(conn, player);
            
            view.OnSeverColorChange(color);
            
            PlayerSpawned?.Invoke(view);
        }
        
        private GameObject OnClientPlayerSpawn(Vector3 position, uint assetId)
        {
            var player = _prefabBase.PlayerPrefab.GetComponent<PlayerView>();
            var obj = Object.Instantiate(player.gameObject);
            var view = obj.GetComponent<IEntityView>();
            
            view.LocalStarted += OnLocalPlayerSpawned;
            
            return obj;
        }
        
        private void OnPlayerDeSpawn(GameObject gameObject)
        {
            var view = gameObject.GetComponent<PlayerView>();
            view.LocalStarted -= OnLocalPlayerSpawned;
            Object.Destroy(gameObject);
        }

        private void OnLocalPlayerSpawned(IEntityView view)
        {
            PlayerSpawned?.Invoke(view);
        }
    }
}