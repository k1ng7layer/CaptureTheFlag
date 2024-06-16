using System;
using Factories;
using Mirror;
using Services.PlayerRepository;
using Services.Spawn;
using Settings;
using UnityEngine;
using Views;
using Zenject;
using Object = UnityEngine.Object;

namespace Systems.Client
{
    public class ClientSpawnPlayerSystem : IInitializable, IDisposable
    {
        private readonly PlayerEntityFactory _playerEntityFactory;
        private readonly IPlayerRepository _playerRepository;
        private readonly PrefabBase _prefabBase;
        private readonly ISpawnService _spawnService;

        public ClientSpawnPlayerSystem(
            ISpawnService spawnService, 
            PrefabBase prefabBase,
            PlayerEntityFactory playerEntityFactory,
            IPlayerRepository playerRepository)
        {
            _spawnService = spawnService;
            _prefabBase = prefabBase;
            _playerEntityFactory = playerEntityFactory;
            _playerRepository = playerRepository;
        }

        public void Dispose()
        {
            NetworkClient.UnregisterPrefab(_prefabBase.Get("Player"));
        }

        public void Initialize()
        {
            NetworkClient.RegisterPrefab(_prefabBase.Get("Player"), OnClientPlayerSpawn, OnPlayerDeSpawn);
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