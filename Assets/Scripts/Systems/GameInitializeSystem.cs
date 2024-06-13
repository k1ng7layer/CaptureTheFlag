using System;
using System.Collections.Generic;
using Mirror;
using Services.FlagRepository;
using Services.Flags;
using Services.Network.Handlers;
using Services.Player;
using Settings;
using Zenject;

namespace Systems
{
    public class GameInitializeSystem : IInitializable, IDisposable
    {
        private readonly IPlayerSpawnService _playerSpawnService;
        private readonly IFlagSpawnService _flagSpawnService;
        private readonly IFlagRepository _flagRepository;
        private readonly GameSettings _gameSettings;
        private readonly Queue<EColor> _colors = new();

        public GameInitializeSystem(
            IPlayerSpawnService playerSpawnService, 
            IFlagSpawnService flagSpawnService,
            IFlagRepository flagRepository,
            GameSettings gameSettings
        )
        {
            _playerSpawnService = playerSpawnService;
            _flagSpawnService = flagSpawnService;
            _flagRepository = flagRepository;
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
            var color = _colors.Dequeue();
            var player = _playerSpawnService.SpawnPlayer(conn.connectionId, color);

            for (var i = 0; i < _gameSettings.FlagsNumPerPlayer; i++)
            {
                var flag = _flagSpawnService.SpawnFlag(player.Color);
                
                _flagRepository.Add(flag);
            }
        }
    }
}