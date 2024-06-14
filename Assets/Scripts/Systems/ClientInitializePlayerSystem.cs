using System;
using System.Collections.Generic;
using Entitites;
using Factories;
using Mirror;
using Services.Network.Handlers;
using Services.Player;
using Services.Spawn;
using Settings;
using UnityEngine;
using Views;
using Zenject;
using Object = UnityEngine.Object;

namespace Systems
{
    public class ClientInitializePlayerSystem : IInitializable, IDisposable
    {
        private readonly ISpawnService _spawnService;
        private readonly PrefabBase _prefabBase;
        private readonly PlayerEntityFactory _playerEntityFactory;
        private readonly PlayerHandler _playerHandler;

        public ClientInitializePlayerSystem(
            ISpawnService spawnService, 
            PrefabBase prefabBase,
            PlayerEntityFactory playerEntityFactory,
            PlayerHandler playerHandler)
        {
            _spawnService = spawnService;
            _prefabBase = prefabBase;
            _playerEntityFactory = playerEntityFactory;
            _playerHandler = playerHandler;
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
            
            view.LocalStarted += OnLocalPlayerSpawned;
            
            return view.Transform.gameObject;
        }
        
        private void OnPlayerDeSpawn(GameObject gameObject)
        {
            Object.Destroy(gameObject);
        }
        
        private void OnLocalPlayerSpawned(IEntityView view)
        {
            view.ClientStarted -= OnLocalPlayerSpawned;
            var entity = _playerEntityFactory.Create();
            entity.IsLocalPlayer = view.IsLocal;
            view.Initialize(entity);
           
            
            _playerHandler.AddPlayer(entity);
        }
    }
}