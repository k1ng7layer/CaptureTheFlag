using System;
using System.Collections.Generic;
using Entitites;
using Factories;
using Mirror;
using Services.Flags;
using Services.Network.Handlers;
using Services.Player;
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
        private readonly PlayerHandler _playerHandler;
        private readonly PlayerEntityFactory _playerEntityFactory;
        private readonly IFlagSpawnService _flagSpawnService;
        private readonly Queue<EColor> _colors = new();

        public ServerInitializePlayerSystem(
            SpawnService spawnService, 
            PlayerHandler playerHandler,
            PlayerEntityFactory playerEntityFactory,
            IFlagSpawnService flagSpawnService
        )
        {
            _spawnService = spawnService;
            _playerHandler = playerHandler;
            _playerEntityFactory = playerEntityFactory;
            _flagSpawnService = flagSpawnService;
        }
        
        public void Initialize()
        {
            NetworkServer.RegisterHandler<PlayerReadyMessage>(PreparePlayerOnHost);
            
            foreach (var color in (EColor[])Enum.GetValues(typeof(EColor)))
            {
                _colors.Enqueue(color);
            }
        }

        private void PreparePlayerOnHost(
            NetworkConnectionToClient conn, 
            PlayerReadyMessage msg
        )
        {
            var playerView = _spawnService.Spawn("Player", Vector3.zero, Quaternion.identity);
            NetworkServer.AddPlayerForConnection(conn, playerView.Transform.gameObject);
          
            var playerEntity = _playerEntityFactory.Create();
            playerEntity.IsLocalPlayer = playerView.IsLocal;
            
            playerView.Initialize(playerEntity);
            
            _playerHandler.AddPlayer(playerEntity);

            var color = _colors.Dequeue();
            playerEntity.SetColor(color);

            for (int i = 0; i < 3; i++)
            {
                _flagSpawnService.SpawnFlag(color);
            }
        }
        
    }
}