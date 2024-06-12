using System;
using System.Collections.Generic;
using Entitites;
using Factories;
using Mirror;
using Services.Flags;
using Services.Network.Handlers;
using Services.Player;
using Services.Presenters;
using Services.Spawn;
using Services.Spawn.Impl;
using Settings;
using UnityEngine;
using Views;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Systems
{
    public class ServerInitializePlayerSystem : IInitializable
    {
        private readonly SpawnService _spawnService;
        private readonly PlayerPresenterFactory _playerPresenterFactory;
        private readonly PlayerHandler _playerHandler;
        private readonly GameEntityFactory _gameEntityFactory;
        private readonly IFlagsService _flagsService;
        private readonly Queue<EColor> _colors = new();
        private readonly List<PlayerPresenter> _playerPresenters = new();

        public ServerInitializePlayerSystem(
            SpawnService spawnService, 
            PlayerPresenterFactory playerPresenterFactory,
            PlayerHandler playerHandler,
            GameEntityFactory gameEntityFactory,
            IFlagsService flagsService
        )
        {
            _spawnService = spawnService;
            _playerPresenterFactory = playerPresenterFactory;
            _playerHandler = playerHandler;
            _gameEntityFactory = gameEntityFactory;
            _flagsService = flagsService;
        }
        
        public void Initialize()
        {
            NetworkServer.RegisterHandler<RequestPlayerSpawnMessage>(PreparePlayerOnHost);
            
            foreach (var color in (EColor[])Enum.GetValues(typeof(EColor)))
            {
                _colors.Enqueue(color);
            }
        }

        private void PreparePlayerOnHost(
            NetworkConnectionToClient conn, 
            RequestPlayerSpawnMessage msg
        )
        {
            var playerView = _spawnService.Spawn("Player", Vector3.zero, Quaternion.identity);
            NetworkServer.AddPlayerForConnection(conn, playerView.Transform.gameObject);

            var playerEntity = _gameEntityFactory.Create();
            playerEntity.IsLocalPlayer = playerView.IsLocal;
            
            InitPresenter(playerView, playerEntity);
            var color = _colors.Dequeue();
            playerEntity.SetColor(color);

            _flagsService.SpawnFlag(color);
        }

        private void InitPresenter(IEntityView view, GameEntity player)
        {
            var playerPresenter = _playerPresenterFactory.Create(view, player);
            playerPresenter.Initialize();
            
            _playerPresenters.Add(playerPresenter);
            _playerHandler.AddPlayer(player);
        }
        
    }
}