using Entitites;
using Factories;
using Mirror;
using Services.Spawn;
using Settings;
using UnityEngine;

namespace Services.Player
{
    public class PlayerSpawnService : IPlayerSpawnService
    {
        private readonly ISpawnService _spawnService;
        private readonly PlayerHandler _playerHandler;
        private readonly GameEntityFactory _gameEntityFactory;

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
            NetworkServer.AddPlayerForConnection(connection, playerView.Transform.gameObject);
          
            var playerEntity = _gameEntityFactory.Create();
            playerEntity.IsLocalPlayer = playerView.IsLocal;
            
            playerView.Initialize(playerEntity);
            
            _playerHandler.AddPlayer(playerEntity);
            
            playerEntity.SetColor(color);

            return playerEntity;
        }
    }
}