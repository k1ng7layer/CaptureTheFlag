using System;
using System.Collections.Generic;
using Entitites;
using Factories;
using Mirror;
using Services.Player;
using Services.Presenters;
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
        private readonly GameEntityFactory _gameEntityFactory;
        private readonly PlayerPresenterFactory _playerPresenterFactory;
        private readonly PlayerHandler _playerHandler;
        private List<PlayerPresenter> _playerPresenters = new();

        public ClientInitializePlayerSystem(
            ISpawnService spawnService, 
            PrefabBase prefabBase,
            GameEntityFactory gameEntityFactory,
            PlayerPresenterFactory playerPresenterFactory,
            PlayerHandler playerHandler)
        {
            _spawnService = spawnService;
            _prefabBase = prefabBase;
            _gameEntityFactory = gameEntityFactory;
            _playerPresenterFactory = playerPresenterFactory;
            _playerHandler = playerHandler;
        }
        
        public void Initialize()
        {
            NetworkClient.RegisterPrefab(_prefabBase.Get("Player"), OnClientPlayerSpawn, OnPlayerDeSpawn);
            NetworkClient.RegisterPrefab(_prefabBase.Get("Flag"), OnFlagSpawn, OnFlagDespawn);
        }
        
        public void Dispose()
        {
            NetworkClient.UnregisterPrefab(_prefabBase.Get("Player"));
            NetworkClient.UnregisterPrefab(_prefabBase.Get("Flag"));
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
            view.LocalStarted -= OnLocalPlayerSpawned;
            var entity = _gameEntityFactory.Create();
            entity.IsLocalPlayer = view.IsLocal;
            
            InitPresenter(view, entity);
        }
        
        private void InitPresenter(IEntityView view, GameEntity player)
        {
            var playerPresenter = _playerPresenterFactory.Create(view, player);
            playerPresenter.Initialize();
            
            _playerPresenters.Add(playerPresenter);
            _playerHandler.AddPlayer(player);
        }

        private GameObject OnFlagSpawn(Vector3 position, uint assetId)
        {
            var view = _spawnService.Spawn("Flag", Vector3.zero, Quaternion.identity);

            return view.Transform.gameObject;
        }

        private void OnFlagDespawn(GameObject gameObject)
        {
            
        }
    }
}