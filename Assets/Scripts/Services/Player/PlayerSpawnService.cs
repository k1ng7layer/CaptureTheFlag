using System.Collections.Generic;
using Entitites;
using Factories;
using Mirror;
using Services.Spawn;
using Settings;
using UnityEngine;
using Views;

namespace Services.Player
{
    public class PlayerSpawnService : IPlayerSpawnService
    {
        private readonly ISpawnService _spawnService;
        private readonly PlayerHandler _playerHandler;
        private readonly GameEntityFactory _gameEntityFactory;
        private readonly Dictionary<int, GameEntity> _gameEntities = new();

        public PlayerSpawnService(
            ISpawnService spawnService, 
            PlayerHandler playerHandler,
            GameEntityFactory gameEntityFactory
        )
        {
            _spawnService = spawnService;
            _playerHandler = playerHandler;
            _gameEntityFactory = gameEntityFactory;
        }
        
        public GameEntity SpawnPlayer(int connectionId, EColor color)
        {
            var connection = NetworkServer.connections[connectionId];
            var playerView = _spawnService.Spawn("Player", Vector3.zero, Quaternion.identity);
         
            
            //NetworkServer.Spawn(playerView.Transform.gameObject, connection);
          
            var playerEntity = _gameEntityFactory.Create();

            playerEntity.IsServerObject = true;
            playerEntity.SetColor(color);
            playerEntity.SetIsLocalPlayer(connectionId == NetworkClient.connection.connectionId);
            playerView.Initialize(playerEntity);
            
            _playerHandler.AddPlayer(playerEntity);
            NetworkServer.AddPlayerForConnection(connection, playerView.Transform.gameObject);
            return playerEntity;
        }
    }
}