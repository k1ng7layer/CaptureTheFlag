using System;
using Mirror;
using Settings;
using UnityEngine;
using Views;
using Zenject;
using Object = UnityEngine.Object;

namespace Services.Spawn
{
    public class NetworkSpawnService2 : ISpawnService, IInitializable
    {
        private readonly PrefabBase _prefabBase;

        public event Action<IEntityView> PlayerSpawned; 
            
        public NetworkSpawnService2(PrefabBase prefabBase)
        { 
            _prefabBase = prefabBase;
        }
        
        public void Initialize()
        {
            var player = _prefabBase.PlayerPrefab.GetComponent<PlayerView>();
            
            NetworkClient.RegisterPrefab(player.gameObject, OnPlayerSpawn, OnPlayerDeSpawn);
        }

        private GameObject OnPlayerSpawn(Vector3 position, uint assetId)
        {
            var player = _prefabBase.PlayerPrefab.GetComponent<PlayerView>();

            var obj = Object.Instantiate(player.gameObject);
            Debug.Log($"OnPlayerSpawn");
            PlayerSpawned?.Invoke(obj.GetComponent<IEntityView>());
            return obj;
        }
        
        private void OnPlayerDeSpawn(GameObject gameObject)
        {
            Object.Destroy(gameObject);
        }
    }
}