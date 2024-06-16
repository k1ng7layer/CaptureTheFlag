using System;
using System.Collections.Generic;
using GameResult;
using Mirror;
using Services.FlagRepository;
using Services.FlagSpawn;
using Services.GameState;
using Services.Network.Handlers;
using Services.PlayerRepository;
using Settings;
using Utils;
using Zenject;

namespace Systems.Server
{
    public class GameInitializeSystem : IInitializable, IDisposable
    {
        private readonly IPlayerSpawnService _playerSpawnService;
        private readonly IFlagSpawnService _flagSpawnService;
        private readonly IFlagRepository _flagRepository;
        private readonly IGameStateService _gameStateService;
        private readonly GameSettings _gameSettings;
        private readonly Queue<EColor> _colors = new();
        private List<ConnectionGameState> _gameConnections = new();

        public GameInitializeSystem(
            IPlayerSpawnService playerSpawnService, 
            IFlagSpawnService flagSpawnService,
            IFlagRepository flagRepository,
            IGameStateService gameStateService,
            GameSettings gameSettings
        )
        {
            _playerSpawnService = playerSpawnService;
            _flagSpawnService = flagSpawnService;
            _flagRepository = flagRepository;
            _gameStateService = gameStateService;
            _gameSettings = gameSettings;
        }
        
        public void Initialize()
        {
            NetworkServer.RegisterHandler<PlayerReadyMessage>(HostInitializePlayer);
            
            foreach (var color in (EColor[])Enum.GetValues(typeof(EColor)))
            {
                _colors.Enqueue(color);
            }
        }
        
        public void Dispose()
        {
            NetworkServer.UnregisterHandler<PlayerReadyMessage>();
        }

        private void HostInitializePlayer(
            NetworkConnectionToClient conn, 
            PlayerReadyMessage msg
        )
        {
            conn.isReady = true;
            
            var gameConnection = new ConnectionGameState(conn.connectionId)
            {
                ConnectionState = EConnectionState.Ready
            };

            _gameConnections.Add(gameConnection);
            
            if (AllReady())
                StartGame();
        }

        private bool AllReady()
        {
            if (_gameConnections.Count < _gameSettings.RequiredPlayers)
                return false;
            
            foreach (var gameConnection in _gameConnections)
            {
                if (gameConnection.ConnectionState != EConnectionState.Ready)
                    return false;
            }

            return true;
        }

        private void StartGame()
        {
            foreach (var connectionEntry in NetworkServer.connections)
            {
                var color = _colors.Dequeue();
                var player = _playerSpawnService.SpawnPlayer(connectionEntry.Value.connectionId, color);

                for (var i = 0; i < _gameSettings.FlagsNumPerPlayer; i++)
                {
                    var flag = _flagSpawnService.SpawnFlag(player.Color, connectionEntry.Value.connectionId);
                
                    _flagRepository.Add(flag);
                }
            }
            
            _gameStateService.SetGameState(EGameState.Running);
        }
    }
}