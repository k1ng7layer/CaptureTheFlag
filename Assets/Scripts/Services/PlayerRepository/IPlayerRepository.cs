using System;
using System.Collections.Generic;
using Entitites;

namespace Services.PlayerRepository
{
    public interface IPlayerRepository
    {
        public GameEntity LocalPlayer { get;}
        public List<PlayerEntity> Players { get; }
        public IReadOnlyDictionary<int, PlayerEntity> PlayerEntities { get; }

        public event Action<PlayerEntity> Added;
        public event Action<PlayerEntity> Removed;
        void AddPlayer(int connectionId, PlayerEntity player);
        void Remove(int connectionId);
    }
}