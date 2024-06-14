using System;
using System.Collections.Generic;
using Entitites;

namespace Services.Player
{
    public class PlayerRepository
    {
        private readonly List<PlayerEntity> _players = new();
        private readonly Dictionary<int, PlayerEntity> _playerEntities = new();
        
        public GameEntity LocalPlayerEntityEntity { get; private set; }
        public List<PlayerEntity> Players => _players;
        public IReadOnlyDictionary<int, PlayerEntity> PlayerEntities => _playerEntities;

        public void AddPlayer(int connectionId, PlayerEntity player)
        {
            if (player.IsLocalPlayer)
                LocalPlayerEntityEntity = player;
            
            _players.Add(player);
            _playerEntities.Add(connectionId, player);
        }

        public void Remove(int connectionId)
        {
            var player = _playerEntities[connectionId];
            
            _playerEntities.Remove(connectionId);
            _players.Remove(player);
        }
    }
}