using System;
using System.Collections.Generic;
using Entitites;

namespace Services.PlayerRepository.Impl
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly Dictionary<int, PlayerEntity> _playerEntities = new();
        private readonly List<PlayerEntity> _players = new();

        public GameEntity LocalPlayer { get; private set; }
        public List<PlayerEntity> Players => _players;
        public IReadOnlyDictionary<int, PlayerEntity> PlayerEntities => _playerEntities;
        public event Action<PlayerEntity> Added;
        public event Action<PlayerEntity> Removed;

        public void AddPlayer(int connectionId, PlayerEntity player)
        {
            if (player.IsLocalPlayer)
                LocalPlayer = player;
            
            _players.Add(player);
            _playerEntities.Add(connectionId, player);
            Added?.Invoke(player);
        }

        public void Remove(int connectionId)
        {
            var player = _playerEntities[connectionId];
            
            _playerEntities.Remove(connectionId);
            _players.Remove(player);
        }
    }
}