using Entitites;
using Factories;
using Mirror;
using Services.Spawn;
using Settings;
using UnityEngine;

namespace Services.PlayerRepository
{
    public class PlayerSpawnService : IPlayerSpawnService
    {
        private readonly PlayerEntityFactory _playerEntityFactory;
        private readonly IPlayerRepository _playerRepository;
        private readonly ISpawnService _spawnService;

        public PlayerSpawnService(
            ISpawnService spawnService, 
            IPlayerRepository playerRepository,
            PlayerEntityFactory playerEntityFactory
        )
        {
            _spawnService = spawnService;
            _playerRepository = playerRepository;
            _playerEntityFactory = playerEntityFactory;
        }

        public GameEntity SpawnPlayer(int connectionId, EColor color)
        {
            var connection = NetworkServer.connections[connectionId];
            var playerView = _spawnService.Spawn("Player", Vector3.zero, Quaternion.identity);
            var playerEntity = _playerEntityFactory.Create();

            playerEntity.IsServerObject = true;
            playerEntity.SetColor(color);
            playerEntity.SetIsLocalPlayer(connectionId == NetworkClient.connection.connectionId);
            playerEntity.ChangeCaptureAbility(true);
            playerEntity.OwnerId = connectionId;
            playerView.Initialize(playerEntity);
            
            _playerRepository.AddPlayer(connectionId, playerEntity);
            NetworkServer.AddPlayerForConnection(connection, playerView.Transform.gameObject);
            
            return playerEntity;
        }
    }
}