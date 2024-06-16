using System;
using Factories;
using Mirror;
using Services.Player;
using Services.Spawn;
using Settings;
using UnityEngine;
using Views;
using Zenject;
using Object = UnityEngine.Object;

namespace Systems.Client
{
    public class ClientInitializePlayerSystem : IInitializable, IDisposable
    {
        private readonly ISpawnService _spawnService;
        private readonly PrefabBase _prefabBase;
        private readonly PlayerEntityFactory _playerEntityFactory;
        private readonly PlayerRepository _playerRepository;

        public ClientInitializePlayerSystem(
            ISpawnService spawnService, 
            PrefabBase prefabBase,
            PlayerEntityFactory playerEntityFactory,
            PlayerRepository playerRepository)
        {
            _spawnService = spawnService;
            _prefabBase = prefabBase;
            _playerEntityFactory = playerEntityFactory;
            _playerRepository = playerRepository;
        }
        
        public void Initialize()
        {
            NetworkClient.RegisterPrefab(_prefabBase.Get("Player"), OnClientPlayerSpawn, OnPlayerDeSpawn);
        }
        
        public void Dispose()
        {
            NetworkClient.UnregisterPrefab(_prefabBase.Get("Player"));
        }
        
        private GameObject OnClientPlayerSpawn(Vector3 position, uint assetId)
        {
            var view = _spawnService.Spawn("Player", Vector3.zero, Quaternion.identity);
            
            view.LocalPlayerStarted += OnLocalPlayerSpawned;
            
            return view.Transform.gameObject;
        }
        
        private void OnPlayerDeSpawn(GameObject gameObject)
        {
            Object.Destroy(gameObject);
        }
        
        private void OnLocalPlayerSpawned(IEntityView view)
        {
            view.LocalPlayerStarted -= OnLocalPlayerSpawned;
            
            var entity = _playerEntityFactory.Create();
            entity.IsLocalPlayer = true;
            view.Initialize(entity);

            var networkIdentity = view.Transform.GetComponent<NetworkIdentity>();
            _playerRepository.AddPlayer(networkIdentity.connectionToServer.connectionId, entity);
        }
    }
}