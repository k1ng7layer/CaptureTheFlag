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
        private readonly PlayerRepository _playerRepository;
        private readonly PlayerEntityFactory _playerEntityFactory;
        private readonly Dictionary<int, GameEntity> _gameEntities = new();

        public PlayerSpawnService(
            ISpawnService spawnService, 
            PlayerRepository playerRepository,
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
            playerView.Initialize(playerEntity);
            
            _playerRepository.AddPlayer(connectionId, playerEntity);
            NetworkServer.AddPlayerForConnection(connection, playerView.Transform.gameObject);
            
            return playerEntity;
        }
    }
}